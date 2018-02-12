using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using GmExtern;
using TaffyScript.DotNet;
using TaffyScript.Syntax;
using Myst.Collections;

namespace TaffyScript.Backend
{
    internal partial class MsilWeakCodeGen : ISyntaxElementVisitor
    {
        private const string SpecialImportsFileName = "SpecialImports.resource";
        private const float All = -3f;
        private int _secret = 0;
        private Dictionary<string, ISymbolDocumentWriter> _documents = new Dictionary<string, ISymbolDocumentWriter>();

        private readonly bool _isDebug;
        private readonly AssemblyName _asmName;
        private readonly AssemblyBuilder _asm;
        private ModuleBuilder _module;
        private ILEmitter _moduleInitializer = null;
        private readonly DotNetAssemblyLoader _assemblyLoader;
        private readonly DotNetTypeParser _typeParser;

        private SymbolTable _table;
        private readonly Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();
        private readonly LookupTable<string, string, long> _enums = new LookupTable<string, string, long>();
        //private readonly BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        private readonly BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.Static;
        private readonly Dictionary<string, TypeBuilder> _baseTypes = new Dictionary<string, TypeBuilder>();

        private List<Exception> _errors = new List<Exception>();
        private Dictionary<Type, MethodInfo> _gmObjectCasts;
        private Dictionary<Type, ConstructorInfo> _gmConstructors;
        private Dictionary<string, Type> _gmBasicTypes;
        private MethodInfo _getEmptyObject;
        private MethodInfo _getId;
        private MethodInfo _getIdStack;
        private MethodInfo _pushId;
        private MethodInfo _popId;
        private LookupTable<string, Type, MethodInfo> _unaryOps = new LookupTable<string, Type, MethodInfo>();
        private Dictionary<string, LookupTable<Type, Type, MethodInfo>> _binaryOps = new Dictionary<string, LookupTable<Type, Type, MethodInfo>>();
        private Dictionary<string, string> _operators;
        private HashSet<string> _pendingMethods = new HashSet<string>();
        private System.IO.MemoryStream _stream = null;
        private System.IO.StreamWriter _specialImports = null;
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

        private ILEmitter ModuleInitializer
        {
            get
            {
                if (_moduleInitializer == null)
                {
                    var attribs = MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                    var input = new Type[] { };
                    _moduleInitializer = new ILEmitter(_module.DefineGlobalMethod(".cctor", attribs, typeof(void), input), input, _isDebug);
                }
                return _moduleInitializer;
            }
        }

        private MethodInfo GetIdStack
        {
            get
            {
                if(_getIdStack == null)
                    _getIdStack = typeof(GmObject).GetMethod("get_Id");
                return _getIdStack;
            }
        }

        private MethodInfo PushId
        {
            get
            {
                if (_pushId == null)
                    _pushId = typeof(Stack<GmObject>).GetMethod("Push");
                return _pushId;
            }
        }

        private MethodInfo PopId
        {
            get
            {
                if (_popId == null)
                    _popId = typeof(Stack<GmObject>).GetMethod("Pop");
                return _popId;
            }
        }

        private string _namespace = "";

        //This really shouldn't be a member variable.
        //But it's really inconvenient to pass it around.
        //Should only be accessed inside of scripts and object constructor/destructor.
        private ILEmitter emit;
        private Dictionary<ISymbol, LocalBuilder> _locals = new Dictionary<ISymbol, LocalBuilder>();
        private Stack<Label> _loopStart = new Stack<Label>();
        private Stack<Label> _loopEnd = new Stack<Label>();
        private bool _inScript;
        private bool _needAddress = false;
        private LocalBuilder _id = null;


