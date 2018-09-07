using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TaffyScript.Compiler.FrontEnd;
using TaffyScript.Compiler.Syntax;
using NumberConstant = TaffyScript.Compiler.Syntax.ConstantToken<float>;

namespace TaffyScript.Compiler
{
    public class Parser
    {
        private static Regex StringParser = new Regex(@"\\", RegexOptions.Compiled);
        private static HashSet<char> HexCharacters = new HashSet<char>()
        {
            'a', 'b', 'c', 'd', 'e', 'f', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
        };

        private IErrorLogger _logger;
        private Tokenizer _stream;
        private SymbolTable _table;
        private int _lambdaId = 0;
        private bool _canAssign = false;
        private RootNode _root;

        public Parser(IErrorLogger logger, SymbolTable table, RootNode root)
        {
            _logger = logger;
            _table = table;
            _root = root;
        }

        public void Parse(string code)
        {
            _stream = new Tokenizer(code, _logger);
            Parse();
        }

        public void ParseFile(string fpath)
        {
            string code = null;
            using(var reader = new System.IO.StreamReader(fpath))
            {
                code = reader.ReadToEnd();
            }
            _stream = new Tokenizer(code, fpath, _logger);
            Parse();
        }

        private void Parse()
        {
            if (_stream.Finished)
                return;

            _root.CompilationUnits.Add(Usings());
        }

        private UsingsNode Usings()
        {
            var usings = new List<UsingDeclaration>();
            var pos = _stream.Position;
            if (Check(TokenType.Using))
            {
                do
                {
                    var usingToken = Consume(TokenType.Using, "Expected using");
                    var ns = ResolveNamespace();
                    usings.Add(new UsingDeclaration(ns, usingToken.Position));

                    while (Match(TokenType.SemiColon)) ;
                }
                while (Check(TokenType.Using));
            }

            var declarations = Declarations(0);

            return new UsingsNode(usings, declarations, pos);
        }

        private List<ISyntaxElement> Declarations(int tableDepth)
        {
            var declarations = new List<ISyntaxElement>();
            while(!_stream.Finished)
            {
                if (Check(TokenType.CloseBrace))
                    break;
                var declaration = Declaration();
                if(declaration != null)
                    declarations.Add(declaration);
            }

            return declarations;
        }

        private ISyntaxElement Declaration()
        {
            try
            {
                switch (_stream.Current.Type)
                {
                    case TokenType.Enum: return EnumDeclaration();
                    case TokenType.Import: return ImportDeclaration();
                    case TokenType.Namespace: return NamespaceDeclaration();
                    case TokenType.Object: return ObjectDeclaration();
                    case TokenType.Script: return ScriptDeclaration(SymbolScope.Global);
                    case TokenType.SemiColon:
                        _stream.Read();
                        return null;
                    default:
                        Error(_stream.Current, "Expected a declaration");
                        return null;
                }
            }
            catch (ParseException)
            {
                Synchronize(TokenType.Enum, TokenType.Import, TokenType.Namespace, TokenType.Object, TokenType.Script);
                return null;
            }
        }

        private EnumNode EnumDeclaration()
        {
            var start = Consume(TokenType.Enum, "Expected 'enum'");
            var name = Consume(TokenType.Identifier, "Expected enum name");
            if (!_table.TryEnterNew(name.Text, SymbolType.Enum))
                Error(name, $"Name conflict between enum and {_table.Defined(name.Text).Type} {name.Text}");

            Consume(TokenType.OpenBrace, "Expected '{' after enum declaration", 1);
            var values = new List<EnumValue>();
            if (!Check(TokenType.CloseBrace))
            {
                long value = 0;
                do
                {
                    var valueName = Consume(TokenType.Identifier, "Expected name for enum value", 1);
                    if (Match(TokenType.Assign))
                    {
                        var negate = Match(TokenType.Minus);
                        var valueToken = Consume(TokenType.Number, "Enum value must be a numberic literal", 1);
                        var style = NumberStyles.Integer;
                        var text = valueToken.Text;
                        if(text.StartsWith("0x"))
                        {
                            text = text.Substring(2);
                            style = NumberStyles.HexNumber;
                        }
                        else if(text.StartsWith("?"))
                        {
                            text = text.Substring(1);
                            style = NumberStyles.HexNumber;
                        }
                        if(!long.TryParse(text, style, CultureInfo.InvariantCulture, out value))
                            Error(valueToken, "Enum value must be an integer constant", 1);

                        if (negate)
                            value = -value;
                    }
                    if(!_table.AddLeaf(valueName.Text, value))
                    {
                        _table.Exit();
                        Error(valueName, "Cannot have two enum values with the same name", 1);
                    }
                    values.Add(new EnumValue(valueName.Text, value++));
                }
                while (Match(TokenType.Comma));
            }
            Consume(TokenType.CloseBrace, "Expect '}' after enum values", 1);
            _table.Exit();
            return new EnumNode(name.Text, values, name.Position);
        }

        private ISyntaxElement ImportDeclaration()
        {
            var import = Consume(TokenType.Import, "Expected 'import'");
            if (Check(TokenType.Object))
                return ImportObject();
            return ImportScript(import);
        }

