using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TaffyScript;
using TaffyScript.Collections;
using TaffyScriptCompiler.DotNet;
using TaffyScriptCompiler.Syntax;

namespace TaffyScriptCompiler.Backend
{
    /// <summary>
    /// Generates an assembly based off of an abstract syntax tree.
    /// </summary>
    internal partial class MsilWeakCodeGen : ISyntaxElementVisitor
    {
        #region Constants

        /// <summary>
        /// Special file name that will be stored in the output assembly manifest that contains a list of imported methods with the <see cref="WeakMethodAttribute"/>.
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

        private SymbolTable _table;

        /// <summary>
        /// Row=Namespace, Col=MethodName, Value=MethodInfo
        /// </summary>
        private readonly LookupTable<string, string, MethodInfo> _methods = new LookupTable<string, string, MethodInfo>();
        private readonly LookupTable<string, string, long> _enums = new LookupTable<string, string, long>();
        private readonly BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.Static;
        private readonly Dictionary<string, TypeBuilder> _baseTypes = new Dictionary<string, TypeBuilder>();

        /// <summary>
        /// Keeps a list of any errors encountered during the compile.
        /// </summary>
        private List<Exception> _errors = new List<Exception>();

        /// <summary>
        /// Keeps a list of any warnings the compiler generates.
        /// </summary>
        private List<string> _warnings = new List<string>();

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
        /// Determines whether the compiler is currently in a script.
        /// </summary>
        private bool _inGlobalScript;

        private bool _inInstanceScript;

        /// <summary>
        /// Determines if the current element should emit an address if possible.
        /// </summary>
        private bool _needAddress = false;

        /// <summary>
        /// Contains a set of LocalBuilders that were created by the compiler in an effort to reuse variables when possible.
        /// </summary>
        private Dictionary<Type, Stack<LocalBuilder>> _secrets = new Dictionary<Type, Stack<LocalBuilder>>();

        private string _resolveNamespace = "";