        public MsilWeakCodeGen(SymbolTable table, MsilWeakBuildConfig config)
        {
            _table = table;
            _isDebug = config.Mode == CompileMode.Debug;
            _asmName = new AssemblyName(System.IO.Path.GetFileName(config.Output));
            _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.Save);
            CustomAttributeBuilder attrib;
            if (_isDebug)
            {
                var aType = typeof(DebuggableAttribute);
                var ctor = aType.GetConstructor(new[] { typeof(DebuggableAttribute.DebuggingModes) });
                attrib = new CustomAttributeBuilder(ctor, new object[] { DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.Default });
                _asm.SetCustomAttribute(attrib);
            }
            _assemblyLoader = new DotNetAssemblyLoader();
            _typeParser = new DotNetTypeParser(_assemblyLoader);
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(GmObject)));
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(Console)));

            foreach (var asm in config.References.Select(s => _assemblyLoader.LoadAssembly(s)))
            {
                if(asm.GetCustomAttribute<WeakLibraryAttribute>() != null)
                {
                    FindValidMethods(asm);
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
                                    var external = input[1];
                                    var owner = external.Remove(external.LastIndexOf('.'));
                                    var methodName = external.Substring(owner.Length + 1);
                                    var type = _typeParser.GetType(owner);
                                    var method = GetMethodToImport(type, methodName, new[] { typeof(GmObject[]) });
                                    _methods[input[0]] = method;
                                }
                            }
                        }
                    }
                }
            }
            InitGmMethods();
            attrib = new CustomAttributeBuilder(typeof(WeakLibraryAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            _asm.SetCustomAttribute(attrib);
        }

        public CompilerResult CompileTree(ISyntaxTree tree)
        {
            var output = ".dll";
            if (_table.Defined("main", out var symbol) && symbol.Type == SymbolType.Script)
                output = ".exe";

            _module = _asm.DefineDynamicModule(_asmName.Name, _asmName.Name + output, _isDebug);

            tree.Root.Accept(this);
            if (_errors.Count != 0)
                return new CompilerResult(_errors);

            foreach (var type in _baseTypes.Values)
                type.CreateType();

            if (_specialImports != null)
            {
                _specialImports.Flush();
                _module.DefineManifestResource(SpecialImportsFileName, _stream, ResourceAttributes.Public);
            }

            if (_moduleInitializer != null)
            {
                _moduleInitializer.Ret();
                _module.CreateGlobalFunctions();
            }

            _asm.Save(_asmName.Name + output);
            if (_specialImports != null)
            {
                _specialImports.Dispose();
                _stream.Dispose();
                _specialImports = null;
                _stream = null;
            }

            return new CompilerResult(_asm, System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _asmName.Name + output));
        }

        private void FindValidMethods(Assembly asm)
        {
            foreach(var type in asm.ExportedTypes)
            {
                if (type.GetCustomAttribute<WeakObjectAttribute>() != null)
                {
                    var parts = type.FullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    for(var i = 0; i < parts.Length; i++)
                        _table.EnterNew(parts[i], i == parts.Length - 1 ? SymbolType.Namespace : SymbolType.Object);
                    for (var i = 0; i < parts.Length; i++)
                        _table.Exit();
                }
                else
                {
                    foreach (var method in type.GetMethods(_methodFlags).Where(mi => IsMethodValid(mi)))
                    {
                        _methods.Add(method.Name, method);
                        _table.AddLeaf(method.Name, SymbolType.Script, SymbolScope.Global);
                    }
                }
            }
        }

        private bool IsMethodValid(MethodInfo method)
        {
            if (method.ReturnType != typeof(GmObject))
                return false;

            var args = method.GetParameters();
            if (args.Length != 1 || args[0].ParameterType != typeof(GmObject[]))
                return false;

            return true;
        }

        private MethodInfo GetMethodToImport(Type owner, string name, Type[] argTypes)
        {
            return owner.GetMethod(name, _methodFlags, null, argTypes, null);
        }

        private MethodBuilder StartMethod(string name)
        {
            if (_methods.TryGetValue(name, out var result))
            {
                //Todo: Define name conflict exception
                var m = result as MethodBuilder;
                if (m == null)
                    _errors.Add(new NameConflictException($"Function with name {name} is already defined by {m.GetModule()}"));
                return m;
            }
            if (!_table.Defined(name, out var symbol) || symbol.Type != SymbolType.Script)
            {
                _errors.Add(new CompileException($"Tried to call an undefined function: {name}"));
                return null;
            }
            var mb = GetBaseType().DefineMethod(name, MethodAttributes.Public | MethodAttributes.Static, typeof(GmObject), new[] { typeof(GmObject[]) });
            if (_isDebug)
                mb.DefineParameter(1, ParameterAttributes.None, "__args_");
            _methods.Add(name, mb);
            return mb;
        }

        private TypeBuilder GetBaseType()
        {
            if(!_baseTypes.TryGetValue(_namespace, out var type))
            {
                var name = $"{_namespace}.BasicType";
                if (name.StartsWith("."))
                    name = name.TrimStart('.');
                type = _module.DefineType(name, TypeAttributes.Public);
                _baseTypes.Add(_namespace, type);
            }
            return type;
        }

        private void GenerateWeakMethodForImport(MethodInfo method, string importName)
        {
            var mb = StartMethod(importName);
            var emit = new ILEmitter(mb, new[] { typeof(GmObject[]) }, _isDebug);
            var paramArray = method.GetParameters();
            var paramTypes = new Type[paramArray.Length];
            for (var i = 0; i < paramArray.Length; ++i)
                paramTypes[i] = paramArray[i].ParameterType;

            for (var i = 0; i < paramTypes.Length; i++)
            {
                emit.LdArg(0)
                    .LdInt(i);
                //Only cast the the GmObject if needed.
                if (paramTypes[i] == typeof(object))
                {
                    emit.LdElem(typeof(GmObject))
                        .Box(typeof(GmObject));
                }
                else if (paramTypes[i] != typeof(GmObject))
                {
                    emit.LdElemA(typeof(GmObject))
                        .Call(_gmObjectCasts[paramTypes[i]]);
                }
                else
                    emit.LdElem(typeof(GmObject));
            }
            emit.Call(method);
            if (method.ReturnType == typeof(void))
                emit.Call(_getEmptyObject);
            else if (_gmConstructors.TryGetValue(method.ReturnType, out var init))
                emit.New(init);
            else if (method.ReturnType != typeof(GmObject))
                _errors.Add(new InvalidProgramException("Imported method had an invalid return type."));

            emit.Ret();
        }

        private void GenerateEntryPoint(MethodInfo entry)
        {
            var input = new[] { typeof(string[]) };
            var method = GetBaseType().DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), input);
            var emit = new ILEmitter(method, input, _isDebug);
            var args = emit.DeclareLocal(typeof(GmObject[]), "args");
            var i = emit.DeclareLocal(typeof(int), "i");
            var count = emit.DeclareLocal(typeof(int), "count");
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();
            //The following IL is equivalent to this method:
            //public static void Main(string[] arg1)
            //{
            //    var count = arg1.Length;
            //    var args = new GmObject[count];
            //    for(var i = 0; i < count; i++)
            //    {
            //        args[i] = new GmObject(arg1[i])
            //    }
            //    main(args);
            //}
            emit.LdArg(0)
                .LdLen()
                .Dup()
                .StLocal(count)
                .NewArr(typeof(GmObject))
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
                .New(_gmConstructors[typeof(string)])
                .StElem(typeof(GmObject))
                .LdLocal(i)
                .LdInt(1)
                .Add()
                .StLocal(i)
                .Br(start)
                .MarkLabel(end)
                .LdLocal(args)
                .Call(entry, 1, typeof(GmObject))
                .Pop()
                .Ret();

            _asm.SetEntryPoint(method);
        }

        private void InitGmMethods()
        {
            var objType = typeof(GmObject);
            _gmObjectCasts = new Dictionary<Type, MethodInfo>()
            {
                { typeof(string), objType.GetMethod("GetString") },
                { typeof(float), objType.GetMethod("GetNum") },
                { typeof(int), objType.GetMethod("GetNumAsInt") },
                { typeof(long), objType.GetMethod("GetNumAsLong") },
                { typeof(bool), objType.GetMethod("GetBool") },
                { typeof(GmInstance), objType.GetMethod("GetInstance") },
                { typeof(GmObject[]), objType.GetMethod("GetArray1D") },
                { typeof(GmObject[][]), objType.GetMethod("GetArray2D") }
            };

            _gmConstructors = new Dictionary<Type, ConstructorInfo>()
            {
                { typeof(bool), objType.GetConstructor(new[] { typeof(bool) }) },
                { typeof(int), objType.GetConstructor(new[] { typeof(int) }) },
                { typeof(float), objType.GetConstructor(new[] { typeof(float) }) },
                { typeof(string), objType.GetConstructor(new[] { typeof(string) }) },
                { typeof(GmObject[]), objType.GetConstructor(new[] { typeof(GmObject[]) }) },
                { typeof(GmObject[][]), objType.GetConstructor(new[] { typeof(GmObject[][]) }) },
            };

            _gmBasicTypes = new Dictionary<string, Type>()
            {
                { "bool", typeof(bool) },
                { "float", typeof(float) },
                { "int", typeof(int) },
                { "string", typeof(string) },
                { "instance", typeof(GmInstance) },
                { "array1d", typeof(GmObject[]) },
                { "array2d", typeof(GmObject[][]) },
                { "object", typeof(GmObject) }
            };

            _operators = new Dictionary<string, string>()
            {
                { "+", "op_Addition" },
                { "&", "op_BitwiseAnd" },
                { "|", "op_BitwiseOr" },
                { "--", "op_Decrement" },
                { "/", "op_Division" },
                { "==", "op_Equality" },
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
                { "true", "op_True" }
            };

            var table = new LookupTable<Type, Type, MethodInfo>();
            table.Add(typeof(string), typeof(string), typeof(string).GetMethod("Concat", _methodFlags, null, new[] { typeof(string), typeof(string) }, null));
            _binaryOps.Add("+", table);

            _getEmptyObject = objType.GetMethod("Empty");
            _getId = objType.GetMethod("GetId");
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

        private MethodInfo GetOperator(string op, Type[] types, TokenPosition pos)
        {
            if (types.Length == 1)
                return GetOperator(op, types[0], pos);
            else if (types.Length == 2)
                return GetOperator(op, types[0], types[1], pos);
            else
            {
                //This doesn't specify a token position, but it also should never be encountered.
                _errors.Add(new CompileException($"Operator {op} does not exist for the type(s) {string.Join(" and ", types.ToList())} {pos}."));
                return null;
            }
        }

        private LocalBuilder MakeSecret()
        {
            var name = $"__0secret{_secret++}";
            return emit.DeclareLocal(typeof(GmObject), name);
        }

        private LocalBuilder MakeSecret(Type type)
        {
            return emit.DeclareLocal(type, $"__0secret_{_secret++}");
        }

        private void GetAddressIfPossible(ISyntaxElement element)
        {
            if (element.Type == SyntaxType.Variable || element.Type == SyntaxType.ArrayAccess || element.Type == SyntaxType.ReadOnlyValue)
            {
                _needAddress = true;
                element.Accept(this);
                _needAddress = false;
            }
            else
                element.Accept(this);
        }

        private void CallInstanceMethod(MethodInfo method, TokenPosition pos)
        {
            var top = emit.GetTop();
            if (top == typeof(GmObject))
            {
                var secret = MakeSecret();
                emit.StLocal(secret)
                    .LdLocalA(secret)
                    .Call(method);
            }
            else if (top == typeof(GmObject).MakePointerType())
                emit.Call(method);
            else
                _errors.Add(new CompileException($"Something went wrong {pos}"));
        }

        private LocalBuilder GetId()
        {
            if (_id == null)
                _id = MakeSecret();
            return _id;
        }

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
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right}"));
                    return;
                }
                else if (right == typeof(GmObject))
                    emit.Call(GetOperator(additive.Value, left, right, additive.Position));
                else
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right}"));
                    return;
                }
            }
            else if(left == typeof(string))
            {
                if (additive.Value != "+")
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right}"));
                    return;
                }
                if (right == typeof(float))
                {
                    _errors.Add(new CompileException($"Cannot {additive.Value} types {left} and {right}"));
                    return;
                }
                else if(right == typeof(string))
                    emit.Call(typeof(string).GetMethod("Concat", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null));
                else if(right == typeof(GmObject))
                    emit.Call(GetOperator(additive.Value, left, right, additive.Position));
            }
            else if(left == typeof(GmObject))
            {
                var method = GetOperator(additive.Value, left, right, additive.Position);
                emit.Call(method);
            }
        }

        public void Visit(ArgumentAccessNode argumentAccess)
        {
            emit.LdArg(0);
            if(argumentAccess.Index is IConstantToken<float> index)
            {
                emit.LdInt((int)index.Value);
            }
            else
            {
                GetAddressIfPossible(argumentAccess.Index);
                var type = emit.GetTop();
                if (type == typeof(float))
                    emit.ConvertInt(false);
                else if (type == typeof(GmObject))
                {
                    var secret = MakeSecret();
                    emit.StLocal(secret)
                        .LdLocalA(secret)
                        .Call(_gmObjectCasts[typeof(int)]);
                }
                else if (type == typeof(GmObject).MakePointerType())
                    emit.Call(_gmObjectCasts[typeof(int)]);
                else if (type != typeof(int))
                    _errors.Add(new CompileException($"Invalid argument access {argumentAccess.Position}"));
            }
            if (_needAddress)
                emit.LdElemA(typeof(GmObject));
            else
                emit.LdElem(typeof(GmObject));
        }

        public void Visit(ArrayAccessNode arrayAccess)
        {
            var address = _needAddress;
            _needAddress = false;
            GetAddressIfPossible(arrayAccess.Left);
            CallInstanceMethod(_gmObjectCasts[arrayAccess.Children.Count == 2 ? typeof(GmObject[]) : typeof(GmObject[][])], arrayAccess.Position);
            GetAddressIfPossible(arrayAccess.Children[1]);
            var top = emit.GetTop();
            if (top == typeof(float))
                emit.ConvertInt(false);
            else if (top != typeof(int) || top != typeof(bool))
                CallInstanceMethod(_gmObjectCasts[typeof(int)], arrayAccess.Position);
            if(arrayAccess.Children.Count == 2)
            {
                if (address)
                    emit.LdElemA(typeof(GmObject));
                else
                    emit.LdElem(typeof(GmObject));
            }
            else
            {
                emit.LdElem(typeof(GmObject[]));
                GetAddressIfPossible(arrayAccess.Children[2]);
                top = emit.GetTop();
                if (top == typeof(float))
                    emit.ConvertInt(false);
                else if (top != typeof(int) || top != typeof(bool))
                    CallInstanceMethod(_gmObjectCasts[typeof(int)], arrayAccess.Position);

                if (address)
                    emit.LdElemA(typeof(GmObject));
                else
                    emit.LdElem(typeof(GmObject));
            }
        }

        public void Visit(ArrayLiteralNode arrayLiteral)
        {
            emit.LdInt(arrayLiteral.Children.Count)
                .NewArr(typeof(GmObject));
            for (var i = 0; i < arrayLiteral.Children.Count; i++)
            {
                emit.Dup()
                    .LdInt(i);

                arrayLiteral.Children[i].Accept(this);
                var type = emit.GetTop();
                if (type != typeof(GmObject))
                    emit.New(_gmConstructors[type]);
                emit.StElem(typeof(GmObject));
            }
            emit.New(_gmConstructors[typeof(GmObject[])]);
        }

        public void Visit(AssignNode assign)
        {
            if(assign.Value != "=")
            {
                ProcessAssignExtra(assign);
                return;
            }

            if (assign.Left is ArrayAccessNode array)
            {
                //Here we have to resize the array if needed, so more work needs to be done.
                GetAddressIfPossible(array.Left);
                if (emit.GetTop() == typeof(GmObject))
                {
                    var secret = MakeSecret();
                    emit.StLocal(secret)
                        .LdLocalA(secret);
                }

                var argTypes = new Type[array.Children.Count];
                for (var i = 1; i < array.Children.Count; i++)
                {
                    array.Children[i].Accept(this);
                    argTypes[i - 1] = emit.GetTop();
                }
                assign.Right.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(GmObject))
                    emit.New(_gmConstructors[top]);
                argTypes[argTypes.Length - 1] = emit.GetTop();
                emit.Call(typeof(GmObject).GetMethod("ArraySet", argTypes));
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
                    if (result != typeof(GmObject))
                        emit.New(_gmConstructors[result]);
                    emit.StLocal(_locals[symbol]);
                }
                else
                {
                    var id = GetId();
                    emit.Call(_getId)
                        .StLocal(id)
                        .LdLocalA(id)
                        .LdStr(variable.Text);
                    assign.Right.Accept(this);
                    var argTypes = new[] { typeof(string), emit.GetTop() };
                    emit.Call(typeof(GmObject).GetMethod("MemberSet", BindingFlags.Public | BindingFlags.Instance, null, argTypes, null));
                }
            }
            else if (assign.Left is MemberAccessNode member)
            {
                GetAddressIfPossible(member.Left);
                var top = emit.GetTop();
                if (top == typeof(GmObject))
                {
                    var secret = MakeSecret();
                    emit.StLocal(secret)
                        .LdLocalA(secret);
                }
                emit.LdStr(((ISyntaxToken)member.Right).Text);
                assign.Right.Accept(this);
                var argTypes = new[] { typeof(string), emit.GetTop() };
                emit.Call(typeof(GmObject).GetMethod("MemberSet", BindingFlags.Public | BindingFlags.Instance, null, argTypes, null));
            }
            else
                _errors.Add(new CompileException($"This assignment is not yet supported {assign.Position}"));
            //Todo: Finish assignments
        }

        private void ProcessAssignExtra(AssignNode assign)
        {
            var op = assign.Value.Replace("=", "");
            if (assign.Left is ArrayAccessNode array)
            {
                //Because this has to access the array location,
                //we can safely just get the address of the array elem and overwrite
                //the the data pointed to by that address.
                GetAddressIfPossible(array);
                emit.Dup()
                    .LdObj(typeof(GmObject));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(GmObject), emit.GetTop(), assign.Position))
                    .StObj(typeof(GmObject));
            }
            else if(assign.Left is VariableToken variable)
            {
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        _errors.Add(new CompileException($"Cannot assign to the value {symbol.Name} {variable.Position}"));
                    GetAddressIfPossible(assign.Left);
                    emit.Dup()
                        .LdObj(typeof(GmObject));
                    assign.Right.Accept(this);
                    emit.Call(GetOperator(op, typeof(GmObject), emit.GetTop(), assign.Position))
                        .StObj(typeof(GmObject));
                }
                else
                    _errors.Add(new CompileException($"This assignment is not yet supported {assign.Position}"));
            }
            else if(assign.Left is MemberAccessNode member)
            {
                member.Left.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(GmObject))
                    _errors.Add(new CompileException($"Invalid syntax detected {member.Position}"));
                var name = ((ISyntaxToken)member.Right).Text;
                var secret = MakeSecret();
                emit.StLocal(secret)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .Call(typeof(GmObject).GetMethod("MemberGet"));
                assign.Right.Accept(this);
                emit.Call(GetOperator(op, typeof(GmObject), emit.GetTop(), assign.Position))
                    .Call(typeof(GmObject).GetMethod("MemberSet", new[] { typeof(string), emit.GetTop() }));
            }
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
                bitwise.Left.Accept(this);
                if (emit.GetTop() != typeof(GmObject))
                    _errors.Add(new CompileException($"Cannot perform operator {bitwise.Value} on the type {emit.GetTop()} {bitwise.Position}"));
                emit.Call(_gmObjectCasts[typeof(long)]);
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
                bitwise.Right.Accept(this);
                if (!emit.TryGetTop(out var top) || top != typeof(GmObject))
                    _errors.Add(new CompileException($"Cannot perform operator {bitwise.Value} on the type {emit.GetTop()} {bitwise.Position}"));
                emit.Call(_gmObjectCasts[typeof(long)]);
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
            var test = conditionalNode.Children[0];
            var left = conditionalNode.Children[1];
            var right = conditionalNode.Children[2];
            
            conditionalNode.Test.Accept(this);
            var brFalse = emit.DefineLabel();
            var brFinal = emit.DefineLabel();
            emit.BrFalse(brFalse);
            conditionalNode.Left.Accept(this);
            emit.Br(brFinal)
                .MarkLabel(brFalse);
            conditionalNode.Right.Accept(this);
            emit.MarkLabel(brFinal);
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
                emit.Call(_getEmptyObject)
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
            else if (type == typeof(GmObject))
                emit.Call(_gmObjectCasts[typeof(bool)]);
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
            //Empty statement. Means nothing in GM.
        }

        public void Visit(EnumNode enumNode)
        {
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

                _enums.Add(enumNode.Value, expr.Value, current++);
            }
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
                else if (right == typeof(GmObject))
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
                else if (right == typeof(string) || right == typeof(GmObject))
                    emit.Call(GetOperator(op, left, right, pos));
                else
                    _errors.Add(new CompileException($"Cannot {op} types {left} and {right} {pos}"));
            }
            else if (left == typeof(GmObject))
                emit.Call(GetOperator(op, left, right, pos));
            else
                _errors.Add(new CompileException($"Cannot {op} types {left} and {right} {pos}"));
        }

        public void Visit(ExitToken exitToken)
        {
            if (_inScript && !emit.TryGetTop(out _))
                emit.Call(_getEmptyObject);
            emit.Ret();
        }

        public void Visit(EventNode eventNode)
        {
            _errors.Add(new CompileException($"Encountered invalid program {eventNode.Position}"));
        }

        public void Visit(ExplicitArrayAccessNode explicitArrayAccess)
        {
            _errors.Add(new NotImplementedException($"Currently explicit array access is not available {explicitArrayAccess.Position}."));
        }

        public void Visit(ForNode forNode)
        {
            var forStart = emit.DefineLabel();
            var forIter = emit.DefineLabel();
            var forFinal = emit.DefineLabel();
            forNode.Initializer.Accept(this);

            _loopStart.Push(forIter);
            _loopEnd.Push(forFinal);

            emit.MarkLabel(forStart);
            forNode.Body.Accept(this);
            emit.MarkLabel(forIter);
            forNode.Iterator.Accept(this);
            forNode.Condition.Accept(this);
            emit.BrTrue(forStart);
            emit.MarkLabel(forFinal);

            _loopStart.Pop();
            _loopEnd.Pop();
        }

        public void Visit(FunctionCallNode functionCall)
        {
            //All methods callable from GML should have the same sig:
            //GmObject func_name(GmObject[]);
            var name = functionCall.Value;
            if (!_methods.TryGetValue(name, out var method))
            {
                method = StartMethod(name);
                _pendingMethods.Add(name);
            }

            if (_isDebug && functionCall.Position.File != null)
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
                MarkSequencePoint(functionCall.Position.File,
                                  functionCall.Position.Line,
                                  functionCall.Position.Column,
                                  line,
                                  end);
            }

            emit.LdInt(functionCall.Children.Count)
                .NewArr(typeof(GmObject));

            for(var i = 0; i < functionCall.Children.Count; ++i)
            {
                emit.Dup()
                    .LdInt(i);

                functionCall.Children[i].Accept(this);
                var top = emit.GetTop();
                if(top != typeof(GmObject))
                    emit.New(_gmConstructors[top]);

                emit.StElem(typeof(GmObject));
            }
            //The argument array should still be on top.
            emit.Call(method, 1, typeof(GmObject));
        }

        public void Visit(GridAccessNode gridAccess)
        {
            _errors.Add(new NotImplementedException($"Currently grid access is not available {gridAccess.Position}."));
        }

        public void Visit(IfNode ifNode)
        {
            // If there is no else node, just let execution flow naturally.
            // Otherwise, bracnh to the end forcefully at the end of the if body.

            var ifFalse = emit.DefineLabel();
            var ifTrue = emit.DefineLabel();
            ifNode.Condition.Accept(this);
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
            var argWrappers = import.GetArguments();
            var args = new Type[argWrappers.Count];
            var externalName = import.ExternalName.Value;
            var internalName = import.InternalName.Value;
            _pendingMethods.Remove(internalName);

            for (var i = 0; i < args.Length; i++)
            {
                if (!_gmBasicTypes.TryGetValue(argWrappers[i].Value, out var argType))
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
            }
            if (method.GetCustomAttribute<WeakMethodAttribute>() != null && IsMethodValid(method))
            {
                _methods.Add(internalName, method);
                SpecialImports.Write(internalName);
                SpecialImports.Write(':');
                SpecialImports.WriteLine(externalName);
            }
            else
                GenerateWeakMethodForImport(method, internalName);
        }

        public void Visit(ListAccessNode listAccess)
        {
            _errors.Add(new NotImplementedException($"Currently list access is not available {listAccess.Position}."));
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

            logical.Left.Accept(this);
            var left = emit.GetTop();
            if (left == typeof(string))
                emit.Pop().LdInt(0);
            else if (left == typeof(float))
                emit.LdFloat(.5f).Cge();
            else if (left == typeof(GmObject))
                emit.Call(_gmObjectCasts[typeof(bool)]);
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

            logical.Right.Accept(this);
            var right = emit.GetTop();
            if (right == typeof(string))
                emit.Pop().LdInt(0);
            else if (left == typeof(float))
                emit.LdFloat(.5f).Cge();
            else if (left == typeof(GmObject))
                emit.Call(_gmObjectCasts[typeof(bool)]);
            else if (left != typeof(bool))
                _errors.Add(new CompileException($"Encountered invalid syntax {logical.Right.Position}"));

            emit.MarkLabel(end);
        }

        public void Visit(MapAccessNode mapAccess)
        {
            _errors.Add(new NotImplementedException($"Currently map access is not available {mapAccess.Position}."));
        }

        public void Visit(MemberAccessNode memberAccess)
        {
            //If left is enum name, find it in _enums.
            //Otherwise, Accept left, and call member access on right.
            if (memberAccess.Left is VariableToken enumVar && _table.Defined(enumVar.Text, out var symbol) && symbol.Type == SymbolType.Enum)
            {
                if (memberAccess.Right is VariableToken enumValue)
                {
                    if (!_enums.TryGetValue(enumVar.Text, enumValue.Text, out var value))
                        _errors.Add(new CompileException($"The enum {enumVar.Text} does not declare value {enumValue.Text} {enumValue.Position}"));
                    emit.LdFloat(value);
                }
                else
                    _errors.Add(new CompileException($"Invalid enum access syntax {enumVar.Position}"));
            }
            else
            {
                GetAddressIfPossible(memberAccess.Left);
                var left = emit.GetTop();
                if(left == typeof(GmObject))
                {
                    var secret = MakeSecret();
                    emit.StLocal(secret)
                        .LdLocalA(secret);
                }
                else if(left != typeof(GmObject).MakePointerType())
                {

                    //Todo: MemberAccess class variables.
                    //There won't be any static class variables,
                    //instead it will set the variable on the first instance of the type.
                    //Alternatively, you might just have it set the variable on all instances of that type,
                    //but that would break GM compatibility.
                    _errors.Add(new NotImplementedException($"Accessing a variable through a type is not yet supported {memberAccess.Left} {memberAccess.Left.Position}"));
                }
                if (memberAccess.Right is VariableToken right)
                {
                    emit.LdStr(right.Text)
                        .Call(typeof(GmObject).GetMethod("MemberGet", new[] { typeof(string) }));
                }
                else if (memberAccess.Right is ReadOnlyToken readOnly)
                {
                    if (readOnly.Text != "id" && readOnly.Text != "self")
                        _errors.Add(new NotImplementedException($"Only the read only variables id and self can be accessed from an instance currently {readOnly.Position}"));
                    emit.LdStr("id")
                        .Call(typeof(GmObject).GetMethod("MemberGet", new[] { typeof(string) }));
                }
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {memberAccess.Position}"));
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
                    emit.Mul();
                else if (right == typeof(string))
                    _errors.Add(new CompileException($"Cannot {multiplicative.Value} types {left} and {right} {multiplicative.Position}"));
                else if (right == typeof(GmObject))
                    emit.Call(GetOperator(multiplicative.Value, left, right, multiplicative.Position));
                else
                    _errors.Add(new CompileException($"Cannot {multiplicative.Value} types {left} and {right} {multiplicative.Position}"));
            }
            else if (left == typeof(string)) {
                _errors.Add(new CompileException($"Cannot {multiplicative.Value} types {left} and {right} {multiplicative.Position}"));
            }
            else if (left == typeof(GmObject))
                emit.Call(GetOperator(multiplicative.Value, left, right, multiplicative.Position));
        }

        public void Visit(ObjectNode objectNode)
        {
            var name = $"{_namespace}.{objectNode.Value}";
            if (name.StartsWith("."))
                name = name.TrimStart('.');
            var type = _module.DefineType(name, TypeAttributes.Public);
            _table.Enter(objectNode.Value);
            var input = new[] { typeof(GmInstance) };
            var addMethod = typeof(Dictionary<string, InstanceEvent>).GetMethod("Add");
            var objectIds = typeof(GmInstance).GetField("ObjectIndexMapping");
            var events = typeof(GmInstance).GetField("Events");
            var addObjectId = typeof(Dictionary<Type, string>).GetMethod("Add");
            var init = ModuleInitializer;
            init.LdFld(objectIds)
                .LdType(type)
                .Call(typeof(Type).GetMethod("GetTypeFromHandle"))
                .LdStr(name)
                .Call(addObjectId)
                .LdFld(events)
                .LdStr(name)
                .New(typeof(Dictionary<string, InstanceEvent>).GetConstructor(Type.EmptyTypes));

            foreach (EventNode ev in objectNode.Children)
            {
                var method = type.DefineMethod(ev.Value, MethodAttributes.Public | MethodAttributes.Static, typeof(void), input);
                emit = new ILEmitter(method, input, _isDebug);
                emit.Call(GetIdStack)
                    .LdArg(0)
                    .Call(typeof(GmInstance).GetMethod("get_Id"))
                    .New(_gmConstructors[typeof(float)])
                    .Call(PushId);
                ev.Body.Accept(this);
                emit.Call(GetIdStack)
                    .Call(PopId)
                    .Pop()
                    .Ret();

                _id = null;

                init.Dup()
                    .LdStr(ev.Value)
                    .LdNull()
                    .LdFtn(method)
                    .New(typeof(InstanceEvent).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                    .Call(addMethod);
            }
            init.Call(typeof(Dictionary<string, Dictionary<string, InstanceEvent>>).GetMethod("Add"));

            var attrib = new CustomAttributeBuilder(typeof(WeakObjectAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            type.SetCustomAttribute(attrib);
            type.CreateType();

            _table.Exit();
        }

        public void Visit(PostfixNode postfix)
        {
            if (postfix.Child is ArrayAccessNode array)
            {
                var secret = MakeSecret();
                GetAddressIfPossible(array);
                emit.Dup()
                    .Dup()
                    .LdObj(typeof(GmObject))
                    .StLocal(secret)
                    .LdObj(typeof(GmObject))
                    .Call(GetOperator(postfix.Value, typeof(GmObject), postfix.Position))
                    .StObj(typeof(GmObject))
                    .LdLocal(secret);
            }
            else if (postfix.Child is VariableToken variable)
            {
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        _errors.Add(new CompileException($"Cannot perform {postfix.Value} on an identifier that is not a variable {postfix.Position}"));
                    var secret = MakeSecret();
                    GetAddressIfPossible(variable);
                    emit.Dup()
                        .Dup()
                        .LdObj(typeof(GmObject))
                        .StLocal(secret)
                        .LdObj(typeof(GmObject))
                        .Call(GetOperator(postfix.Value, typeof(GmObject), postfix.Position))
                        .StObj(typeof(GmObject))
                        .LdLocal(secret);
                }
                else
                    _errors.Add(new CompileException($"Cannot perform {postfix.Value} on an identifier that is not a variable {postfix.Position}"));
            }
            else if (postfix.Child is MemberAccessNode member)
            {
                member.Left.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(GmObject))
                    _errors.Add(new CompileException($"Cannot access member on type {top} {member.Left.Position}"));
                var name = ((ISyntaxToken)member.Right).Text;
                var secret = MakeSecret();
                var value = MakeSecret();
                emit.StLocal(secret)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .Call(typeof(GmObject).GetMethod("MemberGet"))
                    .StLocal(value)
                    .Call(typeof(GmObject).GetMethod("MemberGet"))
                    .Call(GetOperator(postfix.Value, typeof(GmObject), postfix.Position))
                    .Call(typeof(GmObject).GetMethod("MemberSet", new[] { typeof(string), emit.GetTop() }))
                    .LdLocal(value);
            }
            else
                _errors.Add(new CompileException($"Invalid syntax detected {postfix.Child.Position}"));
        }

        public void Visit(PrefixNode prefix)
        {
            if (prefix.Child is ArrayAccessNode array)
            {
                GetAddressIfPossible(array);
                emit.Dup()
                    .Dup()
                    .LdObj(typeof(GmObject))
                    .Call(GetOperator(prefix.Value, typeof(GmObject), prefix.Position))
                    .StObj(typeof(GmObject))
                    .LdObj(typeof(GmObject));
            }
            else if (prefix.Child is VariableToken variable)
            {
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        _errors.Add(new CompileException($"Tried to access an identifier that wasn't a variable {prefix.Child.Position}"));
                    variable.Accept(this);
                    emit.Call(GetOperator(prefix.Value, typeof(GmObject), prefix.Position))
                        .StLocal(_locals[symbol])
                        .LdLocal(_locals[symbol]);
                }
                else
                    _errors.Add(new CompileException($"Invalid syntax detected {prefix.Child.Position}"));
            }
            else if (prefix.Child is MemberAccessNode member)
            {
                member.Left.Accept(this);
                var top = emit.GetTop();
                if (top != typeof(GmObject))
                    _errors.Add(new CompileException($"Invalid syntax detected {prefix.Child.Position}"));
                var name = ((ISyntaxToken)member.Right).Text;
                var secret = MakeSecret();
                emit.StLocal(secret)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .LdLocalA(secret)
                    .LdStr(name)
                    .Call(typeof(GmObject).GetMethod("MemberGet"))
                    .Call(GetOperator(prefix.Value, typeof(GmObject), prefix.Position))
                    .Call(typeof(GmObject).GetMethod("MemberSet", new[] { typeof(string), emit.GetTop() }))
                    .Call(typeof(GmObject).GetMethod("MemberGet"));
            }
            else
                _errors.Add(new CompileException($"Invalid syntax detected {prefix.Position}"));
        }

        public void Visit(ReadOnlyToken readOnlyToken)
        {
            switch(readOnlyToken.Text)
            {
                case "self":
                case "id":
                    emit.Call(_getId);
                    break;
                case "argument_count":
                    emit.LdArg(0)
                        .LdLen()
                        .ConvertFloat();
                    break;
                case "global":
                    if (_needAddress)
                        emit.LdFldA(typeof(GmObject).GetField("Global"));
                    else
                        emit.LdFld(typeof(GmObject).GetField("Global"));
                    break;
                case "pi":
                    emit.LdFloat((float)Math.PI);
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
                else if (right == typeof(GmObject))
                    emit.Call(GetOperator(relational.Value, left, right, relational.Position));
            }
            else if (left == typeof(GmObject))
                emit.Call(GetOperator(relational.Value, left, right, relational.Position));
            else
                _errors.Add(new CompileException($"Invalid syntax detected {relational.Position}"));

            return;
        }

        public void Visit(RepeatNode repeat)
        {
            var secret = MakeSecret(typeof(float));
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
            else if (top == typeof(GmObject))
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
        }

        public void Visit(ReturnNode returnNode)
        {
            returnNode.ReturnValue.Accept(this);

            if (!emit.TryGetTop(out var returnType))
                _errors.Add(new CompileException($"Tried to return without a return value. If this is expected, use exit instead {returnNode.Position}"));

            if (returnType != typeof(GmObject))
                emit.New(_gmConstructors[returnType]);

            emit.Ret();
        }

        public void Visit(RootNode root)
        {
            foreach(var child in root.Children)
            {
                //Todo: Create exception for invalid node type.
                //Todo: Process everything before scripts.
                //      Alternatively, process titles, then go through sequentially.
                if (child.Type != SyntaxType.Enum && child.Type != SyntaxType.Import && child.Type != SyntaxType.Script && child.Type != SyntaxType.Object)
                    _errors.Add(new CompileException($"Invalid syntax detected {child.Position}"));
                else
                    child.Accept(this);
            }
        }

        public void Visit(ScriptNode script)
        {
            var name = script.Value;
            _pendingMethods.Remove(name);
            _table.Enter(name);
            var mb = StartMethod(name);
            emit = new ILEmitter(mb, new[] { typeof(GmObject[]) }, _isDebug);
            _locals = new Dictionary<ISymbol, LocalBuilder>();
            _inScript = true;
            foreach (var symbol in _table.Symbols.Where(s => s.Type == SymbolType.Variable))
                _locals.Add(symbol, emit.DeclareLocal(typeof(GmObject), symbol.Name));

            script.Body.Accept(this);
            
            if (!emit.TryGetTop(out _))
            {
                emit.Call(_getEmptyObject);
            }

            emit.Ret();

            _inScript = false;
            _table.Exit();
            _id = null;

            var init = ModuleInitializer;
            init.LdFld(typeof(GmInstance).GetField("Functions"))
                .LdStr(name)
                .LdNull()
                .LdFtn(mb)
                .New(typeof(TaffyFunction).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .Call(typeof(Dictionary<string, TaffyFunction>).GetMethod("Add"));

            if (name == "main")
                GenerateEntryPoint(mb);
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
            else if(right == typeof(GmObject) || right == typeof(GmObject).MakePointerType())
            {
                CallInstanceMethod(_gmObjectCasts[typeof(int)], shift.Right.Position);
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
            else if (left == typeof(GmObject))
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

        public void Visit(VariableToken variableToken)
        {
            var name = variableToken.Text;
            if(_table.Defined(name, out var symbol))
            {
                switch(symbol.Type)
                {
                    case SymbolType.Object:
                    case SymbolType.Script:
                        emit.LdStr(name);
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
                var id = GetId();

                emit.Call(_getId)
                    .StLocal(id)
                    .LdLocalA(id)
                    .LdStr(variableToken.Text)
                    .Call(typeof(GmObject).GetMethod("MemberGet", new[] { typeof(string) }));
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
                emit.New(_gmConstructors[typeof(int)]);
                top = typeof(GmObject);
            }
            if(top == typeof(float))
            {
                emit.New(_gmConstructors[typeof(float)]);
                top = typeof(GmObject);
            }
            var start = emit.DefineLabel();
            var end = emit.DefineLabel();
            var final = emit.DefineLabel();
            var gen = MakeSecret(typeof(InstanceEnumerator));
            _loopStart.Push(start);
            _loopEnd.Push(end);
            emit.New(typeof(InstanceEnumerator).GetConstructor(new[] { top }))
                .StLocal(gen)
                .MarkLabel(start)
                .LdLocalA(gen)
                .Call(typeof(InstanceEnumerator).GetMethod("MoveNext"))
                .BrFalse(final)
                .Call(GetIdStack)
                .LdLocalA(gen)
                .Call(typeof(InstanceEnumerator).GetMethod("get_Current"))
                .Call(PushId);
            with.Body.Accept(this);
            emit.MarkLabel(end)
                .Call(GetIdStack)
                .Call(PopId)
                .Pop()
                .MarkLabel(final);
        }

        #endregion
    }
}