        private ISyntaxElement ImportObject()
        {
            ImportObjectNode node;
            var start = Consume(TokenType.Object, "Expected 'object'");
            var importArgs = new List<ObjectImportArgument>();
            if(Match(TokenType.OpenParen) && !Match(TokenType.CloseParen))
            {
                do
                {
                    var argName = Consume(TokenType.Identifier, "Expected import argument name");
                    Consume(TokenType.Assign, "Expected '=' after import argument");

                    var value = _stream.Current.Type == TokenType.Bool ? 
                                Consume(TokenType.Bool, "Expected value for import argument") :
                                Consume(TokenType.Identifier, "Expected value for import argument");

                    importArgs.Add(new ObjectImportArgument(argName.Text, value.Text, argName.Position));
                }
                while (Match(TokenType.Comma));
                Consume(TokenType.CloseParen, "Expected ')' after import object arguments");
            }

            var type = ReadDotNetType();
            var name = type;
            if (Match(TokenType.As))
                name = Consume(TokenType.Identifier, "Expected name after import object declaration").Text;

            var index = name.LastIndexOf('.');
            if (index++ != -1)
                name = name.Substring(index, name.Length - index);

            if(!Check(TokenType.OpenBrace))
            {
                node = new ImportObjectNode(type, name, _logger, importArgs, start.Position);
                if(!_table.TryAdd(new ImportObjectSymbol(_table.Current, name, node)))
                    Error(start, $"Name conflict with imported object and {_table.Defined(name).Type} {name}");
                return node;
            }

            Consume(TokenType.OpenBrace, "Expected '{' after import object declaration");

            // Clear floating Semi Colons from the text.
            while (Match(TokenType.SemiColon)) ;

            ImportObjectConstructor ctor = null;
            List<ImportObjectMethod> methods = new List<ImportObjectMethod>();
            List<ImportObjectField> fields = new List<ImportObjectField>();
            while(!Check(TokenType.CloseBrace))
            {
                if (Check(TokenType.New))
                {
                    var token = Consume(TokenType.New, "Expected 'new'");
                    if (ctor != null)
                        Error(token, "Imported types can only define one constructor");

                    Consume(TokenType.OpenParen, "Expected '(' after constructor declaration in import object defintion");
                    var arguments = ReadImportArguments();
                    Consume(TokenType.CloseParen, "Expected ')' after constructor arguments");
                    ctor = new ImportObjectConstructor(arguments, token.Position);
                }
                else if (Check(TokenType.Identifier))
                {
                    var memberName = _stream.Read();
                    if (Check(TokenType.OpenParen) || Check(TokenType.LessThan))
                    {
                        List<string> generics = null;
                        if (Match(TokenType.LessThan))
                        {
                            generics = new List<string>();
                            do
                                generics.Add(ReadDotNetType());
                            while (Match(TokenType.Comma));
                            Consume(TokenType.GreaterThan, "Expected '>' after generic types");
                        }
                        Consume(TokenType.OpenParen, "Expected '(' after script declaration in import object definition");
                        var arguments = ReadImportArguments();
                        Consume(TokenType.CloseParen, "Expected ')' after script arguments");
                        var importName = memberName.Text;
                        if (Match(TokenType.As))
                            importName = Consume(TokenType.Identifier, "Expected name for method in import object definition").Text;
                        methods.Add(new ImportObjectMethod(memberName.Text, importName, generics, arguments, memberName.Position));
                    }
                    else
                    {
                        var importName = memberName.Text;
                        if (Match(TokenType.As))
                            importName = Consume(TokenType.Identifier, "Expected name for field in import object definition").Text;
                        fields.Add(new ImportObjectField(memberName.Text, importName, memberName.Position));
                    }
                }
                else
                    Error(_stream.Current, "Unexpected token in import object definition");

                while (Match(TokenType.SemiColon)) ;
            }
            Consume(TokenType.CloseBrace, "Expected '}' after import object definition");
            if (ctor == null)
                Error(start, "Imported object with an explicit implementation must declare a constructor");

            node = new ImportObjectNode(type, name, _logger, importArgs, fields, ctor, methods, start.Position);
            if (!_table.TryAdd(new ImportObjectSymbol(_table.Current, name, node)))
                Error(start, $"Name conflict with imported object and {_table.Defined(name).Type} {name}");

            return node;
        }

        private ISyntaxElement ImportScript(Token importToken)
        {
            var lastDot = -1;
            var sb = new StringBuilder();
            do
            {
                sb.Append(Consume(TokenType.Identifier, "Expect name for script import").Text);
                if (Match(TokenType.GreaterThan))
                {
                    sb.Append(ReadDotNetType());
                    while (Match(TokenType.Comma))
                        sb.Append(',').Append(ReadDotNetType());
                    Consume(TokenType.LessThan, "Expect '>' after generic types");
                }
                if (_stream.Current.Type == TokenType.Dot)
                {
                    sb.Append('.');
                    lastDot = sb.Length;
                }
            }
            while (Match(TokenType.Dot));
            if (lastDot == -1)
                Error(importToken, "Expected method name for script import.");

            var text = sb.ToString();
            var type = text.Substring(0, lastDot - 1);
            var method = text.Substring(lastDot);

            Consume(TokenType.OpenParen, "Expect '(' after .NET method");
            var arguments = ReadImportArguments();
            Consume(TokenType.CloseParen, "Expected ')' after .NET method import arguments");
            string importName = method;
            if (Match(TokenType.As))
                importName = Consume(TokenType.Identifier, "Expected name for imported .NET method").Text;
            else if (method.IndexOf('<') != -1)
                Error(importToken, "Must provide an import name for .NET method that has a generic type.");

            var result = new ImportScriptNode(type, method, importName, arguments, importToken.Position);

            if (!_table.TryAdd(new ImportLeaf(_table.Current, importName, SymbolScope.Global, result)))
                Error(importToken, $"Name conflict between imported c# method and {_table.Defined(importName).Type} {importName}");

            return result;
        }

