using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaffyScriptCompiler.FrontEnd;
using TaffyScriptCompiler.Syntax;

namespace TaffyScriptCompiler
{
    /// <summary>
    /// Parses TaffyScript code and generates an AST based on the input.
    /// </summary>
    public class Parser
    {
        private static Regex StringParser = new Regex(@"\\");
        private static HashSet<char> _hexCharacters = null;

        private static HashSet<char> HexCharacters
        {
            get
            {
                if(_hexCharacters == null)
                    _hexCharacters = new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                return _hexCharacters;
            }
        }

        private ISyntaxTree _tree;
        private Tokenizer _stream;
        private SymbolTable _table;
        private ISyntaxElementFactory _factory;

        /// <summary>
        /// Flag used to determine whether = should mean assignment or equality.
        /// </summary>
        private int _canAssign = 0;

        public SymbolTable Table => _table;
        public ISyntaxTree Tree => _tree;
        public List<Exception> Errors { get; } = new List<Exception>();

        /// <summary>
        /// Creates a new TaffyScript parser.
        /// </summary>
        public Parser()
        {
            _table = new SymbolTable();
            _tree = new SyntaxTree(_table);
            _factory = new SyntaxElementFactory();
        }

        /// <summary>
        /// Parses a string comprised of TaffyScript code.
        /// </summary>
        /// <param name="code">The TaffyScript code to parse.</param>
        public void Parse(string code)
        {
            using (_stream = new Tokenizer(code))
            {
                Parse(_tree.Root);
            }
        }

        /// <summary>
        /// Parses a file containing TaffyScript code.
        /// </summary>
        /// <param name="file">The file to parse.</param>
        public void ParseFile(string file)
        {
            using (var fs = new System.IO.FileStream(file, System.IO.FileMode.Open))
            {
                using(_stream = new Tokenizer(fs))
                {
                    Parse(_tree.Root);
                }
            }
        }

        private void Parse(ISyntaxNode root)
        {
            if (_stream.Finished)
                return;
            _stream.ErrorEncountered += (e) => Errors.Add(e);

            root.AddChild(Usings());
        }

        private ISyntaxElement Usings()
        {
            ISyntaxNode node = null;
            while(Try(TokenType.Using, out var token))
            {
                if (node == null)
                    node = _factory.CreateNode(SyntaxType.Usings, token.Position);
                var ns = GetNamespace();
                node.AddChild(_factory.CreateConstant(ConstantType.String, ns, token.Position));
            }

            // There are no using statements.
            // Should not throw an error, so just leave the position as null.
            if (node == null)
                node = _factory.CreateNode(SyntaxType.Usings, new TokenPosition(0, 0, 0, null));

            // This needs to be a block so that the enums aren't pushed to the front of the usings node.
            var block = _factory.CreateNode(SyntaxType.Block, null);
            AddDeclarations(block);
            node.AddChild(block);

            return node;
        }

        private void AddDeclarations(ISyntaxNode node)
        {
            var enums = new List<ISyntaxElement>();
            // Check for stream finsihed for global declarations,
            // and check for end brace for namespace declarations.
            while (!_stream.Finished && !Try(TokenType.CloseBrace))
            {
                var child = Declaration();
                if (child != null && (child.Type == SyntaxType.Enum || child.Type == SyntaxType.Import))
                {
                    enums.Add(child);
                    child.Parent = node;
                }
                else
                    node.AddChild(child);
            }

            //Make sure that enums get processed before anything else.
            node.Children.InsertRange(0, enums);
        }

