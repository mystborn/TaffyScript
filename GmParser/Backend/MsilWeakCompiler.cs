using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Myst.Collections;
using GmExtern;

namespace GmParser.Backend
{
    public class MsilWeakCompiler : DotNetCompiler
    {
        public delegate void NodeHandler(ISyntaxElement element, ILEmitter emit, Dictionary<string, LocalBuilder> locals);

        /// <summary>
        /// Storage container for enums. The row is the enum name, the columns are the value names, and the the values are the actual values assigned to the name.
        /// When the enum is referenced, it will be replaced by the constant value in the source code.
        /// </summary>
        private LookupTable<string, string, long> _enums = new LookupTable<string, string, long>();
        private Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();
        private Dictionary<Type, MethodInfo> _gmObjectCasts;
        private Dictionary<Type, ConstructorInfo> _gmConstructors;
        private Dictionary<string, Type> _gmBasicTypes;
        private MethodInfo _getEmptyObject;
        private SymbolTable _table;
        private Dictionary<SyntaxType, NodeHandler> _nodeHandlers;

        public MsilWeakCompiler(string asmName) 
            : base(asmName)
        {
            InitGmMethods();
            InitBCL();
            InitNodeHandlers();
        }

        public MsilWeakCompiler(string asmName, params string[] externalAssemblies)
            : base(asmName, externalAssemblies)
        {
            InitGmMethods();
            InitBCL();
            InitNodeHandlers();
        }

        public override void Compile(SyntaxTree tree)
        {
            _table = tree.Table;
            Root(tree.Root);
            _baseType.CreateType();
            _asm.Save(_asmName);
        }

        private void Root(SyntaxNode rootNode)
        {
            foreach (SyntaxNode child in rootNode.Children)
            {
                switch(child.Type)
                {
                    case SyntaxType.Enum:
                        DefineEnum(child);
                        break;
                    case SyntaxType.Import:
                        DefineImport(child);
                        break;
                    case SyntaxType.Script:
                        DefineScript(child);
                        break;
                    default:
                        throw new InvalidProgramException();
                }
            }
        }

        private void DefineScript(SyntaxNode scriptNode)
        {
            var name = scriptNode.Value;
            _table.Enter(name);
            MethodBuilder mb;
            if (!_methods.TryGetValue(name, out var method))
            {
                mb = StartMethod(name);
                _methods.Add(name, mb);
            }
            else
                mb = (MethodBuilder)method;
            var il = mb.GetILGenerator();
            var emit = new ILEmitter(il, _isDebug);
            var locals = new Dictionary<string, LocalBuilder>();
            foreach(var local in _table.Symbols.Where(symbol => symbol.Type == SymbolType.Variable))
                locals.Add(local.Name, emit.DeclareLocal(Parent, local.Name));

            /*foreach (SyntaxNode symbol in scriptNode.Children)
                _nodeHandlers[symbol.Type](symbol, emit, locals);*/

            emit.Call(_getEmptyObject)
                .Ret();

            _table.Exit();
        }

        public void Block(ISyntaxElement element, ILEmitter emit, Dictionary<string, LocalBuilder> locals)
        {
            var blockNode = (SyntaxNode)element;
            foreach (var child in blockNode.Children)
                _nodeHandlers[child.Type](child, emit, locals);
        }

        public void Assign(ISyntaxElement element, ILEmitter emit, Dictionary<string, LocalBuilder> locals)
        {
            var assignNode = (SyntaxNode)element;
            var left = assignNode.Children[0] as ConstantToken<string>;
            var right = assignNode.Children[1];

            _nodeHandlers[right.Type](right, emit, locals);
            if (!emit.TryGetTop(out var top))
                throw new InvalidProgramException();

            if (top != typeof(GmObject))
                emit.New(_gmConstructors[top]);

            emit.StLocal(locals[left.Value]);
        }

        public void Locals(ISyntaxElement element, ILEmitter emit, Dictionary<string, LocalBuilder> locals)
        {
            var localsNode = (SyntaxNode)element;
            // The localsNode can only contain two types of children:
            // * Assign
            // * Declare
            //
            // Due to GM's strange scoping rules, all of the variables will have been declared already.
            // So we only need to process the assign nodes.

            foreach (var child in localsNode.Children.Where(c => c.Type == SyntaxType.Assign))
                Assign(child, emit, locals);
        }

        public void Conditional(ISyntaxElement element, ILEmitter emit, Dictionary<string, LocalBuilder> locals)
        {
            var conditionalNode = (SyntaxNode)element;
            var test = conditionalNode.Children[0];
            var left = conditionalNode.Children[1];
            var right = conditionalNode.Children[2];

            _nodeHandlers[test.Type](element, emit, locals);
            var brFalse = emit.DefineLabel();
            var brFinal = emit.DefineLabel();
            emit.BrFalse(brFalse);
            _nodeHandlers[left.Type](left, emit, locals);
            emit.Br(brFinal)
                .MarkLabel(brFinal);
            _nodeHandlers[right.Type](right, emit, locals);
            emit.MarkLabel(brFinal);
        }