        private NamespaceNode NamespaceDeclaration()
        {
            var start = Consume(TokenType.Namespace, "Expected 'namespace'");
            var name = ResolveNamespace();
            Consume(TokenType.OpenBrace, "Expected '{' after namespace declaration");
            int count;
            try
            {
                count = _table.EnterNamespace(name);
            }
            catch(Exception e)
            {
                Error(start, e.Message);
                return null;
            }
            var declarations = Declarations(count);

            Consume(TokenType.CloseBrace, "Expected '}' after namespace declarations", count);
            _table.Exit(count);
            return new NamespaceNode(name, declarations, start.Position);
        }

        private ObjectNode ObjectDeclaration()
        {
            Consume(TokenType.Object, "Expected 'object'");
            var name = Consume(TokenType.Identifier, "Expected name for object");
            if(!_table.TryAdd(new ObjectSymbol(_table.Current, name.Text)))
                Error(name, $"Name conflict between object and {_table.Defined(name.Text).Type} {name.Text}");
            _table.Enter(name.Text);

            ISyntaxElement parent = null;
            if (Match(TokenType.Colon))
                parent = ReadTaffyScriptType("Invalid type name for object parent");

            Consume(TokenType.OpenBrace, "Expected '{' after object declaration", 1);

            var scripts = new List<ScriptNode>();
            var staticScripts = new List<ScriptNode>();
            var fields = new List<ObjectField>();
            var staticFields = new List<ObjectField>();
            while(!Check(TokenType.CloseBrace) && !_stream.Finished)
            {
                switch(_stream.Current.Type)
                {
                    case TokenType.SemiColon:
                        _stream.Read();
                        continue;
                    case TokenType.Identifier:
                        fields.Add(FieldDeclaration(SymbolScope.Member));
                        break;
                    case TokenType.Script:
                        scripts.Add(ScriptDeclaration(SymbolScope.Member));
                        break;
                    case TokenType.Static:
                        var stat = _stream.Read();
                        switch(_stream.Current.Type)
                        {
                            case TokenType.Script:
                                staticScripts.Add(ScriptDeclaration(SymbolScope.Global));
                                break;
                            case TokenType.Identifier:
                                staticFields.Add(FieldDeclaration(SymbolScope.Global));
                                break;
                            default:
                                Error(stat, "Expected 'script' or field declaration after 'static'");
                                break;
                        }
                        break;
                    default:
                        _logger.Error("Invalid token inside of Object definition", _stream.Current.Position);
                        SynchronizeBraces(false, TokenType.Script, TokenType.Static);
                        break;
                }
            }
            Consume(TokenType.CloseBrace, "Expected '}' after object members", 1);
            _table.Exit();
            return new ObjectNode(name.Text, parent, fields, staticFields, scripts, staticScripts, name.Position);
        }

        private ObjectField FieldDeclaration(SymbolScope scope)
        {
            var field = Consume(TokenType.Identifier, "Expected field name.");
            ISyntaxElement defaultValue = null;
            if (Match(TokenType.Assign))
            {
                if (IsConstant())
                    defaultValue = Constant(0);
                else if (_stream.Current.Type == TokenType.ReadOnly)
                {
                    var token = _stream.Read();
                    if (token.Text != "null")
                        Error(token, "Invalid default value for field");
                    defaultValue = new ReadOnlyToken(token.Text, token.Position);
                }
                else
                    Error(_stream.Read(), "Invalid default value for field");
            }
            if (!_table.TryAdd(new SymbolLeaf(_table.Current, field.Text, SymbolType.Field, scope)))
                Error(field, $"Object {_table.Current.Name} already defines a member with name {field.Text}");

            return new ObjectField(field.Text, field.Position, defaultValue);
        }

        private ScriptNode ScriptDeclaration(SymbolScope scope)
        {
            Token start = null;
            start = Consume(TokenType.Script, "Expected 'script'");

            var name = Consume(TokenType.Identifier, "Expected name for script");
            if(!_table.TryEnterNew(name.Text, SymbolType.Script, scope))
                Error(name, $"Name conflict between script and {_table.Defined(name.Text).Type} {name.Text}");

            var arguments = ReadScriptArguments();
            var body = BlockStatement();
            _table.Exit();
            return new ScriptNode(name.Text, arguments, body, name.Position);
        }

        private ISyntaxElement Statement()
        {
            if (Check(TokenType.Var))
                return Locals();

            return EmbeddedStatement();
        }

        private LocalsNode Locals()
        {
            var start = Consume(TokenType.Var, "Expected 'var'");
            var locals = new List<VariableDeclaration>();
            do
            {
                var name = Consume(TokenType.Identifier, "Expected name for local variable");
                if (!_table.AddLeaf(name.Text, SymbolType.Variable, SymbolScope.Local) && _table.Defined(name.Text).Type != SymbolType.Variable)
                    Error(name, $"Identifier {name.Text} is already defined");

                ISyntaxElement value = null;
                if (Check(TokenType.Assign))
                {
                    Consume(TokenType.Assign, "Expected '='");
                    value = Expression();
                }
                locals.Add(new VariableDeclaration(name.Text, value, name.Position));
            }
            while (Match(TokenType.Comma));
            return new LocalsNode(locals, start.Position);
        }

        private ISyntaxElement EmbeddedStatement()
        {
            if (Check(TokenType.OpenBrace))
                return BlockStatement();
            else
                return SimpleStatement();
        }

