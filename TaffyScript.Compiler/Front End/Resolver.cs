﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Compiler.Syntax;

namespace TaffyScript.Compiler.FrontEnd
{
    /// <summary>
    /// Represents an AST walker that verifies correctness.
    /// </summary>
    public class Resolver : ISyntaxElementVisitor
    {
        private bool _inLambda = false;
        private bool _hasParent = false;
        private LoopType _currentLoop = LoopType.None;
        private MethodType _currentMethod = MethodType.None;
        private Environment _environment = new Environment();
        private IErrorLogger _logger;
        private SymbolTable _table;
        private SymbolResolver _resolver;
        private Dictionary<string, string> _inheritance = new Dictionary<string, string>();

        public Resolver(IErrorLogger logger, SymbolTable table, SymbolResolver resolver)
        {
            _logger = logger;
            _table = table;
            _resolver = resolver;
        }

        public void Resolve(RootNode root)
        {
            root.Accept(this);
        }

        // This class sets the parents of all of the nodes as it traverses the AST.
        // Technically that step could be skipped to increase compile times
        // (though I saw almost no increase in time after making the change)
        // it allows for some extra optimizations while compiling.

        // In addition, to makes sure all of the code is correct
        // e.g. break and continue are used within loops.

        // It also determines which variables need to be captured for use
        // within a lambda.

        // Todo: Have Resolver determine if a script returns on all branches.
        //       If not, add a ReturnNode at the end of any ScriptNode so
        //       the compiler doesn't have to.
        //       This should also allow for some dead code eleminatation,
        //       which will help with output size to some degree.

        public void Visit(AdditiveNode additive)
        {
            additive.Left.Parent = additive;
            additive.Right.Parent = additive;
            additive.Left.Accept(this);
            additive.Right.Accept(this);
        }

        public void Visit(ArgumentAccessNode argumentAccess)
        {
            argumentAccess.Index.Parent = argumentAccess;
            argumentAccess.Index.Accept(this);
        }

        public void Visit(ArrayAccessNode arrayAccess)
        {
            foreach (var index in arrayAccess.Arguments)
            {
                index.Parent = arrayAccess;
                index.Accept(this);
            }
        }

        public void Visit(ArrayLiteralNode arrayLiteral)
        {
            foreach (var element in arrayLiteral.Elements)
            {
                element.Parent = arrayLiteral;
                element.Accept(this);
            }
        }

        public void Visit(AssignNode assign)
        {
            assign.Left.Parent = assign;
            assign.Right.Parent = assign;
            assign.Left.Accept(this);
            assign.Right.Accept(this);
        }

        public void Visit(BaseNode baseNode)
        {
            if (_currentMethod != MethodType.Instance)
                _logger.Error("Cannot use 'base' outside of an instance script.", baseNode.Position);

            if (!_hasParent)
                _logger.Error("Cannot use 'base' inside of a type that doesn't have a parent.", baseNode.Position);

            foreach(var arg in baseNode.Arguments)
            {
                arg.Parent = baseNode;
                arg.Accept(this);
            }
        }

        public void Visit(BitwiseNode bitwise)
        {
            bitwise.Left.Parent = bitwise;
            bitwise.Right.Parent = bitwise;
            bitwise.Left.Accept(this);
            bitwise.Right.Accept(this);
        }

        public void Visit(BlockNode block)
        {
            foreach (var element in block.Body)
            {
                element.Parent = block;
                element.Accept(this);
            }
        }

        public void Visit(BreakToken breakToken)
        {
            if (_currentLoop == LoopType.None)
                _logger.Error("Cannot use 'break' outside of a loop or switch case.", breakToken.Position);

            return;
        }

        public void Visit(ConditionalNode conditional)
        {
            conditional.Condition.Parent = conditional;
            conditional.Left.Parent = conditional;
            conditional.Right.Parent = conditional;
            conditional.Condition.Accept(this);
            conditional.Left.Accept(this);
            conditional.Right.Accept(this);
        }

        public void Visit(IConstantToken constantToken)
        {
            return;
        }

        public void Visit(ContinueToken continueToken)
        {
            if (_currentLoop != LoopType.Loop)
                _logger.Error("Cannot use 'continue' outside of a loop.", continueToken.Position);

            return;
        }

        public void Visit(DoNode doNode)
        {
            doNode.Body.Parent = doNode;
            doNode.Condition.Parent = doNode;

            var loop = _currentLoop;
            _currentLoop = LoopType.Loop;
            doNode.Body.Accept(this);
            _currentLoop = loop;
            doNode.Condition.Accept(this);
        }

        public void Visit(EndToken endToken)
        {
        }

        public void Visit(EnumNode enumDeclaration)
        {
        }

