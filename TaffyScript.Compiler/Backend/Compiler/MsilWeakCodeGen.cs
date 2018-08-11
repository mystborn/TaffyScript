using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TaffyScript;
using TaffyScript.Collections;
using TaffyScript.Compiler.DotNet;
using TaffyScript.Compiler.Syntax;

namespace TaffyScript.Compiler.Backend
{
    /// <summary>
    /// Generates an assembly based off of an abstract syntax tree.
    /// </summary>
    public class MsilWeakCodeGen : ISyntaxElementVisitor
    {
        #region Constants

        /// <summary>
        /// Special file name that will be stored in the output assembly manifest that contains a list of imported methods with the <see cref="TaffyScriptMethodAttribute"/>.
        /// </summary>
        private const string SpecialImportsFileName = "SpecialImports.resource";

        /// <summary>
        /// The value of the keyword all.
        /// </summary>
        private const float All = -3f;

        #endregion

        #region Fields

        /// <summary>
        /// Whenever a local variable needs to be created by the compiler, it uses this number to create it, and then increments this to avoid naming conflicts.
        /// </summary>
        private int _secret = 0;

        /// <summary>
        /// If the compiler is in debug mode, this will be used to write symbol information. Currently the only symbols written are method calls to help with debugging.
        /// </summary>
        private Dictionary<string, ISymbolDocumentWriter> _documents = new Dictionary<string, ISymbolDocumentWriter>();

        private readonly bool _isDebug;
        private readonly AssemblyName _asmName;
        private readonly AssemblyBuilder _asm;
        private ModuleBuilder _module;
        private ILEmitter _initializer = null;
        private readonly DotNetAssemblyLoader _assemblyLoader;
        private readonly DotNetTypeParser _typeParser;
        private string _entryPoint;
        private string _projectName;
        private TargetCpu _targetCpu;

        private SymbolTable _table;
        private IErrorLogger _logger;
        private SymbolResolver _resolver;
        private AssetStore _assets;

        /// <summary>
        /// Row=Namespace, Col=MethodName, Value=MethodInfo
        /// </summary>
        private readonly LookupTable<string, string, MethodInfo> _methods = new LookupTable<string, string, MethodInfo>();
        private readonly LookupTable<string, string, long> _enums = new LookupTable<string, string, long>();
        private readonly BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// Maps namespaces to the types that hold the global scripts inside that namespace.
        /// </summary>
        private readonly Dictionary<string, TypeBuilder> _baseTypes = new Dictionary<string, TypeBuilder>();

        private Dictionary<TypeBuilder, TypeBuilder> _unresolvedTypes = new Dictionary<TypeBuilder, TypeBuilder>(TypeBuilderEqualityComparer.Single);

        /// <summary>
        /// Used to memoize unary operaters.
        /// </summary>
        private LookupTable<string, Type, MethodInfo> _unaryOps = new LookupTable<string, Type, MethodInfo>();

        /// <summary>
        /// Used to memoize binary operators.
        /// </summary>
        private Dictionary<string, LookupTable<Type, Type, MethodInfo>> _binaryOps = new Dictionary<string, LookupTable<Type, Type, MethodInfo>>();

        /// <summary>
        /// Converts an operator to its implementation name. + -> op_Addition
        /// </summary>
        private Dictionary<string, string> _operators;

        /// <summary>
        /// Maintains a collection of methods that haven't been found.
        /// </summary>
        private Dictionary<string, TokenPosition> _pendingMethods = new Dictionary<string, TokenPosition>();

        private HashSet<SyntaxType> _declarationTypes;
        
        /// <summary>
        /// Do not use. Backing stream of SpecialImports.
        /// </summary>
        private System.IO.MemoryStream _stream = null;

        /// <summary>
        /// Do not use directly.
        /// </summary>
        private System.IO.StreamWriter _specialImports = null;

        /// <summary>
        /// Currently this will always be an empty string, but when namespaces/modules are implemented, this will contain the name of the current namespace.
        /// </summary>
        private string _namespace = "";

        /// <summary>
        /// The ILEmitter of the current script or event.
        /// </summary>
        private ILEmitter emit;

        /// <summary>
        /// The local variables in the current scope.
        /// </summary>
        private Dictionary<ISymbol, LocalBuilder> _locals = new Dictionary<ISymbol, LocalBuilder>();

        /// <summary>
        /// The top value contains the position of the current loops start.
        /// </summary>
        private Stack<Label> _loopStart = new Stack<Label>();

        /// <summary>
        /// The top value contains the position of the current loops end.
        /// </summary>
        private Stack<Label> _loopEnd = new Stack<Label>();

        /// <summary>
        /// Contains a set of LocalBuilders that were created by the compiler in an effort to reuse variables when possible.
        /// </summary>
        private Dictionary<Type, Stack<LocalBuilder>> _secrets = new Dictionary<Type, Stack<LocalBuilder>>();

        private LocalBuilder _argumentCount = null;

        private Closure _closure = null;
        private int _closures = 0;
        private ISymbol _parent = null;
        private int _argumentArray = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the StreamWriter used to write any special imports into the assembly manifest.
        /// </summary>
        private System.IO.StreamWriter SpecialImports
        {
            get
            {
                if(_specialImports == null)
                {
                    _stream = new System.IO.MemoryStream();
                    _specialImports = new System.IO.StreamWriter(_stream);
                }
                return _specialImports;
            }
        }

        private string ProjectName
        {
            get
            {
                if(_projectName == null)
                    _projectName = _asmName.Name.Replace('.', '_');
                return _projectName;
            }
        }

        /// <summary>
        /// Gets the ILEmitter for the initializer method.
        /// </summary>
        private ILEmitter Initializer
        {
            get
            {
                if (_initializer == null)
                {
                    var asm = _asmName.Name;
                    var name = $"{asm}.{ProjectName}_Initializer";
                    var type = _module.DefineType(name, TypeAttributes.Public);
                    _initializer = new ILEmitter(type.DefineMethod("Initialize", MethodAttributes.Public | MethodAttributes.Static, typeof(void), Type.EmptyTypes), Type.EmptyTypes);
                    _baseTypes.Add(name, type);
                }
                return _initializer;
            }
        }

        private MethodInfo GetGlobalScripts { get; } = typeof(TsReflection).GetMethod("get_GlobalScripts");

        #endregion

        #region Public

        /// <summary>
        /// Initializes a new code generator that will emit weakly typed MSIL.
        /// </summary>
        /// <param name="table">The symbols defined for this code generator.</param>
        /// <param name="config">The build config used when creating the final assembly.</param>
        public MsilWeakCodeGen(SymbolTable table, SymbolResolver resolver, BuildConfig config, IErrorLogger errorLogger)
        {
            _logger = errorLogger;
            _table = table;
            _resolver = resolver;
            _isDebug = config.Mode == CompileMode.Debug;
            _targetCpu = config.Target;
            _asmName = new AssemblyName(System.IO.Path.GetFileName(config.Output));
            _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.Save);
            _asm.DefineVersionInfoResource(config.Product, config.Version, config.Company, config.Copyright, config.Trademark);
            _entryPoint = config.EntryPoint;
            _assets = new AssetStore(this, table, resolver, errorLogger);

            CustomAttributeBuilder attrib;
            if (_isDebug)
            {
                // If running in debug mode, this will allow you to step through the assembly using a debugger.
                // It also might be necessary in order to read debug symbols?
                var aType = typeof(DebuggableAttribute);
                var ctor = aType.GetConstructor(new[] { typeof(DebuggableAttribute.DebuggingModes) });
                attrib = new CustomAttributeBuilder(ctor, new object[] { DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.Default });
                _asm.SetCustomAttribute(attrib);
            }

            _assemblyLoader = new DotNetAssemblyLoader();
            _typeParser = new DotNetTypeParser(_assemblyLoader);