        private ISyntaxElement SimpleStatement()
        {
            Token token;
            switch(_stream.Current.Type)
            {
                case TokenType.SemiColon:
                    return new EndToken(Consume(TokenType.SemiColon, "Expected ;").Position);
                case TokenType.Break:
                    return new BreakToken(Consume(TokenType.Break, "Expected 'break'").Position);
                case TokenType.Continue:
                    return new ContinueToken(Consume(TokenType.Continue, "Expected 'continue;").Position);
                case TokenType.Return:
                    token = Consume(TokenType.Return, "Expected 'return'");
                    ISyntaxElement result = null;
                    if (!(Check(TokenType.OpenBrace) || Check(TokenType.CloseBrace) || Check(TokenType.SemiColon)))
                        result = Expression();
                    return new ReturnNode(result, token.Position);
                case TokenType.Repeat:
                    token = Consume(TokenType.Repeat, "Expected 'repeat'");
                    var paren = Match(TokenType.OpenParen);
                    var repeatCount = Expression();
                    if (paren)
                        Consume(TokenType.CloseParen, "Expected matching ')' after repeat count");
                    var body = BodyStatement();
                    return new RepeatNode(repeatCount, body, token.Position);
                case TokenType.While:
                    token = Consume(TokenType.While, "Expected 'while'");
                    paren = Match(TokenType.OpenParen);
                    var condition = Expression();
                    if (paren)
                        Consume(TokenType.CloseParen, "Expected matching ')' after while condition");
                    body = BodyStatement();
                    return new WhileNode(condition, body, token.Position);
                case TokenType.Do:
                    token = Consume(TokenType.Do, "Expected 'do'");
                    body = BodyStatement();
                    while (Match(TokenType.SemiColon)) ;
                    Consume(TokenType.Until, "Expected 'until' after do body");
                    paren = Match(TokenType.OpenParen);
                    condition = Expression();
                    if (paren)
                        Consume(TokenType.CloseParen, "Expected matching ')' after with target");
                    return new DoNode(body, condition, token.Position);
                case TokenType.If:
                    token = Consume(TokenType.If, "Expected 'if'");
                    paren = Match(TokenType.OpenParen);
                    condition = Expression();
                    if (paren)
                        Consume(TokenType.CloseParen, "Expected matching ')' after if condition");
                    body = BodyStatement();
                    Match(TokenType.SemiColon);
                    ISyntaxElement elseBranch = null;
                    if (Match(TokenType.Else))
                        elseBranch = BodyStatement();
                    return new IfNode(condition, body, elseBranch, token.Position);
                case TokenType.For:
                    token = Consume(TokenType.For, "Expected 'for'");
                    Consume(TokenType.OpenParen, "Expected '(' after for");
                    ISyntaxElement init = null;
                    ISyntaxElement increment = null;

                    // If there is no initializer, we can make init an EndToken
                    // because they don't get processed but they can be visited.
                    if (!Check(TokenType.SemiColon))
                        init = BodyStatement();
                    else
                        init = new EndToken(_stream.Position);

                    var semi = Consume(TokenType.SemiColon, "Expected ';' after for initializer");

                    //Todo: Make sure this could potentially be a bool value.
                    if (!Check(TokenType.SemiColon))
                        condition = Expression();
                    else
                        condition = new ConstantToken<bool>("true", true, ConstantType.Bool, semi.Position);

                    Consume(TokenType.SemiColon, "Expected ';' after for condition");

                    if (!Check(TokenType.CloseParen))
                        increment = BodyStatement();
                    else
                        increment = new EndToken(_stream.Position);

                    Consume(TokenType.CloseParen, "Expected ')' after for increment");
                    body = BodyStatement();
                    return new ForNode(init, condition, increment, body, token.Position);
                case TokenType.Switch:
                    token = Consume(TokenType.Switch, "Expected 'switch'");
                    paren = Match(TokenType.OpenParen);
                    var value = Expression();
                    if (paren)
                        Consume(TokenType.CloseParen, "Expected matching ')' after switch value");
                    Consume(TokenType.OpenBrace, "Expected '{' after switch value");
                    var cases = new List<SwitchCase>();
                    ISyntaxElement defaultCase = null;
                    int defaultIndex = -1;
                    var i = 0;
                    while(!Check(TokenType.CloseBrace))
                    {
                        if(Match(TokenType.Case))
                            cases.Add(new SwitchCase(Expression(), SwitchBody()));
                        else if(Check(TokenType.Default))
                        {
                            var defaultToken = Consume(TokenType.Default, "Expected 'default'");
                            if (defaultCase != null)
                                Error(defaultToken, "A switch statement can only have one default case");
                            defaultCase = SwitchBody();
                            defaultIndex = i;
                        }
                        i++;
                    }
                    Consume(TokenType.CloseBrace, "Expected '}' after switch cases");
                    return new SwitchNode(value, cases, defaultCase, defaultIndex, token.Position);
                default:
                    return ExpressionStatement();
            }
        }

        private BlockNode SwitchBody()
        {
            List<ISyntaxElement> statements = new List<ISyntaxElement>();
            var start = Consume(TokenType.Colon, "Expected ':' after switch case");
            while (!_stream.Finished && !Check(TokenType.Case) && !Check(TokenType.Default) && !Check(TokenType.CloseBrace))
                statements.Add(Statement());
            return new BlockNode(statements, start.Position);
        }

        private BlockNode BlockStatement()
        {
            var start = Consume(TokenType.OpenBrace, "Expected '{' at start of block");
            var statements = new List<ISyntaxElement>();
            while (!_stream.Finished && !Check(TokenType.CloseBrace))
                statements.Add(Statement());

            Consume(TokenType.CloseBrace, "Expected '}' after block body");
            return new BlockNode(statements, start.Position);
        }