        private ISyntaxElement Declaration()
        {
            switch(_stream.Peek().Type)
            {
                case TokenType.Namespace:
                    var token = Confirm(TokenType.Namespace);
                    var nsName = GetNamespace();
                    var ns = new NamespaceNode(nsName, token.Position);
                    Confirm(TokenType.OpenBrace);
                    try
                    {
                        _table.EnterNamespace(nsName);
                    }
                    catch(Exception e)
                    {
                        Throw(e);
                        return null;
                    }
                    AddDeclarations(ns);

                    //Make sure the declarations didn't end because the file ended.
                    Confirm(TokenType.CloseBrace);
                    _table.ExitNamespace(nsName);
                    return ns;
                case TokenType.Object:
                    Confirm(TokenType.Object);
                    var objName = Confirm(TokenType.Identifier);
                    _table.EnterNew(objName.Value, SymbolType.Object);
                    var node = _factory.CreateNode(SyntaxType.Object, objName.Value, objName.Position);
                    if (Validate(TokenType.Colon))
                    {
                        var parent = Confirm(TokenType.Identifier);
                        node.AddChild(_factory.CreateConstant(ConstantType.String, parent.Value, parent.Position));
                    }
                    else
                        node.AddChild(_factory.CreateConstant(ConstantType.String, "", objName.Position));
                    Confirm(TokenType.OpenBrace);
                    while (!Try(TokenType.CloseBrace))
                    {
                        node.AddChild(Script(SymbolScope.Member));
                    }
                    Confirm(TokenType.CloseBrace);
                    _table.Exit();
                    return node;
                case TokenType.Enum:
                    Confirm(TokenType.Enum);
                    var enumName = Confirm(TokenType.Identifier);
                    _table.EnterNew(enumName.Value, SymbolType.Enum);
                    node = _factory.CreateNode(SyntaxType.Enum, enumName.Value, enumName.Position);
                    Confirm(TokenType.OpenBrace);
                    if(!Try(TokenType.CloseBrace))
                    {
                        var value = 0;
                        do
                        {
                            var name = Confirm(TokenType.Identifier);
                            ISyntaxNode nameNode;
                            if (Validate(TokenType.Assign))
                            {
                                nameNode = _factory.CreateNode(SyntaxType.Assign, name.Value, name.Position);
                                var num = Confirm(TokenType.Number);
                                ConstantToken<float> numToken = (ConstantToken<float>)_factory.CreateConstant(ConstantType.Real, num.Value, num.Position);
                                value = (int)numToken.Value;
                                _table.AddLeaf(name.Value, value);
                                nameNode.AddChild(numToken);
                            }
                            else
                            {
                                nameNode = _factory.CreateNode(SyntaxType.Declare, name.Value, name.Position);
                                _table.AddLeaf(name.Value, value);
                            }
                            value++;
                            node.AddChild(nameNode);
                        }
                        while (Validate(TokenType.Comma));
                    }
                    Confirm(TokenType.CloseBrace);
                    _table.Exit();
                    return node;
                case TokenType.Import:
                    var import = Confirm(TokenType.Import);
                    if (Try(TokenType.Object))
                        return ImportObject(import.Position);
                    else
                        return ImportScript(import.Position);
                case TokenType.Script:
                    return Script(SymbolScope.Global);
                case TokenType.SemiColon:
                    Confirm(TokenType.SemiColon);
                    return null;
                default:
                    Throw(new InvalidTokenException(_stream.Peek(), $"Expected declaration, got {_stream.Read().Type}"));
                    return null;
            }
        }

        private ISyntaxElement ImportObject(TokenPosition pos)
        {
            Confirm(TokenType.Object);
            List<ObjectImportArgument> importArgs = new List<ObjectImportArgument>();
            if(Validate(TokenType.OpenParen))
            {
                if (!Try(TokenType.CloseParen))
                {
                    do
                    {
                        var argName = _stream.Read();
                        Confirm(TokenType.Assign);
                        var argValue = Confirm(TokenType.Identifier).Value;
                        importArgs.Add(new ObjectImportArgument(argName.Value, argValue, argName.Position));
                    }
                    while (Validate(TokenType.Comma));
                }
                Confirm(TokenType.CloseParen);
            }
            var sb = new StringBuilder();
            var startPos = GetImportType(sb);
            ImportObjectNode node;
            if (Validate(TokenType.As))
            {
                var importName = Confirm(TokenType.Identifier);
                node = new ImportObjectNode((IConstantToken<string>)_factory.CreateConstant(ConstantType.String, importName.Value, importName.Position), sb.ToString(), startPos);
            }
            else
                node = new ImportObjectNode(sb.ToString(), startPos);

            _table.AddChild(new ImportObjectLeaf(_table.Current, node.ImportName.Value, node));

            node.ParseArguments(importArgs);
            if(!Try(TokenType.OpenBrace))
            {
                node.AutoImplement = true;
                return node;
            }
            Confirm(TokenType.OpenBrace);
            if(!Try(TokenType.CloseBrace))
            {
                bool hasCtor = false;
                do
                {
                    if(Try(TokenType.New, out var newToken))
                    {
                        if (hasCtor)
                            Throw(new InvalidTokenException(newToken, $"Import types can only define one constructor"));
                        var newNode = new ImportObjectConstructor(newToken.Position);
                        Confirm(TokenType.OpenParen);
                        if (!Try(TokenType.CloseParen))
                        {
                            do
                            {
                                var token = _stream.Read();
                                var type = token.Value;
                                if (type == "array")
                                    type = "array1d";
                                if (!TsTypes.BasicTypes.ContainsKey(type))
                                    Throw(new InvalidTokenException(token, $"Import type must be one of the following: {string.Join(", ", TsTypes.BasicTypes.Keys)}\n"));
                                else
                                    newNode.ArgumentTypes.Add(type);
                            }
                            while (Validate(TokenType.Comma));
                        }
                        Confirm(TokenType.CloseParen);
                        node.Constructor = newNode;
                    }
                    else
                    {
                        var memberName = Confirm(TokenType.Identifier);
                        if (Try(TokenType.OpenParen))
                        {
                            var methodNode = new ImportObjectMethod(memberName.Value, memberName.Position);
                            if (Validate(TokenType.LessThan))
                            {
                                do
                                {
                                    sb.Clear();
                                    pos = GetImportType(sb);
                                    methodNode.Generics.Add((IConstantToken<string>)_factory.CreateConstant(ConstantType.String, sb.ToString(), pos));
                                }
                                while (Validate(TokenType.Comma));
                                Confirm(TokenType.GreaterThan);
                            }
                            Confirm(TokenType.OpenParen);
                            if (!Try(TokenType.CloseParen))
                            {
                                do
                                {
                                    var token = _stream.Read();
                                    var type = token.Value;
                                    if (type == "array")
                                        type = "array1d";
                                    if (!TsTypes.BasicTypes.ContainsKey(type))
                                        Throw(new InvalidTokenException(token, $"Import type must be one of the following: {string.Join(", ", TsTypes.BasicTypes.Keys)}\n"));
                                    else
                                        methodNode.ArgumentTypes.Add(type);
                                }
                                while (Validate(TokenType.Comma));
                            }
                            Confirm(TokenType.CloseParen);

                            if (Validate(TokenType.As))
                                methodNode.ImportName = Confirm(TokenType.Identifier).Value;

                            node.Methods.Add(methodNode);
                        }
                        else
                        {
                            if (Validate(TokenType.As))
                            {
                                var importName = Confirm(TokenType.Identifier).Value;
                                node.Fields.Add(new ImportObjectField(memberName.Value, importName, memberName.Position));
                            }
                            else
                                node.Fields.Add(new ImportObjectField(memberName.Value, memberName.Position));
                        }
                    }

                    while (Validate(TokenType.SemiColon)) ;
                }
                while (!Try(TokenType.CloseBrace));
            }
            Confirm(TokenType.CloseBrace);

            return node;
        }