            // Initialize the basic assemblies needed to compile.
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(TsObject)));
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(Console)));
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(HashSet<int>)));

            // Initialize all specified references.
            foreach (var asm in config.References.Select(s => _assemblyLoader.LoadAssembly(s)))
            {
                if(asm.GetCustomAttribute<TaffyScriptLibraryAttribute>() != null)
                {
                    ProcessWeakAssembly(asm);
                    ReadResources(asm);
                }
                else
                {
                    ProcessStrongAssembly(asm);
                }
            }
            
            //Initializes methods relating to TsObjects.
            InitTsMethods();

            //Marks this assembly as weakly typed.
            attrib = new CustomAttributeBuilder(typeof(TaffyScriptLibraryAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            _asm.SetCustomAttribute(attrib);
        }

        public CompilerResult CompileTree(RootNode root)
        {
            //If a main script was defined, output an exe. Otherwise outputs a dll.
            var output = ".dll";

            var ns = "";
            var index = _entryPoint.LastIndexOf('.');
            var script = _entryPoint;
            if (index != -1)
            {
                ns = _entryPoint.Remove(index);
                script = _entryPoint.Substring(index + 1);
            }
            var count = _table.EnterNamespace(ns);

            if (_table.Defined(script, out var symbol) && symbol.Type == SymbolType.Script)
                output = ".exe";

            _table.Exit(count);

            _module = _asm.DefineDynamicModule(_asmName.Name, _asmName.Name + output, true);

            if (output == ".exe")
            {
                var init = Initializer;
                foreach (var asm in _assemblyLoader.LoadedAssemblies.Values.Where(a => a.GetCustomAttribute<TaffyScriptLibraryAttribute>() != null))
                {
                    var name = asm.GetName().Name;
                    var initMethod = asm.GetType($"{name}.{name.Replace('.', '_')}_Initializer")?.GetMember("Initialize");
                    if(initMethod != null)
                        init.Call(asm.GetType($"{name}.{name.Replace('.', '_')}_Initializer").GetMethod("Initialize"));
                }
            }

            _assets.InitializeModule(_module, Initializer);
#if DEBUG
            root.Accept(this);
#else
            try
            {
                root.Accept(this);
            }
            catch(Exception e)
            {
                _logger.Error($"The compiler encountered an error. Please report it.\n    Exception: {e}");
            }
#endif

            foreach (var pending in _pendingMethods)
                _logger.Error($"Could not find function {pending.Key}", pending.Value);

            if (_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            Initializer.Ret();

            //Finalize any types that were created.
            foreach (var type in _baseTypes.Values)
                type.CreateType();

            if(_unresolvedTypes.Count != 0)
            {
                var swap = new Dictionary<TypeBuilder, TypeBuilder>();
                while (_unresolvedTypes.Count > 0)
                {
                    foreach (var kvp in _unresolvedTypes)
                    {
                        if (kvp.Value.IsCreated())
                            kvp.Key.CreateType();
                        else
                            swap.Add(kvp.Key, kvp.Value);
                    }
                    var temp = _unresolvedTypes;
                    _unresolvedTypes = swap;
                    swap = temp;
                }
            }

            //Write any special imports to the module manifest.
            if (_specialImports != null)
            {
                _specialImports.Flush();
                _module.DefineManifestResource(SpecialImportsFileName, _stream, ResourceAttributes.Public);
            }

            if(_targetCpu == TargetCpu.x86)
                _asm.Save(_asmName.Name + output, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
            else
                _asm.Save(_asmName.Name + output, PortableExecutableKinds.ILOnly, ImageFileMachine.AMD64);


            //Dispose any dynamic resources.
            if (_specialImports != null)
            {
                _specialImports.Dispose();
                _stream.Dispose();
                _specialImports = null;
                _stream = null;
            }

            return new CompilerResult(_asm, System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _asmName.Name + output), _logger);
        }

#endregion

#region Helpers

        private void ProcessStrongAssembly(Assembly asm)
        {
            foreach(var type in asm.ExportedTypes.Where(t => t.IsEnum))
            {
                ProcessEnum(type);
            }
        }
        
        private void ProcessWeakAssembly(Assembly asm)
        {
            foreach(var type in asm.ExportedTypes)
            {
                if (type.GetCustomAttribute<TaffyScriptObjectAttribute>() != null)
                {
                    //Todo: Optimize this
                    //      If the namespace of this iteration matches the last one, no need to exit and reenter.
                    //      Just stay in the namespace.
                    var count = _table.EnterNamespace(type.Namespace);
                    if (!_table.TryEnterNew(type.Name, SymbolType.Object))
                        _logger.Warning($"Name conflict encountered with object {type.Name} defined in assembly {asm.GetName().Name}");
                    else
                        _assets.AddExternalType(_table.Current, type);

                    _table.Exit(count + 1);
                }
                else if(type.IsEnum)
                {
                    ProcessEnum(type);
                }
                else if(type.GetCustomAttribute<TaffyScriptBaseTypeAttribute>() != null)
                {
                    var count = _table.EnterNamespace(type.Namespace);
                    foreach (var method in type.GetMethods(_methodFlags).Where(mi => IsMethodValid(mi)))
                    {
                        _methods.Add(type.Namespace ?? "", method.Name, method);
                        _table.AddLeaf(method.Name, SymbolType.Script, SymbolScope.Global);
                    }
                    _table.Exit(count);
                }
            }
        }

        private void ProcessEnum(Type enumType)
        {
            var count = _table.EnterNamespace(enumType.Namespace);
            var name = enumType.Name;
            if(_table.TryEnterNew(name, SymbolType.Enum))
            {
                var names = Enum.GetNames(enumType);
                var values = Enum.GetValues(enumType);
                for (var i = 0; i < names.Length; i++)
                    _enums.Add(name, names[i], Convert.ToInt64(values.GetValue(i)));
                _table.Exit();
            }

            _table.Exit(count);
        }

        /// <summary>
        /// Reads any special methods stored in an assembly's manifest and makes them usable.
        /// </summary>
        /// <param name="asm"></param>
        private void ReadResources(Assembly asm)
        {
            var resource = asm.GetManifestResourceNames().Where(f => f.EndsWith(SpecialImportsFileName)).FirstOrDefault();
            if (resource != null)
            {
                using (var stream = asm.GetManifestResourceStream(resource))
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var input = line.Split(':');
                            var importType = (ImportType)byte.Parse(input[0]);
                            switch(importType)
                            {
                                case ImportType.Script:
                                    var external = input[3];
                                    var owner = external.Remove(external.LastIndexOf('.'));
                                    var methodName = external.Substring(owner.Length + 1);
                                    var type = _typeParser.GetType(owner);
                                    var method = GetMethodToImport(type, methodName, TsTypes.ArgumentTypes);
                                    var count = _table.EnterNamespace(input[1]);
                                    _table.AddLeaf(input[2], SymbolType.Script, SymbolScope.Global);
                                    _table.Exit(count);
                                    _methods[input[1], input[2]] = method;
                                    break;
                                case ImportType.Object:
                                    external = input[3];
                                    type = _typeParser.GetType(external);
                                    count = _table.EnterNamespace(input[1]);
                                    var leaf = new SymbolLeaf(_table.Current, input[2], SymbolType.Object, SymbolScope.Global);
                                    _assets.AddExternalType(leaf, type);
                                    _table.AddChild(leaf);
                                    _table.Exit(count);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a method has the signature TsObject MethodName(TsObject[]) and is therefore callable by TaffyScript.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private bool IsMethodValid(MethodInfo method)
        {
            if (!typeof(TsObject).IsAssignableFrom(method.ReturnType))
                return false;

            var args = method.GetParameters();
            if (args.Length != 1 || args[0].ParameterType != typeof(TsObject[]))
                return false;

            return true;
        }

        /// <summary>
        /// Helper method to find a method on the specified type.
        /// </summary>
        private MethodInfo GetMethodToImport(Type owner, string name, Type[] argTypes)
        {
            return owner.GetMethod(name, _methodFlags, null, argTypes, null);
        }
        
        /// <summary>
        /// Starts a TaffyScript method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private MethodBuilder StartMethod(string name, string ns)
        {
            // If the method is encountered before it's created, a blank MethodBuilder is created.
            // If this happens, use that to generate the method.
            if (_methods.TryGetValue(ns, name, out var result))
            {
                var m = result as MethodBuilder;
                if (m == null)
                    _logger.Error($"Function with name {name} is already defined by {m?.GetModule().ToString() ?? "<Unknown Module>"}");
                return m;
            }

            var mb = GetBaseType(ns).DefineMethod(name, MethodAttributes.Public | MethodAttributes.Static, typeof(TsObject), TsTypes.ArgumentTypes);
            mb.DefineParameter(1, ParameterAttributes.None, "__args_");
            _methods.Add(ns, name, mb);
            return mb;
        }

        private TypeBuilder GetBaseType(string ns)
        {
            if (!_baseTypes.TryGetValue(ns, out var type))
            {
                var name = $"{ns}.{ProjectName}";
                if (name.StartsWith("."))
                    name = name.TrimStart('.');
                type = _module.DefineType(name, TypeAttributes.Public);
                var attrib = new CustomAttributeBuilder(typeof(TaffyScriptBaseTypeAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
                type.SetCustomAttribute(attrib);
                _baseTypes.Add(ns, type);
            }
            return type;
        }

        /// <summary>
        /// Helper method to wrap an imported method in a valid TaffyScript method.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="importName"></param>
        private void GenerateWeakMethodForImport(MethodInfo method, string importName)
        {
            var mb = StartMethod(importName, _namespace);
            var emit = new ILEmitter(mb, TsTypes.ArgumentTypes);
            var paramArray = method.GetParameters();
            var paramTypes = new Type[paramArray.Length];
            for (var i = 0; i < paramArray.Length; ++i)
                paramTypes[i] = paramArray[i].ParameterType;

            for (var i = 0; i < paramTypes.Length; i++)
            {
                //The first arg is the target, the second is the arg array.
                emit.LdArg(1)
                    .LdInt(i);
                //Only cast the the TsObject if needed.
                if (paramTypes[i] == typeof(object) || typeof(TsObject).IsAssignableFrom(paramTypes[i]))
                {
                    emit.LdElem(typeof(TsObject));
                }
                else if (!typeof(TsObject).IsAssignableFrom(paramTypes[i]))
                {
                    emit.LdElemA(typeof(TsObject))
                        .Call(TsTypes.ObjectCasts[paramTypes[i]]);
                }
                else
                    _logger.Error($"Imported method {importName} had an invalid argument type {paramTypes[i].Name} {paramArray[i].Name}");
            }
            emit.Call(method);
            if (method.ReturnType == typeof(void))
                emit.Call(TsTypes.Empty);
            else if (TsTypes.Constructors.TryGetValue(method.ReturnType, out var ctor))
                emit.New(ctor);
            else if (!typeof(TsObject).IsAssignableFrom(method.ReturnType))
                _logger.Error($"Imported method {importName} had an invalid return type {method.ReturnType.Name}.");

            emit.Ret();
            var name = $"{_namespace}.{importName}".TrimStart('.');
            var init = Initializer;
            init.Call(GetGlobalScripts)
                .LdStr(name)
                .LdNull()
                .LdFtn(mb)
                .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .LdStr(name)
                .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                .Call(typeof(Dictionary<string, TsDelegate>).GetMethod("Add"));
        }

        /// <summary>
        /// Generates an entry point from a TaffyScript method.
        /// </summary>
        /// <param name="entry"></param>
        private void GenerateEntryPoint(MethodInfo entry)
        {
            var input = new[] { typeof(string[]) };
            var method = GetBaseType(_asmName.Name).DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), input);
            var emit = new ILEmitter(method, input);
            var args = emit.DeclareLocal(typeof(TsObject[]), "args");
            var i = emit.DeclareLocal(typeof(int), "i");
            var count = emit.DeclareLocal(typeof(int), "count");
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();
            //The following IL is equivalent to this method:
            //public static void Main(string[] arg1)
            //{
            //    var count = arg1.Length;
            //    var args = new TsObject[count];
            //    for(var i = 0; i < count; i++)
            //    {
            //        args[i] = new TsObject(arg1[i])
            //    }
            //    main(null, args);
            //}
            emit.Call(Initializer.Method as MethodBuilder, 0, typeof(void))
                .LdArg(0)
                .LdLen()
                .Dup()
                .StLocal(count)
                .NewArr(typeof(TsObject))
                .StLocal(args)
                .LdInt(0)
                .StLocal(i)
                .MarkLabel(start)
                .LdLocal(i)
                .LdLocal(count)
                .Bge(end)
                .LdLocal(args)
                .LdLocal(i)
                .LdArg(0)
                .LdLocal(i)
                .LdElem(typeof(string))
                .New(TsTypes.Constructors[typeof(string)])
                .StElem(typeof(TsObject))
                .LdLocal(i)
                .LdInt(1)
                .Add()
                .StLocal(i)
                .Br(start)
                .MarkLabel(end)
                .LdLocal(args)
                .Call(entry, 1, typeof(TsObject))
                .Pop()
                .Ret();

            _asm.SetEntryPoint(method);
        }

        /// <summary>
        /// Initializes any needed variables relating to <see cref="TsObject"/>s.
        /// </summary>
        private void InitTsMethods()
        {
            _operators = new Dictionary<string, string>()
            {
                { "+", "op_Addition" },
                { "&", "op_BitwiseAnd" },
                { "|", "op_BitwiseOr" },
                { "--", "op_Decrement" },
                { "/", "op_Division" },
                { "==", "op_Equality" },
                { "!=", "op_Inequality" },
                { "^", "op_ExclusiveOr" },
                { "explicit", "op_Explicit" },
                { "false", "op_False" },
                { ">", "op_GreaterThan" },
                { ">=", "op_GreaterThanOrEqual" },
                { "++", "op_Increment" },
                { "<<", "op_LeftShift" },
                { "<", "op_LessThan" },
                { "<=", "op_LessThanOrEqual" },
                { "!", "op_LogicalNot" },
                { "%", "op_Modulus" },
                { "*", "op_Multiply" },
                { "~", "op_OnesComplement" },
                { ">>", "op_RightShift" },
                { "-", "op_Subtraction" },
                { "true", "op_True" },
            };

            _declarationTypes = new HashSet<SyntaxType>()
            {
                SyntaxType.Enum,
                SyntaxType.ImportScript,
                SyntaxType.Namespace,
                SyntaxType.Object,
                SyntaxType.Script,
                SyntaxType.ImportObject
            };

            var table = new LookupTable<Type, Type, MethodInfo>
            {
                { typeof(string), typeof(string), typeof(string).GetMethod("Concat", _methodFlags, null, new[] { typeof(string), typeof(string) }, null) }
            };
            _binaryOps.Add("+", table);
        }

        /// <summary>
        /// Gets the specified Unary operator on the given type.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private MethodInfo GetOperator(string op, Type type, TokenPosition pos)
        {
            if(!_unaryOps.TryGetValue(op, type, out var method))
            {
                string name;
                if (op == "-")
                    name = "op_UnaryNegation";
                else if (op == "+")
                    name = "op_UnaryPlus";
                else if (!_operators.TryGetValue(op, out name))
                {
                    //This doesn't specify a token position, but it also should never be encountered.
                    _logger.Error($"Operator {op} does not exist", pos);
                    return null;
                }
                method = type.GetMethod(name, _methodFlags, null, new[] { type }, null);
                if (method == null)
                    _logger.Error($"No operator function is defined for the operator {op} and the type {type}", pos);
            }
            return method;
        }

        /// <summary>
        /// Gets the specified binary operator for the given types.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private MethodInfo GetOperator(string op, Type left, Type right, TokenPosition pos)
        {
            if (!_binaryOps.TryGetValue(op, out var table))
            {
                table = new LookupTable<Type, Type, MethodInfo>();
                _binaryOps.Add(op, table);
            }

            if(!table.TryGetValue(left, right, out var method))
            {
                if (!_operators.TryGetValue(op, out var opName))
                {
                    _logger.Error($"Operator {op} does not exist", pos);
                    return null;
                }

                var argTypes = new Type[] { left, right };
                method = left.GetMethod(opName, _methodFlags, null, argTypes, null) ??
                         right.GetMethod(opName, _methodFlags, null, argTypes, null);
                if (method == null)
                    _logger.Error($"No operator function is defined for the operator {op} and the types {left} and {right}", pos);
            }

            return method;
        }

        /// <summary>
        /// Creates a secret <see cref="TsObject"/> local variable.
        /// </summary>
        /// <returns></returns>
        private LocalBuilder MakeSecret()
        {
            var name = $"__0secret{_secret++}";
            return emit.DeclareLocal(typeof(TsObject), name);
        }

        /// <summary>
        /// Creates a secret variable of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private LocalBuilder MakeSecret(Type type)
        {
            return emit.DeclareLocal(type, $"__0secret_{_secret++}");
        }

        /// <summary>
        /// If the compiler is in debug mode, marks a sequence point in the IL stream.
        /// <para>
        /// Currently this is only called before methods, and it makes the console show better information when an exception is thrown.
        /// </para>
        /// </summary>
        /// <param name="element">The element to mark.</param>
        private void MarkSequencePoint(TokenPosition start, TokenPosition end)
        {
            if (!_isDebug || start.File == null)
                return;

            if(!_documents.TryGetValue(start.File, out var writer))
            {
                writer = _module.DefineDocument(start.File, Guid.Empty, Guid.Empty, Guid.Empty);
                _documents.Add(start.File, writer);
            }
            emit.MarkSequencePoint(writer, start.Line, start.Column, end.Line, end.Column);
        }

        private void ScriptStart(string scriptName, MethodBuilder method, Type[] args)
        {
            emit = new ILEmitter(method, args);
            _table.Enter(scriptName);
            DeclareLocals(true);
        }

        private void DeclareLocals(bool declareLocalLambda)
        {
            foreach(var local in _table.Symbols)
            {
                if(local is VariableLeaf leaf)
                {
                    if (leaf.IsCaptured)
                    {
                        if (_closure == null)
                            GetClosure(declareLocalLambda);
                        var field = _closure.Type.DefineField(local.Name, typeof(TsObject), FieldAttributes.Public);
                        _closure.Fields.Add(local.Name, field);
                    }
                    else
                        _locals.Add(local, emit.DeclareLocal(typeof(TsObject), local.Name));
                }
            }
        }

        private void ScriptEnd()
        {
            _table.Exit();
            _locals.Clear();
            _secrets.Clear();
            _secret = 0;
            _argumentCount = null;
            if(_closure != null)
            {
                _closure.Type.CreateType();
                _closure = null;
            }
        }

        private Closure GetClosure(bool setLocal)
        {
            if(_closure == null)
            {
                var bt = GetBaseType(_namespace);
                var type = bt.DefineNestedType($"<>{emit.Method.Name}_Display", TypeAttributes.NestedAssembly | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                var constructor = type.DefineConstructor(MethodAttributes.Public |
                                                          MethodAttributes.HideBySig |
                                                          MethodAttributes.SpecialName |
                                                          MethodAttributes.RTSpecialName,
                                                      CallingConventions.HasThis,
                                                      Type.EmptyTypes);

                var temp = emit;

                emit = new ILEmitter(constructor, new[] { typeof(object) });
                emit.LdArg(0)
                    .CallBase(typeof(object).GetConstructor(Type.EmptyTypes))
                    .Ret();

                emit = temp;

                _closure = new Closure(type, constructor);

                if (setLocal)
                {
                    var local = emit.DeclareLocal(type, "__0closure");

                    if (emit.Method.IsStatic) {
                        emit.New(constructor, 0)
                            .StLocal(local);
                    }
                    else
                    {
                        // Todo: Only do this if instance variable/self is accessed.
                        // Todo: Test on nested closure.
                        _closure.Target = type.DefineField("__0Target0__", typeof(ITsInstance), FieldAttributes.Assembly);
                        emit.New(constructor, 0)
                            .Dup()
                            .StLocal(local)
                            .LdArg(0)
                            .StFld(_closure.Target);
                    }

                    _closure.Self = local;
                }
            }

            return _closure;
        }

        private LocalBuilder GetLocal() => GetLocal(typeof(TsObject));

        private LocalBuilder GetLocal(Type type)
        {
            if(!_secrets.TryGetValue(type, out var locals))
            {
                locals = new Stack<LocalBuilder>();
                _secrets.Add(type, locals);
            }
            if (locals.Count != 0)
                return locals.Pop();
            else
                return MakeSecret(type);
        }

        private void FreeLocal(LocalBuilder local)
        {
            if(!_secrets.TryGetValue(local.LocalType, out var locals))
            {
                locals = new Stack<LocalBuilder>();
                _secrets.Add(local.LocalType, locals);
            }
            locals.Push(local);
        }

        private void AcceptDeclarations(List<ISyntaxElement> declarations)
        {
            foreach(var child in declarations)
            {
                if (!_declarationTypes.Contains(child.Type))
                    _logger.Error($"Encountered invalid declaration {child.Position}");
                else
                    child.Accept(this);
            }
        }

        private string GetAssetNamespace(ISymbol symbol)
        {
            var sb = new System.Text.StringBuilder("");
            var parent = symbol.Parent;
            while (parent != null && parent.Type == SymbolType.Namespace)
            {
                sb.Insert(0, parent.Name + ".");
                parent = parent.Parent;
            }
            return sb.ToString().TrimEnd('.');
        }

        private void ConvertTopToObject()
        {
            var top = emit.GetTop();
            if(!typeof(TsObject).IsAssignableFrom(top))
            {
                if (typeof(ITsInstance).IsAssignableFrom(top))
                    top = typeof(ITsInstance);
                emit.New(TsTypes.Constructors[top]);
            }
        }

        private void ConvertTopToObject(ILEmitter emitter)
        {
            var top = emitter.GetTop();
            if(!typeof(TsObject).IsAssignableFrom(top))
            {
                if (typeof(ITsInstance).IsAssignableFrom(top))
                    top = typeof(ITsInstance);
                emitter.New(TsTypes.Constructors[top]);
            }
        }

        private ILEmitter LoadTarget()
        {
            if (_closures > 0)
            {
                emit.LdLocal(_closure.Self)
                    .LdFld(_closure.Target);
            }
            else
                emit.LdArg(0);

            return emit;
        }

#endregion

#region Visitor

        public void Visit(AdditiveNode additive)
        {
            additive.Left.Accept(this);
            var left = emit.GetTop();
            if(left == typeof(bool) || left == typeof(int))
            {
                emit.ConvertFloat();
                left = typeof(float);
            }
            additive.Right.Accept(this);
            var right = emit.GetTop();
            if (right == typeof(bool) || right == typeof(int))
            {
                emit.ConvertFloat();
                right = typeof(float);
            }

            if (left == typeof(float))
            {
                if (right == typeof(float))
                {
                    if (additive.Op == "+")
                        emit.Add();
                    else
                        emit.Sub();
                }
                else if (right == typeof(string))
                {
                    _logger.Error($"Cannot {additive.Op} types {left} and {right}", additive.Position);
                    return;
                }
                else if (typeof(TsObject).IsAssignableFrom(right))
                    emit.Call(GetOperator(additive.Op, left, right, additive.Position));
                else
                {
                    _logger.Error($"Cannot {additive.Op} types {left} and {right}", additive.Position);
                    return;
                }
            }
            else if(left == typeof(string))
            {
                if (additive.Op != "+")
                {
                    _logger.Error($"Cannot {additive.Op} types {left} and {right}", additive.Position);
                    return;
                }
                if (right == typeof(float))
                {
                    _logger.Error($"Cannot {additive.Op} types {left} and {right}", additive.Position);
                    return;
                }
                else if(right == typeof(string))
                    emit.Call(typeof(string).GetMethod("Concat", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null));
                else if(typeof(TsObject).IsAssignableFrom(right))
                    emit.Call(GetOperator(additive.Op, left, right, additive.Position));
            }
            else if(typeof(TsObject).IsAssignableFrom(left))
            {
                var method = GetOperator(additive.Op, left, right, additive.Position);
                emit.Call(method);
            }
        }

        public void Visit(ArgumentAccessNode argumentAccess)
        {
            emit.LdArg(_argumentArray);
            LoadElementAsInt(argumentAccess.Index);
            emit.LdElem(typeof(TsObject));
        }

        public void Visit(ArrayAccessNode arrayAccess)
        {
            arrayAccess.Left.Accept(this);
            var top = emit.GetTop();

            if(top == typeof(string))
            {
                if (arrayAccess.Arguments.Count > 1)
                {
                    _logger.Error("Invalid number of arguments for string access", arrayAccess.Position);
                    emit.Pop()
                        .Call(TsTypes.Empty);
                }
                var chr = GetLocal(typeof(char));
                LoadElementAsInt(arrayAccess.Arguments[0]);
                emit.Call(typeof(string).GetMethod("get_Chars"))
                    .StLocal(chr)
                    .LdLocalA(chr)
                    .Call(typeof(char).GetMethod("ToString", Type.EmptyTypes));
                FreeLocal(chr);
                return;
            }
            else if (!typeof(TsObject).IsAssignableFrom(top))
            {
                _logger.Error("Encountered invalid syntax", arrayAccess.Position);
                emit.Pop()
                    .Call(TsTypes.Empty);
                return;
            }

            LocalBuilder secret = null;

            if(CanBeArrayAccess(arrayAccess))
            {
                var isInstance = emit.DefineLabel();
                var end = emit.DefineLabel();

                emit.Dup()
                    .Call(typeof(TsObject).GetMethod("get_Type"))
                    .LdInt((int)VariableType.Array)
                    .Bne(isInstance);

                Type[] argTypes;
                var count = arrayAccess.Arguments.Count;
                if (count > 3)
                {
                    argTypes = new[] { typeof(int[]) };
                    emit.LdInt(count)
                        .NewArr(typeof(int));
                    for (var i = 0; i < count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        LoadElementAsInt(arrayAccess.Arguments[i]);
                        emit.StElem(typeof(int));
                    }
                }
                else
                {
                    argTypes = new Type[count];
                    for (var i = 0; i < count; i++)
                    {
                        argTypes[i] = typeof(int);
                        LoadElementAsInt(arrayAccess.Arguments[i]);
                    }
                }

                emit.Call(typeof(TsObject).GetMethod("ArrayGet", argTypes))
                    .Br(end)
                    .MarkLabel(isInstance)
                    .PopTop() //The stack would get imbalanced here otherwise.
                    .PushType(typeof(TsObject))
                    .Call(typeof(TsObject).GetMethod("GetInstance"))
                    .LdStr("get")
                    .LdInt(arrayAccess.Arguments.Count)
                    .NewArr(typeof(TsObject));

                for (var i = 0; i < arrayAccess.Arguments.Count; i++)
                {
                    emit.Dup()
                        .LdInt(i);
                    arrayAccess.Arguments[i].Accept(this);
                    ConvertTopToObject();
                    emit.StElem(typeof(TsObject));
                }

                emit.Call(typeof(ITsInstance).GetMethod("Call"))
                    .MarkLabel(end);
            }
            else
            {
                emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                    .LdStr("get")
                    .LdInt(arrayAccess.Arguments.Count)
                    .NewArr(typeof(TsObject));

                for (var i = 0; i < arrayAccess.Arguments.Count; i++)
                {
                    emit.Dup()
                        .LdInt(i);
                    arrayAccess.Arguments[i].Accept(this);
                    ConvertTopToObject();
                    emit.StElem(typeof(TsObject));
                }

                emit.Call(typeof(ITsInstance).GetMethod("Call"));
            }
        }

        private bool CanBeArrayAccess(ArrayAccessNode array)
        {
            foreach(var node in array.Arguments)
            {
                if (node is IConstantToken constant && (constant.ConstantType == ConstantType.Bool || constant.ConstantType == ConstantType.String))
                    return false;
            }
            return true;
        }

        public void Visit(ArrayLiteralNode arrayLiteral)
        {
            emit.LdInt(arrayLiteral.Elements.Count)
                .NewArr(typeof(TsObject));
            for (var i = 0; i < arrayLiteral.Elements.Count; i++)
            {
                emit.Dup()
                    .LdInt(i);

                arrayLiteral.Elements[i].Accept(this);
                ConvertTopToObject();
                emit.StElem(typeof(TsObject));
            }
            emit.New(TsTypes.Constructors[typeof(TsObject[])]);
        }

        public void Visit(AssignNode assign)
        {
            if(assign.Op != "=")
            {
                ProcessAssignExtra(assign);
                return;
            }

            if (assign.Left is ArgumentAccessNode arg)
            {
                emit.LdArg(_argumentArray);
                LoadElementAsInt(arg.Index);
                assign.Right.Accept(this);
                ConvertTopToObject();
                emit.StElem(typeof(TsObject));
            }
            else if (assign.Left is ArrayAccessNode array)
            {
                array.Left.Accept(this);
                var top = emit.GetTop();
                if (!typeof(TsObject).IsAssignableFrom(top))
                {
                    _logger.Error("Encountered invalid syntax", array.Position);
                    return;
                }

                if (CanBeArrayAccess(array))
                {
                    var isArray = emit.DefineLabel();
                    var end = emit.DefineLabel();

                    emit.Dup()
                        .Call(typeof(TsObject).GetMethod("get_Type"))
                        .LdInt((int)VariableType.Instance)
                        .Bne(isArray)
                        .Call(typeof(TsObject).GetMethod("GetInstance"))
                        .LdStr("set")
                        .LdInt(array.Arguments.Count + 1)
                        .NewArr(typeof(TsObject));

                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        emit.StElem(typeof(TsObject));
                    }
                    emit.Dup()
                        .LdInt(array.Arguments.Count);
                    assign.Right.Accept(this);
                    ConvertTopToObject();
                    emit.StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop()
                        .Br(end)
                        .PushType(typeof(TsObject))
                        .MarkLabel(isArray);

                    Type[] argTypes;
                    assign.Right.Accept(this);
                    ConvertTopToObject();
                    if (array.Arguments.Count > 3)
                    {
                        argTypes = new[] { typeof(TsObject), typeof(int[]) };

                        emit.LdInt(array.Arguments.Count)
                            .NewArr(typeof(int));

                        for (var i = 0; i < array.Arguments.Count; i++)
                        {
                            emit.Dup()
                                .LdInt(i);
                            LoadElementAsInt(array.Arguments[i]);
                            emit.StElem(typeof(int));
                        }
                    }
                    else
                    {
                        argTypes = new Type[array.Arguments.Count + 1];
                        argTypes[0] = typeof(TsObject);
                        for (var i = 0; i < array.Arguments.Count; i++)
                        {
                            LoadElementAsInt(array.Arguments[i]);
                            argTypes[i + 1] = typeof(int);
                        }
                    }

                    emit.Call(typeof(TsObject).GetMethod("ArraySet", argTypes))
                        .MarkLabel(end);
                }
                else
                {
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                        .LdStr("set")
                        .LdInt(array.Arguments.Count + 1)
                        .NewArr(typeof(TsObject));

                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        emit.StElem(typeof(TsObject));
                    }
                    emit.Dup()
                        .LdInt(array.Arguments.Count);
                    assign.Right.Accept(this);
                    ConvertTopToObject();
                    emit.StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop();
                }
            }
            else if (assign.Left is VariableToken variable)
            {
                //Check if the variable is a local variable.
                //If not, then it MUST be a member var.
                if (_table.Defined(variable.Name, out var symbol))
                {
                    var leaf = symbol as VariableLeaf;
                    if (leaf is null)
                        _logger.Error($"Cannot assign to the value {symbol.Name}", variable.Position);

                    if (leaf.IsCaptured)
                    {
                        if (_closures > 0)
                            emit.LdArg(0);
                        else
                            emit.LdLocal(_closure.Self);
                    }

                    assign.Right.Accept(this);
                    ConvertTopToObject();

                    if (leaf.IsCaptured)
                        emit.StFld(_closure.Fields[leaf.Name]);
                    else
                        emit.StLocal(_locals[symbol]);
                }
                else
                {
                    LoadTarget()
                        .LdStr(variable.Name);
                    assign.Right.Accept(this);
                    ConvertTopToObject();
                    emit.Call(typeof(ITsInstance).GetMethod("set_Item"));
                }
            }
            else if (assign.Left is MemberAccessNode member)
            {
                if (member.Left is ReadOnlyToken token)
                {
                    if (member.Right is VariableToken right)
                    {
                        switch (token.Name)
                        {
                            case "global":
                                emit.LdFld(typeof(TsInstance).GetField("Global"));
                                break;
                            case "self":
                                LoadTarget();
                                break;
                            case "other":
                                emit.Call(typeof(TsInstance).GetMethod("get_Other"));
                                break;
                            default:
                                _logger.Error($"Cannot access member on readonly value {token.Name}", token.Position);
                                return;
                        }
                        emit.LdStr(right.Name);
                        assign.Right.Accept(this);
                        ConvertTopToObject();
                        emit.Call(typeof(ITsInstance).GetMethod("set_Item"));
                    }
                    else
                        _logger.Error("Cannot access readonly value from global", member.Right.Position);
                }
                else
                {
                    member.Left.Accept(this);
                    var top = emit.GetTop();
                    if (!typeof(TsObject).IsAssignableFrom(top))
                        _logger.Error("Invalid syntax detected", member.Left.Position);

                    emit.LdStr(((VariableToken)member.Right).Name);
                    assign.Right.Accept(this);
                    ConvertTopToObject();
                    emit.Call(typeof(TsObject).GetMethod("MemberSet", new[] { typeof(string), typeof(TsObject) }));
                }
            }
            else
                //Todo: This should never be hit. Consider removing it.
                _logger.Error("This type of assignment is not yet supported", assign.Position);
        }

        private void ProcessAssignExtra(AssignNode assign)
        {
            var op = assign.Op.Replace("=", "");
            if (assign.Left is ArgumentAccessNode argument)
            {
                emit.LdArg(_argumentArray);
                LoadElementAsInt(argument.Index);
                emit.LdElemA(typeof(TsObject))
                    .Dup()
                    .LdObj(typeof(TsObject));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                    .StObj(typeof(TsObject));
            }
            else if (assign.Left.Type == SyntaxType.ArrayAccess)
            {
                var value = GetLocal();

                var array = (ArrayAccessNode)assign.Left;
                array.Left.Accept(this);
                var top = emit.GetTop();
                if (!typeof(TsObject).IsAssignableFrom(top))
                {
                    _logger.Error("Invalid syntax detected", array.Position);
                    return;
                }

                var secret = GetLocal(typeof(ITsInstance));

                if (CanBeArrayAccess(array))
                {
                    var isArray = emit.DefineLabel();
                    var end = emit.DefineLabel();

                    emit.Dup()
                        .Call(typeof(TsObject).GetMethod("get_Type"))
                        .LdInt((int)VariableType.Instance)
                        .Bne(isArray)
                        .Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdStr("get")
                        .LdInt(array.Arguments.Count)
                        .NewArr(typeof(TsObject));

                    var indeces = new LocalBuilder[array.Arguments.Count];
                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        indeces[i] = GetLocal();
                        emit.Dup()
                            .StLocal(indeces[i])
                            .StElem(typeof(TsObject));
                    }

                    emit.Call(typeof(ITsInstance).GetMethod("Call"));
                    assign.Right.Accept(this);
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                        .StLocal(value)
                        .LdLocal(secret)
                        .LdStr("set")
                        .LdInt(indeces.Length + 1)
                        .NewArr(typeof(TsObject));

                    FreeLocal(secret);

                    for (var i = 0; i < indeces.Length; i++)
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdLocal(indeces[i])
                            .StElem(typeof(TsObject));
                        FreeLocal(indeces[i]);
                    }
                    emit.Dup()
                        .LdInt(indeces.Length)
                        .LdLocal(value)
                        .StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop()
                        .Br(end)
                        .PushType(typeof(TsObject))
                        .MarkLabel(isArray);

                    FreeLocal(value);

                    Type[] argTypes;
                    var count = array.Arguments.Count - 1;
                    if (count > 2)
                    {
                        argTypes = new[] { typeof(int[]) };
                        emit.LdInt(count)
                            .NewArr(typeof(int));
                        for (var i = 0; i < count; i++)
                        {
                            emit.Dup()
                                .LdInt(i);
                            LoadElementAsInt(array.Arguments[i]);
                            emit.StElem(typeof(int));
                        }
                    }
                    else
                    {
                        argTypes = new Type[count];
                        for (var i = 0; i < count; i++)
                        {
                            argTypes[i] = typeof(int);
                            LoadElementAsInt(array.Arguments[i]);
                        }
                    }

                    emit.Call(typeof(TsObject).GetMethod("GetArray", argTypes));
                    LoadElementAsInt(array.Arguments[count]);

                    emit.LdElemA(typeof(TsObject))
                        .Dup()
                        .LdObj(typeof(TsObject));
                    assign.Right.Accept(this);
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                        .StObj(typeof(TsObject))
                        .MarkLabel(end);
                }
                else
                {
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdStr("get")
                        .LdInt(array.Arguments.Count)
                        .NewArr(typeof(TsObject));

                    var indeces = new LocalBuilder[array.Arguments.Count];
                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        indeces[i] = GetLocal();
                        emit.Dup()
                            .StLocal(indeces[i])
                            .StElem(typeof(TsObject));
                    }

                    emit.Call(typeof(ITsInstance).GetMethod("Call"));
                    assign.Right.Accept(this);
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                        .StLocal(value)
                        .LdLocal(secret)
                        .LdStr("set")
                        .LdInt(indeces.Length + 1)
                        .NewArr(typeof(TsObject));

                    FreeLocal(secret);

                    for (var i = 0; i < indeces.Length; i++)
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdLocal(indeces[i])
                            .StElem(typeof(TsObject));
                        FreeLocal(indeces[i]);
                    }
                    emit.Dup()
                        .LdInt(indeces.Length)
                        .LdLocal(value)
                        .StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop();

                    FreeLocal(value);
                }
            }
            else if(assign.Left is VariableToken variable)
            {
                if (_table.Defined(variable.Name, out var symbol))
                {
                    var leaf = symbol as VariableLeaf;
                    if (leaf is null)
                        _logger.Error($"Cannot assign to the value {symbol.Name}", variable.Position);

                    if (leaf.IsCaptured)
                    {
                        if (_closures > 0)
                            emit.LdArg(0);
                        else
                            emit.LdLocal(_closure.Self);
                        emit.LdFldA(_closure.Fields[variable.Name]);
                    }
                    else
                        emit.LdLocalA(_locals[symbol]);

                    emit.Dup()
                        .LdObj(typeof(TsObject));
                    assign.Right.Accept(this);
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                        .StObj(typeof(TsObject));
                }
                else
                {
                    SelfAccessSet(variable);
                    emit.Call(typeof(ITsInstance).GetMethod("get_Item"));
                    assign.Right.Accept(this);
                    var result = emit.GetTop();
                    if (result == typeof(int) || result == typeof(bool))
                        emit.ConvertFloat();
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position));
                    emit.Call(typeof(ITsInstance).GetMethod("set_Item"));
                }
            }
            else if(assign.Left is MemberAccessNode member)
            {
                if(!(member.Right is VariableToken value))
                {
                    _logger.Error($"Cannot assign to readonly value {((ReadOnlyToken)member.Right).Name}", member.Right.Position);
                    return;
                }
                if(member.Left is ReadOnlyToken read)
                {
                    Func<ILEmitter> loadTarget = GetReadOnlyLoadFunc(read);

                    loadTarget()
                        .LdStr(value.Name);
                    loadTarget()
                        .LdStr(value.Name)
                        .Call(typeof(ITsInstance).GetMethod("get_Item"));

                    assign.Right.Accept(this);
                    var top = emit.GetTop();
                    if (top == typeof(int) || top == typeof(bool))
                    {
                        emit.ConvertFloat();
                        top = typeof(float);
                    }

                    emit.Call(GetOperator(op, typeof(TsObject), top, assign.Position))
                        .Call(typeof(ITsInstance).GetMethod("set_Item"));
                }
                else
                {
                    member.Left.Accept(this);
                    var top = emit.GetTop();
                    if(typeof(TsObject).IsAssignableFrom(top))
                        emit.Call(TsTypes.ObjectCasts[typeof(ITsInstance)]);
                    else if (!typeof(ITsInstance).IsAssignableFrom(top))
                    {
                        _logger.Error($"Expected an instance, got {top}", member.Left.Position);
                        emit.Pop()
                            .Call(TsTypes.Empty);
                        return;
                    }

                    var secret = GetLocal(typeof(ITsInstance));

                    emit.StLocal(secret)
                        .LdLocal(secret)
                        .LdStr(value.Name)
                        .LdLocal(secret)
                        .LdStr(value.Name)
                        .Call(typeof(ITsInstance).GetMethod("get_Item"));

                    assign.Right.Accept(this);
                    ConvertTopToObject();
                    emit.Call(GetOperator(op, typeof(TsObject), top, assign.Position))
                        .Call(typeof(ITsInstance).GetMethod("set_Item"));

                    FreeLocal(secret);
                }
            }
        }

        private void SelfAccessSet(VariableToken variable)
        {
            LoadTarget()
                .LdStr(variable.Name);
            LoadTarget()
                .LdStr(variable.Name);
        }

        public void Visit(BaseNode baseNode)
        {
            if(_parent == null)
            {
                _logger.Error("Cannot call 'base' from a type with no parent.", baseNode.Position);
                return;
            }
            var script = _assets.GetInstanceMethod(_parent, emit.Method.Name, baseNode.Position);
            if(script == null)
            {
                emit.Call(TsTypes.Empty);
                _logger.Error("Tried to call a method on a parent that doesn't implement it", baseNode.Position);
                return;
            }

            MarkSequencePoint(baseNode.Position, baseNode.EndPosition);
            LoadTarget();
            LoadFunctionArguments(baseNode.Arguments);
            emit.Call(script, 2, typeof(TsObject));
        }

        public void Visit(BitwiseNode bitwise)
        {
            if(bitwise.Left.Type == SyntaxType.Constant)
            {
                var leftConst = bitwise.Left as IConstantToken<float>;
                if (leftConst == null)
                    _logger.Error($"Cannot perform operator {bitwise.Op} on the constant type {(bitwise.Left as IConstantToken).ConstantType}", bitwise.Position);
                else
                    emit.LdLong((long)leftConst.Value);
            }
            else
            {
                bitwise.Left.Accept(this);
                var top = emit.GetTop();
                if (!typeof(TsObject).IsAssignableFrom(top))
                    _logger.Error($"Cannot perform operator {bitwise.Op} on the type {top}", bitwise.Position);
                emit.Call(TsTypes.ObjectCasts[typeof(long)]);
            }

            if(bitwise.Right.Type == SyntaxType.Constant)
            {
                var rightConst = bitwise.Right as IConstantToken<float>;
                if (rightConst == null)
                    _logger.Error($"Cannot perform operator {bitwise.Op} on the constant type {(bitwise.Right as IConstantToken).ConstantType}", bitwise.Position);
                else
                    emit.LdLong((long)rightConst.Value);
            }
            else
            {
                bitwise.Right.Accept(this);
                var top = emit.GetTop();
                if (!typeof(TsObject).IsAssignableFrom(top))
                    _logger.Error($"Cannot perform operator {bitwise.Op} on the type {top}", bitwise.Position);
                emit.Call(TsTypes.ObjectCasts[typeof(long)]);
            }

            switch(bitwise.Op)
            {
                case "|":
                    emit.Or();
                    break;
                case "&":
                    emit.And();
                    break;
                case "^":
                    emit.Xor();
                    break;
                default:
                    //Should be impossible.
                    _logger.Error($"Invalid bitwise operator detected: {bitwise.Op}", bitwise.Position);
                    break;
            }
            emit.ConvertFloat();
        }

        public void Visit(BlockNode block)
        {
            for (var i = 0; i < block.Body.Count; ++i)
            {
                block.Body[i].Accept(this);
                //Each child in a block is either a statement which leaves nothing on the stack,
                //or an expression which will leave one value.
                //If there's a value left, pop it.
                //If there's more than one value, the compiler failed somehow.
                //It should never happen, but we log an error just in case.
                switch(emit.Types.Count)
                {
                    case 0:
                        break;
                    case 1:
                        emit.Pop();
                        break;
                    default:
                        while (emit.Types.Count > 0)
                            emit.Pop();
                        _logger.Error("Stack is unbalanced inside of a block", block.Position);
                        break;
                }
            }
        }

        public void Visit(BreakToken breakToken)
        {
            if (_loopEnd.Count > 0)
                emit.Br(_loopEnd.Peek());
            else
                _logger.Error("Tried to break outside of a loop", breakToken.Position);
        }

        public void Visit(ConditionalNode conditionalNode)
        {
            conditionalNode.Condition.Accept(this);

            // Todo: Optimize comparison on float constant
            var top = emit.GetTop();
            if (typeof(TsObject).IsAssignableFrom(top))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            else if (top == typeof(float))
                emit.LdFloat(0).Cgt();
            else if (top != typeof(bool))
                _logger.Error("Detected invalid syntax", conditionalNode.Condition.Position);
            var brFalse = emit.DefineLabel();
            var brFinal = emit.DefineLabel();
            emit.BrFalse(brFalse);

            // We convert the result of the expression into a TsObject
            // in order to get a unified output. Otherwise, the program could have
            // undefined behaviour.

            conditionalNode.Left.Accept(this);
            ConvertTopToObject();

            emit.Br(brFinal)
                .MarkLabel(brFalse);

            conditionalNode.Right.Accept(this);
            ConvertTopToObject();

            // This unbalances the execution stack.
            // Maunually pop the top value off the stack.

            emit.MarkLabel(brFinal);
            emit.PopTop();
        }

        public void Visit(IConstantToken constantToken)
        {
            switch (constantToken.ConstantType)
            {
                case ConstantType.Bool:
                    emit.LdBool(((ConstantToken<bool>)constantToken).Value);
                    break;
                case ConstantType.Real:
                    emit.LdFloat(((ConstantToken<float>)constantToken).Value);
                    break;
                case ConstantType.String:
                    emit.LdStr(((ConstantToken<string>)constantToken).Value);
                    break;
            }
        }

        public void Visit(ContinueToken continueToken)
        {
            emit.Br(_loopStart.Peek());
        }

        public void Visit(DoNode doNode)
        {
            var doStart = emit.DefineLabel();
            var doCondition = emit.DefineLabel();
            var doFinal = emit.DefineLabel();

            _loopStart.Push(doCondition);
            _loopEnd.Push(doFinal);

            emit.MarkLabel(doStart);
            doNode.Body.Accept(this);
            emit.MarkLabel(doCondition);
            doNode.Condition.Accept(this);
            var type = emit.GetTop();

            // Todo: Optimize float case
            if (type == typeof(float))
                emit.LdFloat(0f).Cgt();
            else if (typeof(TsObject).IsAssignableFrom(type))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            else if (type != typeof(bool) && type != typeof(int))
            {
                emit.Pop();
                _logger.Error("Invalid type for until condition", doNode.Condition.Position);
                return;
            }
            emit.BrFalse(doStart)
                .MarkLabel(doFinal);

            _loopStart.Pop();
            _loopEnd.Pop();
        }

        public void Visit(EndToken endToken)
        {
            //Empty statement. Means nothing in TS.
        }

        public void Visit(EnumNode enumNode)
        {
            var name = $"{_namespace}.{enumNode.Name}".TrimStart('.');
            var type = _module.DefineEnum(name, TypeAttributes.Public, typeof(long));
            
            foreach (var value in enumNode.Values)
            {
                _enums[name, value.Name] = value.Value;
                type.DefineLiteral(value.Name, value.Value);
            }

            type.CreateType();
        }

        public void Visit(EqualityNode equality)
        {
            equality.Left.Accept(this);
            var left = emit.GetTop();
            if (left == typeof(bool) || left == typeof(int))
            {
                emit.ConvertFloat();
                left = typeof(float);
            }
            equality.Right.Accept(this);
            var right = emit.GetTop();
            if (right == typeof(bool) || right == typeof(int))
            {
                emit.ConvertFloat();
                right = typeof(float);
            }

            TestEquality(equality.Op, left, right, equality.Position);
        }

        private void TestEquality(string op, Type left, Type right, TokenPosition pos)
        {
            if (left == typeof(float))
            {
                if (right == typeof(float))
                {
                    if (op == "==")
                        emit.Ceq();
                    else
                        emit.Neq();
                }
                else if (right == typeof(string))
                {
                    emit.Pop()
                        .Pop();
                    if (op == "==")
                        emit.LdBool(false);
                    else
                        emit.LdBool(true);
                }
                else if (typeof(TsObject).IsAssignableFrom(right))
                    emit.Call(GetOperator(op, left, right, pos));
                else
                    _logger.Error($"Cannot {op} types {left} and {right}", pos);
            }
            else if (left == typeof(string))
            {
                if (right == typeof(float))
                {
                    emit.Pop()
                        .Pop();
                    if (op == "==")
                        emit.LdBool(false);
                    else
                        emit.LdBool(true);
                }
                else if (right == typeof(string) || typeof(TsObject).IsAssignableFrom(right))
                    emit.Call(GetOperator(op, left, right, pos));
                else
                    _logger.Error($"Cannot {op} types {left} and {right}", pos);
            }
            else if (typeof(TsObject).IsAssignableFrom(left))
                emit.Call(GetOperator(op, left, right, pos));
            else
                _logger.Error($"Cannot {op} types {left} and {right}", pos);
        }

        public void Visit(ForNode forNode)
        {
            var forStart = emit.DefineLabel();
            var forCondition = emit.DefineLabel();
            var forIter = emit.DefineLabel();
            var forFinal = emit.DefineLabel();
            forNode.Initialize.Accept(this);

            _loopStart.Push(forIter);
            _loopEnd.Push(forFinal);

            emit.MarkLabel(forStart);
            LoadElementAsInt(forNode.Condition);
            emit.BrFalse(forFinal);
            forNode.Body.Accept(this);
            emit.MarkLabel(forIter);
            forNode.Increment.Accept(this);
            emit.Br(forStart);
            emit.MarkLabel(forFinal);

            _loopStart.Pop();
            _loopEnd.Pop();
        }

        public void Visit(FunctionCallNode functionCall)
        {
            //All methods callable from TaffyScript should have the same sig:
            //TsObject func_name(ITsInstance target, TsObject[] args);
            var callSite = functionCall.Callee;
            string name = null;
            if (callSite is VariableToken token)
                name = token.Name;
            else if (functionCall.Callee is MemberAccessNode memberAccess)
            {
                if (_resolver.TryResolveNamespace(memberAccess, out callSite, out var namespaceNode))
                {
                    //explicit namespace -> function call
                    //e.g. name.space.func_name();
                    if(namespaceNode.Children.TryGetValue(((VariableToken)callSite).Name, out var scriptSymbol))
                    {
                        CallScript(GetAssetNamespace(scriptSymbol), scriptSymbol, functionCall);
                        return;
                    }
                    else
                    {
                        _logger.Error($"Namespace {(GetAssetNamespace(namespaceNode) + "." + namespaceNode.Name).TrimStart('.')} does not define script {((VariableToken)callSite).Name}");
                        emit.Call(TsTypes.Empty);
                        return;
                    }
                }
                else
                {
                    //instance script call
                    //e.g. inst.script_name();
                    name = (memberAccess.Right as VariableToken)?.Name;
                    if (name == null)
                    {
                        _logger.Error("Invalid symbol for instance script", memberAccess.Right.Position);
                        emit.Call(TsTypes.Empty);
                        return;
                    }
                    memberAccess.Left.Accept(this);
                    CallEvent(name, false, functionCall, memberAccess.Right.Position);
                    return;
                }
            }
            else
            {
                MarkSequencePoint(functionCall.Position, functionCall.EndPosition);
                callSite.Accept(this);
                var top = emit.GetTop();
                if(!typeof(TsObject).IsAssignableFrom(top))
                {
                    _logger.Error("Invalid syntax detected", callSite.Position);
                    emit.Pop()
                        .Call(TsTypes.Empty);
                    return;
                }
                LoadFunctionArguments(functionCall.Arguments);
                emit.Call(typeof(TsObject).GetMethod("DelegateInvoke", new[] { typeof(TsObject[]) }));

                return;
            }

            if(!_table.Defined(name, out var symbol) || (symbol.Type == SymbolType.Script && symbol.Scope == SymbolScope.Member))
            {
                CallEvent(name, true, functionCall, callSite.Position);
                return;
            }

            if(symbol.Type == SymbolType.Variable)
            {
                MarkSequencePoint(functionCall.Position, functionCall.EndPosition);
                emit.LdLocal(_locals[symbol]);
                LoadFunctionArguments(functionCall.Arguments);
                emit.Call(typeof(TsObject).GetMethod("DelegateInvoke", new[] { typeof(TsObject[]) }));
                return;
            }
            else if (symbol.Type != SymbolType.Script)
            {
                _logger.Error("Tried to call something that wasn't a script. Check for name conflict", functionCall.Position);
                emit.Call(TsTypes.Empty);
                return;
            }
            var ns = GetAssetNamespace(symbol);

            CallScript(ns, symbol, functionCall);
        }

        private void CallScript(string ns, ISymbol scriptSymbol, FunctionCallNode functionCall)
        {
            if (!_methods.TryGetValue(ns, scriptSymbol.Name, out var method))
            {
                // Special case hack needed for getting the MethodInfo for weak methods before
                // their ImportNode has been hit.
                if (scriptSymbol is ImportLeaf leaf)
                {
                    var temp = _namespace;
                    _namespace = ns;
                    leaf.Node.Accept(this);
                    method = _methods[_namespace, leaf.Name];
                    _namespace = temp;
                }
                else
                {
                    method = StartMethod(scriptSymbol.Name, ns);
                    _pendingMethods.Add($"{ns}.{scriptSymbol.Name}".TrimStart('.'), functionCall.Position);
                }
            }

            MarkSequencePoint(functionCall.Position, functionCall.EndPosition);

            LoadFunctionArguments(functionCall.Arguments);

            emit.Call(method, 1, typeof(TsObject));
        }

        /// <summary>
        /// Loads the arguments from a <see cref="FunctionCallNode"/> to the eval stack, leaving the arg array on top.
        /// <para>
        /// If there are no args, loads null to avoid allocating a new array.
        /// </para>
        /// </summary>
        private void LoadFunctionArguments(List<ISyntaxElement> arguments)
        {
            if (arguments.Count > 0)
            {
                emit.LdInt(arguments.Count)
                    .NewArr(typeof(TsObject));

                for (var i = 0; i < arguments.Count; ++i)
                {
                    emit.Dup()
                        .LdInt(i);

                    arguments[i].Accept(this);
                    ConvertTopToObject(); 

                    emit.StElem(typeof(TsObject));
                }
            }
            else
                emit.LdNull();
        }

        private void CallEvent(string name, bool loadId, FunctionCallNode functionCall, TokenPosition start)
        {
            MarkSequencePoint(start, functionCall.EndPosition);

            if (loadId)
                LoadTarget();

            var top = emit.GetTop();

            if(top == typeof(string))
            {
                ConvertTopToObject();
                top = typeof(TsObject);
            }
            else if (typeof(TsObject).IsAssignableFrom(top))
                emit.Call(TsTypes.ObjectCasts[typeof(ITsInstance)]);
            else if (!typeof(ITsInstance).IsAssignableFrom(top))
            {
                _logger.Error("Invalid syntax detected", start);
                emit.Call(TsTypes.Empty);
                return;
            }
            
            emit.LdStr(name);
            LoadFunctionArguments(functionCall.Arguments);
            emit.Call(typeof(ITsInstance).GetMethod("Call"));
        }

        public void Visit(IfNode ifNode)
        {
            // If there is no else node, just let execution flow naturally.
            // Otherwise, branch to the end forcefully at the end of the if body.

            var ifFalse = emit.DefineLabel();
            var ifTrue = emit.DefineLabel();
            ifNode.Condition.Accept(this);
            var top = emit.GetTop();
            if (typeof(TsObject).IsAssignableFrom(top))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            emit.BrFalse(ifFalse);
            ifNode.ThenBrach.Accept(this);

            if (ifNode.ElseBranch != null)
                emit.Br(ifTrue);
            
            emit.MarkLabel(ifFalse);

            if (ifNode.ElseBranch != null)
            {
                ifNode.ElseBranch.Accept(this);
                emit.MarkLabel(ifTrue);
            }
        }

        public void Visit(ImportScriptNode import)
        {
            if (_methods.ContainsIndex(_namespace, import.ImportName))
                return;
            
            var args = new Type[import.Arguments.Count];
            _pendingMethods.Remove($"{_namespace}.{import.ImportName}".TrimStart('.'));

            for (var i = 0; i < args.Length; i++)
            {
                // Todo: make the import arguments have a position.
                if (!TsTypes.BasicTypes.TryGetValue(import.Arguments[i], out var argType))
                {
                    _logger.Error($"Could not import the function {import.ImportName} because one of the arguments was invalid {import.Arguments[i]}", import.Position);
                    StartMethod(import.ImportName, _namespace);
                    return;
                }

                args[i] = argType;
            }
            
            var owner = _typeParser.GetType(import.DotNetType);
            var method = GetMethodToImport(owner, import.MethodName, args);
            if(method == null)
            {
                _logger.Error($"Failed to find the import function {import.DotNetType + "." + import.MethodName}", import.Position);
                StartMethod(import.ImportName, _namespace);
                return;
            }

            if (method.GetCustomAttribute<TaffyScriptMethodAttribute>() != null)
            {
                if(IsMethodValid(method))
                {
                    _methods.Add(_namespace, import.ImportName, method);
                    SpecialImports.Write((byte)ImportType.Script);
                    SpecialImports.Write(':');
                    SpecialImports.Write(_namespace);
                    SpecialImports.Write(':');
                    SpecialImports.Write(import.ImportName);
                    SpecialImports.Write(':');
                    SpecialImports.WriteLine(import.DotNetType + "." + import.MethodName);
                    var init = Initializer;
                    var name = $"{_namespace}.{import.ImportName}".TrimStart('.');
                    init.Call(GetGlobalScripts)
                        .LdStr(name)
                        .LdNull()
                        .LdFtn(method)
                        .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                        .LdStr(name)
                        .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                        .Call(typeof(Dictionary<string, TsDelegate>).GetMethod("Add"));
                }
                else
                {
                    _logger.Warning($"Could not directly import method with the WeakMethod attribute: {method}. Please check the method signature.\n    ", import.Position);
                    GenerateWeakMethodForImport(method, import.ImportName);
                }
            }
            else
                GenerateWeakMethodForImport(method, import.ImportName);
        }

        private void LoadElementAsInt(ISyntaxElement element)
        {
            if (element is IConstantToken<float> number)
            {
                emit.LdInt((int)number.Value);
                return;
            }

            element.Accept(this);
            var top = emit.GetTop();

            if (top == typeof(float))
                emit.ConvertInt(false);
            else if(typeof(TsObject).IsAssignableFrom(top))
                emit.Call(TsTypes.ObjectCasts[typeof(int)]);
            else if(top != typeof(bool))
            {
                emit.Pop()
                    .LdInt(0);
                _logger.Error($"Expected an integer value, got {top.Name}", element.Position);
            }
        }

        public void Visit(LambdaNode lambda)
        {
            var bt = GetBaseType(_namespace);
            var owner = GetClosure(false);
            var closure = owner.Type.DefineMethod($"<{emit.Method.Name}>closure_{lambda.Scope.Remove(0, 5)}",
                                                  MethodAttributes.Assembly | MethodAttributes.HideBySig,
                                                  typeof(TsObject),
                                                  TsTypes.ArgumentTypes);
            _table.Enter(lambda.Scope);
            if (_closures == 0 && owner.Self is null && _table.Symbols.Any())
            {
                owner.Self = emit.DeclareLocal(owner.Type, "__0closure");
                emit.New(owner.Constructor, 0)
                    .StLocal(owner.Self);
            }
            var temp = emit;

            emit = new ILEmitter(closure, TsTypes.ArgumentTypes);
            DeclareLocals(false);
            ++_closures;
            var argumentArray = _argumentArray;
            _argumentArray = 1;

            ProcessScriptArguments(lambda.Arguments);

            lambda.Body.Accept(this);
            //Todo: Don't do this if last node was return stmt.
            if (!emit.TryGetTop(out _))
                emit.Call(TsTypes.Empty);

            emit.Ret();

            --_closures;
            _argumentArray = argumentArray;
            _table.Exit();

            emit = temp;
            if (_closures == 0)
            {
                if (owner.Self == null)
                    emit.New(owner.Constructor, 0);
                else
                    emit.LdLocal(owner.Self);
            }
            else
                emit.LdArg(0);

            emit.LdFtn(closure)
                .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .LdStr("lambda")
                .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }));
        }

        public void Visit(LocalsNode localsNode)
        {
            foreach (var local in localsNode.Locals)
            {
                var symbol = _table.Defined(local.Name);
                if (symbol is VariableLeaf leaf)
                {
                    if (leaf.IsCaptured)
                    {
                        if (_closures > 0)
                            emit.LdArg(0);
                        else
                            emit.LdLocal(_closure.Self);
                    }

                    if (local.HasValue)
                    {
                        local.Value.Accept(this);
                        ConvertTopToObject();
                    }
                    else
                        emit.Call(TsTypes.Empty);

                    if (leaf.IsCaptured)
                        emit.StFld(_closure.Fields[leaf.Name]);
                    else
                        emit.StLocal(_locals[symbol]);
                }
                else
                    _logger.Error("Tried to overwrite the symbol " + symbol.Name, local.Position);
            }
        }

        public void Visit(LogicalNode logical)
        {
            var end = emit.DefineLabel();
            logical.Left.Accept(this);
            var left = emit.GetTop();
            if (left == typeof(float))
                emit.LdFloat(.0f).Cgt();
            else if (typeof(TsObject).IsAssignableFrom(left))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            else if (left != typeof(bool))
                _logger.Error("Encountered invalid syntax", logical.Left.Position);

            if (logical.Op == "&&")
            {
                emit.Dup()
                    .BrFalse(end)
                    .Pop();
            }
            else
            {
                emit.Dup()
                    .BrTrue(end)
                    .Pop();
            }

            logical.Right.Accept(this);

            var right = emit.GetTop();
            if (right == typeof(float))
                emit.LdFloat(.0f).Cgt();
            else if (typeof(TsObject).IsAssignableFrom(right))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            else if (right != typeof(bool))
                _logger.Error("Encountered invalid syntax", logical.Right.Position);

            emit.MarkLabel(end);
        }

        public void Visit(MemberAccessNode memberAccess)
        {
            ISymbol enumSymbol;
            if(_resolver.TryResolveNamespace(memberAccess, out var resolved, out var namespaceNode))
            {
                if (resolved is MemberAccessNode member)
                {
                    if (member.Left is VariableToken enumType && namespaceNode.Children.TryGetValue(enumType.Name, out enumSymbol))
                        ProcessEnumAccess(member, enumType, enumSymbol);
                    else
                    {
                        emit.Call(TsTypes.Empty);
                        _logger.Error("Invalid member access syntax. Expected enum at the end of namespace", member.Position);
                    }
                }
                else if (resolved is VariableToken variable)
                {
                    if (namespaceNode.Children.TryGetValue(variable.Name, out var symbol))
                        ProcessVariableToken(variable, symbol);
                    else
                    {
                        emit.Call(TsTypes.Empty);
                        _logger.Error("Invalid token at the end of namespace access", resolved.Position);
                    }
                }
                else
                {
                    emit.Call(TsTypes.Empty);
                    _logger.Error("Invalid token at the end of namespace access.");
                }
                return;
            }
            //If left is enum name, find it in _enums.
            //Otherwise, Accept left, and call member access on right.
            if (memberAccess.Left is VariableToken enumVar && _table.Defined(enumVar.Name, out enumSymbol) && enumSymbol.Type == SymbolType.Enum)
            {
                ProcessEnumAccess(memberAccess, enumVar, enumSymbol);
                return;
            }
            if (memberAccess.Left is ReadOnlyToken read)
            {
                var right = memberAccess.Right as VariableToken;
                if(right is null)
                {
                    _logger.Error("Could not access readonly variable from readonly variable.");
                    emit.Call(TsTypes.Empty);
                    return;
                }
                switch(read.Name)
                {
                    case "global":
                        emit.LdFld(typeof(TsInstance).GetField("Global"));
                        break;
                    case "self":
                        LoadTarget();
                        break;
                    case "other":
                        emit.Call(typeof(TsInstance).GetMethod("get_Other"));
                        break;
                    default:
                        _logger.Error("Invalid syntax detected", right.Position);
                        emit.Call(TsTypes.Empty);
                        return;
                }
                emit.LdStr(right.Name)
                    .Call(typeof(ITsInstance).GetMethod("get_Item"));
            }
            else
            {
                memberAccess.Left.Accept(this);
                var left = emit.GetTop();
                if(left == typeof(string))
                {
                    ConvertTopToObject();
                    left = typeof(TsObject);
                }
                else if(!typeof(TsObject).IsAssignableFrom(left))
                {
                    _logger.Error($"Static member access is not yet supported.", memberAccess.Left.Position);
                    emit.Pop()
                        .Call(TsTypes.Empty);
                    return;
                }

                if (memberAccess.Right is VariableToken right)
                {
                    emit.LdStr(right.Name)
                        .Call(typeof(TsObject).GetMethod("MemberGet", new[] { typeof(string) }));
                }
                else if (memberAccess.Right is ReadOnlyToken readOnly)
                {
                    if (readOnly.Name != "self")
                    {
                        _logger.Error("Only the read only variable self can be accessed from an instance currently", readOnly.Position);
                        emit.Call(TsTypes.Empty);
                        return;
                    }
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"));
                }
                else
                    _logger.Error("Invalid syntax detected", memberAccess.Position);
            }
        }

        private void ProcessEnumAccess(MemberAccessNode memberAccess, VariableToken enumType, ISymbol enumSymbol)
        {
            if (memberAccess.Right is VariableToken enumValue)
            {
                // Todo: refactor Visit(EnumNode) to match this implementation.
                if (!_enums.TryGetValue(enumType.Name, enumValue.Name, out var value))
                {
                    var node = (SymbolNode)enumSymbol;
                    if (node.Children.TryGetValue(enumValue.Name, out enumSymbol) && enumSymbol is EnumLeaf leaf)
                    {
                        value = leaf.Value;
                        _enums[enumType.Name, leaf.Name] = value;
                    }
                    else
                        _logger.Error($"The enum {enumType.Name} does not declare value {enumValue.Name}", enumValue.Position);
                }
                emit.LdFloat(value);
            }
            else
            {
                _logger.Error("Invalid enum access", enumType.Position);
                emit.LdFloat(0f);
            }
        }

        public void Visit(MultiplicativeNode multiplicative)
        {
            multiplicative.Left.Accept(this);
            var left = emit.GetTop();
            if (left == typeof(bool) || left == typeof(int))
            {
                left = typeof(float);
                emit.ConvertFloat();
            }

            multiplicative.Right.Accept(this);
            var right = emit.GetTop();
            if (right == typeof(bool) || right == typeof(int))
            {
                right = typeof(float);
                emit.ConvertFloat();
            }

            if (left == typeof(float))
            {
                if (right == typeof(float))
                {
                    switch (multiplicative.Op)
                    {
                        case "*":
                            emit.Mul();
                            break;
                        case "/":
                            emit.Div();
                            break;
                        case "%":
                            emit.Rem();
                            break;
                    }
                }
                else if (right == typeof(string))
                    _logger.Error($"Cannot {multiplicative.Op} types {left} and {right}", multiplicative.Position);
                else if (typeof(TsObject).IsAssignableFrom(right))
                    emit.Call(GetOperator(multiplicative.Op, left, right, multiplicative.Position));
                else
                    _logger.Error($"Cannot {multiplicative.Op} types {left} and {right}", multiplicative.Position);
            }
            else if (left == typeof(string))
                _logger.Error($"Cannot {multiplicative.Op} types {left} and {right}", multiplicative.Position);
            else if (typeof(TsObject).IsAssignableFrom(left))
                emit.Call(GetOperator(multiplicative.Op, left, right, multiplicative.Position));
        }

        public void Visit(NamespaceNode namespaceNode)
        {
            var parent = _namespace;
            if(parent != "")
            {
                _table.AddSymbolToDefinitionLookup(_table.Current);
                _namespace += "." + namespaceNode.Name;
            }
            else
                _namespace = namespaceNode.Name;

            var count = _table.EnterNamespace(namespaceNode.Name);

            AcceptDeclarations(namespaceNode.Declarations);

            _table.Exit(count);

            if (parent != "")
                _table.RemoveSymbolFromDefinitionLookup(_table.Current);

            _namespace = parent;
        }

        public void Visit(NewNode newNode)
        {
            if (!_resolver.TryResolveType(newNode.TypeName, out var symbol))
            {
                _logger.Error("Tried to create type that doesn't exist", newNode.TypeName.Position);
                emit.Call(TsTypes.Empty);
                return;
            }

            if(symbol.Type == SymbolType.Object)
            {
                MarkSequencePoint(newNode.Position, newNode.EndPosition);
                var ctor = _assets.GetConstructor(symbol, newNode.Position);
                emit.LdInt(newNode.Arguments.Count)
                        .NewArr(typeof(TsObject));

                for (var i = 0; i < newNode.Arguments.Count; ++i)
                {
                    emit.Dup()
                        .LdInt(i);

                    newNode.Arguments[i].Accept(this);
                    ConvertTopToObject();

                    emit.StElem(typeof(TsObject));
                }

                emit.New(ctor, 1);
            }
            else
            {
                _logger.Error($"Tried to create an instance of something that wasn't an object: {newNode.TypeName}", newNode.Position);
                emit.Call(TsTypes.Empty);
            }
        }

        public void Visit(ObjectNode objectNode)
        {
            var name = $"{_namespace}.{objectNode.Name}".TrimStart('.');
            _table.Enter(objectNode.Name);

            var temp = _parent;
            if (objectNode.Inherits != null)
            {
                if(!_resolver.TryResolveType(objectNode.Inherits, out _parent))
                    _logger.Error($"Tried to inherit from non-existant type: {((ISyntaxToken)objectNode.Inherits).Name}", objectNode.Inherits.Position);
            }
            else
                _parent = null;

            Type parent = null;

            if (_parent != null)
                parent = _assets.GetDefinedType(_parent, objectNode.Inherits.Position);

            var info = _assets.GetObjectInfo(_table.Current, objectNode.Position);
            var type = (TypeBuilder)info.Type;

            var scripts = new List<MethodInfo>();

            _argumentArray = 1;

            for (var i = 0; i < objectNode.Scripts.Count; i++)
            {
                var script = objectNode.Scripts[i];
                MethodBuilder method = _assets.GetInstanceMethod(_table.Current, script.Name, script.Position) as MethodBuilder;

                ScriptStart(script.Name, method, TsTypes.ArgumentTypes);

                ProcessScriptArguments(script.Arguments);
                script.Body.Accept(this);
                BlockNode body = (BlockNode)script.Body;
                var last = body.Body.Count == 0 ? null : body.Body[body.Body.Count - 1];
                if (body.Body.Count == 0 || last.Type != SyntaxType.Return)
                {
                    emit.Call(TsTypes.Empty)
                        .Ret();
                }

                ScriptEnd();
                scripts.Add(method);
            }

            _assets.FinalizeType(info, _parent, scripts, objectNode.Position);

            if (parent != null && parent is TypeBuilder builder && !builder.IsCreated())
                _unresolvedTypes.Add(type, builder);
            else
                type.CreateType();

            _table.Exit();
            _parent = temp;
        }

        public void Visit(PostfixNode postfix)
        {
            if (postfix.Left is ArgumentAccessNode argument)
            {
                var secret = GetLocal();
                emit.LdArg(_argumentArray);
                LoadElementAsInt(argument.Index);
                emit.LdElemA(typeof(TsObject))
                    .Dup()
                    .LdObj(typeof(TsObject))
                    .Dup()
                    .StLocal(secret)
                    .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                    .StObj(typeof(TsObject))
                    .LdLocal(secret);
                FreeLocal(secret);
            }
            else if (postfix.Left is ArrayAccessNode array)
            {
                var value = GetLocal();
                var result = GetLocal();

                array.Left.Accept(this);
                var top = emit.GetTop();
                if (!typeof(TsObject).IsAssignableFrom(top))
                {
                    _logger.Error("Encountered invalid syntax", array.Position);
                    emit.Call(TsTypes.Empty);
                    return;
                }

                var secret = GetLocal(typeof(ITsInstance));

                if (CanBeArrayAccess(array))
                {
                    var isArray = emit.DefineLabel();
                    var end = emit.DefineLabel();

                    emit.Dup()
                        .Call(typeof(TsObject).GetMethod("get_Type"))
                        .LdInt((int)VariableType.Instance)
                        .Bne(isArray)
                        .Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdStr("get")
                        .LdInt(array.Arguments.Count)
                        .NewArr(typeof(TsObject));

                    var indeces = new LocalBuilder[array.Arguments.Count];
                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        indeces[i] = GetLocal();
                        emit.Dup()
                            .StLocal(indeces[i])
                            .StElem(typeof(TsObject));
                    }

                    emit.Call(typeof(ITsInstance).GetMethod("Call"))
                        .Dup()
                        .StLocal(value)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .StLocal(result)
                        .LdLocal(secret)
                        .LdStr("set")
                        .LdInt(indeces.Length + 1)
                        .NewArr(typeof(TsObject));

                    for (var i = 0; i < indeces.Length; i++)
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdLocal(indeces[i])
                            .StElem(typeof(TsObject));
                        FreeLocal(indeces[i]);
                    }
                    emit.Dup()
                        .LdInt(indeces.Length)
                        .LdLocal(result)
                        .StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop()
                        .LdLocal(value)
                        .Br(end)
                        .PushType(typeof(TsObject))
                        .MarkLabel(isArray);

                    Type[] argTypes;
                    var count = array.Arguments.Count - 1;
                    if (count > 2)
                    {
                        argTypes = new[] { typeof(int[]) };
                        emit.LdInt(count)
                            .NewArr(typeof(int));
                        for (var i = 0; i < count; i++)
                        {
                            emit.Dup()
                                .LdInt(i);
                            LoadElementAsInt(array.Arguments[i]);
                            emit.StElem(typeof(int));
                        }
                    }
                    else
                    {
                        argTypes = new Type[count];
                        for (var i = 0; i < count; i++)
                        {
                            argTypes[i] = typeof(int);
                            LoadElementAsInt(array.Arguments[i]);
                        }
                    }

                    emit.Call(typeof(TsObject).GetMethod("GetArray", argTypes));
                    LoadElementAsInt(array.Arguments[count]);

                    emit.LdElemA(typeof(TsObject))
                        .Dup()
                        .LdObj(typeof(TsObject))
                        .Dup()
                        .StLocal(value)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .StObj(typeof(TsObject))
                        .LdLocal(value)
                        .MarkLabel(end)
                        .PopTop();
                }
                else
                {
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdStr("get")
                        .LdInt(array.Arguments.Count)
                        .NewArr(typeof(TsObject));

                    var indeces = new LocalBuilder[array.Arguments.Count];
                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        indeces[i] = GetLocal();
                        emit.Dup()
                            .StLocal(indeces[i])
                            .StElem(typeof(TsObject));
                    }

                    emit.Call(typeof(ITsInstance).GetMethod("Call"))
                        .Dup()
                        .StLocal(value)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .StLocal(result)
                        .LdLocal(secret)
                        .LdStr("set")
                        .LdInt(indeces.Length + 1)
                        .NewArr(typeof(TsObject));

                    for (var i = 0; i < indeces.Length; i++)
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdLocal(indeces[i])
                            .StElem(typeof(TsObject));
                        FreeLocal(indeces[i]);
                    }
                    emit.Dup()
                        .LdInt(indeces.Length)
                        .LdLocal(result)
                        .StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop()
                        .LdLocal(value);
                }

                FreeLocal(result);
                FreeLocal(value);
                FreeLocal(secret);
            }
            else if (postfix.Left is VariableToken variable)
            {
                var secret = GetLocal();
                if (_table.Defined(variable.Name, out var symbol))
                {
                    var leaf = symbol as VariableLeaf;
                    if (leaf is null)
                        _logger.Error($"Cannot assign to the value {symbol.Name}", variable.Position);

                    if (leaf.IsCaptured)
                    {
                        if (_closures > 0)
                            emit.LdArg(0);
                        else
                            emit.LdLocal(_closure.Self);
                        emit.LdFldA(_closure.Fields[variable.Name]);
                    }
                    else
                        emit.LdLocalA(_locals[symbol]);

                    emit.Dup()
                        .LdObj(typeof(TsObject))
                        .Dup()
                        .StLocal(secret)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .StObj(typeof(TsObject))
                        .LdLocal(secret);
                }
                else
                {
                    SelfAccessSet(variable);
                    emit.Call(typeof(ITsInstance).GetMethod("GetMember"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .Call(typeof(ITsInstance).GetMethod("set_Item", new[] { typeof(string), typeof(TsObject) }))
                        .LdLocal(secret);
                }
                FreeLocal(secret);
            }
            else if (postfix.Left is MemberAccessNode member)
            {
                var secret = GetLocal();
                var value = member.Right as VariableToken;
                if (value is null)
                {
                    _logger.Error("Invalid member access", member.Position);
                    emit.Call(TsTypes.Empty);
                    return;
                }
                if (member.Left is ReadOnlyToken read)
                {
                    Func<ILEmitter> loadTarget = GetReadOnlyLoadFunc(read);

                    loadTarget()
                        .LdStr(value.Name)
                        .Call(typeof(ITsInstance).GetMethod("get_Item"))
                        .StLocal(secret)
                        .LdLocal(secret);
                    loadTarget()
                        .LdStr(value.Name)
                        .LdLocal(secret)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .Call(typeof(ITsInstance).GetMethod("set_Item"));
                }
                else
                {
                    member.Left.Accept(this);
                    var target = GetLocal();
                    emit.StLocal(target)
                        .LdLocal(target)
                        .LdStr(value.Name)
                        .Call(typeof(TsObject).GetMethod("MemberGet"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdLocal(target)
                        .LdStr(value.Name)
                        .LdLocal(secret)
                        .Call(GetOperator(postfix.Op, typeof(TsObject), postfix.Position))
                        .Call(typeof(TsObject).GetMethod("MemberSet", new[] { typeof(string), typeof(TsObject) }));

                    FreeLocal(target);
                }
                FreeLocal(secret);
            }
            else
                _logger.Error("Invalid syntax detected", postfix.Left.Position);
        }

        private Func<ILEmitter> GetReadOnlyLoadFunc(ReadOnlyToken read)
        {
            if (read.Name == "global")
                return () => emit.LdFld(typeof(TsInstance).GetField("Global"));
            else if (read.Name == "other")
                return () => emit.Call(typeof(TsInstance).GetMethod("get_Other"));
            else
                return () => LoadTarget();
        }

        public void Visit(PrefixNode prefix)
        {
            //Todo: Only load result if not parent != block
            if(prefix.Op != "++" && prefix.Op != "--")
            {
                prefix.Right.Accept(this);
                var top = emit.GetTop();
                if (prefix.Op == "-" && (top == typeof(float) || top == typeof(int) || top == typeof(bool)))
                    emit.Neg();
                else if (prefix.Op != "+")
                    emit.Call(GetOperator(prefix.Op, emit.GetTop(), prefix.Position));
                return;
            }

            if (prefix.Right is ArgumentAccessNode argument)
            {
                var secret = GetLocal();
                emit.LdArg(_argumentArray);
                LoadElementAsInt(argument.Index);
                emit.LdElemA(typeof(TsObject))
                    .Dup()
                    .Dup()
                    .LdObj(typeof(TsObject))
                    .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                    .StObj(typeof(TsObject))
                    .LdObj(typeof(TsObject));
                FreeLocal(secret);
            }
            else if (prefix.Right is ArrayAccessNode array)
            {
                var value = GetLocal();

                array.Left.Accept(this);
                var top = emit.GetTop();
                if (!typeof(TsObject).IsAssignableFrom(top))
                {
                    _logger.Error("Invalid syntax detected", array.Position);
                    emit.Call(TsTypes.Empty);
                    return;
                }

                var secret = GetLocal(typeof(ITsInstance));

                if (CanBeArrayAccess(array))
                {
                    var isArray = emit.DefineLabel();
                    var end = emit.DefineLabel();

                    emit.Dup()
                        .Call(typeof(TsObject).GetMethod("get_Type"))
                        .LdInt((int)VariableType.Instance)
                        .Bne(isArray)
                        .Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdStr("get")
                        .LdInt(array.Arguments.Count)
                        .NewArr(typeof(TsObject));

                    var indeces = new LocalBuilder[array.Arguments.Count];
                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        indeces[i] = GetLocal();
                        emit.Dup()
                            .StLocal(indeces[i])
                            .StElem(typeof(TsObject));
                    }

                    emit.Call(typeof(ITsInstance).GetMethod("Call"))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .StLocal(value)
                        .LdLocal(secret)
                        .LdStr("set")
                        .LdInt(indeces.Length + 1)
                        .NewArr(typeof(TsObject));

                    for (var i = 0; i < indeces.Length; i++)
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdLocal(indeces[i])
                            .StElem(typeof(TsObject));
                        FreeLocal(indeces[i]);
                    }
                    emit.Dup()
                        .LdInt(indeces.Length)
                        .LdLocal(value)
                        .StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop()
                        .LdLocal(value)
                        .Br(end)
                        .PushType(typeof(TsObject).MakePointerType())
                        .MarkLabel(isArray);

                    Type[] argTypes;
                    var count = array.Arguments.Count - 1;
                    if (count > 2)
                    {
                        argTypes = new[] { typeof(int[]) };
                        emit.LdInt(count)
                            .NewArr(typeof(int));
                        for (var i = 0; i < count; i++)
                        {
                            emit.Dup()
                                .LdInt(i);
                            LoadElementAsInt(array.Arguments[i]);
                            emit.StElem(typeof(int));
                        }
                    }
                    else
                    {
                        argTypes = new Type[count];
                        for (var i = 0; i < count; i++)
                        {
                            argTypes[i] = typeof(int);
                            LoadElementAsInt(array.Arguments[i]);
                        }
                    }

                    emit.Call(typeof(TsObject).GetMethod("GetArray", argTypes));
                    LoadElementAsInt(array.Arguments[count]);

                    emit.LdElemA(typeof(TsObject))
                        .Dup()
                        .Dup()
                        .LdObj(typeof(TsObject))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .StObj(typeof(TsObject))
                        .LdObj(typeof(TsObject))
                        .MarkLabel(end)
                        .PopTop();
                }
                else
                {
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdStr("get")
                        .LdInt(array.Arguments.Count)
                        .NewArr(typeof(TsObject));

                    var indeces = new LocalBuilder[array.Arguments.Count];
                    for (var i = 0; i < array.Arguments.Count; i++)
                    {
                        emit.Dup()
                            .LdInt(i);
                        array.Arguments[i].Accept(this);
                        ConvertTopToObject();
                        indeces[i] = GetLocal();
                        emit.Dup()
                            .StLocal(indeces[i])
                            .StElem(typeof(TsObject));
                    }

                    emit.Call(typeof(ITsInstance).GetMethod("Call"))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .StLocal(value)
                        .LdLocal(secret)
                        .LdStr("set")
                        .LdInt(indeces.Length + 1)
                        .NewArr(typeof(TsObject));

                    for (var i = 0; i < indeces.Length; i++)
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdLocal(indeces[i])
                            .StElem(typeof(TsObject));
                        FreeLocal(indeces[i]);
                    }
                    emit.Dup()
                        .LdInt(indeces.Length)
                        .LdLocal(value)
                        .StElem(typeof(TsObject))
                        .Call(typeof(ITsInstance).GetMethod("Call"))
                        .Pop()
                        .LdLocal(value);
                }

                FreeLocal(secret);
                FreeLocal(value);
            }
            else if (prefix.Right is VariableToken variable)
            {
                if (_table.Defined(variable.Name, out var symbol))
                {
                    var leaf = symbol as VariableLeaf;
                    if (leaf is null)
                        _logger.Error($"Cannot assign to the value {symbol.Name}", variable.Position);

                    if (leaf.IsCaptured)
                    {
                        if (_closures > 0)
                            emit.LdArg(0);
                        else
                            emit.LdLocal(_closure.Self);
                        emit.LdFldA(_closure.Fields[variable.Name]);
                    }
                    else
                        emit.LdLocalA(_locals[symbol]);

                    emit.Dup()
                        .Dup()
                        .LdObj(typeof(TsObject))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .StObj(typeof(TsObject))
                        .LdObj(typeof(TsObject));
                }
                else
                {
                    var secret = GetLocal();
                    SelfAccessSet(variable);
                    emit.Call(typeof(ITsInstance).GetMethod("get_Item"))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .Call(typeof(ITsInstance).GetMethod("set_Item"))
                        .LdLocal(secret);
                    FreeLocal(secret);
                }
            }
            else if (prefix.Right is MemberAccessNode member)
            {
                if (!(member.Right is VariableToken value))
                {
                    _logger.Error("Cannot assign to readonly value", member.Right.Position);
                    emit.Call(TsTypes.Empty);
                    return;
                }
                var secret = GetLocal();
                if (member.Left is ReadOnlyToken read)
                {
                    Func<ILEmitter> loadTarget = GetReadOnlyLoadFunc(read);

                    loadTarget()
                        .LdStr(value.Name);
                    loadTarget()
                        .LdStr(value.Name)
                        .Call(typeof(ITsInstance).GetMethod("get_Item"))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .Call(typeof(ITsInstance).GetMethod("set_Item"))
                        .LdLocal(secret);
                }
                else
                {
                    var target = GetLocal(typeof(ITsInstance));
                    member.Left.Accept(this);
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                        .StLocal(target)
                        .LdLocal(target)
                        .LdStr(value.Name)
                        .LdLocal(target)
                        .LdStr(value.Name)
                        .Call(typeof(ITsInstance).GetMethod("GetMember"))
                        .Call(GetOperator(prefix.Op, typeof(TsObject), prefix.Position))
                        .Dup()
                        .StLocal(secret)
                        .Call(typeof(ITsInstance).GetMethod("SetMember"))
                        .LdLocal(secret);

                    FreeLocal(target);
                }
                FreeLocal(secret);
            }
            else
                _logger.Error("Invalid syntax detected", prefix.Position);
        }

        public void Visit(ReadOnlyToken readOnlyToken)
        {
            switch(readOnlyToken.Name)
            {
                case "self":
                    LoadTarget();
                    break;
                case "other":
                    emit.Call(typeof(TsInstance).GetMethod("get_Other"));
                    break;
                case "argument_count":
                    if(_argumentCount == null)
                    {
                        _argumentCount = emit.DeclareLocal(typeof(float), "argument_count");
                        var isNull = emit.DefineLabel();
                        var end = emit.DefineLabel();
                        emit.LdArg(_argumentArray)
                            .Dup()
                            .BrFalse(isNull)
                            .LdLen()
                            .ConvertFloat()
                            .Br(end)
                            .MarkLabel(isNull)
                            .Pop()
                            .LdFloat(0)
                            .MarkLabel(end)
                            .StLocal(_argumentCount);
                    }
                    emit.LdLocal(_argumentCount);
                    break;
                case "global":
                    emit.LdFld(typeof(TsInstance).GetField("Global"));
                    break;
                case "pi":
                    emit.LdFloat((float)System.Math.PI);
                    break;
                case "noone":
                    _logger.Warning("The keyword noone is obsolete and will be deprecated next major release. Consider removing it", readOnlyToken.Position);
                    emit.LdFloat(-4f);
                    break;
                case "null":
                    emit.Call(TsTypes.Empty);
                    break;
                default:
                    _logger.Error($"Currently the readonly value {readOnlyToken.Name} is not implemented", readOnlyToken.Position);
                    break;
            }
        }

        public void Visit(RelationalNode relational)
        {
            relational.Left.Accept(this);
            var left = emit.GetTop();
            if (left == typeof(bool) || left == typeof(int))
            {
                emit.ConvertFloat();
                left = typeof(float);
            }
            else if(left == typeof(string))
            {
                emit.Call(typeof(string).GetMethod("GetHashCode"))
                    .ConvertFloat();
                left = typeof(float);
            }

            relational.Right.Accept(this);
            var right = emit.GetTop();
            if(right == typeof(bool) || right == typeof(int))
            {
                emit.ConvertFloat();
                right = typeof(float);
            }
            else if (right == typeof(string))
            {
                emit.Call(typeof(string).GetMethod("GetHashCode"))
                    .ConvertFloat();
                right = typeof(float);
            }

            if (left == typeof(float))
            {
                if (right == typeof(float))
                {
                    switch (relational.Op)
                    {
                        case "<":
                            emit.Clt();
                            break;
                        case "<=":
                            emit.Cle();
                            break;
                        case ">":
                            emit.Cgt();
                            break;
                        case ">=":
                            emit.Cge();
                            break;
                        default:
                            _logger.Error($"Cannot {relational.Op} types {left} and {right}", relational.Position);
                            break;
                    }
                }
                else if (typeof(TsObject).IsAssignableFrom(right))
                    emit.Call(GetOperator(relational.Op, left, right, relational.Position));
            }
            else if (typeof(TsObject).IsAssignableFrom(left))
                emit.Call(GetOperator(relational.Op, left, right, relational.Position));
            else
                _logger.Error("Invalid syntax detected", relational.Position);

            return;
        }

        public void Visit(RepeatNode repeat)
        {
            var secret = GetLocal(typeof(int));
            var count = GetLocal(typeof(int));
            var body = emit.DefineLabel();
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();
            _loopStart.Push(start);
            _loopEnd.Push(end);


            emit.LdInt(0)
                .StLocal(secret);
            LoadElementAsInt(repeat.Count);
            emit.StLocal(count)
                .MarkLabel(body)
                .LdLocal(secret)
                .LdLocal(count)
                .Bge(end);
            repeat.Body.Accept(this);
            emit.MarkLabel(start)
                .LdLocalA(secret)
                .Dup()
                .LdObj(typeof(int))
                .LdInt(1)
                .Add()
                .StObj(typeof(int))
                .Br(body)
                .MarkLabel(end);

            FreeLocal(secret);
        }

        public void Visit(ReturnNode returnNode)
        {
            if(returnNode.Result != null)
            {
                returnNode.Result.Accept(this);
                if (!emit.TryGetTop(out var returnType))
                    _logger.Error("Invalid return value", returnNode.Result.Position);
                else
                    ConvertTopToObject();
            }
            else
                emit.Call(TsTypes.Empty);

            emit.Ret();
        }

        public void Visit(RootNode root)
        {
            foreach(var child in root.CompilationUnits)
            {
                child.Accept(this);
            }
        }

        public void Visit(ScriptNode script)
        {
            var name = script.Name;
            var mb = StartMethod(name, _namespace);
            ScriptStart(name, mb, TsTypes.ArgumentTypes);
            _argumentArray = 0;

            //Process arguments
            ProcessScriptArguments(script.Arguments);

            script.Body.Accept(this);
            
            if (!emit.TryGetTop(out _))
                emit.Call(TsTypes.Empty);

            emit.Ret();

            ScriptEnd();

            name = $"{_namespace}.{name}".TrimStart('.');
            _pendingMethods.Remove(name);

            var init = Initializer;
            init.Call(GetGlobalScripts)
                .LdStr(name)
                .LdNull()
                .LdFtn(mb)
                .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .LdStr(name)
                .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                .Call(typeof(Dictionary<string, TsDelegate>).GetMethod("Add"));

            if (name == _entryPoint)
                GenerateEntryPoint(mb);
        }

        private void ProcessScriptArguments(List<VariableDeclaration> arguments)
        {
            if (arguments.Count > 0)
            {
                if(_isDebug)
                {
                    var minSize = 0;
                    for(var i = 0; i < arguments.Count; i++)
                    {
                        if (!arguments[i].HasValue)
                            minSize++;
                        else
                            break;
                    }
                    if(minSize > 0)
                    {
                        var fine = emit.DefineLabel();
                        var failed = emit.DefineLabel();
                        emit.LdArg(_argumentArray)
                            .LdNull()
                            .Beq(failed)
                            .LdArg(_argumentArray)
                            .LdLen()
                            .LdInt(minSize)
                            .Bge(fine)
                            .MarkLabel(failed)
                            .LdStr("Not enough arguments passed to script.")
                            .New(typeof(ArgumentException).GetConstructor(new[] { typeof(string) }))
                            .Throw()
                            .MarkLabel(fine);
                    }
                }

                emit.LdArg(_argumentArray);
                for (var i = 0; i < arguments.Count; i++)
                {
                    var arg = arguments[i];
                    if(!_table.Defined(arg.Name, out var symbol))
                    {
                        _logger.Error("Unknown exception occurred", arg.Position);
                        continue;
                    }
                    var leaf = symbol as VariableLeaf;
                    if (arg.HasValue)
                    {
                        var lte = emit.DefineLabel();
                        var end = emit.DefineLabel();
                        emit.Dup()
                            .LdNull()
                            .Beq(lte)
                            .Dup()
                            .LdLen()
                            .LdInt(i)
                            .Ble(lte)
                            .Dup()
                            .LdInt(i)
                            .LdElem(typeof(TsObject));

                        if (leaf.IsCaptured)
                        {
                            var secret = GetLocal();
                            emit.StLocal(secret);
                            if (_closures == 0)
                                emit.LdLocal(_closure.Self);
                            else
                                emit.LdArg(0);
                            emit.LdLocal(secret)
                                .StFld(_closure.Fields[leaf.Name]);

                            FreeLocal(secret);
                        }
                        else
                            emit.StLocal(_locals[symbol]);

                        emit.Br(end)
                            .MarkLabel(lte);
                        //Must be ConstantToken

                        if (leaf.IsCaptured)
                        {
                            if (_closures == 0)
                                emit.LdLocal(_closure.Self);
                            else
                                emit.LdArg(0);
                        }

                        arg.Value.Accept(this);
                        ConvertTopToObject();

                        if (leaf.IsCaptured)
                            emit.StFld(_closure.Fields[leaf.Name]);
                        else
                            emit.StLocal(_locals[symbol]);

                        emit.MarkLabel(end);
                    }
                    else
                    {
                        emit.Dup()
                            .LdInt(i)
                            .LdElem(typeof(TsObject));

                        if (leaf.IsCaptured)
                        {
                            var secret = GetLocal();
                            emit.StLocal(secret);
                            if (_closures == 0)
                                emit.LdLocal(_closure.Self);
                            else
                                emit.LdArg(0);

                            emit.LdLocal(secret)
                                .StFld(_closure.Fields[leaf.Name]);

                            FreeLocal(secret);
                        }
                        else
                            emit.StLocal(_locals[symbol]);
                    }
                }
                //Pops the last remaining reference to the arugment array from the stack.
                emit.Pop();
            }
        }

        public void Visit(ShiftNode shift)
        {
            shift.Left.Accept(this);
            var left = emit.GetTop();
            if (left == typeof(float) || left == typeof(int) || left == typeof(bool))
            {
                emit.ConvertLong(false);
                left = typeof(long);
            }
            else if (left == typeof(string))
                _logger.Error("Tried to shift a string", shift.Left.Position);
            LoadElementAsInt(shift.Right);
            var right = typeof(int);

            if (left == typeof(long))
            {
                emit.Call(GetOperator(shift.Op, left, right, shift.Position))
                    .ConvertFloat();
            }
            else if (typeof(TsObject).IsAssignableFrom(left))
            {
                 emit.Call(GetOperator(shift.Op, left, right, shift.Position));
            }
            else
                _logger.Error("Invalid syntax detected", shift.Position);
        }

        public void Visit(SwitchNode switchNode)
        {
            var end = emit.DefineLabel();
            _loopEnd.Push(end);

            switchNode.Value.Accept(this);
            var left = emit.GetTop();
            if(left == typeof(int) || left == typeof(bool))
            {
                emit.ConvertFloat();
                left = typeof(float);
            }

            int defaultLocation = -1;
            var labels = new Label[switchNode.Cases.Count + (switchNode.DefaultCase == null ? 0 : 1)];
            var bodies = new ISyntaxElement[labels.Length];
            for (int label = 0, i = 0; label < labels.Length; label++, i++)
            {
                labels[label] = emit.DefineLabel();
                if(label == switchNode.DefaultIndex)
                {
                    bodies[label] = switchNode.DefaultCase;
                    defaultLocation = label++;
                    continue;
                }
                if (i != switchNode.Cases.Count - 1)
                    emit.Dup();

                switchNode.Cases[i].Expression.Accept(this);
                var right = emit.GetTop();
                if (right == typeof(int) || right == typeof(bool))
                {
                    emit.ConvertFloat();
                    right = typeof(float);
                }
                TestEquality("==", left, right, switchNode.Cases[i].Expression.Position);
                var isFalse = emit.DefineLabel();
                emit.BrFalse(isFalse);

                if (i != switchNode.Cases.Count - 1)
                    emit.Pop(false);

                emit.Br(labels[label])
                    .MarkLabel(isFalse);

                bodies[label] = switchNode.Cases[i].Body;
            }

            if (defaultLocation != -1)
                emit.Br(labels[defaultLocation]);
            else
                emit.Br(end);

            for(var i = 0; i < labels.Length; i++)
            {
                emit.MarkLabel(labels[i]);
                bodies[i].Accept(this);
            }

            emit.MarkLabel(end);

            _loopEnd.Pop();
        }

        public void Visit(UsingsNode usings)
        {
            foreach (var ns in usings.Usings)
                _table.AddNamespaceToDefinitionLookup(ns.Namespace);

            AcceptDeclarations(usings.Declarations);
            _table.RemoveAllNamespacesFromDefinitionLookup();
        }

        public void Visit(VariableToken variableToken)
        {
            if (_table.Defined(variableToken.Name, out var symbol))
                ProcessVariableToken(variableToken, symbol);
            else
            {
                LoadTarget()
                    .LdStr(variableToken.Name)
                    .Call(typeof(ITsInstance).GetMethod("get_Item"));
            }
        }

        private void ProcessVariableToken(VariableToken variableToken, ISymbol variableSymbol)
        {
            var name = variableToken.Name;
            switch (variableSymbol.Type)
            {
                case SymbolType.Object:
                    var ns = GetAssetNamespace(variableSymbol);
                    if (ns != "")
                        name = $"{ns}.{name}";
                    emit.LdStr(name);
                    break;
                case SymbolType.Script:
                    if (variableSymbol.Scope == SymbolScope.Member)
                    {
                        // Todo: Consider forcing this to load the exact function.
                        //       That would make it so the function couldn't be changed during runtime,
                        //       but of course it makes execution faster.
                        LoadTarget()
                            .LdStr(name)
                            .Call(typeof(ITsInstance).GetMethod("GetDelegate"))
                            .New(TsTypes.Constructors[typeof(TsDelegate)]);
                    }
                    else
                    {
                        ns = GetAssetNamespace(variableSymbol);
                        if (!_methods.TryGetValue(ns, name, out var method))
                        {
                            method = StartMethod(name, ns);
                            _pendingMethods.Add($"{ns}.{name}".TrimStart('.'), variableToken.Position);
                        }
                        emit.LdNull()
                            .LdFtn(method)
                            .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                            .LdStr(variableSymbol.Name)
                            .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                            .New(TsTypes.Constructors[typeof(TsDelegate)]);
                    }
                    break;
                case SymbolType.Variable:
                    if (_locals.TryGetValue(variableSymbol, out var local))
                    {
                        emit.LdLocal(local);
                    }
                    else if (variableSymbol is VariableLeaf leaf && leaf.IsCaptured)
                    {
                        var field = _closure.Fields[variableSymbol.Name];

                        if (_closures == 0)
                            emit.LdLocal(_closure.Self);
                        else
                            emit.LdArg(0);

                        emit.LdFld(field);
                    }
                    else
                        _logger.Error($"Tried to reference a non-existant variable {variableToken.Name}", variableToken.Position);
                    break;
                default:
                    _logger.Error($"Currently cannot reference indentifier {variableSymbol.Type} by it's raw value.", variableToken.Position);
                    break;
            }
        }

        public void Visit(WhileNode whileNode)
        {
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();

            _loopStart.Push(start);
            _loopEnd.Push(end);

            emit.MarkLabel(start);
            whileNode.Condition.Accept(this);
            var top = emit.GetTop();
            if (typeof(TsObject).IsAssignableFrom(top))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            else if(top == typeof(float))
                emit.ConvertInt(false);
            else if(top != typeof(bool) && top != typeof(int))
            {
                _logger.Error("Expected a value that can be true or false", whileNode.Condition.Position);
                emit.Pop();
                return;
            }

            emit.BrFalse(end);
            whileNode.Body.Accept(this);
            emit.Br(start)
                .MarkLabel(end);

            _loopStart.Pop();
            _loopEnd.Pop();
        }

        public void Visit(ImportObjectNode importNode)
        {
            // This method must not use `emit` becuse it might be called
            // in the middle of a methods generation.

            ImportObjectLeaf leaf = (ImportObjectLeaf)_table.Defined(importNode.ImportName);
            if (leaf.HasImportedObject)
                return;

            leaf.HasImportedObject = true;

            var importType = _typeParser.GetType(importNode.DotNetType);

            if (importType.IsAbstract || importType.IsEnum || !importType.IsClass)
                _logger.Error($"Could not import the type {importType.FullName}. Imported types must be concrete and currently must be a class.", importNode.Position);

            var ns = GetAssetNamespace(leaf);
            var name = $"{ns}.{importNode.ImportName}".TrimStart('.');

            if(typeof(ITsInstance).IsAssignableFrom(importType))
            {
                var ctor = importType.GetConstructor(new[] { typeof(TsObject[]) });
                if(ctor is null)
                {
                    _logger.Error($"Could not import type that inherits from ITsInstance because it does not have a valid constructor", importNode.Position);
                    return;
                }

                leaf.Constructor = ctor;
                var bt = GetBaseType(ns);
                var wrappedCtor = bt.DefineMethod($"New_0{importNode.ImportName}", MethodAttributes.Public | MethodAttributes.Static, typeof(ITsInstance), new[] { typeof(TsObject[]) });
                var wctr = new ILEmitter(wrappedCtor, new[] { typeof(TsObject[]) });
                wctr.LdArg(0)
                    .New(ctor)
                    .Ret();

                _assets.AddObjectInfo(leaf, new ObjectInfo(importType, null, null, ctor, null));

                Initializer.Call(typeof(TsReflection).GetMethod("get_Constructors"))
                           .LdStr(name)
                           .LdNull()
                           .LdFtn(wrappedCtor)
                           .New(typeof(Func<TsObject[], ITsInstance>).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                           .Call(typeof(Dictionary<string, Func<TsObject[], ITsInstance>>).GetMethod("Add", new[] { typeof(string), typeof(Func<TsObject[], ITsInstance>) }));

                SpecialImports.Write((byte)ImportType.Object);
                SpecialImports.Write(':');
                SpecialImports.Write(ns);
                SpecialImports.Write(':');
                SpecialImports.Write(importNode.ImportName);
                SpecialImports.Write(':');
                SpecialImports.WriteLine(importType.Name);
                return;
            }
            var parent = importNode.WeaklyTyped ? typeof(ObjectWrapper) : typeof(Object);
            var type = _module.DefineType(name, TypeAttributes.Public, parent, new[] { typeof(ITsInstance) });

            var source = type.DefineField("_source", importType, FieldAttributes.Private);
            var objectType = type.DefineProperty("ObjectType", PropertyAttributes.None, typeof(string), null);
            var getObjectType = type.DefineMethod("get_ObjectType",
                                                  MethodAttributes.Public |
                                                      MethodAttributes.HideBySig |
                                                      MethodAttributes.NewSlot |
                                                      MethodAttributes.SpecialName |
                                                      MethodAttributes.Virtual |
                                                      MethodAttributes.Final,
                                                  typeof(string),
                                                  Type.EmptyTypes);
            var gen = getObjectType.GetILGenerator();
            gen.Emit(OpCodes.Ldstr, name);
            gen.Emit(OpCodes.Ret);

            objectType.SetGetMethod(getObjectType);

            var methodFlags = MethodAttributes.Public |
                              MethodAttributes.HideBySig |
                              MethodAttributes.NewSlot |
                              MethodAttributes.Virtual;

            var callMethod = type.DefineMethod("Call", methodFlags, typeof(TsObject), new[] { typeof(string), typeof(TsObject[]) });
            var getMemberMethod = type.DefineMethod("GetMember", methodFlags, typeof(TsObject), new[] { typeof(string) });
            var setMemberMethod = type.DefineMethod("SetMember", methodFlags, typeof(void), new[] { typeof(string), typeof(TsObject) });
            var tryGetDelegateMethod = type.DefineMethod("TryGetDelegate", methodFlags, typeof(bool), new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
            leaf.TryGetDelegate = tryGetDelegateMethod;

            var call = new ILEmitter(callMethod, new[] { typeof(string), typeof(TsObject[]) });
            var getm = new ILEmitter(getMemberMethod, new[] { typeof(string) });
            var setm = new ILEmitter(setMemberMethod, new[] { typeof(string), typeof(TsObject) });
            var tryd = new ILEmitter(tryGetDelegateMethod, new[] { typeof(string), typeof(TsDelegate) });
            tryGetDelegateMethod.DefineParameter(2, ParameterAttributes.Out, "del");
            var paramsAttribute = new CustomAttributeBuilder(typeof(ParamArrayAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
            callMethod.DefineParameter(2, ParameterAttributes.None, "args").SetCustomAttribute(paramsAttribute);

            var castTo = type.DefineMethod("op_Explicit",
                                           MethodAttributes.Public |
                                               MethodAttributes.Static |
                                               MethodAttributes.HideBySig |
                                               MethodAttributes.SpecialName,
                                           type,
                                           new[] { typeof(TsObject) });

            var cast = new ILEmitter(castTo, new[] { typeof(TsObject) });
            cast.LdArg(0)
                .Call(typeof(TsObject).GetMethod("get_WeakValue"))
                .CastClass(type)
                .Ret();

            var castFrom = type.DefineMethod("op_Implicit",
                                             MethodAttributes.Public |
                                                 MethodAttributes.Static |
                                                 MethodAttributes.HideBySig |
                                                 MethodAttributes.SpecialName,
                                             typeof(TsObject),
                                             new Type[] { type });
            cast = new ILEmitter(castFrom, new Type[] { type });
            cast.LdArg(0)
                .New(TsTypes.Constructors[typeof(ITsInstance)])
                .Ret();

            var wrapCtor = type.DefineConstructor(MethodAttributes.Public |
                                                      MethodAttributes.HideBySig |
                                                      MethodAttributes.SpecialName |
                                                      MethodAttributes.RTSpecialName,
                                                  CallingConventions.HasThis,
                                                  new[] { importType });
            
            var wrap = new ILEmitter(wrapCtor, new[] { type, importType });
            wrap.LdArg(0);

            if (importNode.WeaklyTyped)
                wrap.CallBase(typeof(ObjectWrapper).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null));
            else
                wrap.CallBase(typeof(Object).GetConstructor(Type.EmptyTypes));

            wrap.LdArg(0)
                .LdArg(1)
                .StFld(source)
                .Ret();

            var instanceMethods = new Dictionary<string, MethodInfo>();

            var writeOnly = new List<string>();
            var readOnly = new List<string>();
            var members = typeof(ObjectWrapper).GetField("_members", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var del = getm.DeclareLocal(typeof(TsDelegate), "del");

            Label getError;

            if (!importNode.AutoImplement)
            {
                foreach(var fld in importNode.Fields)
                {
                    var field = importType.GetMember(fld.ExternalName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
                    if(field == null)
                    {
                        _logger.Error($"Could not find the Field or Property {fld.ExternalName} on the type {importType.FullName}", fld.Position);
                        continue;
                    }
                    switch(field)
                    {
                        case FieldInfo fi:
                            AddFieldToTypeWrapper(fi, type, fld.ImportName, fld.Position, source, getm, setm);
                            break;
                        case PropertyInfo pi:
                            AddPropertyToTypeWrapper(pi, type, fld.ImportName, fld.Position, source, getm, setm, writeOnly, readOnly);
                            break;
                    }
                }

                foreach(var importMethod in importNode.Methods)
                {
                    var args = new Type[importMethod.Arguments.Count];
                    for(var i = 0; i < args.Length; i++)
                    {
                        if (TsTypes.BasicTypes.TryGetValue(importMethod.Arguments[i], out var argType))
                            args[i] = argType;
                        else
                        {
                            _logger.Error($"Could not import the method {importMethod.ExternalName} because one of the arguments was invalid {importMethod.Arguments[i]}", importMethod.Position);
                            args[i] = typeof(TsObject);
                        }
                    }

                    var method = importType.GetMethod(importMethod.ExternalName, BindingFlags.Public | BindingFlags.Instance, null, args, null);
                    AddMethodToTypeWrapper(method, methodFlags, type, importMethod.ImportName, importMethod.Position, source, call, tryd, readOnly, instanceMethods);
                }

                var ctorArgNames = importNode.Constructor.Arguments;
                var ctorArgs = new Type[ctorArgNames.Count];
                for (var i = 0; i < ctorArgs.Length; i++)
                {
                    if (TsTypes.BasicTypes.TryGetValue(ctorArgNames[i], out var argType))
                        ctorArgs[i] = argType;
                    else
                        _logger.Error($"Could not import the constructor for the type {name} because one of the arguments was invalid {ctorArgs[i]}", importNode.Constructor.Position);
                }

                var ctor = importType.GetConstructor(ctorArgs);
                if (ctor is null)
                    _logger.Error($"Could not find ctor on the type {name} with the arguments {string.Join(", ", ctorArgNames)}", importNode.Constructor.Position);
                else
                    AddConstructorToTypeWrapper(ctor, type, source, importNode.WeaklyTyped, leaf);
            }
            else
            {
                var publicMembers = importType.GetMembers(BindingFlags.Public | BindingFlags.Instance);
                Func<string, string> transformName;
                switch(importNode.Casing)
                {
                    case ImportCasing.Camel:
                        transformName = StringUtils.ConvertToCamelCase;
                        break;
                    case ImportCasing.Pascal:
                        transformName = StringUtils.ConvertToPascalCase;
                        break;
                    case ImportCasing.Snake:
                        transformName = StringUtils.ConvertToSnakeCase;
                        break;
                    default:
                        transformName = (s) => s;
                        break;
                }
                bool foundIndexer = false;
                bool foundConstructor = false;
                Type memberType;
                var validMethods = new HashSet<MethodInfo>();
                var ignoreMethods = new HashSet<string>();
                foreach(var publicMember in publicMembers)
                {
                    switch(publicMember)
                    {
                        case FieldInfo fi:
                            memberType = fi.FieldType;
                            if (typeof(ITsInstance).IsAssignableFrom(memberType))
                                memberType = typeof(ITsInstance);
                            if (typeof(TsObject).IsAssignableFrom(memberType) || TsTypes.ObjectCasts.ContainsKey(memberType))
                                AddFieldToTypeWrapper(fi, type, transformName(fi.Name), importNode.Position, source, getm, setm);
                            break;
                        case PropertyInfo pi:
                            memberType = pi.PropertyType;
                            if (typeof(ITsInstance).IsAssignableFrom(memberType))
                                memberType = typeof(ITsInstance);
                            if (typeof(TsObject).IsAssignableFrom(memberType) || TsTypes.ObjectCasts.ContainsKey(memberType))
                            {
                                if((pi.CanRead && pi.GetMethod.GetParameters().Length > 0) || 
                                    (pi.CanWrite && pi.SetMethod.GetParameters().Length > 1))
                                {
                                    if (foundIndexer)
                                        continue;
                                    if(pi.CanRead && IsMethodSupported(pi.GetMethod))
                                    {
                                        foundIndexer = true;
                                        AddMethodToTypeWrapper(pi.GetMethod, methodFlags, type, "get", importNode.Position, source, call, tryd, readOnly, instanceMethods);
                                    }
                                    if(pi.CanWrite && IsMethodSupported(pi.SetMethod))
                                    {
                                        foundIndexer = true;
                                        AddMethodToTypeWrapper(pi.SetMethod, methodFlags, type, "set", importNode.Position, source, call, tryd, readOnly, instanceMethods);
                                    }
                                }
                                else
                                {
                                    AddPropertyToTypeWrapper(pi, type, transformName(pi.Name), importNode.Position, source, getm, setm, writeOnly, readOnly);
                                }
                                if(pi.CanRead)
                                {
                                    validMethods.Remove(pi.GetMethod);
                                    ignoreMethods.Add(pi.GetMethod.Name);
                                }
                                if(pi.CanWrite)
                                {
                                    validMethods.Remove(pi.SetMethod);
                                    ignoreMethods.Add(pi.SetMethod.Name);
                                }
                            }
                            break;
                        case MethodInfo mi:
                            if (!ignoreMethods.Contains(mi.Name) && IsMethodSupported(mi))
                            {
                                if (!importNode.IncludeStandard && TsTypes.StandardMethods.Contains(mi.Name))
                                    break;
                                validMethods.Add(mi);
                                ignoreMethods.Add(mi.Name);
                            }
                            break;
                        case ConstructorInfo ci:
                            if(!foundConstructor && AreMethodParametersSupported(ci.GetParameters()))
                            {
                                AddConstructorToTypeWrapper(ci, type, source, importNode.WeaklyTyped, leaf);
                                foundConstructor = true;
                            }
                            break;
                    }
                }
                foreach(var mi in validMethods)
                    AddMethodToTypeWrapper(mi, methodFlags, type, transformName(mi.Name), importNode.Position, source, call, tryd, readOnly, instanceMethods);

                if (!foundConstructor)
                    _logger.Error($"No valid constructor was found for the imported type {importType}", importNode.Position);
            }

            if (importNode.WeaklyTyped)
            {
                if (readOnly.Count > 0)
                {
                    var setError = setm.DefineLabel();
                    for (var i = 0; i < readOnly.Count; i++)
                    {
                        setm.LdArg(1)
                            .LdStr(readOnly[i])
                            .Call(GetOperator("==", typeof(string), typeof(string), new TokenPosition(0, 0, 0, null)))
                            .BrTrue(setError);
                    }
                    setm.LdArg(0)
                        .LdFld(members)
                        .LdArg(1)
                        .LdArg(2)
                        .Call(typeof(Dictionary<string, TsObject>).GetMethod("set_Item"))
                        .Ret()
                        .MarkLabel(setError)
                        .LdStr($"Member {{0}} on type {name} is readonly")
                        .LdArg(1)
                        .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                        .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                        .Throw();
                }
                else
                {
                    setm.LdArg(0)
                        .LdFld(members)
                        .LdArg(1)
                        .LdArg(2)
                        .Call(typeof(Dictionary<string, TsObject>).GetMethod("set_Item"))
                        .Ret();
                }
                var member = call.DeclareLocal(typeof(TsObject), "member");
                var callError = call.DefineLabel();
                call.LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(member)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalse(callError)
                    .LdLocal(member)
                    .Call(typeof(TsObject).GetMethod("get_Type"))
                    .LdInt((int)VariableType.Delegate)
                    .Bne(callError)
                    .LdLocal(member)
                    .Call(typeof(TsObject).GetMethod("GetDelegate"))
                    .LdArg(2)
                    .Call(typeof(TsDelegate).GetMethod("Invoke", new[] { typeof(TsObject[]) }))
                    .Ret()
                    .MarkLabel(callError)
                    .LdStr($"The type {name} does not define a script called {{0}}")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();

                member = tryd.DeclareLocal(typeof(TsObject), "member");
                var tryFail = tryd.DefineLabel();
                tryd.LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(member)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalse(tryFail)
                    .LdLocal(member)
                    .Call(typeof(TsObject).GetMethod("get_Type"))
                    .LdInt((int)VariableType.Delegate)
                    .Bne(tryFail)
                    .LdArg(2)
                    .LdLocal(member)
                    .Call(typeof(TsObject).GetMethod("GetDelegate"))
                    .StIndRef()
                    .LdBool(true)
                    .Ret()
                    .MarkLabel(tryFail)
                    .LdArg(2)
                    .LdNull()
                    .StIndRef()
                    .LdBool(false)
                    .Ret();

                getError = getm.DefineLabel();
                var getMember = getm.DefineLabel();
                member = getm.DeclareLocal(typeof(TsObject), "member");
                foreach (var wo in writeOnly)
                {
                    getm.LdArg(1)
                        .LdStr(wo)
                        .Call(GetOperator("==", typeof(string), typeof(string), new TokenPosition(0, 0, 0, null)))
                        .BrTrue(getError);
                }
                getm.LdArg(0)
                    .LdArg(1)
                    .LdLocalA(del)
                    .Call(tryGetDelegateMethod, 3, typeof(bool))
                    .BrFalse(getMember)
                    .LdLocal(del)
                    .New(TsTypes.Constructors[typeof(TsDelegate)])
                    .Ret()
                    .MarkLabel(getMember)
                    .LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(member)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalse(getError)
                    .LdLocal(member)
                    .Ret()
                    .MarkLabel(getError)
                    .LdStr($"Member with the name {{0}} is readonly or doesn't exist on the type {name}")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();
            }
            else
            {
                setm.LdStr($"Member {{0}} on type {name} is readonly or doesn't exist")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();

                call.LdStr($"The type {name} does not define a script called {{0}}.")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();

                tryd.LdArg(2)
                    .LdNull()
                    .StIndRef()
                    .LdBool(false)
                    .Ret();

                getError = getm.DefineLabel();
                getm.LdArg(0)
                    .LdArg(1)
                    .LdLocalA(del)
                    .Call(tryGetDelegateMethod, 3, typeof(bool))
                    .BrFalse(getError)
                    .LdLocal(del)
                    .New(TsTypes.Constructors[typeof(TsDelegate)])
                    .Ret()
                    .MarkLabel(getError)
                    .LdStr($"Couldn't find member with the name {{0}} on the type {name}")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();
            }

            var getDelegate = type.DefineMethod("GetDelegate", methodFlags, typeof(TsDelegate), new[] { typeof(string) });
            var mthd = new ILEmitter(getDelegate, new[] { typeof(string) });
            del = mthd.DeclareLocal(typeof(TsDelegate), "del");
            getError = mthd.DefineLabel();

            mthd.LdArg(0)
                .LdArg(1)
                .LdLocalA(del)
                .Call(tryGetDelegateMethod, 3, typeof(bool))
                .BrFalse(getError)
                .LdLocal(del)
                .Ret()
                .MarkLabel(getError)
                .LdStr($"The type {name} does not define a script called {{0}}")
                .LdArg(1)
                .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                .Throw();

            var indexer = type.DefineProperty("Item", PropertyAttributes.None, typeof(TsObject), new[] { typeof(string) });
            var indexGet = type.DefineMethod("get_Item",
                                             MethodAttributes.Public |
                                                 MethodAttributes.HideBySig |
                                                 MethodAttributes.NewSlot |
                                                 MethodAttributes.SpecialName |
                                                 MethodAttributes.Virtual |
                                                 MethodAttributes.Final,
                                             typeof(TsObject),
                                             new[] { typeof(string) });
            mthd = new ILEmitter(indexGet, new[] { typeof(string) });
            mthd.LdArg(0)
                .LdArg(1)
                .Call(getMemberMethod, 2, typeof(TsObject))
                .Ret();

            indexer.SetGetMethod(indexGet);

            var indexSet = type.DefineMethod("set_Item",
                                             MethodAttributes.Public |
                                                 MethodAttributes.HideBySig |
                                                 MethodAttributes.NewSlot |
                                                 MethodAttributes.SpecialName |
                                                 MethodAttributes.Virtual |
                                                 MethodAttributes.Final,
                                             typeof(void),
                                             new[] { typeof(string), typeof(TsObject) });
            mthd = new ILEmitter(indexSet, new[] { typeof(string), typeof(TsObject) });
            mthd.LdArg(0)
                .LdArg(1)
                .LdArg(2)
                .Call(setMemberMethod, 3, typeof(void))
                .Ret();

            indexer.SetSetMethod(indexSet);
            var defaultMember = new CustomAttributeBuilder(typeof(DefaultMemberAttribute).GetConstructor(new[] { typeof(string) }), new[] { "Item" });
            type.SetCustomAttribute(defaultMember);

            type.CreateType();
            _assets.AddObjectInfo(leaf, new ObjectInfo(type, null, tryGetDelegateMethod, null, leaf.ImportObject.WeaklyTyped ? members : null));
        }

        private void AddFieldToTypeWrapper(FieldInfo field, TypeBuilder type, string importName, TokenPosition position, FieldInfo source, ILEmitter getm, ILEmitter setm)
        {
            var next = getm.DefineLabel();
            getm.LdArg(1)
                .LdStr(importName)
                .Call(GetOperator("==", typeof(string), typeof(string), position))
                .BrFalse(next)
                .LdArg(0)
                .LdFld(source)
                .LdFld(field);

            ConvertTopToObject(getm);
            getm.Ret()
                .MarkLabel(next);

            next = setm.DefineLabel();
            setm.LdArg(1)
                .LdStr(importName)
                .Call(GetOperator("==", typeof(string), typeof(string), position))
                .BrFalse(next)
                .LdArg(0)
                .LdFld(source)
                .LdArg(2)
                .StFld(field)
                .Ret()
                .MarkLabel(next);
        }

        private void AddPropertyToTypeWrapper(PropertyInfo property, 
                                              TypeBuilder type, 
                                              string importName, 
                                              TokenPosition position, 
                                              FieldInfo source, 
                                              ILEmitter getm, 
                                              ILEmitter setm, 
                                              List<string> writeOnly,
                                              List<string> readOnly)
        {
            if (!property.CanRead)
                writeOnly.Add(importName);
            else
            {
                var next = getm.DefineLabel();
                getm.LdArg(1)
                    .LdStr(importName)
                    .Call(GetOperator("==", typeof(string), typeof(string), position))
                    .BrFalse(next)
                    .LdArg(0)
                    .LdFld(source)
                    .Call(property.GetMethod);
                ConvertTopToObject(getm);
                getm.Ret()
                    .MarkLabel(next);
            }

            if (!property.CanWrite)
                readOnly.Add(importName);
            else
            {
                var next = setm.DefineLabel();
                setm.LdArg(1)
                    .LdStr(importName)
                    .Call(GetOperator("==", typeof(string), typeof(string), position))
                    .BrFalse(next)
                    .LdArg(0)
                    .LdFld(source)
                    .LdArg(2)
                    .Call(property.SetMethod)
                    .Ret()
                    .MarkLabel(next);
            }
        }

        private void AddMethodToTypeWrapper(MethodInfo method, 
                                            MethodAttributes weakFlags, 
                                            TypeBuilder type, 
                                            string importName, 
                                            TokenPosition methodPosition, 
                                            FieldInfo source, 
                                            ILEmitter call, 
                                            ILEmitter tryd,
                                            List<string> readOnly,
                                            Dictionary<string, MethodInfo> methods)
        {
            var weakMethod = type.DefineMethod(importName, weakFlags, typeof(TsObject), TsTypes.ArgumentTypes);
            weakMethod.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var weak = new ILEmitter(weakMethod, TsTypes.ArgumentTypes);
            weak.LdArg(0)
                .LdFld(source);

            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                weak.LdArg(1)
                    .LdInt(i);

                if (typeof(TsObject).IsAssignableFrom(parameters[i].ParameterType) || parameters[i].ParameterType == typeof(object))
                    weak.LdElem(typeof(TsObject));
                else
                {
                    weak.LdElem(typeof(TsObject))
                        .Call(TsTypes.ObjectCasts[parameters[i].ParameterType]);
                }
            }

            weak.Call(method);
            if (method.ReturnType == typeof(void))
                weak.Call(TsTypes.Empty);
            else if (TsTypes.Constructors.TryGetValue(method.ReturnType, out var objCtor))
                weak.New(objCtor);
            else if (!typeof(TsObject).IsAssignableFrom(method.ReturnType))
                _logger.Error($"Imported method { method.Name } had an invalid return type {method.ReturnType}");
            weak.Ret();

            var next = call.DefineLabel();
            call.LdArg(1)
                .LdStr(importName)
                .Call(GetOperator("==", typeof(string), typeof(string), methodPosition))
                .BrFalse(next)
                .LdArg(0)
                .LdArg(2)
                .Call(weakMethod, 2, typeof(TsObject))
                .Ret()
                .MarkLabel(next);

            next = tryd.DefineLabel();
            tryd.LdArg(1)
                .LdStr(importName)
                .Call(GetOperator("==", typeof(string), typeof(string), methodPosition))
                .BrFalse(next)
                .LdArg(2)
                .LdArg(0)
                .LdFtn(weakMethod)
                .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .LdStr(importName)
                .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                .StIndRef()
                .LdBool(true)
                .Ret()
                .MarkLabel(next);

            readOnly.Add(importName);
            methods.Add(importName, weakMethod);
        }

        private void AddConstructorToTypeWrapper(ConstructorInfo ctor, TypeBuilder type, FieldInfo source, bool isWeaklyTyped, ImportObjectLeaf leaf)
        {
            var createMethod = type.DefineConstructor(MethodAttributes.Public |
                                                          MethodAttributes.HideBySig |
                                                          MethodAttributes.SpecialName |
                                                          MethodAttributes.RTSpecialName,
                                                      CallingConventions.Standard,
                                                      new[] { typeof(TsObject[]) });

            var mthd = new ILEmitter(createMethod, new[] { typeof(TsObject[]) });
            mthd.LdArg(0);

            var ctorParams = ctor.GetParameters();
            for (var i = 0; i < ctorParams.Length; i++)
            {
                mthd.LdArg(1)
                    .LdInt(i);

                if (typeof(TsObject).IsAssignableFrom(ctorParams[i].ParameterType) || ctorParams[i].ParameterType == typeof(object))
                    mthd.LdElem(typeof(TsObject));
                else
                {
                    mthd.LdElem(typeof(TsObject))
                        .Call(TsTypes.ObjectCasts[ctorParams[i].ParameterType]);
                }
            }
            mthd.New(ctor)
                .StFld(source)
                .LdArg(0);
            if (isWeaklyTyped)
                mthd.CallBase(typeof(ObjectWrapper).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null));
            else
                mthd.CallBase(typeof(Object).GetConstructor(Type.EmptyTypes));

            mthd.Ret();

            leaf.Constructor = createMethod;

            var wrappedCtor = type.DefineMethod("Create", MethodAttributes.Public | MethodAttributes.Static, type, new[] { typeof(TsObject[]) });
            var wctr = new ILEmitter(wrappedCtor, new[] { typeof(TsObject[]) });

            wctr.LdArg(0)
                .New(createMethod, 1)
                .Ret();
            
            Initializer.Call(typeof(TsReflection).GetMethod("get_Constructors"))
                       .LdStr(type.FullName)
                       .LdNull()
                       .LdFtn(wrappedCtor)
                       .New(typeof(Func<TsObject[], ITsInstance>).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                       .Call(typeof(Dictionary<string, Func<TsObject[], ITsInstance>>).GetMethod("Add", new[] { typeof(string), typeof(Func<TsObject[], ITsInstance>) }));
        }

        private bool IsMethodSupported(MethodInfo method)
        {
            var type = method.ReturnType;
            if(type != typeof(void))
            {
                if (typeof(ITsInstance).IsAssignableFrom(type))
                    type = typeof(ITsInstance);
                if (!typeof(TsObject).IsAssignableFrom(type) && !TsTypes.Constructors.ContainsKey(type))
                    return false;
            }
            var parameters = method.GetParameters();
            return AreMethodParametersSupported(method.GetParameters());
        }

        private bool AreMethodParametersSupported(ParameterInfo[] parameters)
        {
            foreach (var parameter in parameters)
            {
                var type = parameter.ParameterType;
                if (typeof(ITsInstance).IsAssignableFrom(type))
                    type = typeof(ITsInstance);
                if (!typeof(TsObject).IsAssignableFrom(type) && !TsTypes.Constructors.ContainsKey(type))
                    return false;
            }
            return true;
        }

#endregion
    }
}