        private BlockNode BodyStatement()
        {
            if (Check(TokenType.OpenBrace))
                return BlockStatement();

            var body = Statement();
            return new BlockNode(new List<ISyntaxElement>() { body }, body.Position);
        }

        private ISyntaxElement ExpressionStatement()
        {
            _canAssign = true;
            var value = Expression();
            _canAssign = false;
            if (!ExpressionIsStatement(value))
                Error(value.Position, "Only assignment, call, increment, decrement, and new expressions can be used as a statement");
            return value;
        }

        private ISyntaxElement Expression()
        {
            return LambdaExpression();
        }

        private ISyntaxElement LambdaExpression()
        {
            if(Check(TokenType.Script))
            {
                var token = _stream.Read();
                var scope = $"lambda{_lambdaId++}";
                _table.EnterNew(scope, SymbolType.Script, SymbolScope.Local);
                var arguments = ReadScriptArguments();
                var body = BlockStatement();
                _table.Exit();
                return new LambdaNode(scope, arguments, body, token.Position);
            }
            return AssignmentExpression();
        }

        private ISyntaxElement AssignmentExpression()
        {
            var expr = ConditionalExpression();
            if (expr is null)
                return null;

            if(_canAssign && IsAssignment() && (expr.Type == SyntaxType.Variable || expr.Type.ToString().Contains("Access")))
            {
                _canAssign = false;
                var op = _stream.Read();
                var next = Expression();
                expr = new AssignNode(expr, op.Text, next, op.Position);
            }

            return expr;
        }

        private ISyntaxElement ConditionalExpression()
        {
            var expr = LogicalOrExpression();
            if(Check(TokenType.QuestionMark))
            {
                var question = Consume(TokenType.QuestionMark, "Expected '?'");
                var left = ConditionalExpression();
                Consume(TokenType.Colon, "Expected ':' between conditional values");
                var right = ConditionalExpression();
                expr = new ConditionalNode(expr, left, right, question.Position);
            }
            return expr;
        }

        private ISyntaxElement LogicalOrExpression()
        {
            var expr = LogicalAndExpression();
            while(Check(TokenType.LogicalOr))
            {
                var symbol = _stream.Read();
                expr = new LogicalNode(expr, "||", LogicalAndExpression(), symbol.Position);
            }
            return expr;
        }

        private ISyntaxElement LogicalAndExpression()
        {
            var expr = BitwiseOrExpression();
            while (Check(TokenType.LogicalAnd))
            {
                var symbol = _stream.Read();
                expr = new LogicalNode(expr, "&&", BitwiseOrExpression(), symbol.Position);
            }
            return expr;
        }

        private ISyntaxElement BitwiseOrExpression()
        {
            var expr = BitwiseXorExpression();
            while (Check(TokenType.BitwiseOr))
            {
                var symbol = _stream.Read();
                expr = new BitwiseNode(expr, symbol.Text, BitwiseXorExpression(), symbol.Position);
            }
            return expr;
        }

        private ISyntaxElement BitwiseXorExpression()
        {
            var expr = BitwiseAndExpression();
            while (Check(TokenType.Xor))
            {
                var symbol = _stream.Read();
                expr = new BitwiseNode(expr, symbol.Text, BitwiseAndExpression(), symbol.Position);
            }
            return expr;
        }

        private ISyntaxElement BitwiseAndExpression()
        {
            var expr = EqualityExpression();
            while (Check(TokenType.BitwiseAnd))
            {
                var symbol = _stream.Read();
                expr = new BitwiseNode(expr, symbol.Text, EqualityExpression(), symbol.Position);
            }
            return expr;
        }

        private ISyntaxElement EqualityExpression()
        {
            var expr = RelationalExpression();
            while (Check(TokenType.Equal) || Check(TokenType.NotEqual) || (!_canAssign && Check(TokenType.Assign)))
            {
                var symbol = _stream.Read();
                expr = new EqualityNode(expr, symbol.Text, RelationalExpression(), symbol.Position);
            }
            return expr;
        }

        private ISyntaxElement RelationalExpression()
        {
            var expr = ShiftExpression();

            while(Check(TokenType.LessThan) || 
                  Check(TokenType.LessThanOrEqual) || 
                  Check(TokenType.GreaterThan) || 
                  Check(TokenType.GreaterThanOrEqual))
            {
                var symbol = _stream.Read();
                expr = new RelationalNode(expr, symbol.Text, ShiftExpression(), symbol.Position);
            }

            return expr;
        }

        private ISyntaxElement ShiftExpression()
        {
            var expr = AdditiveExpression();

            while(Check(TokenType.ShiftLeft) || Check(TokenType.ShiftRight))
            {
                var symbol = _stream.Read();
                expr = new ShiftNode(expr, symbol.Text, AdditiveExpression(), symbol.Position);
            }

            return expr;
        }

        private ISyntaxElement AdditiveExpression()
        {
            var expr = MultiplicativeExpression();
            while(Check(TokenType.Plus) || Check(TokenType.Minus))
            {
                var symbol = _stream.Read();
                expr = new AdditiveNode(expr, symbol.Text, MultiplicativeExpression(), symbol.Position);
            }

            return expr;
        }

        private ISyntaxElement MultiplicativeExpression()
        {
            var expr = UnaryExpression();

            while(Check(TokenType.Multiply) || Check(TokenType.Divide) || Check(TokenType.Modulo))
            {
                var symbol = _stream.Read();
                expr = new MultiplicativeNode(expr, symbol.Text, UnaryExpression(), symbol.Position);
            }

            return expr;
        }