        private ISyntaxElement ImportScript(TokenPosition pos)
        {
            var node = _factory.CreateNode(SyntaxType.Import, pos);
            var start = Confirm(TokenType.Identifier);
            var baseType = new StringBuilder(start.Value);
            do
            {
                baseType.Append(Confirm(TokenType.Dot).Value);
                baseType.Append(Confirm(TokenType.Identifier).Value);
            }
            while (Try(TokenType.Dot));
            node.AddChild(_factory.CreateConstant(ConstantType.String, baseType.ToString(), start.Position));
            Confirm(TokenType.OpenParen);
            if (!Try(TokenType.CloseParen))
            {
                do
                {
                    var token = _stream.Read();
                    var type = token.Value;
                    if (type == "array")
                        type = "array1d";
                    if (!TsTypes.BasicTypes.ContainsKey(type))
                    {
                        Throw(new InvalidTokenException(token, $"Import type must be one of the following: {string.Join(", ", TsTypes.BasicTypes.Keys)}\n"));
                        node.AddChild(null);
                    }
                    else
                        node.AddChild(_factory.CreateConstant(ConstantType.String, type, token.Position));
                }
                while (Validate(TokenType.Comma));
            }
            Confirm(TokenType.CloseParen);
            Confirm(TokenType.As);
            var importName = Confirm(TokenType.Identifier);
            _table.AddChild(new ImportLeaf(_table.Current, importName.Value, SymbolScope.Global, (ImportNode)node));
            //_table.AddLeaf(importName.Value, SymbolType.Script, SymbolScope.Global);
            node.AddChild(_factory.CreateConstant(ConstantType.String, importName.Value, importName.Position));
            return node;
        }

        private ISyntaxElement Script(SymbolScope scope)
        {
            if (!(Validate(TokenType.Script) || Validate(TokenType.Event)))
            {
                Throw(new InvalidTokenException(_stream.Peek(), $"Expected a script, got {_stream.Read().Type}"));
                return null;
            }

            var scriptName = Confirm(TokenType.Identifier);
            _table.EnterNew(scriptName.Value, SymbolType.Script, scope);
            var node = _factory.CreateNode(SyntaxType.Script, scriptName.Value, scriptName.Position);
            if (Validate(TokenType.OpenParen) && !Validate(TokenType.CloseParen))
            {
                var optional = false;
                do
                {
                    var parameterToken = Confirm(TokenType.Identifier);
                    var parameter = _factory.CreateToken(SyntaxType.Variable, parameterToken.Value, parameterToken.Position);
                    ISyntaxElement parameterElement;
                    if (Try(TokenType.Assign, out var assign))
                    {
                        optional = true;
                        ISyntaxElement value;
                        if (!IsConstant())
                        {
                            var next = _stream.Peek().Type;
                            switch(next)
                            {
                                case TokenType.Minus:
                                    Confirm(TokenType.Minus);
                                    var prefix = "-";
                                    if (Validate(TokenType.Dot))
                                        prefix += ".";
                                    var num = Confirm(TokenType.Number);
                                    value = _factory.CreateConstant(ConstantType.Real, prefix + num.Value, num.Position);
                                    break;
                                case TokenType.Dot:
                                    Confirm(TokenType.Dot);
                                    num = Confirm(TokenType.Number);
                                    value = _factory.CreateConstant(ConstantType.Real, TokenType.Dot + num.Value, num.Position);
                                    break;
                                case TokenType.ReadOnly:
                                    var read = Confirm(TokenType.ReadOnly);
                                    value = _factory.CreateToken(SyntaxType.ReadOnlyValue, read.Value, read.Position);
                                    break;
                                default:
                                    Throw(new InvalidTokenException(_stream.Peek(), "Optional arguments must have a constant value"));
                                    Validate(TokenType.Identifier);
                                    continue;
                            }
                        }
                        else
                            value = Constant();

                        var temp = _factory.CreateNode(SyntaxType.Assign, "=", assign.Position);
                        temp.AddChild(parameter);
                        temp.AddChild(value);
                        parameterElement = temp;
                    }
                    else
                    {
                        if (optional)
                            Throw(new InvalidTokenException(parameterToken, "Can't have non-optional arguments after an optional argument."));

                        parameterElement = parameter;
                    }
                    node.AddChild(parameterElement);
                    _table.AddLeaf(parameterToken.Value, SymbolType.Variable, SymbolScope.Local);
                }
                while (Validate(TokenType.Comma));
                Confirm(TokenType.CloseParen);
            }
            node.AddChild(Statement());
            _table.Exit();
            return node;
        }