        public void Visit(EqualityNode equality)
        {
            equality.Left.Parent = equality;
            equality.Right.Parent = equality;
            equality.Left.Accept(this);
            equality.Right.Accept(this);
        }

        public void Visit(ForNode forNode)
        {
            if(forNode.Initialize != null)
            {
                forNode.Initialize.Parent = forNode;
                forNode.Initialize.Accept(this);
            }

            if(forNode.Condition != null)
            {
                forNode.Condition.Parent = forNode;
                forNode.Condition.Accept(this);
            }

            forNode.Body.Parent = forNode;

            var loop = _currentLoop;
            _currentLoop = LoopType.Loop;
            forNode.Body.Accept(this);
            _currentLoop = loop;

            if(forNode.Increment != null)
            {
                forNode.Increment.Parent = forNode;
                forNode.Increment.Accept(this);
            }
        }

        public void Visit(FunctionCallNode functionCall)
        {
            functionCall.Callee.Parent = functionCall;
            functionCall.Callee.Accept(this);
            foreach (var arg in functionCall.Arguments)
            {
                arg.Parent = functionCall;
                arg.Accept(this);
            }
        }

        public void Visit(IfNode ifNode)
        {
            ifNode.Condition.Parent = ifNode;
            ifNode.Condition.Accept(this);
            ifNode.ThenBrach.Parent = ifNode;
            ifNode.ThenBrach.Accept(this);
            if(ifNode.ElseBranch!= null)
            {
                ifNode.ElseBranch.Parent = ifNode;
                ifNode.ElseBranch.Accept(this);
            }
        }

        public void Visit(ImportObjectNode importObjectNode)
        {
            return;
        }

        public void Visit(ImportScriptNode import)
        {
            return;
        }

        public void Visit(LambdaNode lambdaNode)
        {
            _table.Enter(lambdaNode.Scope);
            _environment = new Environment(_environment);
            foreach (var arg in lambdaNode.Arguments)
                _environment.Define(arg.Name);

            var inLambda = _inLambda;
            _inLambda = true;
            var enclosing = _currentMethod;
            _currentMethod = MethodType.Lambda;
            lambdaNode.Body.Parent = lambdaNode;
            lambdaNode.Body.Accept(this);
            _inLambda = inLambda;
            _currentMethod = enclosing;
            _environment = _environment.Enclosing;
            _table.Exit();
        }

        public void Visit(LocalsNode locals)
        {
            foreach(var variable in locals.Locals)
            {
                if (_environment.Encountered(variable.Name))
                    _logger.Error("Tried to declare a variable that has already been encountered in the current scope.", variable.Position);
                _environment.Define(variable.Name);

                variable.Value?.Accept(this);
            }
        }

        public void Visit(LogicalNode logical)
        {
            logical.Left.Parent = logical;
            logical.Left.Accept(this);
            logical.Right.Parent = logical;
            logical.Right.Accept(this);
        }

        public void Visit(MemberAccessNode memberAccess)
        {
            memberAccess.Left.Parent = memberAccess;
            memberAccess.Right.Parent = memberAccess;

            // Only visit the left side of member accesses
            // The right side doesn't belong to the scope so it doesn't matter.
            memberAccess.Left.Accept(this);
        }

        public void Visit(MultiplicativeNode multiplicative)
        {
            multiplicative.Left.Parent = multiplicative;
            multiplicative.Right.Parent = multiplicative;
            multiplicative.Left.Accept(this);
            multiplicative.Right.Accept(this);
        }

        public void Visit(NamespaceNode namespaceNode)
        {
            var count = _table.EnterNamespace(namespaceNode.Name);

            foreach (var element in namespaceNode.Declarations)
            {
                element.Parent = namespaceNode;
                element.Accept(this);
            }

            _table.Exit(count);
        }

        public void Visit(NewNode newNode)
        {
            foreach (var arg in newNode.Arguments)
            {
                arg.Parent = newNode;
                arg.Accept(this);
            }
        }

        public void Visit(ObjectNode objectNode)
        {
            _table.Enter(objectNode.Name);

            if (objectNode.Inherits != null)
            {
                if(_resolver.TryResolveType(objectNode.Inherits, out var parentSymbol) && _table.Current is ObjectSymbol obj)
                {
                    obj.Inherits = parentSymbol;
                    _hasParent = true;
                }
                else
                    _logger.Error("Tried to inherit from a non-existant type", objectNode.Position);
            }
            foreach (var script in objectNode.Scripts)
            {
                script.Parent = objectNode;
                script.Accept(this);
            }
            _table.Exit();
            _hasParent = false;
        }

        public void Visit(PostfixNode postfix)
        {
            postfix.Left.Parent = postfix;
            postfix.Left.Accept(this);
        }

        public void Visit(PrefixNode prefix)
        {
            prefix.Right.Parent = prefix;
            prefix.Right.Accept(this);
        }