        private LocalBuilder _argumentCount = null;

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
                    var name = $"{asm}.{asm.Replace('.', '_')}_Initializer";
                    var type = _module.DefineType(name, TypeAttributes.Public);
                    _initializer = new ILEmitter(type.DefineMethod("Initialize", MethodAttributes.Public | MethodAttributes.Static, typeof(void), Type.EmptyTypes), Type.EmptyTypes);
                    _baseTypes.Add(name, type);
                }
                return _initializer;
            }
        }

        private MethodInfo GetGlobalScripts { get; } = typeof(TsInstance).GetMethod("get_GlobalScripts");

        #endregion

        #region Public

        /// <summary>
        /// Initializes a new code generator that will emit weakly typed MSIL.
        /// </summary>
        /// <param name="table">The symbols defined for this code generator.</param>
        /// <param name="config">The build config used when creating the final assembly.</param>
        public MsilWeakCodeGen(SymbolTable table, BuildConfig config)
        {
            _table = table;
            _isDebug = config.Mode == CompileMode.Debug;
            _asmName = new AssemblyName(System.IO.Path.GetFileName(config.Output));
            _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.Save);
            _asm.DefineVersionInfoResource(config.Product, config.Version, config.Company, config.Copyright, config.Trademark);
            _entryPoint = config.EntryPoint;

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

            // Initialize all specified references.
            foreach (var asm in config.References.Select(s => _assemblyLoader.LoadAssembly(s)))
            {
                if(asm.GetCustomAttribute<WeakLibraryAttribute>() != null)
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
            attrib = new CustomAttributeBuilder(typeof(WeakLibraryAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            _asm.SetCustomAttribute(attrib);
        }

        public CompilerResult CompileTree(ISyntaxTree tree)
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

            if(output == ".exe")
            {
                var init = Initializer;
                foreach (var asm in _assemblyLoader.LoadedAssemblies.Values.Where(a => a.GetCustomAttribute<WeakLibraryAttribute>() != null))
                {
                    var name = asm.GetName().Name;
                    init.Call(asm.GetType($"{name}.{name.Replace('.', '_')}_Initializer").GetMethod("Initialize"));
                }
            }

            try
            {
                tree.Root.Accept(this);
            }
            catch(Exception e)
            {
                _errors.Add(new CompileException($"The compiler encountered an error. Please report it.\n    Exception: {e}"));
            }

            foreach (var pending in _pendingMethods)
                _errors.Add(new CompileException($"Could not find function {pending.Key} {pending.Value}"));

            if (_errors.Count != 0)
                return new CompilerResult(_errors);

            Initializer.Ret();

            //Finalize any types that were created.
            foreach (var type in _baseTypes.Values)
                type.CreateType();

            //Write any special imports to the module manifest.
            if (_specialImports != null)
            {
                _specialImports.Flush();
                _module.DefineManifestResource(SpecialImportsFileName, _stream, ResourceAttributes.Public);
            }

            _asm.Save(_asmName.Name + output);

            //Dispose any dynamic resources.
            if (_specialImports != null)
            {
                _specialImports.Dispose();
                _stream.Dispose();
                _specialImports = null;
                _stream = null;
            }

            return new CompilerResult(_asm, System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _asmName.Name + output));
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
                if (type.GetCustomAttribute<WeakObjectAttribute>() != null)
                {
                    var count = _table.EnterNamespace(type.Namespace);
                    _table.TryCreate(type.Name, SymbolType.Object);
                    _table.Exit(count);
                }
                else if(type.IsEnum)
                {
                    ProcessEnum(type);
                }
                else if(type.GetCustomAttribute<WeakBaseTypeAttribute>() != null)
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
            if(_table.TryCreate(name, SymbolType.Enum))
            {
                _table.Enter(name);
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
            var resources = asm.GetManifestResourceNames();
            if (resources.Contains(SpecialImportsFileName))
            {
                using (var stream = asm.GetManifestResourceStream(SpecialImportsFileName))
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var input = line.Split(':');
                            var external = input[2];
                            var owner = external.Remove(external.LastIndexOf('.'));
                            var methodName = external.Substring(owner.Length + 1);
                            var type = _typeParser.GetType(owner);
                            var method = GetMethodToImport(type, methodName, new[] { typeof(TsInstance), typeof(TsObject[]) });
                            var count = _table.EnterNamespace(input[0]);
                            _table.AddLeaf(input[1], SymbolType.Script, SymbolScope.Global);
                            _table.Exit(count);
                            _methods[input[0], input[1]] = method;
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
            if (method.ReturnType != typeof(TsObject))
                return false;

            var args = method.GetParameters();
            if (args.Length != 2 || args[0].ParameterType != typeof(TsInstance) ||  args[1].ParameterType != typeof(TsObject[]))
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
                    _errors.Add(new NameConflictException($"Function with name {name} is already defined by {m?.GetModule().ToString() ?? "<Unknown Module>"}"));
                return m;
            }
            if (!_table.Defined(name, out var symbol) || symbol.Type != SymbolType.Script)
            {
                _errors.Add(new CompileException($"Tried to call an undefined function: {name}"));
                return null;
            }
            var mb = GetBaseType(GetAssetNamespace(symbol)).DefineMethod(name, MethodAttributes.Public | MethodAttributes.Static, typeof(TsObject), new[] { typeof(TsInstance), typeof(TsObject[]) });
            mb.DefineParameter(1, ParameterAttributes.None, "__target_");
            mb.DefineParameter(2, ParameterAttributes.None, "__args_");
            _methods.Add(ns, name, mb);
            return mb;
        }

        private TypeBuilder GetBaseType(string ns)
        {
            if (!_baseTypes.TryGetValue(ns, out var type))
            {
                var name = $"{ns}.BasicType";
                if (name.StartsWith("."))
                    name = name.TrimStart('.');
                type = _module.DefineType(name, TypeAttributes.Public);
                var attrib = new CustomAttributeBuilder(typeof(WeakBaseTypeAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
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
            var emit = new ILEmitter(mb, new[] { typeof(TsInstance), typeof(TsObject[]) });
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
                if (paramTypes[i] == typeof(object))
                {
                    emit.LdElem(typeof(TsObject))
                        .Box(typeof(TsObject));
                }
                else if (paramTypes[i] != typeof(TsObject))
                {
                    emit.LdElemA(typeof(TsObject))
                        .Call(TsTypes.ObjectCasts[paramTypes[i]]);
                }
                else
                    emit.LdElem(typeof(TsObject));
            }
            emit.Call(method);
            if (method.ReturnType == typeof(void))
                emit.Call(TsTypes.Empty);
            else if (TsTypes.Constructors.TryGetValue(method.ReturnType, out var ctor))
                emit.New(ctor);
            else if (method.ReturnType != typeof(TsObject))
                _errors.Add(new InvalidProgramException($"Imported method {importName} had an invalid return type {method.ReturnType}."));

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
                .LdNull()
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
                SyntaxType.Import,
                SyntaxType.Namespace,
                SyntaxType.Object,
                SyntaxType.Script
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
                    _errors.Add(new CompileException($"Operator {op} does not exist {pos}."));
                    return null;
                }
                method = type.GetMethod(name, _methodFlags, null, new[] { type }, null);
                if (method == null)
                    _errors.Add(new CompileException($"No operator function is defined for the operator {op} and the type {type} {pos}."));
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
                    //This doesn't specify a token position, but it also should never be encountered.
                    _errors.Add(new CompileException($"Operator {op} does not exist {pos}."));
                    return null;
                }

                var argTypes = new Type[] { left, right };
                method = left.GetMethod(opName, _methodFlags, null, argTypes, null) ??
                         right.GetMethod(opName, _methodFlags, null, argTypes, null);
                if (method == null)
                    _errors.Add(new CompileException($"No operator function is defined for the operator {op} and the types {left} and {right} {pos}."));
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
        /// Loads the address of an element if possible, otherwise it loads the value.
        /// </summary>
        /// <param name="element"></param>
        private void GetAddressIfPossible(ISyntaxElement element)
        {
            if (element.Type == SyntaxType.Variable || element.Type == SyntaxType.ArrayAccess || element.Type == SyntaxType.ReadOnlyValue ||
                element.Type == SyntaxType.ArgumentAccess)
            {
                _needAddress = true;
                element.Accept(this);
                _needAddress = false;
            }
            else
                element.Accept(this);
        }

        /// <summary>
        /// Calls a <see cref="TsObject"/> instance method that takes 0 arguments.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="pos"></param>
        private void CallInstanceMethod(MethodInfo method, TokenPosition pos)
        {
            var top = emit.GetTop();
            if (top == typeof(TsObject))
            {
                var secret = GetLocal();
                emit.StLocal(secret)
                    .LdLocalA(secret)
                    .Call(method);
                FreeLocal(secret);
            }
            else if (top == typeof(TsObject).MakePointerType())
                emit.Call(method);
            else
                _errors.Add(new CompileException($"Something went wrong {pos}"));
        }

        /// <summary>
        /// If the compiler is in debug mode, marks a sequence point in the IL stream.
        /// <para>
        /// Currently this is only called before methods, and it makes the console show better information when an exception is thrown.
        /// </para>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="startLine"></param>
        /// <param name="startColumn"></param>
        /// <param name="endLine"></param>
        /// <param name="endColumn"></param>
        private void MarkSequencePoint(string file, int startLine, int startColumn, int endLine, int endColumn)
        {
            if(_isDebug && file != null)
            {
                if(!_documents.TryGetValue(file, out var writer))
                {
                    writer = _module.DefineDocument(file, Guid.Empty, Guid.Empty, Guid.Empty);
                    _documents.Add(file, writer);
                }
                emit.MarkSequencePoint(writer, startLine, startColumn, endLine, endColumn);
            }
        }

        private void ScriptStart(string scriptName, MethodBuilder method, Type[] args)
        {
            emit = new ILEmitter(method, args);
            _table.Enter(scriptName);
            var locals = new List<ISymbol>(_table.Symbols);
            _table.Exit();
            foreach(var local in locals)
            {
                //Raise local variables up one level.
                _table.AddChild(local);
                _locals.Add(local, emit.DeclareLocal(typeof(TsObject), local.Name));
            }
        }

        private void ScriptEnd()
        {
            foreach (var local in _locals.Keys)
                _table.Undefine(local.Name);
            _locals.Clear();
            _secrets.Clear();
            _secret = 0;
            _argumentCount = null;
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

        private ISyntaxElement ResolveNamespace(MemberAccessNode node)
        {
            if (node.Left is ISyntaxToken token && _table.Defined(token.Text, out var symbol) && symbol.Type == SymbolType.Namespace)
            {
                _table.Enter(symbol.Name);
                _resolveNamespace = symbol.Name;
                return node.Right;
            }
            else if (node.Left is MemberAccessNode)
            {
                var ns = new Stack<ISyntaxToken>();
                var result = node.Right;
                var start = node;
                while (node.Left is MemberAccessNode member)
                {
                    node = member;
                    if (node.Right is ISyntaxToken id)
                        ns.Push(id);
                    else
                        return start;
                }

                if (node.Left is ISyntaxToken left)
                    ns.Push(left);
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {node.Left.Position}"));

                var sb = new System.Text.StringBuilder();
                var iterations = 0;
                while (ns.Count > 0)
                {
                    var top = ns.Pop();
                    sb.Append(top.Text);
                    sb.Append(".");
                    if (_table.Defined(top.Text, out symbol) && symbol.Type == SymbolType.Namespace)
                    {
                        _table.Enter(top.Text);
                        iterations++;
                    }
                    else
                    {
                        _table.Exit(iterations);
                        return start;
                    }
                }
                _resolveNamespace = sb.ToString().TrimEnd('.');
                return result;
            }
            else
                return node;
        }

        private bool TryResolveNamespace(MemberAccessNode node, out ISyntaxElement resolved)
        {
            resolved = default(ISyntaxElement);
            if (node.Left is ISyntaxToken token && _table.Defined(token.Text, out var symbol) && symbol.Type == SymbolType.Namespace)
            {
                _table.Enter(symbol.Name);
                _resolveNamespace = symbol.Name;
                resolved = node.Right;
                return true;
            }
            else if (node.Left is MemberAccessNode)
            {
                var ns = new Stack<ISyntaxToken>();
                resolved = node.Right;
                var start = node;
                while (node.Left is MemberAccessNode member)
                {
                    node = member;
                    if (node.Right is ISyntaxToken id)
                        ns.Push(id);
                    else
                        return false;
                }

                if (node.Left is ISyntaxToken left)
                    ns.Push(left);
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {node.Left.Position}"));

                var sb = new System.Text.StringBuilder();
                var iterations = 0;
                while (ns.Count > 0)
                {
                    var top = ns.Pop();
                    sb.Append(top.Text);
                    sb.Append(".");
                    if (_table.Defined(top.Text, out symbol) && symbol.Type == SymbolType.Namespace)
                    {
                        _table.Enter(top.Text);
                        iterations++;
                    }
                    else
                    {
                        _table.Exit(iterations);
                        return false;
                    }
                }
                _resolveNamespace = sb.ToString().TrimEnd('.');
                return true;
            }
            else
                return false;
        }

        private void UnresolveNamespace()
        {
            if (_resolveNamespace == "")
                return;
            _table.ExitNamespace(_resolveNamespace);
            _resolveNamespace = "";
        }

        private SymbolTable CopyTable(SymbolTable table)
        {
            var copy = new SymbolTable();
            foreach (var symbol in table.Symbols)
                copy.AddChild(symbol);

            return copy;
        }

        private void CopyTable(SymbolTable src, SymbolTable dest, TokenPosition position)
        {
            foreach (var symbol in src.Symbols)
            {
                if (!dest.AddChild(symbol))
                    _warnings.Add($"Encountered name conflict {position}");
            }
        }

        private void TryCopyTable(SymbolTable src, SymbolTable dest)
        {
            foreach(var symbol in src.Symbols)
            {
                dest.AddChild(symbol);
            }
        }

        private void AcceptDeclarations(ISyntaxNode block)
        {
            foreach(var child in block.Children)
            {
                if (!_declarationTypes.Contains(child.Type))
                    _errors.Add(new CompileException($"Encountered invalid declaration {child.Position}"));
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

        #endregion

        #region Visitor

        public void Visit(AdditiveNode additive)
        {
            additive.Left.Accept(this);
            var left = emit.GetTop();
            if(left == typeof(bool))
            {
                emit.ConvertFloat();
                left = typeof(float);
            }
            additive.Right.Accept(this);
            var right = emit.GetTop();
            if (right == typeof(bool))
            {
                emit.ConvertFloat();
                right = typeof(float);
            }

            if (left == typeof(float))
            {
                if (right == typeof(float))
                {
                    if (additive.Value == "+")
                        emit.Add();
                    else
                        emit.Sub();
                }
                else if (right == typeof(string))
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right} {additive.Position}"));
                    return;
                }
                else if (right == typeof(TsObject))
                    emit.Call(GetOperator(additive.Value, left, right, additive.Position));
                else
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right} {additive.Position}"));
                    return;
                }
            }
            else if(left == typeof(string))
            {
                if (additive.Value != "+")
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right} {additive.Position}"));
                    return;
                }
                if (right == typeof(float))
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right} {additive.Position}"));
                    return;
                }
                else if(right == typeof(string))
                    emit.Call(typeof(string).GetMethod("Concat", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null));
                else if(right == typeof(TsObject))
                    emit.Call(GetOperator(additive.Value, left, right, additive.Position));
            }
            else if(left == typeof(TsObject))
            {
                var method = GetOperator(additive.Value, left, right, additive.Position);
                emit.Call(method);
            }
        }

        public void Visit(ArgumentAccessNode argumentAccess)
        {
            emit.LdArg(1);
            if(argumentAccess.Index is IConstantToken<float> index)
            {
                emit.LdInt((int)index.Value);
            }
            else if(!TryLoadElementAsInt(argumentAccess.Index))
                _errors.Add(new CompileException($"Invalid argument access {argumentAccess.Position}"));


            if (_needAddress)
                emit.LdElemA(typeof(TsObject));
            else
                emit.LdElem(typeof(TsObject));
        }

        public void Visit(ArrayAccessNode arrayAccess)
        {
            var address = _needAddress;
            _needAddress = false;
            GetAddressIfPossible(arrayAccess.Left);
            CallInstanceMethod(TsTypes.ObjectCasts[arrayAccess.Children.Count == 2 ? typeof(TsObject[]) : typeof(TsObject[][])], arrayAccess.Position);
            LoadElementAsInt(arrayAccess.Children[1]);

            if(arrayAccess.Children.Count == 2)
            {
                if (address)
                    emit.LdElemA(typeof(TsObject));
                else
                    emit.LdElem(typeof(TsObject));
            }
            else
            {
                emit.LdElem(typeof(TsObject[]));
                LoadElementAsInt(arrayAccess.Children[2]);

                if (address)
                    emit.LdElemA(typeof(TsObject));
                else
                    emit.LdElem(typeof(TsObject));
            }
        }

        public void Visit(ArrayLiteralNode arrayLiteral)
        {
            emit.LdInt(arrayLiteral.Children.Count)
                .NewArr(typeof(TsObject));
            for (var i = 0; i < arrayLiteral.Children.Count; i++)
            {
                emit.Dup()
                    .LdInt(i);

                arrayLiteral.Children[i].Accept(this);
                var type = emit.GetTop();
                if (type != typeof(TsObject))
                    emit.New(TsTypes.Constructors[type]);
                emit.StElem(typeof(TsObject));
            }
            emit.New(TsTypes.Constructors[typeof(TsObject[])]);
        }

        public void Visit(AssignNode assign)
        {
            if(assign.Value != "=")
            {
                ProcessAssignExtra(assign);
                return;
            }

            if(assign.Left is ArgumentAccessNode arg)
            {
                GetAddressIfPossible(arg);
                assign.Right.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(TsObject))
                    emit.New(TsTypes.Constructors[top]);
                emit.StObj(typeof(TsObject));
            }
            else if (assign.Left is ArrayAccessNode array)
            {
                //Here we have to resize the array if needed, so more work needs to be done.
                GetAddressIfPossible(array.Left);
                if (emit.GetTop() == typeof(TsObject))
                {
                    var secret = GetLocal();
                    emit.StLocal(secret)
                        .LdLocalA(secret);
                    FreeLocal(secret);
                }

                var argTypes = new Type[array.Children.Count];
                for (var i = 1; i < array.Children.Count; i++)
                {
                    LoadElementAsInt(array.Children[i]);
                    argTypes[i - 1] = typeof(int);
                }

                assign.Right.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(TsObject))
                    emit.New(TsTypes.Constructors[top]);
                argTypes[argTypes.Length - 1] = emit.GetTop();
                emit.Call(typeof(TsObject).GetMethod("ArraySet", argTypes));
            }
            else if(assign.Left is ListAccessNode list)
            {
                LoadElementAsInt(list.Left);
                LoadElementAsInt(list.Right);
                assign.Right.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(TsObject))
                    emit.New(TsTypes.Constructors[top]);
                emit.Call(typeof(DsList).GetMethod("DsListSet", new[] { typeof(int), typeof(int), typeof(TsObject) }));
            }
            else if(assign.Left is GridAccessNode grid)
            {
                LoadElementAsInt(grid.Left);
                LoadElementAsInt(grid.X);
                LoadElementAsInt(grid.Y);
                assign.Right.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(TsObject))
                    emit.New(TsTypes.Constructors[top]);
                emit.Call(typeof(DsGrid).GetMethod("DsGridSet"));
            }
            else if(assign.Left is MapAccessNode map)
            {
                LoadElementAsInt(map.Left);
                map.Right.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(TsObject))
                    emit.New(TsTypes.Constructors[top]);
                assign.Right.Accept(this);
                top = emit.GetTop();
                if (top != typeof(TsObject))
                    emit.New(TsTypes.Constructors[top]);
                emit.Call(typeof(DsMap).GetMethod("DsMapReplace"));
            }
            else if (assign.Left is VariableToken variable)
            {
                //Check if the variable is a local variable.
                //If not, then it MUST be a member var.
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        _errors.Add(new CompileException($"Cannot assign to the value {symbol.Name} {variable.Position}"));

                    assign.Right.Accept(this);
                    var result = emit.GetTop();
                    if (result != typeof(TsObject))
                        emit.New(TsTypes.Constructors[result]);
                    emit.StLocal(_locals[symbol]);
                }
                else
                {
                    emit.LdArg(0)
                        .LdStr(variable.Text);
                    assign.Right.Accept(this);
                    var result = emit.GetTop();
                    if (result != typeof(TsObject))
                        emit.New(TsTypes.Constructors[result]);
                    emit.Call(typeof(TsInstance).GetMethod("set_Item", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(TsObject) }, null));
                }
            }
            else if (assign.Left is MemberAccessNode member)
            {
                if (member.Left is ReadOnlyToken token)
                {
                    if (member.Right is VariableToken right)
                    {
                        switch (token.Text)
                        {
                            case "global":
                                emit.LdFld(typeof(TsInstance).GetField("Global"));
                                break;
                            case "self":
                                emit.LdArg(0);
                                break;
                            default:
                                _errors.Add(new CompileException($"Cannot access member on non-global readonly value {token.Position}"));
                                return;
                        }
                        emit.LdStr(right.Text);
                        assign.Right.Accept(this);
                        var top = emit.GetTop();
                        if (top != typeof(TsObject))
                            emit.New(TsTypes.Constructors[top]);
                        emit.Call(typeof(TsInstance).GetMethod("set_Item"));
                    }
                    else
                        _errors.Add(new CompileException($"Cannot access readonly value from global {member.Right.Position}"));
                }
                else
                {
                    GetAddressIfPossible(member.Left);
                    var top = emit.GetTop();
                    if (top == typeof(TsObject))
                    {
                        var secret = GetLocal();
                        emit.StLocal(secret)
                            .LdLocalA(secret);
                        FreeLocal(secret);
                    }
                    else if (top != typeof(TsObject).MakePointerType())
                        _errors.Add(new CompileException($"Invalid syntax detected {member.Left.Position}"));

                    emit.LdStr(((ISyntaxToken)member.Right).Text);
                    assign.Right.Accept(this);
                    top = emit.GetTop();
                    if (top == typeof(int))
                    {
                        emit.ConvertFloat();
                        top = typeof(float);
                    }
                    var argTypes = new[] { typeof(string), top };
                    var assignMethod = typeof(TsObject).GetMethod("MemberSet", argTypes);
                    if(assignMethod == null)
                    {
                        _errors.Add(new CompileException($"Invalid syntax detected {assign.Right.Position}"));
                        emit.Call(TsTypes.Empty);
                    }
                    else
                        emit.Call(typeof(TsObject).GetMethod("MemberSet", argTypes));
                }
            }
            else
                _errors.Add(new CompileException($"This assignment is not yet supported {assign.Position}"));
        }

        private void ProcessAssignExtra(AssignNode assign)
        {
            var op = assign.Value.Replace("=", "");
            if (assign.Left.Type == SyntaxType.ArrayAccess || assign.Left.Type == SyntaxType.ArgumentAccess)
            {
                //Because this has to access the array location,
                //we can safely just get the address of the array elem and overwrite
                //the data pointed to by that address.
                GetAddressIfPossible(assign.Left);
                emit.Dup()
                    .LdObj(typeof(TsObject));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                    .StObj(typeof(TsObject));
            }
            else if (assign.Left is ListAccessNode list)
            {
                ListAccessSet(list, 2);
                emit.Call(typeof(List<TsObject>).GetMethod("get_Item"));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                    .Call(typeof(List<TsObject>).GetMethod("set_Item", new[] { typeof(int), typeof(TsObject) }));
            }
            else if (assign.Left is GridAccessNode grid)
            {
                GridAccessSet(grid);
                emit.Call(typeof(Grid<TsObject>).GetMethod("get_Item"));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                    .Call(typeof(Grid<TsObject>).GetMethod("set_Item"));
            }
            else if (assign.Left is MapAccessNode map)
            {
                MapAccessSet(map);
                emit.Call(typeof(Dictionary<TsObject, TsObject>).GetMethod("get_Item"));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                    .Call(typeof(Dictionary<TsObject, TsObject>).GetMethod("set_Item"));
            }
            else if(assign.Left is VariableToken variable)
            {
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        _errors.Add(new CompileException($"Cannot assign to the value {symbol.Name} {variable.Position}"));
                    GetAddressIfPossible(assign.Left);
                    emit.Dup()
                        .LdObj(typeof(TsObject));
                    assign.Right.Accept(this);
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position))
                        .StObj(typeof(TsObject));
                }
                else
                {
                    SelfAccessSet(variable);
                    emit.Call(typeof(TsInstance).GetMethod("GetVariable"));
                    assign.Right.Accept(this);
                    var result = emit.GetTop();
                    if (result == typeof(int) || result == typeof(bool))
                        emit.ConvertFloat();
                    emit.Call(GetOperator(op, typeof(TsObject), emit.GetTop(), assign.Position));
                    emit.Call(typeof(TsInstance).GetMethod("set_Item", new[] { typeof(string), typeof(TsObject) }));
                }
            }
            else if(assign.Left is MemberAccessNode member)
            {
                if(!(member.Right is VariableToken value))
                {
                    _errors.Add(new CompileException($"Cannot assign to readonly value {member.Right.Position}"));
                    return;
                }
                if(member.Left is ReadOnlyToken read)
                {
                    Func<ILEmitter> loadTarget = GetReadOnlyLoadFunc(read);

                    loadTarget()
                        .LdStr(value.Text);
                    loadTarget()
                        .LdStr(value.Text)
                        .Call(typeof(TsInstance).GetMethod("get_Item"));

                    assign.Right.Accept(this);
                    var top = emit.GetTop();
                    if (top == typeof(int) || top == typeof(bool))
                    {
                        emit.ConvertFloat();
                        top = typeof(float);
                    }

                    emit.Call(GetOperator(op, typeof(TsObject), top, assign.Position))
                        .Call(typeof(TsInstance).GetMethod("set_Item"));
                }
                else
                {
                    var secret = GetLocal();
                    member.Left.Accept(this);

                    emit.StLocal(secret)
                        .LdLocalA(secret)
                        .LdStr(value.Text)
                        .LdLocalA(secret)
                        .LdStr(value.Text)
                        .Call(typeof(TsObject).GetMethod("MemberGet"));

                    assign.Right.Accept(this);
                    var top = emit.GetTop();
                    if (top == typeof(int) || top == typeof(bool))
                    {
                        emit.ConvertFloat();
                        top = typeof(float);
                    }

                    emit.Call(GetOperator(op, typeof(TsObject), top, assign.Position))
                        .Call(typeof(TsObject).GetMethod("MemberSet", new[] { typeof(string), typeof(TsObject) }));


                    FreeLocal(secret);
                }
            }
        }

        private void ListAccessSet(ListAccessNode listAccess, int accesses)
        {
            var id = GetLocal(typeof(List<TsObject>));
            var index = GetLocal(typeof(int));

            LoadElementAsInt(listAccess.Left);
            emit.Call(typeof(DsList).GetMethod("DsListGet"))
                .StLocal(id);

            LoadElementAsInt(listAccess.Right);
            emit.StLocal(index);
            for(var i = 0; i < accesses; i++)
            {
                emit.LdLocal(id)
                    .LdLocal(index);
            }
            FreeLocal(id);
            FreeLocal(index);
        }

        private void GridAccessSet(GridAccessNode grid)
        {
            var id = GetLocal(typeof(Grid<TsObject>));
            var x = GetLocal(typeof(int));
            var y = GetLocal(typeof(int));
            LoadElementAsInt(grid.Left);
            emit.Call(typeof(DsGrid).GetMethod("DsGridGetGrid"))
                .StLocal(id);
            LoadElementAsInt(grid.X);
            emit.StLocal(x);
            LoadElementAsInt(grid.Y);
            emit.StLocal(y)
                .LdLocal(id)
                .LdLocal(x)
                .LdLocal(y)
                .LdLocal(id)
                .LdLocal(x)
                .LdLocal(y);
            FreeLocal(id);
            FreeLocal(x);
            FreeLocal(y);
        }

        private void MapAccessSet(MapAccessNode map)
        {
            var id = GetLocal(typeof(Dictionary<TsObject, TsObject>));
            var key = GetLocal();
            LoadElementAsInt(map.Left);
            emit.Call(typeof(DsMap).GetMethod("DsMapGet"))
                .StLocal(id);

            map.Right.Accept(this);
            var top = emit.GetTop();
            if (top != typeof(TsObject))
                emit.New(TsTypes.Constructors[top]);
            emit.StLocal(key);
            emit.LdLocal(id)
                .LdLocal(key)
                .LdLocal(id)
                .LdLocal(key);

            FreeLocal(id);
            FreeLocal(key);
        }

        private void SelfAccessSet(VariableToken variable)
        {
            emit.LdArg(0)
                .LdStr(variable.Text)
                .LdArg(0)
                .LdStr(variable.Text);
        }

        public void Visit(BitwiseNode bitwise)
        {
            if(bitwise.Left.Type == SyntaxType.Constant)
            {
                var leftConst = bitwise.Left as IConstantToken<float>;
                if (leftConst == null)
                    _errors.Add(new CompileException($"Cannot perform operator {bitwise.Value} on the constant type {(bitwise.Left as IConstantToken).ConstantType} {bitwise.Position}"));
                else
                    emit.LdLong((long)leftConst.Value);
            }
            else
            {
                GetAddressIfPossible(bitwise.Left);
                var top = emit.GetTop();
                if (top != typeof(TsObject) && top != typeof(TsObject).MakePointerType())
                    _errors.Add(new CompileException($"Cannot perform operator {bitwise.Value} on the type {emit.GetTop()} {bitwise.Position}"));
                emit.Call(TsTypes.ObjectCasts[typeof(long)]);
            }

            if(bitwise.Right.Type == SyntaxType.Constant)
            {
                var rightConst = bitwise.Right as IConstantToken<float>;
                if(rightConst == null)
                    _errors.Add(new CompileException($"Cannot perform operator {bitwise.Value} on the constant type {(bitwise.Right as IConstantToken).ConstantType} {bitwise.Position}"));
                else
                    emit.LdLong((long)rightConst.Value);
            }
            else
            {
                GetAddressIfPossible(bitwise.Right);
                var top = emit.GetTop();
                if (top != typeof(TsObject) && top != typeof(TsObject).MakePointerType())
                    _errors.Add(new CompileException($"Cannot perform operator {bitwise.Value} on the type {top} {bitwise.Position}"));
                emit.Call(TsTypes.ObjectCasts[typeof(long)]);
            }

            switch(bitwise.Value)
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
                    _errors.Add(new CompileException($"Invalid bitwise operator detected: {bitwise.Value} {bitwise.Position}"));
                    break;
            }
            emit.ConvertFloat();
        }

        public void Visit(BlockNode block)
        {
            var size = block.Children.Count;
            for (var i = 0; i < size; ++i)
            {
                block.Children[i].Accept(this);
                if (block.Children[i].Type == SyntaxType.FunctionCall || block.Children[i].Type == SyntaxType.Postfix || block.Children[i].Type == SyntaxType.Prefix)
                    emit.Pop();
            }
        }

        public void Visit(BreakToken breakToken)
        {
            emit.Br(_loopEnd.Peek());
        }

        public void Visit(CaseNode caseNode)
        {
            //Due to the way that a switch statements logic gets seperated, the SwitchNode implements
            //the logic for both Default and CaseNodes.
            _errors.Add(new CompileException($"Encountered invalid program {caseNode.Position}"));
        }

        public void Visit(ConditionalNode conditionalNode)
        {
            GetAddressIfPossible(conditionalNode.Test);
            var top = emit.GetTop();
            if (top == typeof(TsObject) || top == typeof(TsObject).MakePointerType())
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(bool)], conditionalNode.Test.Position);
            else if (top == typeof(float))
                emit.ConvertInt(false);
            else if(top != typeof(bool))
                _errors.Add(new CompileException($"Detected invalid syntax {conditionalNode.Test.Position}"));
            var brFalse = emit.DefineLabel();
            var brFinal = emit.DefineLabel();
            emit.BrFalse(brFalse);

            // We convert the result of the expression into a TsObject
            // in order to get a unified output. Otherwise, the program could have
            // undefined behaviour.

            conditionalNode.Left.Accept(this);
            top = emit.GetTop();
            if (top != typeof(TsObject))
                emit.New(TsTypes.Constructors[top]);

            emit.Br(brFinal)
                .MarkLabel(brFalse);

            conditionalNode.Right.Accept(this);
            top = emit.GetTop();
            if (top != typeof(TsObject))
                emit.New(TsTypes.Constructors[top]);

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
                    emit.LdInt(((ConstantToken<bool>)constantToken).Value ? 1 : 0);
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

        public void Visit(DeclareNode declare)
        {
            if (_table.Defined(declare.Value, out var symbol) && symbol.Type == SymbolType.Variable && _locals.TryGetValue(symbol, out var local))
            {
                emit.Call(TsTypes.Empty)
                    .StLocal(local);
            }
            else
                _errors.Add(new NameConflictException($"Tried to overwrite the symbol {symbol} {declare.Position}"));
        }

        public void Visit(DefaultNode defaultNode)
        {
            //Due to the way that a switch statements logic gets seperated, the SwitchNode implements
            //the logic for both Default and CaseNodes.
            _errors.Add(new CompileException($"Encountered invalid program {defaultNode.Position}"));
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
            doNode.Until.Accept(this);
            var type = emit.GetTop();
            if (type == typeof(float))
                emit.ConvertInt(false);
            else if (type == typeof(TsObject))
                emit.Call(TsTypes.ObjectCasts[typeof(bool)]);
            else if(type == typeof(string))
            {
                //All string should eval to false.
                emit.Pop()
                    .LdInt(0);
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
            var name = $"{_namespace}.{enumNode.Value}".TrimStart('.');
            var type = _module.DefineEnum(name, TypeAttributes.Public, typeof(long));

            long current = 0;
            foreach (ISyntaxNode expr in enumNode.Children)
            {
                if (expr.Type == SyntaxType.Assign)
                {
                    if (expr.Children[0] is IConstantToken<float> value)
                        current = (long)value.Value;
                    else
                        _errors.Add(new InvalidSymbolException($"Enum value must be equal to an integer constant {expr.Position}."));
                }
                else if (expr.Type != SyntaxType.Declare)
                    _errors.Add(new CompileException($"Encountered error while compiling enum {enumNode.Value} {expr.Position}."));

                _enums[name, expr.Value] = current;
                type.DefineLiteral(expr.Value, current++);
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

            TestEquality(equality.Value, left, right, equality.Position);
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
                else if (right == typeof(TsObject))
                    emit.Call(GetOperator(op, left, right, pos));
                else
                    _errors.Add(new CompileException($"Cannot {op} types {left} and {right} {pos}"));
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
                else if (right == typeof(string) || right == typeof(TsObject))
                    emit.Call(GetOperator(op, left, right, pos));
                else
                    _errors.Add(new CompileException($"Cannot {op} types {left} and {right} {pos}"));
            }
            else if (left == typeof(TsObject))
                emit.Call(GetOperator(op, left, right, pos));
            else
                _errors.Add(new CompileException($"Cannot {op} types {left} and {right} {pos}"));
        }

        public void Visit(ExitToken exitToken)
        {
            if(_inInstanceScript)
            {
                emit.Call(typeof(TsInstance).GetMethod("get_EventType"))
                    .Call(typeof(Stack<string>).GetMethod("Pop"))
                    .Pop()
                    .Call(TsTypes.Empty)
                    .Ret();
                return;
            }
            if (_inGlobalScript && !emit.TryGetTop(out _))
                emit.Call(TsTypes.Empty);
            emit.Ret();
        }

        public void Visit(ExplicitArrayAccessNode explicitArrayAccess)
        {
            _errors.Add(new NotImplementedException($"Currently explicit array access is not available {explicitArrayAccess.Position}."));
        }

        public void Visit(ForNode forNode)
        {
            var forStart = emit.DefineLabel();
            var forCondition = emit.DefineLabel();
            var forIter = emit.DefineLabel();
            var forFinal = emit.DefineLabel();
            forNode.Initializer.Accept(this);

            _loopStart.Push(forIter);
            _loopEnd.Push(forFinal);

            //Check condition before continuing.
            emit.Br(forCondition);
            emit.MarkLabel(forStart);
            forNode.Body.Accept(this);
            emit.MarkLabel(forIter);
            forNode.Iterator.Accept(this);
            emit.MarkLabel(forCondition);
            forNode.Condition.Accept(this);
            emit.BrTrue(forStart);
            emit.MarkLabel(forFinal);

            _loopStart.Pop();
            _loopEnd.Pop();
        }

        public void Visit(FunctionCallNode functionCall)
        {
            //All methods callable from GML should have the same sig:
            //TsObject func_name(TsInstance target, TsObject[] args);
            var nameElem = functionCall.Children[0];
            string name;
            if (nameElem is ISyntaxToken token)
                name = token.Text;
            else if (nameElem is MemberAccessNode memberAccess)
            {
                if (TryResolveNamespace(memberAccess, out nameElem))
                {
                    //explicit namespace -> function call
                    //e.g. name.space.func_name();
                    name = ((ISyntaxToken)nameElem).Text;
                }
                else
                {
                    //instance script call
                    //e.g. inst.script_name();
                    name = (memberAccess.Right as ISyntaxToken)?.Text;
                    if (name == null)
                    {
                        _errors.Add(new CompileException($"Invalid id for script/event {memberAccess.Right.Position}"));
                        emit.Call(TsTypes.Empty);
                        return;
                    }
                    GetAddressIfPossible(memberAccess.Left);
                    CallEvent(name, false, functionCall, memberAccess.Right.Position);
                    return;
                }
            }
            else
            {
                GetAddressIfPossible(nameElem);
                var top = emit.GetTop();
                if(top == typeof(TsObject))
                {
                    var secret = GetLocal();
                    emit.StLocal(secret);
                    emit.LdLocalA(secret);
                    FreeLocal(secret);
                }
                else if(emit.GetTop() != typeof(TsObject).MakePointerType())
                {
                    _errors.Add(new CompileException($"Invalid syntax detected {functionCall.Children[0].Position}"));
                    emit.Call(TsTypes.Empty);
                    return;
                }
                LoadFunctionArguments(functionCall);
                emit.Call(typeof(TsObject).GetMethod("DelegateInvoke", new[] { typeof(TsObject[]) }));

                return;
            }

            if(!_table.Defined(name, out var symbol) || (symbol.Type == SymbolType.Script && symbol.Scope == SymbolScope.Member))
            {
                CallEvent(name, true, functionCall, nameElem.Position);
                return;
            }

            if(symbol.Type == SymbolType.Variable)
            {
                emit.LdLocalA(_locals[symbol]);
                LoadFunctionArguments(functionCall);
                emit.Call(typeof(TsObject).GetMethod("DelegateInvoke", new[] { typeof(TsObject[]) }));
                return;
            }
            else if (symbol.Type != SymbolType.Script)
            {
                _errors.Add(new CompileException($"Tried to call something that wasn't a script. Check for name conflict {functionCall.Position}"));
                emit.Call(TsTypes.Empty);
                return;
            }
            var ns = GetAssetNamespace(symbol);
            if (!_methods.TryGetValue(ns, name, out var method))
            {
                // Special case hack needed for getting the MethodInfo for weak methods before
                // their ImportNode has been hit.
                if (symbol is ImportLeaf leaf)
                {
                    var temp = _namespace;
                    _namespace = ns;
                    leaf.Node.Accept(this);
                    method = _methods[_namespace, leaf.Name];
                    _namespace = temp;
                }
                else
                {
                    method = StartMethod(name, ns);
                    _pendingMethods.Add($"{ns}.{name}".TrimStart('.'), functionCall.Position);
                }
            }

            UnresolveNamespace();

            if (_isDebug)
            {
                var line = functionCall.Position.Line;
                var end = 0;
                ISyntaxElement element = functionCall;
                while(true)
                {
                    if (element.Position.Line > line)
                        line = element.Position.Line;
                    if (element.IsToken)
                    {
                        end = element.Position.Column + ((ISyntaxToken)element).Text.Length + 2;
                        break;
                    }
                    else
                    {
                        var node = element as ISyntaxNode;
                        if (node.Children.Count == 0)
                        {
                            end = node.Position.Column + (node.Value == null ? 0 : node.Value.Length) + 2;
                            break;
                        }
                        else
                        {
                            element = node.Children[node.Children.Count - 1];
                        }
                    }
                }
                MarkSequencePoint(functionCall.Position.File ?? "",
                                  functionCall.Position.Line,
                                  functionCall.Position.Column,
                                  line,
                                  end);
            }

            //Load the target
            emit.LdArg(0);

            LoadFunctionArguments(functionCall);

            //The argument array should still be on top.
            emit.Call(method, 2, typeof(TsObject));
        }

        /// <summary>
        /// Loads the arguments from a <see cref="FunctionCallNode"/> to the eval stack, leaving the arg array on top.
        /// <para>
        /// If there are no args, loads null to avoid allocating a new array.
        /// </para>
        /// </summary>
        private void LoadFunctionArguments(FunctionCallNode functionCall)
        {
            if (functionCall.Children.Count - 1 > 0)
            {
                emit.LdInt(functionCall.Children.Count - 1)
                    .NewArr(typeof(TsObject));

                for (var i = 0; i < functionCall.Children.Count - 1; ++i)
                {
                    emit.Dup()
                        .LdInt(i);

                    functionCall.Children[i + 1].Accept(this);
                    var top = emit.GetTop();
                    if (top != typeof(TsObject))
                        emit.New(TsTypes.Constructors[top]);

                    emit.StElem(typeof(TsObject));
                }
            }
            else
                emit.LdNull();
        }

        private void CallEvent(string name, bool loadId, FunctionCallNode functionCall, TokenPosition start)
        {
            if (_isDebug)
                MarkSequencePoint(start.File ?? "", start.Line, start.Column, start.Line, start.Column + name.Length + 2);

            if (loadId)
                emit.LdArg(0);

            var top = emit.GetTop();

            if (top == typeof(TsObject) || top == typeof(TsObject).MakePointerType())
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(TsInstance)], start);
            else if (top != typeof(TsInstance))
            {
                _errors.Add(new CompileException($"Internal Compile Exception encountered {start}"));
                emit.Call(TsTypes.Empty);
                return;
            }

            if(name == "destroy")
            {
                //Syntactic sugar for instance_destroy(inst);
                emit.Call(typeof(TsInstance).GetMethod("Destroy"))
                    .Call(TsTypes.Empty);
            }
            else
            {
                //The object returned by GetDelegate should already have a target.
                emit.LdStr(name)
                    .Call(typeof(TsInstance).GetMethod("GetDelegate", BindingFlags.Instance | BindingFlags.Public));
                LoadFunctionArguments(functionCall);
                emit.Call(typeof(TsDelegate).GetMethod("Invoke", new[] { typeof(TsObject[]) }));
            }
        }

        public void Visit(GridAccessNode gridAccess)
        {
            LoadElementAsInt(gridAccess.Left);
            LoadElementAsInt(gridAccess.X);
            LoadElementAsInt(gridAccess.Y);
            emit.Call(typeof(DsGrid).GetMethod("DsGridGet"));
        }

        public void Visit(IfNode ifNode)
        {
            // If there is no else node, just let execution flow naturally.
            // Otherwise, bracnh to the end forcefully at the end of the if body.

            var ifFalse = emit.DefineLabel();
            var ifTrue = emit.DefineLabel();
            GetAddressIfPossible(ifNode.Condition);
            var top = emit.GetTop();
            if (top == typeof(TsObject) || top == typeof(TsObject).MakePointerType())
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(bool)], ifNode.Condition.Position);
            emit.BrFalse(ifFalse);
            ifNode.Body.Accept(this);

            if (ifNode.Children.Count == 3)
                emit.Br(ifTrue);
            
            emit.MarkLabel(ifFalse);

            if (ifNode.Children.Count == 3)
            {
                ifNode.Children[2].Accept(this);
                emit.MarkLabel(ifTrue);
            }
        }

        public void Visit(ImportNode import)
        {
            if (_methods.ContainsIndex(_namespace, import.InternalName.Value))
                return;

            var argWrappers = import.GetArguments();
            var args = new Type[argWrappers.Count];
            var externalName = import.ExternalName.Value;
            var internalName = import.InternalName.Value;
            _pendingMethods.Remove($"{_namespace}.{internalName}".TrimStart('.'));

            for (var i = 0; i < args.Length; i++)
            {
                if (!TsTypes.BasicTypes.TryGetValue(argWrappers[i].Value, out var argType))
                    _errors.Add(new CompileException($"Could not import the function {internalName} because one of the arguments was invalid {argWrappers[i].Value} {argWrappers[i].Position}"));

                args[i] = argType;
            }

            var typeName = externalName.Remove(externalName.LastIndexOf('.'));
            var methodName = externalName.Substring(typeName.Length + 1);
            var owner = _typeParser.GetType(typeName);
            var method = GetMethodToImport(owner, methodName, args);
            if(method == null)
            {
                _errors.Add(new CompileException($"Failed to find the import function {externalName} {import.ExternalName.Position}"));
                StartMethod(methodName, _namespace);
                return;
            }

            if (method.GetCustomAttribute<WeakMethodAttribute>() != null)
            {
                if(IsMethodValid(method))
                {
                    _methods.Add(_namespace, internalName, method);
                    SpecialImports.Write(_namespace);
                    SpecialImports.Write(":");
                    SpecialImports.Write(internalName);
                    SpecialImports.Write(':');
                    SpecialImports.WriteLine(externalName);
                    var init = Initializer;
                    var name = $"{_namespace}.{internalName}".TrimStart('.');
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
                    _warnings.Add($"Could not directly import method with the WeakMethod attribute: {method} {import.Position}\nPlease check the method signature.");
                    GenerateWeakMethodForImport(method, internalName);
                }
            }
            else
                GenerateWeakMethodForImport(method, internalName);
        }

        public void Visit(ListAccessNode listAccess)
        {
            LoadElementAsInt(listAccess.Left);
            LoadElementAsInt(listAccess.Right);
            emit.Call(typeof(DsList).GetMethod("DsListFindValue"));
        }

        private void LoadElementAsInt(ISyntaxElement element)
        {
            GetAddressIfPossible(element);
            var top = emit.GetTop();
            if (top == typeof(bool))
                top = typeof(int);
            else if (top == typeof(float))
            {
                emit.ConvertInt(false);
                top = typeof(int);
            }
            else if (top == typeof(TsObject) || top == typeof(TsObject).MakePointerType())
            {
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(int)], element.Position);
                top = typeof(int);
            }
            else
            {
                _errors.Add(new CompileException($"Invalid syntax detected {element.Position}"));
                emit.LdInt(0);
            }
        }

        /// <summary>
        /// Attempts to load an ISyntaxElement to the eval stack as an int.
        /// <para>
        /// Expects user to handle error.
        /// </para>
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool TryLoadElementAsInt(ISyntaxElement element)
        {
            GetAddressIfPossible(element);
            var top = emit.GetTop();
            if (top == typeof(bool))
                top = typeof(int);
            else if (top == typeof(float))
            {
                emit.ConvertInt(false);
                top = typeof(int);
            }
            else if (top == typeof(TsObject) || top == typeof(TsObject).MakePointerType())
            {
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(int)], element.Position);
                top = typeof(int);
            }
            else
            {
                emit.LdInt(0);
                return false;
            }
            return true;
        }

        public void Visit(LocalsNode localsNode)
        {
            foreach (var child in localsNode.Children)
                child.Accept(this);
        }

        public void Visit(LogicalNode logical)
        {
            //Strings are falsy values
            //Otherwise, convert the value into a bool and compare bools.
            var end = emit.DefineLabel();
            GetAddressIfPossible(logical.Left);
            var left = emit.GetTop();
            if (left == typeof(string))
                emit.Pop().LdInt(0);
            else if (left == typeof(float))
                emit.LdFloat(.5f).Cge();
            else if (left == typeof(TsObject) || left == typeof(TsObject).MakePointerType())
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(bool)], logical.Left.Position);
            else if (left != typeof(bool))
                _errors.Add(new CompileException($"Encountered invalid syntax {logical.Left.Position}"));
            if (logical.Value == "&&")
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

            GetAddressIfPossible(logical.Right);
            var right = emit.GetTop();
            if (right == typeof(string))
                emit.Pop().LdInt(0);
            else if (right == typeof(float))
                emit.LdFloat(.5f).Cge();
            else if (right == typeof(TsObject) || right == typeof(TsObject).MakePointerType())
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(bool)], logical.Right.Position);
            else if (right != typeof(bool))
                _errors.Add(new CompileException($"Encountered invalid syntax {logical.Right.Position}"));

            emit.MarkLabel(end);
        }

        public void Visit(MapAccessNode mapAccess)
        {
            LoadElementAsInt(mapAccess.Left);
            mapAccess.Right.Accept(this);
            var top = emit.GetTop();
            if(top != typeof(TsObject))
            {
                emit.New(TsTypes.Constructors[top]);
                top = typeof(TsObject);
            }
            emit.Call(typeof(DsMap).GetMethod("DsMapFindValue"));
        }

        public void Visit(MemberAccessNode memberAccess)
        {
            var resolved = ResolveNamespace(memberAccess);
            if(_resolveNamespace != "")
            {
                if (resolved is MemberAccessNode member)
                    memberAccess = member;
                else
                {
                    resolved.Accept(this);
                    return;
                }
            }
            //If left is enum name, find it in _enums.
            //Otherwise, Accept left, and call member access on right.
            if (memberAccess.Left is VariableToken enumVar && _table.Defined(enumVar.Text, out var symbol) && symbol.Type == SymbolType.Enum)
            {
                if (memberAccess.Right is VariableToken enumValue)
                {
                    if (!_enums.TryGetValue(enumVar.Text, enumValue.Text, out var value))
                    {
                        _table.Enter(enumVar.Text);
                        if (_table.Defined(enumValue.Text, out symbol) && symbol is EnumLeaf leaf)
                        {
                            value = leaf.Value;
                            _enums[enumVar.Text, leaf.Name] = value;
                        }
                        else
                            _errors.Add(new CompileException($"The enum {enumVar.Text} does not declare value {enumValue.Text} {enumValue.Position}"));
                        _table.Exit();
                    }
                    emit.LdFloat(value);
                }
                else
                    _errors.Add(new CompileException($"Invalid enum access syntax {enumVar.Position}"));
            }
            else if(memberAccess.Left is ReadOnlyToken read)
            {
                if (_resolveNamespace != "" || !(memberAccess.Right is VariableToken right))
                {
                    UnresolveNamespace();
                    _errors.Add(new CompileException($"Invalid syntax detected {read.Position}"));
                    emit.Call(TsTypes.Empty);
                    return;
                }
                switch(read.Text)
                {
                    case "global":
                        emit.LdFld(typeof(TsInstance).GetField("Global"));
                        break;
                    case "self":
                        emit.LdArg(0);
                        break;
                    default:
                        _errors.Add(new CompileException($"Invalid syntax detected {right.Position}"));
                        emit.Call(TsTypes.Empty);
                        return;
                }
                emit.LdStr(right.Text)
                    .Call(typeof(TsInstance).GetMethod("get_Item"));
            }
            else
            {
                if (_resolveNamespace != "")
                {
                    UnresolveNamespace();
                    _errors.Add(new CompileException($"Invalid syntax detected {memberAccess.Left}"));
                    emit.Call(TsTypes.Empty);
                    return;
                }

                GetAddressIfPossible(memberAccess.Left);
                var left = emit.GetTop();
                if(left == typeof(TsObject))
                {
                    var secret = GetLocal();
                    emit.StLocal(secret)
                        .LdLocalA(secret);
                    FreeLocal(secret);
                }
                else if(left != typeof(TsObject).MakePointerType())
                {
                    _errors.Add(new NotImplementedException($"Accessing a variable through a type is not yet supported {memberAccess.Left} {memberAccess.Left.Position}"));
                    emit.Call(TsTypes.Empty);
                    return;
                }

                if (memberAccess.Right is VariableToken right)
                {
                    emit.LdStr(right.Text)
                        .Call(typeof(TsObject).GetMethod("MemberGet", new[] { typeof(string) }));
                }
                else if (memberAccess.Right is ReadOnlyToken readOnly)
                {
                    if (readOnly.Text != "id" && readOnly.Text != "self")
                    {
                        _errors.Add(new NotImplementedException($"Only the read only variables id and self can be accessed from an instance currently {readOnly.Position}"));
                        emit.Call(TsTypes.Empty);
                        return;
                    }
                    emit.Call(typeof(TsObject).GetMethod("GetInstance"))
                        .Call(typeof(TsInstance).GetMethod("get_Id"))
                        .New(TsTypes.Constructors[typeof(float)]);
                }
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {memberAccess.Position}"));
            }

            UnresolveNamespace();
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
                    switch (multiplicative.Value)
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
                    _errors.Add(new CompileException($"Cannot {multiplicative.Value} types {left} and {right} {multiplicative.Position}"));
                else if (right == typeof(TsObject))
                    emit.Call(GetOperator(multiplicative.Value, left, right, multiplicative.Position));
                else
                    _errors.Add(new CompileException($"Cannot {multiplicative.Value} types {left} and {right} {multiplicative.Position}"));
            }
            else if (left == typeof(string)) {
                _errors.Add(new CompileException($"Cannot {multiplicative.Value} types {left} and {right} {multiplicative.Position}"));
            }
            else if (left == typeof(TsObject))
                emit.Call(GetOperator(multiplicative.Value, left, right, multiplicative.Position));
        }

        public void Visit(NamespaceNode namespaceNode)
        {
            var parent = _namespace;
            if (parent == "")
                _namespace = namespaceNode.Value;
            else
                _namespace += "." + namespaceNode.Value;
            _table.EnterNamespace(namespaceNode.Value);
            var temp = _table;
            _table = CopyTable(_table);
            temp.ExitNamespace(namespaceNode.Value);
            CopyTable(temp, _table, namespaceNode.Position);
            AcceptDeclarations(namespaceNode);
            _namespace = parent;
            _table = temp;
        }

        public void Visit(NewNode newNode)
        {
            if(_table.Defined(newNode.Value, out var symbol))
            {
                if(symbol.Type == SymbolType.Object)
                {
                    var name = newNode.Value;
                    var pos = newNode.Position;
                    MarkSequencePoint(pos.File ?? "", pos.Line, pos.Column, pos.Line, pos.Column + newNode.Value.Length);
                    var ctor = typeof(TsInstance).GetConstructor(new[] { typeof(string), typeof(TsObject[]) });
                    var ns = GetAssetNamespace(symbol);
                    if (ns != "")
                        name = $"{ns}.{name}";
                    emit.LdStr(name);

                    if (newNode.Arguments.Count > 0)
                    {
                        emit.LdInt(newNode.Arguments.Count)
                            .NewArr(typeof(TsObject));

                        for (var i = 0; i < newNode.Arguments.Count; ++i)
                        {
                            emit.Dup()
                                .LdInt(i);

                            newNode.Arguments[i].Accept(this);
                            var top = emit.GetTop();
                            if (top != typeof(TsObject))
                                emit.New(TsTypes.Constructors[top]);

                            emit.StElem(typeof(TsObject));
                        }
                    }
                    else
                        emit.LdNull();

                    emit.New(ctor);
                    if (newNode.Parent.Type == SyntaxType.Block)
                        emit.Pop();
                    else
                        emit.New(TsTypes.Constructors[typeof(TsInstance)]);
                }
                else
                {
                    _errors.Add(new CompileException($"Tried to create an instance of something that wasn't an object: {newNode.Value} {newNode.Position}"));
                    emit.Call(TsTypes.Empty);
                }
            }
            else
            {
                _errors.Add(new CompileException($"Tried to create an instance of a type that doesn't exist: {newNode.Value} {newNode.Position}"));
                emit.Call(TsTypes.Empty);
            }
        }

        public void Visit(ObjectNode objectNode)
        {
            var name = $"{_namespace}.{objectNode.Value}";
            if (name.StartsWith("."))
                name = name.TrimStart('.');
            var type = _module.DefineType(name, TypeAttributes.Public);
            _table.Enter(objectNode.Value);
            var parentNode = objectNode.Inherits as IConstantToken<string>;
            if (parentNode == null)
                _errors.Add(new CompileException($"Invalid syntax detected {objectNode.Inherits.Position}"));
            string parent = null;
            if(parentNode.Value != null && _table.Defined(parentNode.Value, out var symbol))
            {
                if (symbol.Type == SymbolType.Object)
                    parent = $"{GetAssetNamespace(symbol)}.{symbol.Name}".TrimStart('.');
                else
                    _errors.Add(new CompileException($"Tried to inherit from non object identifier {objectNode.Inherits.Position}"));
            }



            var input = new[] { typeof(TsInstance), typeof(TsObject[]) };
            var addMethod = typeof(LookupTable<string, string, TsDelegate>).GetMethod("Add", new[] { typeof(string), typeof(string), typeof(TsDelegate) });
            var eventType = typeof(TsInstance).GetMethod("get_EventType");
            var push = typeof(Stack<string>).GetMethod("Push");
            var pop = typeof(Stack<string>).GetMethod("Pop");
            var init = Initializer;

            if(parent != null)
            {
                init.Call(typeof(TsInstance).GetMethod("get_Inherits"))
                    .LdStr(name)
                    .LdStr(parent)
                    .Call(typeof(Dictionary<string, string>).GetMethod("Add"));
            }

            init.Call(typeof(TsInstance).GetMethod("get_Types"))
                .LdStr(name)
                .Call(typeof(List<string>).GetMethod("Add"));

            if(objectNode.Children.Count > 1)
                init.Call(typeof(TsInstance).GetMethod("get_InstanceScripts"));
            _inInstanceScript = true;
            for(var i = 1; i < objectNode.Children.Count; i++)
            {
                var script = (ScriptNode)objectNode.Children[i];
                var method = type.DefineMethod(script.Value, MethodAttributes.Public | MethodAttributes.Static, typeof(TsObject), input);
                ScriptStart(script.Value, method, input);

                emit.Call(eventType)
                    .LdStr(script.Value)
                    .Call(push);

                ProcessScriptArguments(script);
                script.Body.Accept(this);
                BlockNode body = (BlockNode)script.Body;
                var last = body.Children.Count == 0 ? null : body.Children[body.Children.Count - 1];
                if(body.Children.Count == 0 || (last.Type != SyntaxType.Exit && last.Type != SyntaxType.Return))
                {
                    emit.Call(eventType)
                    .Call(pop)
                    .Pop()
                        .Call(TsTypes.Empty)
                    .Ret();
                }

                ScriptEnd();

                if (i != objectNode.Children.Count - 1)
                    init.Dup();

                init.LdStr(name)
                    .LdStr(script.Value)
                    .LdNull()
                    .LdFtn(method)
                    .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                    .LdStr(script.Value)
                    .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                    .Call(addMethod);
            }
            _inInstanceScript = false;

            var attrib = new CustomAttributeBuilder(typeof(WeakObjectAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            type.SetCustomAttribute(attrib);
            type.CreateType();

            _table.Exit();
        }

        public void Visit(PostfixNode postfix)
        {
            var secret = GetLocal();
            if (postfix.Child.Type == SyntaxType.ArrayAccess || postfix.Child.Type == SyntaxType.ArgumentAccess)
            {
                GetAddressIfPossible(postfix.Child);
                emit.Dup()
                    .Dup()
                    .LdObj(typeof(TsObject))
                    .StLocal(secret)
                    .LdObj(typeof(TsObject))
                    .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                    .StObj(typeof(TsObject))
                    .LdLocal(secret);
            }
            else if(postfix.Child is ListAccessNode list)
            {
                ListAccessSet(list, 2);
                emit.Call(typeof(List<TsObject>).GetMethod("get_Item"))
                    .StLocal(secret)
                    .LdLocal(secret)
                    .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                    .Call(typeof(List<TsObject>).GetMethod("set_Item"))
                    .LdLocal(secret);
            }
            else if(postfix.Child is GridAccessNode grid)
            {
                GridAccessSet(grid);
                emit.Call(typeof(Grid<TsObject>).GetMethod("get_Item"))
                    .StLocal(secret)
                    .LdLocal(secret)
                    .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                    .Call(typeof(Grid<TsObject>).GetMethod("set_Item"))
                    .LdLocal(secret);
            }
            else if(postfix.Child is MapAccessNode map)
            {
                MapAccessSet(map);
                emit.Call(typeof(Dictionary<TsObject, TsObject>).GetMethod("get_Item"))
                    .StLocal(secret)
                    .LdLocal(secret)
                    .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                    .Call(typeof(Dictionary<TsObject, TsObject>).GetMethod("set_Item"))
                    .LdLocal(secret);
            }
            else if (postfix.Child is VariableToken variable)
            {
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        _errors.Add(new CompileException($"Cannot perform {postfix.Value} on an identifier that is not a variable {postfix.Position}"));
                    GetAddressIfPossible(variable);
                    emit.Dup()
                        .Dup()
                        .LdObj(typeof(TsObject))
                        .StLocal(secret)
                        .LdObj(typeof(TsObject))
                        .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                        .StObj(typeof(TsObject))
                        .LdLocal(secret);
                }
                else
                {
                    SelfAccessSet(variable);
                    emit.Call(typeof(TsInstance).GetMethod("GetVariable"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                        .Call(typeof(TsInstance).GetMethod("set_Item", new[] { typeof(string), typeof(TsObject) }))
                        .LdLocal(secret);
                }
            }
            else if (postfix.Child is MemberAccessNode member)
            {
                var value = member.Right as VariableToken;
                if (value is null)
                {
                    _errors.Add(new CompileException($"Invalid member access {member.Position}"));
                    emit.Call(TsTypes.Empty);
                    return;
                }
                if (member.Left is ReadOnlyToken read)
                {
                    Func<ILEmitter> loadTarget = GetReadOnlyLoadFunc(read);

                    loadTarget()
                        .LdStr(value.Text)
                        .Call(typeof(TsInstance).GetMethod("get_Item"))
                        .StLocal(secret)
                        .LdLocal(secret);
                    loadTarget()
                        .LdStr(value.Text)
                        .LdLocal(secret)
                        .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                        .Call(typeof(TsInstance).GetMethod("set_Item"));
                }
                else
                {
                    member.Left.Accept(this);
                    var target = GetLocal();
                    emit.StLocal(target)
                        .LdLocalA(target)
                        .LdStr(value.Text)
                        .Call(typeof(TsObject).GetMethod("MemberGet"))
                        .StLocal(secret)
                        .LdLocal(secret)
                        .LdLocalA(target)
                        .LdStr(value.Text)
                        .LdLocal(secret)
                        .Call(GetOperator(postfix.Value, typeof(TsObject), postfix.Position))
                        .Call(typeof(TsObject).GetMethod("MemberSet", new[] { typeof(string), typeof(TsObject) }));

                    FreeLocal(target);
                }
            }
            else
                _errors.Add(new CompileException($"Invalid syntax detected {postfix.Child.Position}"));

            FreeLocal(secret);
        }

        private Func<ILEmitter> GetReadOnlyLoadFunc(ReadOnlyToken read)
        {
            if (read.Text == "global")
                return () => emit.LdFld(typeof(TsInstance).GetField("Global"));
            else
                return () => emit.LdArg(0);
        }

        public void Visit(PrefixNode prefix)
        {
            //Todo: Only load result if not parent != block

            //These operators need special handling
            if (prefix.Value == "++" || prefix.Value == "--")
            {
                var secret = GetLocal();
                if (prefix.Child.Type == SyntaxType.ArrayAccess || prefix.Child.Type == SyntaxType.ArgumentAccess)
                {
                    GetAddressIfPossible(prefix.Child);
                    emit.Dup()
                        .Dup()
                        .LdObj(typeof(TsObject))
                        .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                        .StObj(typeof(TsObject))
                        .LdObj(typeof(TsObject));
                }
                else if (prefix.Child is ListAccessNode list)
                {
                    ListAccessSet(list, 2);
                    emit.Call(typeof(List<TsObject>).GetMethod("get_Item"))
                        .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                        .Dup()
                        .StLocal(secret)
                        .Call(typeof(List<TsObject>).GetMethod("set_Item"))
                        .LdLocal(secret);
                }
                else if (prefix.Child is GridAccessNode grid)
                {
                    GridAccessSet(grid);
                    emit.Call(typeof(Grid<TsObject>).GetMethod("get_Item"))
                        .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                        .Dup()
                        .StLocal(secret)
                        .Call(typeof(Grid<TsObject>).GetMethod("set_Item"))
                        .LdLocal(secret);
                }
                else if (prefix.Child is MapAccessNode map)
                {
                    MapAccessSet(map);
                    emit.Call(typeof(Dictionary<TsObject, TsObject>).GetMethod("get_Item"))
                        .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                        .Dup()
                        .StLocal(secret)
                        .Call(typeof(Dictionary<TsObject, TsObject>).GetMethod("set_Item"))
                        .LdLocal(secret);
                }
                else if (prefix.Child is VariableToken variable)
                {
                    if (_table.Defined(variable.Text, out var symbol))
                    {
                        if (symbol.Type != SymbolType.Variable)
                            _errors.Add(new CompileException($"Tried to access an identifier that wasn't a variable {prefix.Child.Position}"));
                        variable.Accept(this);
                        emit.Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                            .StLocal(_locals[symbol])
                            .LdLocal(_locals[symbol]);
                    }
                    else
                    {
                        SelfAccessSet(variable);
                        emit.Call(typeof(TsInstance).GetMethod("get_Item"))
                            .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                            .StLocal(secret)
                            .LdLocal(secret)
                            .Call(typeof(TsInstance).GetMethod("set_Item"))
                            .LdLocal(secret);
                    }
                }
                else if (prefix.Child is MemberAccessNode member)
                {
                    if (!(member.Right is VariableToken value))
                    {
                        _errors.Add(new CompileException($"Cannot assign to readonly value {member.Right.Position}"));
                        emit.Call(TsTypes.Empty);
                        return;
                    }
                    if (member.Left is ReadOnlyToken read)
                    {
                        Func<ILEmitter> loadTarget = GetReadOnlyLoadFunc(read);

                        loadTarget()
                            .LdStr(value.Text);
                        loadTarget()
                            .LdStr(value.Text)
                            .Call(typeof(TsInstance).GetMethod("get_Item"))
                            .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                            .StLocal(secret)
                            .LdLocal(secret)
                            .Call(typeof(TsInstance).GetMethod("set_Item"))
                            .LdLocal(secret);
                    }
                    else
                    {
                        member.Left.Accept(this);

                        var target = GetLocal();
                        emit.StLocal(target)
                            .LdLocalA(target)
                            .LdStr(value.Text)
                            .LdLocalA(target)
                            .LdStr(value.Text)
                            .Call(typeof(TsObject).GetMethod("MemberGet"))
                            .Call(GetOperator(prefix.Value, typeof(TsObject), prefix.Position))
                            .StLocal(secret)
                            .LdLocal(secret)
                            .Call(typeof(TsObject).GetMethod("MemberSet", new[] { typeof(string), typeof(TsObject) }))
                            .LdLocal(secret);

                        FreeLocal(target);
                    }
                }
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {prefix.Position}"));

                FreeLocal(secret);
            }
            else
            {
                prefix.Child.Accept(this);
                var top = emit.GetTop();
                if (prefix.Value == "-" && (top == typeof(float) || top == typeof(int) || top == typeof(bool)))
                    emit.Neg();
                else if(prefix.Value != "+")
                    emit.Call(GetOperator(prefix.Value, emit.GetTop(), prefix.Position));
            }
        }

        public void Visit(ReadOnlyToken readOnlyToken)
        {
            switch(readOnlyToken.Text)
            {
                case "self":
                case "id":
                    emit.LdArg(0);
                    break;
                case "argument_count":
                    if(_argumentCount == null)
                    {
                        _argumentCount = emit.DeclareLocal(typeof(float), "argument_count");
                        var isNull = emit.DefineLabel();
                        var end = emit.DefineLabel();
                        emit.LdArg(1)
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
                    if (_needAddress)
                        emit.LdFldA(typeof(TsInstance).GetField("Global"));
                    else
                        emit.LdFld(typeof(TsInstance).GetField("Global"));
                    break;
                case "pi":
                    emit.LdFloat((float)Math.PI);
                    break;
                case "instance_count":
                    emit.LdFld(typeof(TsInstance).GetField("Pool", BindingFlags.Static | BindingFlags.NonPublic))
                        .Call(typeof(Dictionary<float, TsInstance>).GetMethod("get_Count"))
                        .ConvertFloat();
                    break;
                case "noone":
                    emit.LdFloat(-4f);
                    break;
                default:
                    _errors.Add(new NotImplementedException($"Currently the readonly value {readOnlyToken.Text} is not implemented {readOnlyToken.Position}"));
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
                    switch (relational.Value)
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
                            _errors.Add(new CompileException($"Cannot {relational.Value} types {left} and {right} {relational.Position}"));
                            break;
                    }
                }
                else if (right == typeof(TsObject))
                    emit.Call(GetOperator(relational.Value, left, right, relational.Position));
            }
            else if (left == typeof(TsObject))
                emit.Call(GetOperator(relational.Value, left, right, relational.Position));
            else
                _errors.Add(new CompileException($"Invalid syntax detected {relational.Position}"));

            return;
        }

        public void Visit(RepeatNode repeat)
        {
            var secret = GetLocal(typeof(float));
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();
            emit.LdFloat(0)
                .StLocal(secret)
                .MarkLabel(start)
                .LdLocal(secret);
            repeat.Condition.Accept(this);
            var top = emit.GetTop();
            if (top == typeof(int) || top == typeof(bool))
            {
                emit.ConvertFloat()
                    .Clt();
            }
            else if (top == typeof(TsObject))
                emit.Call(GetOperator("<", typeof(float), top, repeat.Body.Position));
            else if (top == typeof(float))
                emit.Clt();
            else
                _errors.Add(new CompileException($"Invalid syntax detected {repeat.Condition.Position}"));
            emit.BrFalse(end);
            repeat.Body.Accept(this);
            emit.LdLocal(secret)
                .LdFloat(1)
                .Add()
                .StLocal(secret)
                .Br(start)
                .MarkLabel(end);

            FreeLocal(secret);
        }

        public void Visit(ReturnNode returnNode)
        {
            returnNode.ReturnValue.Accept(this);

            if (!emit.TryGetTop(out var returnType))
                _errors.Add(new CompileException($"Tried to return without a return value. If this is expected, use exit instead {returnNode.Position}"));

            if (returnType != null && returnType != typeof(TsObject))
                emit.New(TsTypes.Constructors[returnType]);

            if (_inInstanceScript)
            {
                emit.Call(typeof(TsInstance).GetMethod("get_EventType"))
                    .Call(typeof(Stack<string>).GetMethod("Pop"))
                    .Pop();
            }

            emit.Ret();
        }

        public void Visit(RootNode root)
        {
            foreach(var child in root.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(ScriptNode script)
        {
            var name = script.Value;
            var mb = StartMethod(name, _namespace);
            _inGlobalScript = true;
            ScriptStart(name, mb, new[] { typeof(TsInstance), typeof(TsObject[]) });

            //Process arguments
            ProcessScriptArguments(script);
            
            script.Body.Accept(this);
            
            if (!emit.TryGetTop(out _))
            {
                emit.Call(TsTypes.Empty);
            }

            emit.Ret();

            _inGlobalScript = false;
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

        private void ProcessScriptArguments(ScriptNode script)
        {
            if (script.Arguments.Count > 0)
            {
                emit.LdArg(1);
                for (var i = 0; i < script.Arguments.Count; i++)
                {
                    var arg = script.Arguments[i];
                    VariableToken left;
                    if (arg is AssignNode assign)
                    {
                        left = (VariableToken)assign.Left;
                        if (!_table.Defined(left.Text, out var symbol))
                        {
                            _errors.Add(new CompileException($"Unknown exception occurred {left.Position}"));
                            continue;
                        }
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
                            .LdElem(typeof(TsObject))
                            .StLocal(_locals[symbol])
                            .Br(end)
                            .MarkLabel(lte);
                        //Must be ConstantToken
                        assign.Right.Accept(this);
                        var top = emit.GetTop();
                        if (top != typeof(TsObject))
                            emit.New(TsTypes.Constructors[emit.GetTop()]);

                        emit.StLocal(_locals[symbol])
                            .MarkLabel(end);
                    }
                    else if (arg is VariableToken variable)
                    {
                        if(!_table.Defined(variable.Text, out var symbol))
                        {
                            _errors.Add(new CompileException($"Unknown exception occurred {variable.Position}"));
                            continue;
                        }
                        emit.Dup()
                            .LdInt(i)
                            .LdElem(typeof(TsObject))
                            .StLocal(_locals[symbol]);
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
                _errors.Add(new CompileException($"Tried to shift a string {shift.Left.Position}"));
            GetAddressIfPossible(shift.Right);
            var right = emit.GetTop();
            if (right == typeof(float))
            {
                emit.ConvertInt(true);
                right = typeof(int);
            }
            else if(right == typeof(TsObject) || right == typeof(TsObject).MakePointerType())
            {
                CallInstanceMethod(TsTypes.ObjectCasts[typeof(int)], shift.Right.Position);
                right = typeof(int);
            }
            else
                _errors.Add(new CompileException($"Tried to shift a string {shift.Right.Position}"));

            if (left == typeof(long))
            {
                if (right == typeof(int))
                {
                    emit.Call(GetOperator(shift.Value, left, right, shift.Position))
                        .ConvertFloat();
                }
                else
                    _errors.Add(new CompileException($"Must shift by a real value {shift.Right.Position}"));
            }
            else if (left == typeof(TsObject))
            {
                if (right == typeof(int))
                    emit.Call(GetOperator(shift.Value, left, right, shift.Position));
                else
                    _errors.Add(new CompileException($"Must shift by a real value {shift.Right.Position}"));
            }
            else
                _errors.Add(new CompileException($"Invalid syntax detected {shift.Position}"));
        }

        public void Visit(SwitchNode switchNode)
        {
            var end = emit.DefineLabel();
            _loopEnd.Push(end);

            switchNode.Test.Accept(this);
            var left = emit.GetTop();
            if(left == typeof(int) || left == typeof(bool))
            {
                emit.ConvertFloat();
                left = typeof(float);
            }

            int defaultLocation = -1;
            var cases = new List<ISyntaxElement>(switchNode.Cases);
            var labels = new Label[cases.Count];
            for (var i = 0; i < labels.Length; i++)
            {
                labels[i] = emit.DefineLabel();
                if (cases[i] is CaseNode caseNode)
                {
                    emit.Dup();
                    caseNode.Condition.Accept(this);
                    var right = emit.GetTop();
                    if (right == typeof(int) || right == typeof(bool))
                    {
                        emit.ConvertFloat();
                        right = typeof(float);
                    }
                    TestEquality("==", left, right, caseNode.Condition.Position);
                    var isFalse = emit.DefineLabel();
                    emit.BrFalse(isFalse)
                        .Pop(false)
                        .Br(labels[i])
                        .MarkLabel(isFalse);
                }
                else
                    defaultLocation = i;
            }

            if (defaultLocation != -1)
                emit.Pop()
                    .Br(labels[defaultLocation]);
            else
            {
                emit.Pop()
                    .Br(end);
            }

            for(var i = 0; i < labels.Length; i++)
            {
                emit.MarkLabel(labels[i]);

                if (cases[i] is CaseNode caseNode)
                    caseNode.Expressions.Accept(this);
                else if (cases[i] is DefaultNode defaultNode)
                    defaultNode.Expressions.Accept(this);
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {cases[i].Position}"));
            }

            emit.MarkLabel(end);

            _loopEnd.Pop();
        }

        public void Visit(UsingsNode usings)
        {
            var temp = _table;
            _table = CopyTable(temp);

            foreach (var ns in usings.Modules)
            {
                var count = temp.EnterNamespace(ns.Text);
                CopyTable(temp, _table, ns.Position);
                temp.Exit(count);
            }
            AcceptDeclarations(usings.Declarations);
            _table = temp;
        }

        public void Visit(VariableToken variableToken)
        {
            var name = variableToken.Text;
            if (_table.Defined(name, out var symbol))
            {
                switch(symbol.Type)
                {
                    case SymbolType.Object:
                        UnresolveNamespace();
                        var ns = GetAssetNamespace(symbol);
                        if (ns != "")
                            name = $"{ns}.{name}";
                        emit.LdStr(name);
                        break;
                    case SymbolType.Script:
                        ns = GetAssetNamespace(symbol);
                        if (!_methods.TryGetValue(ns, name, out var method))
                        {
                            method = StartMethod(name, ns);
                            _pendingMethods.Add($"{ns}.{name}".TrimStart('.'), variableToken.Position);
                        }
                        UnresolveNamespace();
                        emit.LdNull()
                            .LdFtn(method)
                            .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                            .LdStr(symbol.Name)
                            .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                            .New(TsTypes.Constructors[typeof(TsDelegate)]);
                        break;
                    case SymbolType.Variable:
                        if (_locals.TryGetValue(symbol, out var local))
                        {
                            if (_needAddress)
                                emit.LdLocalA(local);
                            else
                                emit.LdLocal(local);
                        }
                        else
                            _errors.Add(new CompileException($"Tried to reference a non-existant variable {variableToken.Text} {variableToken.Position}"));
                        break;
                    default:
                        _errors.Add(new NotImplementedException($"Currently cannot reference indentifier {symbol.Type} by it's raw value."));
                        break;
                }
            }
            else
            {
                emit.LdArg(0)
                    .LdStr(variableToken.Text)
                    .Call(typeof(TsInstance).GetMethod("GetVariable", BindingFlags.Public | BindingFlags.Instance));
            }
        }

        public void Visit(WhileNode whileNode)
        {
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();

            emit.MarkLabel(start);
            whileNode.Condition.Accept(this);
            emit.BrFalse(end);
            whileNode.Body.Accept(this);
            emit.Br(start)
                .MarkLabel(end);
        }

        public void Visit(WithNode with)
        {
            with.Condition.Accept(this);
            var top = emit.GetTop();
            if(top == typeof(int) || top == typeof(bool))
            {
                emit.New(TsTypes.Constructors[typeof(int)]);
                top = typeof(TsObject);
            }
            if(top == typeof(float))
            {
                emit.New(TsTypes.Constructors[typeof(float)]);
                top = typeof(TsObject);
            }
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();
            var gen = GetLocal(typeof(InstanceEnumerator));
            var other = GetLocal(typeof(TsInstance));
            _loopStart.Push(start);
            _loopEnd.Push(end);
            emit.Call(typeof(TsInstance).GetMethod("get_Other"))
                .StLocal(other)
                .LdArg(0)
                .Call(typeof(TsInstance).GetMethod("set_Other"))
                .New(typeof(InstanceEnumerator).GetConstructor(new[] { top }))
                .StLocal(gen)
                .MarkLabel(start)
                .LdLocalA(gen)
                .Call(typeof(InstanceEnumerator).GetMethod("MoveNext"))
                .BrFalse(end)
                .LdLocalA(gen)
                .Call(typeof(InstanceEnumerator).GetMethod("get_Current"))
                .StArg(0);
            with.Body.Accept(this);
            emit.MarkLabel(end)
                .Call(typeof(TsInstance).GetMethod("get_Other"))
                .StArg(0)
                .LdLocal(other)
                .Call(typeof(TsInstance).GetMethod("set_Other"));

            FreeLocal(other);
            FreeLocal(gen);


        }

        #endregion
    }
}