        private ISyntaxElement Statement()
        {
            if (Try(TokenType.Var, out var localToken))
            {
                var locals = _factory.CreateNode(SyntaxType.Locals, localToken.Position);
                do
                {
                    var localName = Confirm(TokenType.Identifier);
                    if (!_table.Defined(localName.Value, out var symbol))
                        _table.AddLeaf(localName.Value, SymbolType.Variable, SymbolScope.Local);
                    else if (symbol.Type != SymbolType.Variable)
                        Throw(new InvalidTokenException(localName, $"Id already defined for higher priority type: {localName.Value} = {symbol.Type}"));
                    
                    if (Try(TokenType.Assign, out var equalToken))
                    {
                        var id = _factory.CreateToken(SyntaxType.Variable, localName.Value, localToken.Position);
                        var assign = _factory.CreateNode(SyntaxType.Assign, "=", equalToken.Position);
                        assign.AddChild(id);
                        assign.AddChild(Expression());
                        locals.AddChild(assign);
                    }
                    else
                    {
                        var declare = _factory.CreateNode(SyntaxType.Declare, localName.Value, localName.Position);
                        locals.AddChild(declare);
                    }
                }
                while (Validate(TokenType.Comma));
                return locals;
            }
            else
                return EmbeddedStatement();
        }

        private ISyntaxElement EmbeddedStatement()
        {
            ISyntaxElement result;
            if (Try(TokenType.OpenBrace))
                result = BlockStatement();
            else
                result = SimpleStatement();
            return result;
        }