        public void Visit(ReadOnlyToken readOnlyToken)
        {
        }

        public void Visit(RelationalNode relational)
        {
            relational.Left.Parent = relational;
            relational.Left.Accept(this);
            relational.Right.Parent = relational;
            relational.Right.Accept(this);
        }

        public void Visit(RepeatNode repeatNode)
        {
            repeatNode.Count.Parent = repeatNode;
            repeatNode.Count.Accept(this);
            var loop = _currentLoop;
            _currentLoop = LoopType.Loop;
            repeatNode.Body.Parent = repeatNode;
            repeatNode.Body.Accept(this);
            _currentLoop = loop;
        }

        public void Visit(ReturnNode returnNode)
        {
            if(returnNode.Result != null)
            {
                returnNode.Result.Parent = returnNode;
                returnNode.Result.Accept(this);
            }
        }

        public void Visit(RootNode root)
        {
            foreach (var unit in root.CompilationUnits)
            {
                unit.Parent = root;
                unit.Accept(this);
            }
        }

        public void Visit(ScriptNode script)
        {
            _table.Enter(script.Name);
            var enclosing = _currentMethod;
            _currentMethod = script.Parent.Type == SyntaxType.Object ? MethodType.Instance : MethodType.Global;
            _environment = new Environment(_environment);
            foreach (var arg in script.Arguments)
                _environment.Define(arg.Name);
            script.Body.Parent = script;
            script.Body.Accept(this);
            _environment = _environment.Enclosing;
            _currentMethod = enclosing;
            _table.Exit();
        }

        public void Visit(ShiftNode shift)
        {
            shift.Left.Parent = shift;
            shift.Left.Accept(this);
            shift.Right.Parent = shift;
            shift.Right.Accept(this);
        }

        public void Visit(SwitchNode switchNode)
        {
            switchNode.Value.Parent = switchNode;
            switchNode.Value.Accept(this);
            var loop = _currentLoop;
            _currentLoop = LoopType.Case;
            foreach(var switchCase in switchNode.Cases)
            {
                switchCase.Expression.Parent = switchNode;
                switchCase.Expression.Accept(this);
                switchCase.Expression.Parent = switchNode;
                switchCase.Body.Accept(this);
            }
            if(switchNode.DefaultCase != null)
            {
                switchNode.DefaultCase.Parent = switchNode;
                switchNode.DefaultCase.Accept(this);
            }
            _currentLoop = loop;
        }

        public void Visit(UsingsNode usingsNode)
        {
            foreach (var ns in usingsNode.Usings)
                _table.AddNamespaceToDefinitionLookup(ns.Namespace);

            foreach (var declaration in usingsNode.Declarations)
            {
                declaration.Parent = usingsNode;
                declaration.Accept(this);
            }

            _table.RemoveAllNamespacesFromDefinitionLookup();
        }

        public void Visit(VariableToken variableToken)
        {
            _environment.Define(variableToken.Name);
            if(_inLambda)
            {
                if(_environment.Depth(variableToken.Name) != 0)
                {
                    if (_table.Defined(variableToken.Name, out var symbol) && symbol is VariableLeaf leaf)
                        leaf.IsCaptured = true;
                }
            }
        }

        public void Visit(WhileNode whileNode)
        {
            whileNode.Condition.Parent = whileNode;
            whileNode.Condition.Accept(this);
            var loop = _currentLoop;
            _currentLoop = LoopType.Loop;
            whileNode.Body.Parent = whileNode;
            whileNode.Body.Accept(this);
            _currentLoop = loop;
        }

        public void Visit(WithNode withNode)
        {
            withNode.Target.Parent = withNode;
            withNode.Target.Accept(this);
            withNode.Body.Parent = withNode;
            withNode.Body.Accept(this);
        }

        private enum LoopType
        {
            None,
            Loop,
            Case
        }

        private enum MethodType
        {
            None,
            Global,
            Instance,
            Lambda
        }

        private class Environment
        {
            private HashSet<string> _encountered = new HashSet<string>();

            public Environment Enclosing { get; }

            public Environment()
            {
                Enclosing = null;
            }

            public Environment(Environment enclosing)
            {
                Enclosing = enclosing;
            }

            public bool Encountered(string name)
            {
                if (_encountered.Contains(name))
                    return true;

                if (Enclosing != null)
                    return Enclosing.Encountered(name);

                return false;
            }

            public bool Define(string name)
            {
                if (!Encountered(name))
                {
                    _encountered.Add(name);
                    return true;
                }
                return false;
            }

            public int Depth(string name)
            {
                var i = 0;
                var env = this;
                while(!env._encountered.Contains(name))
                {
                    i++;
                    env = env.Enclosing;
                }
                return i;
            }
        }
    }
}