        private ISyntaxElement UnaryExpression()
        {
            if(Check(TokenType.Plus) || 
                Check(TokenType.Minus) || 
                Check(TokenType.Not) ||
                Check(TokenType.Increment) ||
                Check(TokenType.Decrement) ||
                Check(TokenType.Complement))
            {
                var symbol = _stream.Read();
                return new PrefixNode(symbol.Text, UnaryExpression(), symbol.Position);
            }
            return PrimaryExpression();
        }

        private ISyntaxElement PrimaryExpression()
        {
            var expr = PrimaryExpressionStart();
            if (expr is null)
                return null;

            while(Check(TokenType.OpenParen) || Check(TokenType.Dot) || Check(TokenType.OpenBracket))
            {
                var symbol = _stream.Read();
                switch(symbol.Type)
                {
                    case TokenType.OpenParen:
                        var callArguments = new List<ISyntaxElement>();
                        if(!Check(TokenType.CloseParen))
                        {
                            do
                                callArguments.Add(Expression());
                            while (Match(TokenType.Comma));
                        }
                        var end = Consume(TokenType.CloseParen, "Expect ')' after script arguments");
                        expr = new FunctionCallNode(expr, callArguments, symbol.Position, end.Position);
                        break;
                    case TokenType.Dot:
                        ISyntaxElement access = null;
                        if (Check(TokenType.Identifier))
                        {
                            var id = Consume(TokenType.Identifier, "Expected identifier after member access");
                            access = new VariableToken(id.Text, id.Position);
                        }
                        else if (Check(TokenType.ReadOnly))
                        {
                            var id = Consume(TokenType.ReadOnly, "Expected readOnly after member access");
                            if (id.Text != "self")
                                Error(id, "Invalid readonly value on the right side of an access expression");

                            access = new ReadOnlyToken(id.Text, id.Position);
                        }
                        else
                            Error(symbol, "The right side of an access expression must be a variable");

                        expr = new MemberAccessNode(expr, access, access.Position);
                        break;
                    case TokenType.OpenBracket:
                        if (expr.Type == SyntaxType.New)
                            Error(symbol, "Cannot use an accessor on a newed value");

                        var accessArguments = new List<ISyntaxElement>();
                        do
                            accessArguments.Add(Expression());
                        while (Match(TokenType.Comma));
                        Consume(TokenType.CloseBracket, "Expected ']' after accessor arguments");
                        expr = new ArrayAccessNode(expr, accessArguments, symbol.Position);
                        break;
                }
            }
            if(Check(TokenType.Increment) || Check(TokenType.Decrement))
            {
                var symbol = _stream.Read();
                expr = new PostfixNode(expr, symbol.Text, symbol.Position);
            }

            return expr;
        }

        private ISyntaxElement PrimaryExpressionStart()
        {
            if (IsConstant())
                return Constant(0);

            Token symbol;
            if(Check(TokenType.ReadOnly))
            {
                symbol = _stream.Read();
                return new ReadOnlyToken(symbol.Text, symbol.Position);
            }

            if(Check(TokenType.Argument))
            {
                symbol = _stream.Read();
                ISyntaxElement index = null;
                if (symbol.Text != "argument")
                {
                    var sub = symbol.Text.Substring(8);
                    if (!int.TryParse(sub, out var result))
                        Error(symbol, "Invalid argument access");
                    index = new NumberConstant(sub, result, ConstantType.Real, symbol.Position);
                }
                else
                {
                    Consume(TokenType.OpenBracket, "Expected '[' after argument");
                    index = Expression();
                    Consume(TokenType.CloseBracket, "Expected ']' after after argument index");
                }
                return new ArgumentAccessNode(index, symbol.Position);
            }

            if(Check(TokenType.Identifier))
            {
                symbol = _stream.Read();
                return new VariableToken(symbol.Text, symbol.Position);
            }

            if(Check(TokenType.OpenParen))
            {
                _stream.Read();
                var result = Expression();
                Consume(TokenType.CloseParen, "Expected ')'");
                return result;
            }

            if(Check(TokenType.OpenBracket))
            {
                symbol = _stream.Read();
                var args = new List<ISyntaxElement>();
                if(!Check(TokenType.CloseBracket))
                {
                    do
                        args.Add(Expression());
                    while (Match(TokenType.Comma));
                }
                Consume(TokenType.CloseBracket, "Expected ']' after array literal elements");
                return new ArrayLiteralNode(args, symbol.Position);
            }

            if(Check(TokenType.New))
            {
                symbol = _stream.Read();
                var type = ReadTaffyScriptType("Invalid type name after 'new'");

                Consume(TokenType.OpenParen, "Expected '(' after new instance name");
                var args = new List<ISyntaxElement>();
                if(!Check(TokenType.CloseParen))
                {
                    do
                        args.Add(Expression());
                    while (Match(TokenType.Comma));
                }
                var end = Consume(TokenType.CloseParen, "Expected ')' after new instance arguments");
                return new NewNode(type, args, symbol.Position, end.Position);
            }

            if(Check(TokenType.Base))
            {
                symbol = _stream.Read();
                Consume(TokenType.OpenParen, "Expected '(' after 'base'");
                var args = new List<ISyntaxElement>();
                if(!Check(TokenType.CloseParen))
                {
                    do
                        args.Add(Expression());
                    while (Match(TokenType.Comma));
                }
                var end = Consume(TokenType.CloseParen, "Expected ')' after base arguments");
                return new BaseNode(args, end.Position, symbol.Position);
            }

            Error(_stream.Current, "Could not parse syntax");
            return null;
        }

