using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using GmExtern;
using GmParser.DotNet;
using GmParser.Syntax;
using Myst.Collections;

namespace GmParser.Backend
{
    internal partial class MsilWeakCodeGen : ISyntaxElementVisitor
    {
        private int _secret = 0;

        private readonly bool _isDebug;
        private readonly AssemblyName _asmName;
        private readonly AssemblyBuilder _asm;
        private ModuleBuilder _module;
        private readonly DotNetAssemblyLoader _assemblyLoader;
        private readonly DotNetTypeParser _typeParser;

        private SymbolTable _table;
        private readonly Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();
        private readonly LookupTable<string, string, long> _enums = new LookupTable<string, string, long>();
        //private readonly BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        private readonly BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.Static;
        private readonly Dictionary<string, TypeBuilder> _baseTypes = new Dictionary<string, TypeBuilder>();

        private Dictionary<Type, MethodInfo> _gmObjectCasts;
        private Dictionary<Type, ConstructorInfo> _gmConstructors;
        private Dictionary<string, Type> _gmBasicTypes;
        private MethodInfo _getEmptyObject;
        private MethodInfo _getId;
        private LookupTable<string, Type, MethodInfo> _unaryOps = new LookupTable<string, Type, MethodInfo>();
        private Dictionary<string, LookupTable<Type, Type, MethodInfo>> _binaryOps = new Dictionary<string, LookupTable<Type, Type, MethodInfo>>();
        private Dictionary<string, string> _operators;
        private HashSet<string> _pendingMethods = new HashSet<string>();


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