        private ISyntaxElement SimpleStatement()
        {
            var next = _stream.Peek();
            ISyntaxElement result;
            switch (next.Type)
            {
                case TokenType.SemiColon:
                    Confirm(TokenType.SemiColon);
                    result = null;
                    break;
                case TokenType.Break:
                    result = _factory.CreateToken(SyntaxType.Break, Confirm(TokenType.Break).Value, next.Position);
                    break;
                case TokenType.Continue:
                    result = _factory.CreateToken(SyntaxType.Continue, Confirm(TokenType.Continue).Value, next.Position);
                    break;
                case TokenType.Exit:
                    result = _factory.CreateToken(SyntaxType.Exit, Confirm(TokenType.Exit).Value, next.Position);
                    break;
                case TokenType.Return:
                    Confirm(TokenType.Return);
                    var temp = _factory.CreateNode(SyntaxType.Return, next.Position);
                    temp.AddChild(Expression());
                    result = temp;
                    break;
                case TokenType.Repeat:
                    Confirm(TokenType.Repeat);
                    temp = _factory.CreateNode(SyntaxType.Repeat, next.Position);
                    var paren = Validate(TokenType.OpenParen);
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(TokenType.CloseParen);
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case TokenType.While:
                    Confirm(TokenType.While);
                    temp = _factory.CreateNode(SyntaxType.While, next.Position);
                    paren = Validate(TokenType.OpenParen);
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(TokenType.CloseParen);
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case TokenType.With:
                    Confirm(TokenType.With);
                    temp = _factory.CreateNode(SyntaxType.With, next.Position);
                    paren = Validate(TokenType.OpenParen);
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(TokenType.CloseParen);
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case TokenType.Do:
                    Confirm(TokenType.Do);
                    temp = _factory.CreateNode(SyntaxType.Do, next.Position);
                    temp.AddChild(BodyStatement());
                    Confirm(TokenType.Until);
                    paren = Validate(TokenType.OpenParen);
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(TokenType.CloseParen);
                    result = temp;
                    break;
                case TokenType.If:
                    Confirm(TokenType.If);
                    temp = _factory.CreateNode(SyntaxType.If, next.Position);
                    paren = Validate(TokenType.OpenParen);
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(TokenType.CloseParen);
                    temp.AddChild(BodyStatement());
                    while (Validate(TokenType.SemiColon)) ;
                    if (Validate(TokenType.Else))
                        temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case TokenType.For:
                    Confirm(TokenType.For);
                    Confirm(TokenType.OpenParen);
                    temp = _factory.CreateNode(SyntaxType.For, next.Position);
                    if (!Try(TokenType.SemiColon))
                        temp.AddChild(BodyStatement());
                    else
                        temp.AddChild(_factory.CreateNode(SyntaxType.Block, next.Position));
                    Confirm(TokenType.SemiColon);
                    if (Try(TokenType.SemiColon))
                        Throw(new InvalidTokenException(_stream.Peek(), "Expected expression in for declaration"));
                    else
                        temp.AddChild(Expression());
                    Confirm(TokenType.SemiColon);
                    temp.AddChild(BodyStatement());
                    Confirm(TokenType.CloseParen);
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case TokenType.Switch:
                    Confirm(TokenType.Switch);
                    temp = _factory.CreateNode(SyntaxType.Switch, next.Position);
                    paren = Validate(TokenType.OpenParen);
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(TokenType.CloseParen);
                    Confirm(TokenType.OpenBrace);
                    ISyntaxNode caseNode;
                    while(!Try(TokenType.CloseBrace))
                    {
                        if (Try(TokenType.Case, out var caseToken))
                        {
                            caseNode = _factory.CreateNode(SyntaxType.Case, caseToken.Position);
                            caseNode.AddChild(Expression());
                        }
                        else if (Try(TokenType.Default, out var defaultToken))
                            caseNode = _factory.CreateNode(SyntaxType.Default, defaultToken.Position);
                        else
                        {
                            Throw(new InvalidTokenException(_stream.Peek(), $"Expected case declaration, got {_stream.Read().Value}"));
                            continue;
                        }
                        var blockStart = Confirm(TokenType.Colon);
                        var block = _factory.CreateNode(SyntaxType.Block, blockStart.Position);
                        while (!Try(TokenType.Case) && !Try(TokenType.Default) && !Try(TokenType.CloseBrace))
                            block.AddChild(Statement());
                        caseNode.AddChild(block);
                        temp.AddChild(caseNode);
                    }
                    Confirm(TokenType.CloseBrace);
                    result = temp;
                    break;
                default:
                    result = Expression();
                    break;
            }

            return result;
        }

        private ISyntaxElement BlockStatement()
        {
            var blockStart = Confirm(TokenType.OpenBrace);
            var result = _factory.CreateNode(SyntaxType.Block, blockStart.Position);
            while (!_stream.Finished && !Try(TokenType.CloseBrace))
                result.AddChild(Statement());

            Confirm(TokenType.CloseBrace);
            return result;
        }

        private ISyntaxElement BodyStatement()
        {
            ISyntaxElement body;
            if (Try(TokenType.OpenBrace))
                body = BlockStatement();
            else
            {
                var child = Statement();
                var temp = _factory.CreateNode(SyntaxType.Block, child.Position);
                temp.AddChild(child);
                body = temp;
            }
            return body;
        }

        private ISyntaxElement Expression()
        {
            var value = AssignmentExpression();
            return value;
        }

        private ISyntaxElement AssignmentExpression()
        {
            // This is used to determine what = means.
            // If this is the first pass through this method, it is an assignment.
            // Otherwise, it should be equivalent to ==.
            // This behaviour was defined by Gamemaker and is stupid, 
            // but it's kept in order to improve backwards compatibilty.
            _canAssign++;

            var value = ConditionalExpression();
            if (value == null)
                return null;

            // This statement is a really hacky.
            // The only types that can be assigned to have Access in their name or are a variable.
            // If the SyntaxType enum is ever changed, this could break.
            if (_canAssign == 1 && (value.Type.ToString().Contains("Access") || value.Type == SyntaxType.Variable) && IsAssignment())
            {
                var next = _stream.Read();
                var assign = _factory.CreateNode(SyntaxType.Assign, next.Value, next.Position);
                assign.AddChild(value);
                assign.AddChild(AssignmentExpression());
                value = assign;
            }
            --_canAssign;
            return value;
        }

        private ISyntaxElement ConditionalExpression()
        {
            
            var value = LogicalOrExpression();

            //Example: true == false ? "hello" : "henlo"
            if (Try(TokenType.QuestionMark, out var token))
            {
                var conditional = _factory.CreateNode(SyntaxType.Conditional, token.Position);
                conditional.AddChild(value);
                conditional.AddChild(AssignmentExpression());
                Confirm(TokenType.Colon);
                conditional.AddChild(AssignmentExpression());
                value = conditional;
            }

            return value;
        }

        private ISyntaxElement LogicalOrExpression()
        {
            var value = LogicalAndExpression();
            while(Try(TokenType.LogicalOr, out var token))
            {
                var or = _factory.CreateNode(SyntaxType.Logical, token.Value, token.Position);
                or.AddChild(value);
                or.AddChild(LogicalAndExpression());
                value = or;
            }
            return value;
        }