        private bool IsAssignment()
        {
            switch (_stream.Current.Type)
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

        private bool IsConstant()
        {
            var type = _stream.Current.Type;
            return type == TokenType.Number || type == TokenType.String || type == TokenType.Bool;
        }

        private bool ExpressionIsStatement(ISyntaxElement element)
        {
            if (element.Type == SyntaxType.Assign ||
                element.Type == SyntaxType.Postfix ||
                element.Type == SyntaxType.FunctionCall ||
                element.Type == SyntaxType.New ||
                element.Type == SyntaxType.Base)
            {
                return true;
            }

            if(element is PrefixNode prefix)
                return prefix.Op == "++" || prefix.Op == "--";

            return false;
        }

        private IConstantToken Constant(int tableDepth)
        {
            if (TryGetNumber(out var num))
                return num;
            else if (TryGetString(tableDepth, out var str))
                return str;
            else if (TryGetBool(out var b))
                return b;
            else
                Error(_stream.Current, "Expected a constant value", tableDepth);

            return null;
        }

        private bool TryGetBool(out ConstantToken<bool> boolConstant)
        {
            if(Check(TokenType.Bool))
            {
                var boolToken = Consume(TokenType.Bool, "Expected bool");
                bool value = boolToken.Text == "true";
                boolConstant = new ConstantToken<bool>(boolToken.Text, value, ConstantType.Bool, boolToken.Position);
                return true;
            }

            boolConstant = null;
            return false;
        }

        private bool TryGetString(int tableDepth, out ConstantToken<string> str)
        {
            if(Check(TokenType.String))
            {
                var stringToken = Consume(TokenType.String, "Expected string");
                var text = stringToken.Text.Substring(1, stringToken.Text.Length - 2);
                var match = StringParser.Match(text);
                while(match.Success)
                {
                    switch(text[match.Index + 1])
                    {
                        case 'u':
                            var i = match.Index + 2;
                            string num = "";
                            while (HexCharacters.Contains(char.ToLower(text[i])) && i - match.Index < 6)
                                num += text[i++];

                            if(num == "")
                            {
                                var sub = text.Substring(0, match.Index + 1);
                                var last = sub.LastIndexOf('\n');
                                var column = last == -1 ? stringToken.Position.Column + sub.Length : 
                                                          sub.Length - last;
                                var errorPos = new TokenPosition(stringToken.Position.Index + i,
                                                                 stringToken.Position.Line + sub.Count(c => c == '\n'),
                                                                 column,
                                                                 stringToken.Position.File);
                                Error(errorPos, "Invalid hex constant", tableDepth);
                            }
                            num = num.PadLeft(4, '0');
                            byte[] hex = new byte[2];
                            hex[1] = byte.Parse(num.Substring(0, 2), NumberStyles.HexNumber);
                            hex[0] = byte.Parse(num.Substring(2, 2), NumberStyles.HexNumber);
                            if(i - match.Index != 6)
                            {
                                if (text[i] == '\\')
                                    i++;
                            }
                            text = text.Remove(match.Index, i - match.Index)
                                       .Insert(match.Index, Encoding.Unicode.GetString(hex));
                            break;
                        case '\\':
                            text = SimpleReplace(text, match.Index, '\\');
                            break;
                        case 'n':
                            text = SimpleReplace(text, match.Index, '\n');
                            break;
                        case 'r':
                            text = SimpleReplace(text, match.Index, '\r');
                            break;
                        case 't':
                            text = SimpleReplace(text, match.Index, '\t');
                            break;
                        case '"':
                            text = SimpleReplace(text, match.Index, '"');
                            break;
                        case '\'':
                            text = SimpleReplace(text, match.Index, '\'');
                            break;
                    }
                    match = StringParser.Match(text, match.Index + 1);
                }
                str = new ConstantToken<string>(stringToken.Text, text, ConstantType.String, stringToken.Position);
                return true;
            }

            str = null;
            return false;

            string SimpleReplace(string text, int index, char character)
            {
                return text.Remove(index, 2).Insert(index, character.ToString());
            }
        }

        private bool TryGetNumber(out NumberConstant number)
        {
            bool negate = false;
            if (Check(TokenType.Minus))
                negate = true;

            if(Check(TokenType.Number))
            {
                var numberToken = Consume(TokenType.Number, "Expected number");
                var text = numberToken.Text;
                if (text.StartsWith("0x"))
                {
                    text = (negate ? "-" : "") + text.Substring(2);
                    number = new NumberConstant(numberToken.Text,
                                                long.Parse(text, NumberStyles.HexNumber),
                                                ConstantType.Real,
                                                numberToken.Position);
                }
                else if (text.StartsWith("?"))
                {
                    text = (negate ? "-" : "") + text.Substring(1);
                    number = new NumberConstant(numberToken.Text,
                                                long.Parse(text, NumberStyles.HexNumber),
                                                ConstantType.Real,
                                                numberToken.Position);
                }
                else
                {
                    number = new NumberConstant(numberToken.Text,
                                                float.Parse((negate ? "-" : "") + text),
                                                ConstantType.Real,
                                                numberToken.Position);
                }

                return true;
            }

            number = null;
            return false;
        }

        private string ResolveNamespace()
        {
            var sb = new StringBuilder();

            var name = sb.Append(Consume(TokenType.Identifier, "Expected identifier for namespace").Text);
            while (Match(TokenType.Dot))
            {
                sb.Append(".")
                  .Append(Consume(TokenType.Identifier, "Expected identifier for namespace").Text);
            }

            return name.ToString();
        }

        private bool Match(TokenType type)
        {
            if (Check(type))
            {
                _stream.Read();
                return true;
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (_stream.Finished)
                return false;

            return _stream.Current.Type == type;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return _stream.Read();

            Error(_stream.Current, message);
            return null;
        }

        private Token Consume(TokenType type, string message, int exitTableCount)
        {
            if (Check(type))
                return _stream.Read();

            Error(_stream.Current, message, exitTableCount);
            return null;
        }

        private void Error(Token token, string message)
        {
            _logger.Error(message, token.Position);
            throw new ParseException(message);
        }

        private void Error(TokenPosition position, string message)
        {
            _logger.Error(message, position);
            throw new ParseException(message);
        }

        private void Error(Token token, string message, int exitTableCount)
        {
            _table.Exit(exitTableCount);
            _logger.Error(message, token.Position);
            throw new ParseException(message);
        }

        private void Error(TokenPosition position, string message, int exitTableCount)
        {
            _table.Exit(exitTableCount);
            _logger.Error(message, position);
            throw new ParseException(message);
        }

        private string ReadDotNetType()
        {
            var sb = new StringBuilder(ResolveNamespace());
            if(Match(TokenType.LessThan))
            {
                sb.Append("<");
                do
                    sb.Append(ReadDotNetType());
                while (Match(TokenType.Comma));
                sb.Append(Consume(TokenType.GreaterThan, "Expect '>' after generic types").Text);
            }
            if(Match(TokenType.OpenBracket))
            {
                sb.Append("[");
                while (Match(TokenType.Comma))
                    sb.Append(",");
                sb.Append(Consume(TokenType.CloseBracket, "Expect ']' after array type").Text);
            }
            return sb.ToString();
        }

        private string ReadDotNetMethod()
        {
            var sb = new StringBuilder(Consume(TokenType.Identifier, "Expected c# method name").Text);
            if (Match(TokenType.LessThan))
            {
                sb.Append("<");
                do
                    sb.Append(ReadDotNetType());
                while (Match(TokenType.Comma));
                sb.Append(Consume(TokenType.GreaterThan, "Expect '>' after generic types").Text);
            }
            return sb.ToString();
        }

        private List<string> ReadImportArguments()
        {
            var arguments = new List<string>();
            if(!Check(TokenType.CloseParen))
            {
                do
                {
                    var token = _stream.Read();

                    if (!TsTypes.BasicTypes.ContainsKey(token.Text))
                        Error(token, $"Import argument must be one of the following: {string.Join(", ", TsTypes.BasicTypes.Keys)}");

                    arguments.Add(token.Text);
                }
                while (Match(TokenType.Comma));
            }

            return arguments;
        }

        private List<VariableDeclaration> ReadScriptArguments()
        {
            var arguments = new List<VariableDeclaration>();
            if (Match(TokenType.OpenParen) && !Match(TokenType.CloseParen))
            {
                var optional = false;
                do
                {
                    var argument = Consume(TokenType.Identifier, "Expected argument name", 1);
                    if (!_table.AddLeaf(argument.Text, SymbolType.Variable, SymbolScope.Local))
                        Error(argument, "Cannot have two arguments with the same name", 1);

                    ISyntaxElement value = null;
                    if (Match(TokenType.Assign))
                    {
                        optional = true;
                        switch (_stream.Current.Type)
                        {
                            case TokenType.Minus:
                                var minus = Consume(TokenType.Minus, "Expected '-'", 1);
                                if (!TryGetNumber(out var num))
                                    Error(minus, "Expected number after '-'", 1);
                                value = new NumberConstant("-" + num, -num.Value, ConstantType.Real, minus.Position);
                                break;
                            case TokenType.ReadOnly:
                                var read = Consume(TokenType.ReadOnly, "Expected readonly value", 1);
                                value = new ReadOnlyToken(read.Text, read.Position);
                                break;
                            default:
                                if (!IsConstant())
                                    Error(_stream.Current, "A default script argument must be a constant value", 1);

                                value = Constant(1);
                                break;
                        }
                    }
                    else
                    {
                        if (optional)
                            Error(_stream.Current, "Cannot have a non-default script argument after a default script argument", 1);
                    }
                    arguments.Add(new VariableDeclaration(argument.Text, value, argument.Position));
                }
                while (Match(TokenType.Comma));
                Consume(TokenType.CloseParen, "Expected ')' after script arguments", 1);
            }

            return arguments;
        }

        private ISyntaxElement ReadTaffyScriptType(string errorMessage)
        {
            var token = Consume(TokenType.Identifier, errorMessage);
            ISyntaxElement type = new VariableToken(token.Text, token.Position);
            while (Match(TokenType.Dot))
            {
                token = Consume(TokenType.Identifier, errorMessage);
                type = new MemberAccessNode(type, new VariableToken(token.Text, token.Position), token.Position);
            }
            return type;
        }

        private void Synchronize(params TokenType[] tokens)
        {
            _stream.Read();
            var set = new HashSet<TokenType>(tokens);
            while(!_stream.Finished)
            {
                if (_stream.Current != null && set.Contains(_stream.Current.Type))
                    return;
                _stream.Read();
            }
        }

        private void SynchronizeBraces(bool startWithOne, params TokenType[] tokens)
        {
            _stream.Read();
            var set = new HashSet<TokenType>(tokens);
            var braces = startWithOne ? 1 : 0;
            while(!_stream.Finished)
            {
                if (_stream.Current == null)
                    continue;

                switch(_stream.Current.Type)
                {
                    case TokenType.OpenBrace:
                        braces++;
                        break;
                    case TokenType.CloseBrace:
                        if (--braces <= 0)
                            return;
                        break;
                    default:
                        if (set.Contains(_stream.Current.Type))
                            return;
                        break;
                }
                _stream.Read();
            }
        }
    }
}