        public void Constant(ISyntaxElement element, ILEmitter emit, Dictionary<string, LocalBuilder> locals)
        {
            var cToken = (IConstantToken)element;
            switch(cToken.ConstantType)
            {
                case ConstantType.Bool:
                    emit.LdInt(((ConstantToken<bool>)cToken).Value ? 1 : 0);
                    break;
                case ConstantType.Real:
                    emit.LdFloat(((ConstantToken<float>)cToken).Value);
                    break;
                case ConstantType.String:
                    emit.LdStr(((ConstantToken<string>)cToken).Value);
                    break;
            }
        }

        private void DefineImport(SyntaxNode importNode)
        {
            var final = importNode.Children.Count - 1;
            var externalName = importNode.Children[0] as ConstantToken<string> 
                ?? throw new InvalidProgramException($"Error when importing the method {((SyntaxToken)importNode.Children[0]).Text} :: Invalid ConstantToken");

            var internalName = importNode.Children[final] as ConstantToken<string>
                ?? throw new InvalidProgramException($"Error when importing the method {((SyntaxToken)importNode.Children[0]).Text} :: Invalid ConstantToken");

            Type[] argTypes = importNode.Children.Count == 2 ? null : new Type[importNode.Children.Count - 2];
            for(var i = 1; i < final; i++)
            {
                Type argType;
                var node = importNode.Children[i] as ConstantToken<string>
                    ?? throw new InvalidProgramException($"Error when importing the method {((SyntaxToken)importNode.Children[0]).Text} :: Invalid ConstantToken");
                if (!_gmBasicTypes.TryGetValue(node.Value, out argType))
                    throw new InvalidProgramException($"Error when importing the method {((SyntaxToken)importNode.Children[0]).Text} :: Invalid Argument Type");
                argTypes[i - 1] = argType;
            }

            var typeName = externalName.Value.Remove(externalName.Value.LastIndexOf('.'));
            var methodName = externalName.Value.Substring(externalName.Value.LastIndexOf('.') + 1);
            var owner = _typeParser.GetType(typeName);
            var method = GetMethodToImport(owner, methodName, argTypes)
                ?? throw new InvalidProgramException($"Error when importing the method {((SyntaxToken)importNode.Children[0]).Text} :: Could not find method");

            method = GenerateWeakMethodForImport(method, internalName.Value, argTypes);
            _methods.Add(internalName.Value, method);
        }

        private MethodInfo GenerateWeakMethodForImport(MethodInfo method, string importName, Type[] argTypes)
        {
            var mb = StartMethod(importName);
            var il = mb.GetILGenerator();
            var emit = new ILEmitter(il, _isDebug);
            for(var i = 0; i < argTypes.Length; i++)
            {
                emit.LdArg(0)
                    .LdInt(i);
                //Only cast the the GmObject if needed.
                if (argTypes[i] == typeof(object))
                {
                    emit.LdElem(typeof(GmObject))
                        .Box(typeof(GmObject));
                }
                else if(argTypes[i] != typeof(GmObject))
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

            return mb;
        }
        
        private void DefineEnum(SyntaxNode enumNode)
        {
            long current = 0;
            foreach(SyntaxNode expr in enumNode.Children)
            {
                if (expr.Type == SyntaxType.Assign)
                {
                    if (expr.Children[0] is IConstantToken<float> value)
                        current = (long)value.Value;
                    else
                        throw new InvalidSymbolException("Enum value must be equal to an integer constant.");
                }
                else if(expr.Type != SyntaxType.Declare)
                    throw new InvalidProgramException($"Encountered error while compiling enum {enumNode.Value}");

                _enums.Add(enumNode.Value, expr.Value, current++);
            }
        }

        private MethodBuilder StartMethod(string name)
        {
            return _baseType.DefineMethod(name, MethodAttributes.Public | MethodAttributes.Static, typeof(GmObject), new[] { typeof(GmObject[]) });
        }

        private void InitGmMethods()
        {
            var objType = typeof(GmObject);
            _gmObjectCasts = new Dictionary<Type, MethodInfo>()
            {
                { typeof(string), objType.GetMethod("GetString") },
                { typeof(double), objType.GetMethod("GetNum") },
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

            _getEmptyObject = objType.GetMethod("Empty");
        }

        private void InitBCL()
        {
            var objType = typeof(GmObject);
            //ImportBclMethod("show_debug_message", typeof(Console), "WriteLine", new[] { objType });
            ImportBclMethod("string", objType, "ToString", new[] { objType });
        }

        private MethodInfo ImportBclMethod(string importName, Type owner, string externName, Type[] types)
        {
            var method = GetMethodToImport(owner, externName, types);
            for (var i = 0; i < types.Length; i++)
                types[i] = types[i] == typeof(object) ? typeof(GmObject) : types[i];
            method = GenerateWeakMethodForImport(method, importName, types);
            _methods.Add(importName, method);
            return method;
        }

        private void InitNodeHandlers()
        {
            _nodeHandlers = new Dictionary<SyntaxType, NodeHandler>()
            {
                { SyntaxType.Block, Block }
            };
        }
    }
}