        private ISyntaxElement LogicalAndExpression()
        {
            var value = BitwiseOrExpression();
            while (Try(TokenType.LogicalAnd, out var token))
            {
                var and = _factory.CreateNode(SyntaxType.Logical, token.Value, token.Position);
                and.AddChild(value);
                and.AddChild(BitwiseOrExpression());
                value = and;
            }
            return value;
        }

        private ISyntaxElement BitwiseOrExpression()
        {
            var value = BitwiseXorExpression();
            while (Try(TokenType.BitwiseOr, out var token))
            {
                var or = _factory.CreateNode(SyntaxType.Bitwise, token.Value, token.Position);
                or.AddChild(value);
                or.AddChild(BitwiseXorExpression());
                value = or;
            }
            return value;
        }

        private ISyntaxElement BitwiseXorExpression()
        {
            var value = BitwiseAndExpression();
            while (Try(TokenType.Xor, out var token))
            {
                var xor = _factory.CreateNode(SyntaxType.Bitwise, token.Value, token.Position);
                xor.AddChild(value);
                xor.AddChild(BitwiseAndExpression());
                value = xor;
            }
            return value;
        }

        private ISyntaxElement BitwiseAndExpression()
        {
            var value = EqualityExpression();
            while (Try(TokenType.BitwiseAnd, out var token))
            {
                var and = _factory.CreateNode(SyntaxType.Bitwise, token.Value, token.Position);
                and.AddChild(value);
                and.AddChild(EqualityExpression());
                value = and;
            }
            return value;
        }

        private ISyntaxElement EqualityExpression()
        {
            var value = RelationalExpression();

            while(Try(TokenType.Equal, out var token) || Try(TokenType.NotEqual, out token) || (_canAssign > 1 && Try(TokenType.Assign, out token)))
            {
                var op = token.Value == "=" ? "==" : token.Value;
                var equal = _factory.CreateNode(SyntaxType.Equality, op, token.Position);
                equal.AddChild(value);
                equal.AddChild(RelationalExpression());
                value = equal;
            }

            return value;
        }

        private ISyntaxElement RelationalExpression()
        {
            var value = ShiftExpression();

            while(Try(TokenType.LessThan, out var token) || Try(TokenType.LessThanOrEqual, out token) || Try(TokenType.GreaterThan, out token) || Try(TokenType.GreaterThanOrEqual, out token))
            {
                var compare = _factory.CreateNode(SyntaxType.Relational, token.Value, token.Position);
                compare.AddChild(value);
                compare.AddChild(ShiftExpression());
                value = compare;
            }

            return value;
        }

        private ISyntaxElement ShiftExpression()
        {
            var value = AdditiveExpression();

            while(Try(TokenType.ShiftLeft, out var token) || Try(TokenType.ShiftRight, out token))
            {
                var shift = _factory.CreateNode(SyntaxType.Shift, token.Value, token.Position);
                shift.AddChild(value);
                shift.AddChild(AdditiveExpression());
                value = shift;
            }

            return value;
        }

        private ISyntaxElement AdditiveExpression()
        {
            var value = MultiplicativeExpression();

            while (Try(TokenType.Plus, out var token) || Try(TokenType.Minus, out token))
            {
                var add = _factory.CreateNode(SyntaxType.Additive, token.Value, token.Position);
                add.AddChild(value);
                add.AddChild(MultiplicativeExpression());
                value = add;
            }

            return value;
        }

        private ISyntaxElement MultiplicativeExpression()
        {
            var value = UnaryExpression();

            while (Try(TokenType.Multiply, out var token) || Try(TokenType.Divide, out token) || Try(TokenType.Modulo, out token))
            {
                var mul = _factory.CreateNode(SyntaxType.Multiplicative, token.Value, token.Position);
                mul.AddChild(value);
                mul.AddChild(UnaryExpression());
                value = mul;
            }

            return value;
        }

        private ISyntaxElement UnaryExpression()
        {
            if (Try(TokenType.Plus, out var token) || Try(TokenType.Minus, out token) || Try(TokenType.Not, out token) ||
                Try(TokenType.Complement, out token) || Try(TokenType.Increment, out token) || Try(TokenType.Decrement, out token))
            {
                var prefix = _factory.CreateNode(SyntaxType.Prefix, token.Value, token.Position);
                prefix.AddChild(UnaryExpression());
                return prefix;
            }
            else
                return PrimaryExpression();
        }