        public MsilWeakCodeGen(string name, MsilWeakBuildConfig config)
        {
            _isDebug = config.Mode == CompileMode.Debug;
            _asmName = new AssemblyName(name);
            _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.Save);
            _assemblyLoader = new DotNetAssemblyLoader();
            _typeParser = new DotNetTypeParser(_assemblyLoader);
            foreach(var asm in config.References.Select(s => _assemblyLoader.LoadAssembly(s)))
            {
                if(asm.GetCustomAttribute<WeakLibraryAttribute>() != null)
                {
                    FindValidMethods(asm);
                }
            }
            InitGmMethods();
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(GmObject)));
            _assemblyLoader.InitializeAssembly(Assembly.GetAssembly(typeof(Console)));
            var attrib = new CustomAttributeBuilder(typeof(WeakLibraryAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            _asm.SetCustomAttribute(attrib);
        }

        public void CompileTree(ISyntaxTree tree, SymbolTable table)
        {
            _table = table;
            var output = ".dll";
            if (_table.Defined("main", out var symbol) && symbol.Type == SymbolType.Script)
                output = ".exe";

            _module = _asm.DefineDynamicModule(_asmName.Name, _asmName.Name + output, _isDebug);

            tree.Root.Accept(this);
            foreach (var type in _baseTypes.Values)
                type.CreateType();
            _asm.Save(_asmName.Name + output);
        }

        private void FindValidMethods(Assembly asm)
        {
            foreach(var type in asm.ExportedTypes)
            {
                foreach(var method in type.GetMethods(_methodFlags).Where(mi => IsMethodValid(mi)))
                {
                    _methods.Add(method.Name, method);
                    _table.AddLeaf(method.Name, SymbolType.Script, SymbolScope.Global);
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
                return (result as MethodBuilder) ?? throw new InvalidProgramException();
            }
            var mb = GetBaseType().DefineMethod(name, MethodAttributes.Public | MethodAttributes.Static, typeof(GmObject), new[] { typeof(GmObject[]) });
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
                type = _module.DefineType(name);
                _baseTypes.Add(_namespace, type);
            }
            return type;
        }

        private void GenerateWeakMethodForImport(MethodInfo method, string importName, Type[] argTypes)
        {
            var mb = StartMethod(importName);
            var emit = new ILEmitter(mb, new[] { typeof(GmObject[]) }, _isDebug);
            for (var i = 0; i < argTypes.Length; i++)
            {
                emit.LdArg(0)
                    .LdInt(i);
                //Only cast the the GmObject if needed.
                if (argTypes[i] == typeof(object))
                {
                    emit.LdElem(typeof(GmObject))
                        .Box(typeof(GmObject));
                }
                else if (argTypes[i] != typeof(GmObject))
                {
                    emit.LdElemA(typeof(GmObject))
                        .Call(_gmObjectCasts[argTypes[i]]);
                }
                else
                    emit.LdElem(typeof(GmObject));
            }
            emit.Call(method);
            if (method.ReturnType == typeof(void))
                emit.Call(_getEmptyObject);
            else if (_gmConstructors.TryGetValue(method.ReturnType, out var init))
                emit.New(init);
            else
                throw new InvalidProgramException("Imported method had an invalid return type.");

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
                { typeof(double), objType.GetMethod("GetNum") },
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
                { "double", typeof(double) },
                { "string", typeof(string) },
                { "instance", typeof(GmInstance) },
                { "array1d", typeof(GmObject[]) },
                { "array2d", typeof(GmObject[][]) },
                { "object", typeof(object) }
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
        private MethodInfo GetOperator(string op, Type type)
        {
            if(!_unaryOps.TryGetValue(op, type, out var method))
            {
                string name;
                if (op == "-")
                    name = "op_UnaryNegation";
                else if (op == "+")
                    name = "op_UnaryPlus";
                else if (!_operators.TryGetValue(op, out name))
                    throw new InvalidOperationException();
                method = type.GetMethod(name, _methodFlags, null, new[] { type }, null)
                    ?? throw new InvalidOperationException();
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
        private MethodInfo GetOperator(string op, Type left, Type right)
        {
            if (!_binaryOps.TryGetValue(op, out var table))
            {
                table = new LookupTable<Type, Type, MethodInfo>();
                _binaryOps.Add(op, table);
            }

            if(!table.TryGetValue(left, right, out var method))
            {
                if (!_operators.TryGetValue(op, out var opName))
                    throw new InvalidOperationException();

                var argTypes = new Type[] { left, right };
                method = left.GetMethod(opName, _methodFlags, null, argTypes, null) ??
                         right.GetMethod(opName, _methodFlags, null, argTypes, null) ??
                         throw new InvalidOperationException();
            }

            return method;
        }

        private MethodInfo GetOperator(string op, Type[] types)
        {
            if (types.Length == 1)
                return GetOperator(op, types[0]);
            else if (types.Length == 2)
                return GetOperator(op, types[0], types[1]);
            else
                throw new InvalidOperationException();
        }

        private LocalBuilder MakeSecret()
        {
            var name = $"__0secret{_secret++}";
            return emit.DeclareLocal(typeof(GmObject), name);
        }

        #region Visitor

        public void Visit(AdditiveNode additive)
        {
            //Todo: Process ReadOnlyValues.

            if (additive.Left.Type == SyntaxType.ReadOnlyValue || additive.Right.Type == SyntaxType.ReadOnlyValue)
                throw new NotImplementedException();

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
                    throw new InvalidOperationException();
                else if (right == typeof(GmObject))
                    emit.Call(GetOperator(additive.Value, left, right));
                else
                    throw new InvalidOperationException();
            }
            else if(left == typeof(string))
            {
                if (additive.Value != "+")
                    throw new InvalidOperationException();
                if (right == typeof(float))
                    throw new InvalidOperationException();
                else if(right == typeof(string))
                    emit.Call(typeof(string).GetMethod("Concat", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(string) }, null));
                else if(right == typeof(GmObject))
                    emit.Call(GetOperator(additive.Value, left, right));
            }
            else if(left == typeof(GmObject))
            {
                var method = GetOperator(additive.Value, left, right)
                    ?? throw new InvalidOperationException();
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
                // Todo: Optimize Argument Access
                // If Index == Variable || Index == MemberAccess, get address,
                // otherwise, get value.
                if(argumentAccess.Index.Type == SyntaxType.Variable)
                {
                    _needAddress = true;
                    argumentAccess.Index.Accept(this);
                    _needAddress = false;
                }
                else
                    argumentAccess.Index.Accept(this);
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
                else if(type == typeof(GmObject).MakePointerType())
                    emit.Call(_gmObjectCasts[typeof(int)]);
                else if (type != typeof(int))
                    throw new InvalidProgramException();
            }
            //Todo: Test if this can be LdElem or if it HAS to be LdElemA
            emit.LdElem(typeof(GmObject));
        }

        public void Visit(ArrayAccessNode arrayAccess)
        {
            arrayAccess.Left.Accept(this);
            if (emit.GetTop() != typeof(GmObject))
                throw new InvalidProgramException();
            var types = new Type[arrayAccess.Children.Count - 1];
            for(var i = 1; i < arrayAccess.Children.Count; i++)
            {
                arrayAccess.Children[i].Accept(this);
                types[i - 1] = emit.GetTop();
            }
            emit.Call(typeof(GmObject).GetMethod("ArrayGet", BindingFlags.Public | BindingFlags.Instance, null, types, null));
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
        }

        public void Visit(AssignNode assign)
        {
            //Todo: allow x = y = 6;
            //Todo: allow x += 4;
            if(assign.Right is AssignNode inner)
            {
                Visit(inner);
                assign.Children[1] = inner.Left;
            }
            if (assign.Left is ArrayAccessNode array)
            {
                array.Left.Accept(this);
                if (emit.GetTop() != typeof(GmObject))
                    throw new InvalidProgramException();

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
                var setter = typeof(GmObject).GetMethod("ArraySet", BindingFlags.Public | BindingFlags.Instance, null, argTypes, null);
                emit.Call(setter);
            }
            else if (assign.Left is VariableToken variable)
            {
                //Check if the variable is a local variable.
                //If not, then it MUST be a member var.
                if (_table.Defined(variable.Text, out var symbol))
                {
                    if (symbol.Type != SymbolType.Variable)
                        throw new InvalidSymbolException();
                    assign.Right.Accept(this);
                    var result = emit.GetTop();
                    if (result != typeof(GmObject))
                        emit.New(_gmConstructors[result]);
                    emit.StLocal(_locals[symbol]);
                }
                else
                {
                    emit.Call(_getId)
                        .LdStr(variable.Text);
                    assign.Right.Accept(this);
                    var argTypes = new[] { typeof(string), emit.GetTop() };
                    emit.Call(typeof(GmObject).GetMethod("SetMember", BindingFlags.Public | BindingFlags.Instance, null, argTypes, null));
                }
            }
            else if (assign.Left is MemberAccessNode member)
            {
                if (member.Right.Type == SyntaxType.ReadOnlyValue)
                    throw new InvalidProgramException("Tried to access a readonly variable.");
                member.Left.Accept(this);
                emit.LdStr(((ISyntaxToken)member.Right).Text);
                assign.Right.Accept(this);
                var argTypes = new[] { typeof(string), emit.GetTop() };
                emit.Call(typeof(GmObject).GetMethod("SetMember", BindingFlags.Public | BindingFlags.Instance, null, argTypes, null));
            }
            else
                throw new NotImplementedException();
            //Todo: Finish assignments
        }

        public void Visit(BitwiseNode bitwise)
        {
            if(bitwise.Left.Type == SyntaxType.Constant)
            {
                var leftConst = bitwise.Left as IConstantToken<float> 
                    ?? throw new InvalidOperationException();
                emit.LdLong((long)leftConst.Value);
            }
            else
            {
                bitwise.Left.Accept(this);
                if (emit.GetTop() != typeof(GmObject))
                    throw new InvalidOperationException();
                emit.Call(_gmObjectCasts[typeof(long)]);
            }
            if(bitwise.Right.Type == SyntaxType.Constant)
            {
                var rightConst = bitwise.Right as IConstantToken<float>
                    ?? throw new InvalidOperationException();
                emit.LdLong((long)rightConst.Value);
            }
            else
            {
                bitwise.Right.Accept(this);
                if (!emit.TryGetTop(out var top) || top != typeof(GmObject))
                    throw new InvalidOperationException();
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
                    throw new InvalidOperationException();
            }
            emit.ConvertFloat();
        }

        public void Visit(BlockNode block)
        {
            var size = block.Children.Count;
            for (var i = 0; i < size; ++i)
                block.Children[i].Accept(this);
        }

        public void Visit(BreakToken breakToken)
        {
            emit.Br(_loopEnd.Peek());
        }

        public void Visit(CaseNode caseNode)
        {
            throw new NotImplementedException();
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
            //throw new NotImplementedException();
            Console.WriteLine("Should I do something on declare?");
        }

        public void Visit(DefaultNode defaultNode)
        {
            throw new NotImplementedException();
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
            //throw new NotImplementedException();
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
                        throw new InvalidSymbolException("Enum value must be equal to an integer constant.");
                }
                else if (expr.Type != SyntaxType.Declare)
                    throw new InvalidProgramException($"Encountered error while compiling enum {enumNode.Value}");

                _enums.Add(enumNode.Value, expr.Value, current++);
            }
        }

        public void Visit(EqualityNode equality)
        {
            //Todo: Add release mode optimization

            var name = equality.Value == "==" ? "op_Equality" : "op_Inequality";

            if (equality.Left is IConstantToken left && equality.Right is IConstantToken right)
            {
                if(left is IConstantToken<string> leftString)
                {
                    if (right is IConstantToken<string> rightString)
                    {
                        emit.LdStr(leftString.Value)
                            .LdStr(rightString.Value)
                            .Call(typeof(string).GetMethod(name, new[] { typeof(string), typeof(string) }));
                    }
                    else
                        emit.LdInt(0);
                }
                else if(left is IConstantToken<float> leftReal)
                {
                    if (right is IConstantToken<float> rightReal)
                    {
                        emit.LdFloat(leftReal.Value)
                            .LdFloat(rightReal.Value);

                        if (equality.Value == "==")
                            emit.Ceq();
                        else
                            emit.Neq();
                    }
                    else if (right is IConstantToken<bool> rightBool)
                    {
                        emit.LdFloat(leftReal.Value)
                            .LdFloat(rightBool.Value ? 1f : 0f);

                        if (equality.Value == "==")
                            emit.Ceq();
                        else
                            emit.Neq();
                    }
                    else
                        emit.LdInt(0);
                }
                else if(left is IConstantToken<bool> leftBool)
                {
                    if (right is IConstantToken<bool> rightBool)
                    {
                        emit.LdInt(leftBool.Value ? 1 : 0)
                            .LdInt(rightBool.Value ? 1 : 0);

                        if (equality.Value == "==")
                            emit.Ceq();
                        else
                            emit.Neq();
                    }
                    else if (right is IConstantToken<float> rightReal)
                    {
                        emit.LdFloat(leftBool.Value ? 1 : 0)
                            .LdFloat(rightReal.Value);

                        if (equality.Value == "==")
                            emit.Ceq();
                        else
                            emit.Neq();
                    }
                    else
                        emit.LdInt(0);
                }
            }
            else
            {
                var argTypes = new Type[2];
                equality.Left.Accept(this);
                argTypes[0] = emit.GetTop();
                equality.Right.Accept(this);
                argTypes[1] = emit.GetTop();

                var opAddition = typeof(GmObject).GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, argTypes, null)
                    ?? throw new NotImplementedException();

                emit.Call(opAddition);
            }
        }

        public void Visit(ExitToken exitToken)
        {
            if (_inScript)
                emit.Call(_getEmptyObject);
            emit.Ret();
        }

        public void Visit(ExplicitArrayAccessNode explicitArrayAccess)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                    throw new InvalidProgramException();

                args[i] = argType;
            }

            var typeName = externalName.Remove(externalName.LastIndexOf('.'));
            var methodName = externalName.Substring(typeName.Length + 1);
            var owner = _typeParser.GetType(typeName);
            var method = GetMethodToImport(owner, methodName, args) ?? throw new InvalidProgramException();
            GenerateWeakMethodForImport(method, internalName, args);
        }

        public void Visit(ListAccessNode listAccess)
        {
            throw new NotImplementedException();
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
                throw new InvalidOperationException();
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
                throw new InvalidOperationException();

            emit.MarkLabel(end);
        }

        public void Visit(MapAccessNode mapAccess)
        {
            throw new NotImplementedException();
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
                        throw new InvalidOperationException();
                    emit.LdFloat(value);
                }
                else
                    throw new InvalidOperationException();
            }
            else
            {
                memberAccess.Left.Accept(this);
                var left = emit.GetTop();
                if(left != typeof(GmObject))
                {
                    //Todo: MemberAccess class variables.
                    //There won't be any static class variables,
                    //instead it will set the variable on the first instance of the type.
                    //Alternatively, you might just have it set the variable on all instances of that type,
                    //but that would break GM compatibility.
                    throw new NotImplementedException();
                }
                if (memberAccess.Right is VariableToken right)
                {
                    emit.LdStr(right.Text)
                        .Call(typeof(GmObject).GetMethod("MemberGet", BindingFlags.Public, null, new[] { typeof(string) }, null));
                }
                else
                    throw new InvalidOperationException();
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
                    throw new InvalidOperationException($"Cannot {(multiplicative.Value == "*" ? "multiply" : (multiplicative.Value == "/" ? "divide" : "mod"))} float and string");
                else if (right == typeof(GmObject))
                    emit.Call(GetOperator(multiplicative.Value, left, right));
                else
                    throw new NotImplementedException();
            }
            else if (left == typeof(string)) {
                //Todo: Get second value.
                throw new InvalidOperationException(
                    $"Cannot {(multiplicative.Value == "*" ? "multiply" : (multiplicative.Value == "/" ? "divide" : "mod"))} string and {(right == typeof(float) ? "float" : right.ToString())}"
                );
            }
            else if (left == typeof(GmObject))
                emit.Call(GetOperator(multiplicative.Value, left, right));
        }

        public void Visit(PostfixNode postfix)
        {
            //Todo: Implement (post|pre)fix operator
            //It works exactly like addition op.
            throw new NotImplementedException();
        }

        public void Visit(PrefixNode prefix)
        {
            throw new NotImplementedException();
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
                    //Todo: Test argument_count
                    // Possible problems:
                    // LdArg might need to be LdArgA
                    // LdLen might not convert to float.
                    // If it doesn't work, uncomment code underneath current implementation.
                    emit.LdArg(0)
                        .LdLen()
                        .ConvertFloat();
                    /*
                    * LdArgA
                    emit.LdArgA(0)
                        .LdLen()
                        .ConvertFloat();

                    * LdLen invalid
                    emit.LdArg(0)
                        .LdInt(0)
                        .Call(typeof(GmObject[]).GetMethod("GetLength", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(int) }, null));
                    */
                    break;
                case "pi":
                    emit.LdFloat((float)Math.PI);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Visit(RelationalNode relational)
        {
            //Todo: (LOW) Get String address as int?


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
                            throw new NotImplementedException();
                    }
                }
                else if (right == typeof(GmObject))
                    emit.Call(GetOperator(relational.Value, left, right));
            }
            else if (left == typeof(GmObject))
                emit.Call(GetOperator(relational.Value, left, right));
            else
                throw new NotImplementedException();

            return;
        }

        public void Visit(ReturnNode returnNode)
        {
            returnNode.ReturnValue.Accept(this);

            if (!emit.TryGetTop(out var returnType))
                throw new InvalidProgramException();

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
                if (child.Type != SyntaxType.Enum && child.Type != SyntaxType.Import && child.Type != SyntaxType.Script)
                    throw new InvalidProgramException();

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

            //Todo: Remove possibly unnecessary return stmt.
            if (!emit.TryGetTop(out _))
            {
                emit.Call(_getEmptyObject);
            }

            emit.Ret();

            _inScript = false;
            _table.Exit();

            if (name == "main")
                GenerateEntryPoint(mb);
        }

        public void Visit(ShiftNode shift)
        {
            throw new NotImplementedException();
        }

        public void Visit(SwitchNode switchNode)
        {
            throw new NotImplementedException();
        }

        public void Visit(VariableToken variableToken)
        {
            var name = variableToken.Text;
            if(_table.Defined(name, out var symbol))
            {
                if (symbol.Type != SymbolType.Variable)
                    throw new InvalidOperationException();

                if (_locals.TryGetValue(symbol, out var local))
                {
                    if (_needAddress)
                        emit.LdLocalA(local);
                    else
                        emit.LdLocal(local);
                }
                else
                    throw new InvalidOperationException();
            }
            else
            {
                emit.Call(_getId);
                emit.LdStr(variableToken.Text);
                emit.Call(typeof(GmObject).GetMethod("GetMember", _methodFlags, null, new[] { typeof(string) }, null));
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