        private ISyntaxElement PrimaryExpression()
        {
            var value = PrimaryExpressionStart();
            if (value == null)
                return null;

            while(true)
            {
                if (Try(TokenType.OpenParen, out var paren))
                {
                    var function = _factory.CreateNode(SyntaxType.FunctionCall, value.Position);
                    function.AddChild(value);
                    if (!Try(TokenType.CloseParen))
                    {
                        do
                        {
                            function.AddChild(Expression());
                        }
                        while (Validate(TokenType.Comma));
                    }
                    Confirm(TokenType.CloseParen);
                    value = function;
                }
                else if (Validate(TokenType.Dot))
                {
                    var temp = _factory.CreateNode(SyntaxType.MemberAccess, value.Position);
                    temp.AddChild(value);
                    if (Try(TokenType.Identifier, out var next))
                    {
                        temp.AddChild(_factory.CreateToken(SyntaxType.Variable, next.Value, next.Position));
                    }
                    else if (Try(TokenType.ReadOnly, out next))
                    {
                        if (next.Value != "self")
                        {
                            Throw(new InvalidTokenException(next, "Invalid readonly token on the right side of an access expression"));
                            return null;
                        }
                        temp.AddChild(_factory.CreateToken(SyntaxType.ReadOnlyValue, next.Value, next.Position));
                    }
                    else
                    {
                        Throw(new InvalidTokenException(next, "The value after a period in an access expression must be a variable."));
                        return null;
                    }
                    value = temp;
                }
                else if (Try(TokenType.OpenBracket, out var accessToken))
                {
                    if (value.Type == SyntaxType.New)
                    {
                        Throw(new InvalidTokenException(accessToken, "Cannot use an accessor on a newed value"));
                        return value;
                    }
                    ISyntaxNode access = _factory.CreateNode(SyntaxType.ArrayAccess, value.Position);

                    access.AddChild(value);
                    access.AddChild(Expression());
                    if (Validate(TokenType.Comma))
                        access.AddChild(Expression());
                    Confirm(TokenType.CloseBracket);
                    value = access;
                }
                else
                    break;
            }
            if(Try(TokenType.Increment, out var token) || Try(TokenType.Decrement, out token))
            {
                var postfix = _factory.CreateNode(SyntaxType.Postfix, token.Value, value.Position);
                postfix.AddChild(value);
                value = postfix;
            }

            return value;
        }

        private ISyntaxElement PrimaryExpressionStart()
        {
            if (IsConstant())
                return Constant();
            else if (Try(TokenType.ReadOnly, out var token))
            {
                var val = token.Value;
                if (val == "id")
                    val = "self";
                return _factory.CreateToken(SyntaxType.ReadOnlyValue, val, token.Position);
            }
            else if (Try(TokenType.Argument, out token))
            {
                var arg = _factory.CreateNode(SyntaxType.ArgumentAccess, token.Position);
                if (token.Value != "argument")
                    arg.AddChild(_factory.CreateConstant(ConstantType.Real, token.Value.Remove(0, 8),
                        new TokenPosition(token.Position.Index + 8, token.Position.Line, token.Position.Column + 8, token.Position.File)));
                else
                {
                    Confirm(TokenType.OpenBracket);
                    arg.AddChild(Expression());
                    Confirm(TokenType.CloseBracket);
                }
                return arg;
            }
            else if (Try(TokenType.Identifier, out token))
            {
                if (!_table.Defined(token.Value, out _))
                    _table.AddPending(token.Value);
                return _factory.CreateToken(SyntaxType.Variable, token.Value, token.Position);
            }
            else if (Validate(TokenType.OpenParen))
            {
                var value = Expression();
                Confirm(TokenType.CloseParen);
                return value;
            }
            else if (Try(TokenType.OpenBracket, out token))
            {
                var array = _factory.CreateNode(SyntaxType.ArrayLiteral, token.Position);
                while (!Try(TokenType.CloseBracket))
                {
                    array.AddChild(Expression());
                    Validate(TokenType.Comma);
                }
                Confirm(TokenType.CloseBracket);
                return array;
            }
            else if (Try(TokenType.New, out token))
            {
                var start = Confirm(TokenType.Identifier);
                var type = start.Value;
                while (Validate(TokenType.Dot))
                    type += "." + Confirm(TokenType.Identifier).Value;

                var newNode = _factory.CreateNode(SyntaxType.New, type, start.Position);
                Confirm(TokenType.OpenParen);
                if (!Try(TokenType.CloseParen))
                {
                    do
                    {
                        newNode.AddChild(Expression());
                    }
                    while (Validate(TokenType.Comma));
                }
                Confirm(TokenType.CloseParen);
                return newNode;
            }
            else
            {
                Throw(new InvalidTokenException(_stream.Read()));
                return null;
            }
        }

        private bool Try(TokenType next)
        {
            if (_stream.Finished || _stream.Peek().Type != next)
                return false;
            return true;
        }

        private bool Try(TokenType next, out Token result)
        {
            result = default(Token);
            if (_stream.Finished || _stream.Peek().Type != next)
                return false;

            result = _stream.Read();
            return true;
        }

        private bool Validate(TokenType next)
        {
            if(!_stream.Finished && _stream.Peek().Type == next)
            {
                _stream.Read();
                return true;
            }
            return false;
        }

        private Token Confirm(TokenType next)
        {
            if (_stream.Finished)
            {
                Throw(new System.IO.EndOfStreamException($"Expected {next}, reached the end of file instead."));
                return null;
            }
            var result = _stream.Read();
            if (result.Type != next)
                Throw(new InvalidTokenException(result, $"Expected {next}, got {result.Type}"));
            return result;
        }

        private bool IsConstant()
        {
            var type = _stream.Peek().Type;
            return type == TokenType.Number || type == TokenType.String || type == TokenType.Bool;
        }

        private bool IsConstant(ISyntaxElement element)
        {
            return element.Type == SyntaxType.Constant;
        }

        private ISyntaxToken Constant()
        {
            if (Try(TokenType.Number, out var token))
                return _factory.CreateConstant(ConstantType.Real, token.Value, token.Position);
            else if (Try(TokenType.String, out token))
            {
                var value = token.Value;
                if (value.StartsWith("'"))
                    value = value.Trim('\'');
                else
                    value = value.Trim('"');
                var match = StringParser.Match(value);
                while(match.Success)
                {
                    switch (value[match.Index + 1])
                    {
                        case 'u':
                            var i = match.Index + 2;
                            string num = "";
                            while (HexCharacters.Contains(char.ToLower(value[i])) && i - match.Index < 6)
                                num += value[i++];
                            if (num == "")
                            {
                                var errorPos = new TokenPosition(token.Position.Index, token.Position.Line, token.Position.Column + i, token.Position.File);
                                Throw(new InvalidOperationException($"Invalid hex constant {errorPos}"));
                            }
                            num = num.PadLeft(4, '0');
                            byte[] hex = new byte[2];
                            hex[1] = (byte)int.Parse(num.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            hex[0] = (byte)int.Parse(num.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            Console.WriteLine($"{hex[0]}, {hex[1]}");
                            if(i - match.Index != 6)
                            {
                                if (value[i] == '\\')
                                    i++;
                            }
                            value = value.Remove(match.Index, i - match.Index);
                            value = value.Insert(match.Index, Encoding.Unicode.GetString(hex));
                            break;
                        case '\t':
                            value = value.Remove(match.Index, 2);
                            value = value.Insert(match.Index, "\t");
                            break;
                        case 'r':
                            value = value.Remove(match.Index, 2);
                            value = value.Insert(match.Index, "\r");
                            break;
                        case 'n':
                            value = value.Remove(match.Index, 2);
                            value = value.Insert(match.Index, "\n");
                            break;
                        case '\\':
                            value = value.Remove(match.Index, 2);
                            value = value.Insert(match.Index, @"\");
                            break;
                        default:
                            break;
                    }
                    match = StringParser.Match(value, match.Index + 1);
                }
                return _factory.CreateConstant(ConstantType.String, value,
                    new TokenPosition(token.Position.Index + 1, token.Position.Line, token.Position.Column + 1, token.Position.File));
            }
            else if (Try(TokenType.Bool, out token))
                return _factory.CreateConstant(ConstantType.Bool, token.Value, token.Position);
            else
                Throw(new InvalidTokenException(_stream.Peek(), $"Expected literal, got {_stream.Peek().Type}"));

            return null;
        }

        private TokenPosition GetImportType(StringBuilder sb)
        {
            var start = Confirm(TokenType.Identifier);
            sb.Append(start.Value);
            while(Validate(TokenType.Dot))
            {
                sb.Append(".");
                sb.Append(Confirm(TokenType.Identifier).Value);
            }
            if(Validate(TokenType.LessThan))
            {
                sb.Append("<");
                do
                {
                    if (Validate(TokenType.Comma))
                        sb.Append(",");
                    GetImportType(sb);
                }
                while (Try(TokenType.Comma));

                Confirm(TokenType.GreaterThan);
                sb.Append(">");
            }
            if(Validate(TokenType.OpenBracket))
            {
                sb.Append("[");
                while (Validate(TokenType.Comma))
                    sb.Append(",");
                Confirm(TokenType.CloseBracket);
                sb.Append("]");
            }
            return start.Position;
        }

        private bool IsAssignment()
        {
            var type = _stream.Peek().Type;
            switch(type)
            {
                case TokenType.Assign:
                case TokenType.PlusEquals:
                case TokenType.SubEquals:
                case TokenType.MulEquals:
                case TokenType.DivEquals:
                case TokenType.ModEquals:
                case TokenType.AndEquals:
                case TokenType.OrEquals:
                case TokenType.XorEquals:
                    return true;
                default:
                    return false;
            }
        }

        private void Throw(Exception exception)
        {
            Errors.Add(exception);
        }

        private string GetNamespace()
        {
            var first = Confirm(TokenType.Identifier);
            var name = first.Value;
            while (Validate(TokenType.Dot))
            {
                name += ".";
                name += Confirm(TokenType.Identifier).Value;
            }

            //Make sure to remove all semi colons.
            while (Validate(TokenType.SemiColon)) ;

            return name;
        }
    }
}
