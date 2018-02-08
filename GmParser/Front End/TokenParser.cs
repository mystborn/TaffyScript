using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myst.LexicalAnalysis;
using GmParser.Syntax;

namespace GmParser
{
    public class Parser
    {
        // Todo:
        // Parse argument constant.

        private static Lexer _lexer = InitLexer();

        private ISyntaxTree _tree;
        private TokenStream _stream;
        private SymbolTable _table;
        private ISyntaxElementFactory _factory;

        public SymbolTable Table => _table;
        public ISyntaxTree Tree => _tree;

        public Parser()
        {
            _table = new SymbolTable();
            _tree = new SyntaxTree(_table);
            _factory = new SyntaxElementFactory();
        }

        public void Parse(string code)
        {
            _stream = new TokenStream(_lexer, code);
            while (!_stream.Finished)
                _tree.Root.AddChild(DeclarationOrStatement());
        }

        private ISyntaxElement DeclarationOrStatement()
        {
            switch(_stream.Peek().Type)
            {
                case "enum":
                    Confirm("enum");
                    var enumName = Confirm("id").Value;
                    _table.EnterNew(enumName, SymbolType.Enum);
                    var node = _factory.CreateNode(SyntaxType.Enum, enumName);
                    //node.AddChild(new SyntaxToken(SyntaxType.Constant, enumName));
                    Confirm("{");
                    if(!Try("}"))
                    {
                        do
                        {
                            var name = Confirm("id").Value;
                            _table.AddLeaf(name, SymbolType.Variable, SymbolScope.Member);
                            ISyntaxNode nameNode;
                            if (Validate("="))
                            {
                                nameNode = _factory.CreateNode(SyntaxType.Assign, name);
                                nameNode.AddChild(_factory.CreateConstant(ConstantType.Real, Confirm("num").Value));
                            }
                            else
                                nameNode = _factory.CreateNode(SyntaxType.Assign, name);
                            node.AddChild(nameNode);
                        }
                        while (Validate(","));
                    }
                    Confirm("}");
                    _table.Exit();
                    return node;
                case "import":
                    Confirm("import");
                    node = _factory.CreateNode(SyntaxType.Import);
                    var baseType = new StringBuilder(Confirm("id").Value);
                    do
                    {
                        baseType.Append(Confirm(".").Value);
                        baseType.Append(Confirm("id").Value);
                    }
                    while (Try("."));
                    node.AddChild(_factory.CreateConstant(ConstantType.String, baseType.ToString()));
                    Confirm("(");
                    if(!Try(")"))
                    {
                        do
                        {
                            var token = Confirm("id");
                            var type = token.Value;
                            if (type != "object" && type != "instance" && type != "double" && type != "string" && type != "array1d" && type != "array2d")
                                throw new InvalidTokenException(token, "Import type must be one of the following: object, double, string, instance, array1d, array2d");
                            node.AddChild(_factory.CreateConstant(ConstantType.String, type));
                        }
                        while (Validate(","));
                    }
                    Confirm(")");
                    Confirm("as");
                    var importName = Confirm("id").Value;
                    _table.AddLeaf(importName, SymbolType.Script, SymbolScope.Global);
                    node.AddChild(_factory.CreateConstant(ConstantType.String, importName));
                    return node;
                case "script":
                    Confirm("script");
                    var scriptName = Confirm("id").Value;
                    _table.EnterNew(scriptName, SymbolType.Script);
                    node = _factory.CreateNode(SyntaxType.Script, scriptName);
                    node.AddChild(Statement());
                    _table.Exit();
                    return node;
                case ";":
                    Confirm(";");
                    return null;
                default:
                    throw new InvalidTokenException(_stream.Peek(), $"Expected declaration, got {_stream.Peek().Type}");
            }
        }

        private ISyntaxElement Statement()
        {
            if (Validate("local"))
            {
                var locals = _factory.CreateNode(SyntaxType.Locals);
                do
                {
                    var localName = Confirm("id");
                    if (!_table.Defined(localName.Value, out var symbol))
                        _table.AddLeaf(localName.Value, SymbolType.Variable, SymbolScope.Local);
                    else if (symbol.Type != SymbolType.Variable)
                        throw new InvalidTokenException(localName, $"Id already defined for higher priority type: {localName.Value} = {symbol.Type}");
                    
                    var id = _factory.CreateToken(SyntaxType.Variable, localName.Value);
                    if (Validate("="))
                    {
                        var assign = _factory.CreateNode(SyntaxType.Assign, "=");
                        assign.AddChild(id);
                        assign.AddChild(Expression());
                        locals.AddChild(assign);
                    }
                    else
                    {
                        var declare = _factory.CreateNode(SyntaxType.Declare);
                        declare.AddChild(id);
                        locals.AddChild(declare);
                    }
                }
                while (Validate(","));
                return locals;
            }
            else
                return EmbeddedStatement();
        }

        private ISyntaxElement EmbeddedStatement()
        {
            ISyntaxElement result;
            if (Try("{"))
                result = BlockStatement();
            else
                result = SimpleStatement();
            return result;
        }

        private ISyntaxElement SimpleStatement()
        {
            var type = _stream.Peek().Type;
            ISyntaxElement result;
            switch (type)
            {
                case ";":
                    Confirm(";");
                    result = null;
                    break;
                case "break":
                    result = _factory.CreateToken(SyntaxType.Break, Confirm("break").Value);
                    break;
                case "continue":
                    result = _factory.CreateToken(SyntaxType.Continue, Confirm("continue").Value);
                    break;
                case "exit":
                    result = _factory.CreateToken(SyntaxType.Exit, Confirm("exit").Value);
                    break;
                case "return":
                    Confirm("return");
                    var temp = _factory.CreateNode(SyntaxType.Return);
                    temp.AddChild(Expression());
                    result = temp;
                    break;
                case "while":
                    Confirm("while");
                    temp = _factory.CreateNode(SyntaxType.While);
                    var paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "with":
                    Confirm("with");
                    temp = _factory.CreateNode(SyntaxType.With);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "do":
                    Confirm("do");
                    temp = _factory.CreateNode(SyntaxType.Do);
                    temp.AddChild(BodyStatement());
                    Confirm("until");
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    result = temp;
                    break;
                case "if":
                    Confirm("if");
                    temp = _factory.CreateNode(SyntaxType.If);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    if (Validate("else"))
                        temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "for":
                    Confirm("for");
                    Confirm("(");
                    temp = _factory.CreateNode(SyntaxType.For);
                    if (!Try(";"))
                        temp.AddChild(BodyStatement());
                    else
                        temp.AddChild(_factory.CreateNode(SyntaxType.Block));
                    Confirm(";");
                    if (Try(";"))
                        throw new InvalidTokenException(_stream.Peek(), "Expected expression in for declaration");
                    temp.AddChild(Expression());
                    Confirm(";");
                    temp.AddChild(BodyStatement());
                    Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "switch":
                    Confirm("switch");
                    temp = _factory.CreateNode(SyntaxType.Switch);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    Confirm("{");
                    ISyntaxNode caseNode;
                    while(!Try("}"))
                    {
                        if (Validate("case"))
                        {
                            caseNode = _factory.CreateNode(SyntaxType.Case);
                            caseNode.AddChild(Expression());
                        }
                        else if (Validate("default"))
                            caseNode = _factory.CreateNode(SyntaxType.Default);
                        else
                            throw new InvalidTokenException(_stream.Peek(), $"Expected case declaration, got {_stream.Peek().Value}");
                        Confirm(":");
                        while (!Try("case") && !Try("default") && !Try("}"))
                            caseNode.AddChild(Statement());
                        temp.AddChild(caseNode);
                    }
                    Confirm("}");
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
            Confirm("{");
            var result = _factory.CreateNode(SyntaxType.Block);
            while (!Try("}"))
                result.AddChild(Statement());

            Confirm("}");
            return result;
        }

        private ISyntaxElement BodyStatement()
        {
            ISyntaxElement body;
            if (Try("{"))
                body = BlockStatement();
            else
            {
                var temp = _factory.CreateNode(SyntaxType.Block);
                temp.AddChild(Statement());
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
            var value = ConditionalExpression();

            //value = 10;
            if ((value.Type.ToString().Contains("Access") || value.Type == SyntaxType.Variable) && IsAssignment())
            {
                var assign = _factory.CreateNode(SyntaxType.Assign, _stream.ReadValue());
                assign.AddChild(value);
                assign.AddChild(AssignmentExpression());
                value = assign;
            }

            return value;
        }

        private ISyntaxElement ConditionalExpression()
        {
            
            var value = LogicalOrExpression();

            //true == false ? "hello" : "henlo"
            if (Validate("?"))
            {
                var conditional = _factory.CreateNode(SyntaxType.Conditional);
                conditional.AddChild(value);
                conditional.AddChild(AssignmentExpression());
                Confirm(":");
                conditional.AddChild(AssignmentExpression());
                value = conditional;
            }

            return value;
        }

        private ISyntaxElement LogicalOrExpression()
        {
            var value = LogicalAndExpression();
            while(Try("||", out var token))
            {
                var or = _factory.CreateNode(SyntaxType.Logical, token.Value);
                or.AddChild(value);
                or.AddChild(LogicalAndExpression());
                value = or;
            }
            return value;
        }

        private ISyntaxElement LogicalAndExpression()
        {
            var value = BitwiseOrExpression();
            while (Try("&&", out var token))
            {
                var and = _factory.CreateNode(SyntaxType.Logical, token.Value);
                and.AddChild(value);
                and.AddChild(BitwiseOrExpression());
                value = and;
            }
            return value;
        }

        private ISyntaxElement BitwiseOrExpression()
        {
            var value = BitwiseXorExpression();
            while (Try("|", out var token))
            {
                var or = _factory.CreateNode(SyntaxType.Bitwise, token.Value);
                or.AddChild(value);
                or.AddChild(BitwiseXorExpression());
                value = or;
            }
            return value;
        }

        private ISyntaxElement BitwiseXorExpression()
        {
            var value = BitwiseAndExpression();
            while (Try("^", out var token))
            {
                var xor = _factory.CreateNode(SyntaxType.Bitwise, token.Value);
                xor.AddChild(value);
                xor.AddChild(BitwiseAndExpression());
                value = xor;
            }
            return value;
        }

        private ISyntaxElement BitwiseAndExpression()
        {
            var value = EqualityExpression();
            while (Try("&", out var token))
            {
                var and = _factory.CreateNode(SyntaxType.Bitwise, token.Value);
                and.AddChild(value);
                and.AddChild(EqualityExpression());
                value = and;
            }
            return value;
        }

        private ISyntaxElement EqualityExpression()
        {
            var value = RelationalExpression();

            while(Try("==", out var token) || Try("!=", out token))
            {
                var equal = _factory.CreateNode(SyntaxType.Equality, token.Value);
                equal.AddChild(value);
                equal.AddChild(RelationalExpression());
                value = equal;
            }

            return value;
        }

        private ISyntaxElement RelationalExpression()
        {
            var value = ShiftExpression();

            while(Try("<", out var token) || Try("<=", out token) || Try(">", out token) || Try(">=", out token))
            {
                var compare = _factory.CreateNode(SyntaxType.Relational, token.Value);
                compare.AddChild(value);
                compare.AddChild(ShiftExpression());
                value = compare;
            }

            return value;
        }

        private ISyntaxElement ShiftExpression()
        {
            var value = AdditiveExpression();

            while(Try("<<", out var token) || Try(">>", out token))
            {
                var shift = _factory.CreateNode(SyntaxType.Shift, token.Value);
                shift.AddChild(value);
                shift.AddChild(AdditiveExpression());
                value = shift;
            }

            return value;
        }

        private ISyntaxElement AdditiveExpression()
        {
            var value = MultiplicativeExpression();

            while (Try("+", out var token) || Try("-", out token))
            {
                var add = _factory.CreateNode(SyntaxType.Additive, token.Value);
                add.AddChild(value);
                add.AddChild(MultiplicativeExpression());
                value = add;
            }

            return value;
        }

        private ISyntaxElement MultiplicativeExpression()
        {
            var value = UnaryExpression();

            while (Try("*", out var token) || Try("/", out token) || Try("%", out token))
            {
                var mul = _factory.CreateNode(SyntaxType.Multiplicative, token.Value);
                mul.AddChild(value);
                mul.AddChild(UnaryExpression());
                value = mul;
            }

            return value;
        }

        private ISyntaxElement UnaryExpression()
        {
            if (Try("+", out var token) || Try("-", out token) || Try("!", out token) ||
                Try("~", out token) || Try("++", out token) || Try("--", out token))
            {
                var prefix = _factory.CreateNode(SyntaxType.Prefix, token.Value);
                prefix.AddChild(UnaryExpression());
                return prefix;
            }
            else
                return PrimaryExpression();
        }

        private ISyntaxElement PrimaryExpression()
        {
            var value = PrimaryExpressionStart();
            if(Try("(", out var paren))
            {
                if (value.Type != SyntaxType.Variable)
                    throw new InvalidTokenException(paren, "Invalid identifier for a function call.");
                var function = _factory.CreateNode(SyntaxType.FunctionCall, ((SyntaxToken)value).Text);
                if(!Try(")"))
                {
                    do
                    {
                        function.AddChild(Expression());
                    }
                    while (Validate(","));
                }
                Confirm(")");
                value = function;
            }
            bool wasAccessor = false;
            while(true)
            {
                if (Validate("."))
                {
                    if (!Try("id", out var next))
                        next = Confirm("this");
                    var temp = _factory.CreateNode(SyntaxType.MemberAccess);
                    temp.AddChild(value);
                    temp.AddChild(_factory.CreateToken(SyntaxType.Variable, next.Value));
                    value = temp;
                    wasAccessor = false;
                }
                else if (Validate("["))
                {
                    if (wasAccessor)
                        throw new InvalidProgramException("Cannot have two accessors in a row.");
                    ISyntaxNode access;
                    if (Validate("|"))
                        access = _factory.CreateNode(SyntaxType.ListAccess);
                    else if (Validate("#"))
                        access = _factory.CreateNode(SyntaxType.GridAccess);
                    else if (Validate("?"))
                        access = _factory.CreateNode(SyntaxType.MapAccess);
                    else if (Validate("@"))
                        access = _factory.CreateNode(SyntaxType.ExplicitArrayAccess);
                    else
                        access = _factory.CreateNode(SyntaxType.ArrayAccess);

                    access.AddChild(value);
                    access.AddChild(Expression());
                    if (Validate(","))
                        access.AddChild(Expression());
                    Confirm("]");
                    value = access;

                    wasAccessor = true;
                }
                else
                    break;
            }
            if(Try("++", out var token) || Try("--", out token))
            {
                var postfix = _factory.CreateNode(SyntaxType.Postfix, token.Value);
                postfix.AddChild(value);
                value = postfix;
            }

            return value;
        }

        private ISyntaxElement PrimaryExpressionStart()
        {
            //Todo:
            //Process self reference. i.e. var i = id|self
            if (IsConstant())
                return Constant();
            else if(Try("readonly", out var token))
                return _factory.CreateToken(SyntaxType.ReadOnlyValue, token.Value);
            else if(Try("argument", out token))
            {
                var arg = _factory.CreateNode(SyntaxType.ArgumentAccess);
                if(token.Value != "argument")
                    arg.AddChild(_factory.CreateConstant(ConstantType.Real, token.Value.Remove(0, 8)));
                else
                {
                    Confirm("[");
                    arg.AddChild(Expression());
                    Confirm("]");
                }
                return arg;
            }
            else if (Try("id", out token))
            {
                if (!_table.Defined(token.Value, out _))
                    _table.AddPending(token.Value);
                return _factory.CreateToken(SyntaxType.Variable, token.Value);
            }
            else if (Validate("("))
            {
                var value = Expression();
                Confirm(")");
                return value;
            }
            else if (Validate("["))
            {
                var array = _factory.CreateNode(SyntaxType.ArrayLiteral);
                while (!Try("]"))
                {
                    array.AddChild(Expression());
                    Validate(",");
                }
                Confirm("]");
                return array;
            }
            else
                throw new InvalidProgramException();
        }

        private bool Try(string next)
        {
            if (_stream.Finished || _stream.Peek().Type != next)
                return false;
            return true;
        }

        private bool Try(string next, out Token result)
        {
            result = default(Token);
            if (_stream.Finished || _stream.Peek().Type != next)
                return false;

            result = _stream.Read();
            return true;
        }

        private bool Validate(string next)
        {
            if(!_stream.Finished && _stream.Peek().Type == next)
            {
                _stream.Read();
                return true;
            }
            return false;
        }

        private Token Confirm(string next)
        {
            if (_stream.Finished)
                throw new InvalidProgramException();
            var result = _stream.Read();
            if (result.Type != next)
                throw new InvalidTokenException(result, $"Expected {next}, got {result.Type}");
            return result;
        }

        private bool IsConstant()
        {
            var type = _stream.Peek().Type;
            return type == "num" || type == "string" || type == "bool";
        }

        private bool IsConstant(ISyntaxElement element)
        {
            return element.Type == SyntaxType.Constant;
        }

        private ISyntaxToken Constant()
        {
            if (Try("num", out var token))
                return _factory.CreateConstant(ConstantType.Real, token.Value);
            else if (Try("string", out token))
                return _factory.CreateConstant(ConstantType.String, token.Value);
            else if (Try("bool", out token))
                return _factory.CreateConstant(ConstantType.Bool, token.Value);
            else
                throw new InvalidTokenException(_stream.Peek(), $"Expected literal, got {_stream.Peek().Type}");
        }

        private bool IsAssignment()
        {
            var type = _stream.Peek().Type;
            return type == "=" ||
                   type == "+=" ||
                   type == "-=" ||
                   type == "*=" ||
                   type == "/=" ||
                   type == "%=" ||
                   type == "&=" ||
                   type == "^=" ||
                   type == "|=";
        }

        private static Lexer InitLexer()
        {
            var lexer = new Lexer();
            lexer.AddDefinition(new TokenDefinition("true|false", "bool"));
            lexer.AddDefinition(new TokenDefinition("var", "local"));
            lexer.AddDefinition(new TokenDefinition("break", "break"));
            lexer.AddDefinition(new TokenDefinition("continue", "continue"));
            lexer.AddDefinition(new TokenDefinition("while", "while"));
            lexer.AddDefinition(new TokenDefinition("do", "do"));
            lexer.AddDefinition(new TokenDefinition("until", "until"));
            lexer.AddDefinition(new TokenDefinition("if", "if"));
            lexer.AddDefinition(new TokenDefinition("else", "else"));
            lexer.AddDefinition(new TokenDefinition("for", "for"));
            lexer.AddDefinition(new TokenDefinition("noone", "noone"));
            lexer.AddDefinition(new TokenDefinition("return", "return"));
            lexer.AddDefinition(new TokenDefinition("exit", "exit"));
            lexer.AddDefinition(new TokenDefinition("switch", "switch"));
            lexer.AddDefinition(new TokenDefinition("case", "case"));
            lexer.AddDefinition(new TokenDefinition("default", "default"));
            lexer.AddDefinition(new TokenDefinition("continue", "continue"));
            lexer.AddDefinition(new TokenDefinition("enum", "enum"));
            lexer.AddDefinition(new TokenDefinition("with", "with"));
            lexer.AddDefinition(new TokenDefinition("this|self|id", "this"));
            lexer.AddDefinition(new TokenDefinition("import", "import"));
            lexer.AddDefinition(new TokenDefinition("script", "script"));
            lexer.AddDefinition(new TokenDefinition(@"argument(\d{1,2})?", "argument"));
            lexer.AddDefinition(new TokenDefinition("argument_count|all|noone|id|self|pi", "readonly"));
            lexer.AddDefinition(new TokenDefinition("as", "as"));
            lexer.AddDefinition(new TokenDefinition(";", ";"));
            lexer.AddDefinition(new TokenDefinition("||", "||"));
            lexer.AddDefinition(new TokenDefinition("&&", "&&"));
            lexer.AddDefinition(new TokenDefinition("!=|<>", "!="));
            lexer.AddDefinition(new TokenDefinition("==", "=="));
            lexer.AddDefinition(new TokenDefinition("<=", "<="));
            lexer.AddDefinition(new TokenDefinition(">=", ">="));
            lexer.AddDefinition(new TokenDefinition("<", "<"));
            lexer.AddDefinition(new TokenDefinition(">", ">"));
            lexer.AddDefinition(new TokenDefinition("!", "!"));
            lexer.AddDefinition(new TokenDefinition("~", "~"));
            lexer.AddDefinition(new TokenDefinition(@"\^", "^"));
            lexer.AddDefinition(new TokenDefinition("|", "|"));
            lexer.AddDefinition(new TokenDefinition("&", "&"));
            lexer.AddDefinition(new TokenDefinition("<<", "<<"));
            lexer.AddDefinition(new TokenDefinition(">>", ">>"));
            lexer.AddDefinition(new TokenDefinition(@"\(", "("));
            lexer.AddDefinition(new TokenDefinition(@"\)", ")"));
            lexer.AddDefinition(new TokenDefinition(@"\[", "["));
            lexer.AddDefinition(new TokenDefinition(@"\]", "]"));
            lexer.AddDefinition(new TokenDefinition(@"\{", "{"));
            lexer.AddDefinition(new TokenDefinition(@"\}", "}"));
            lexer.AddDefinition(new TokenDefinition(@"/\*(?i).*?\*/(?-i)", "multicomment", true));
            lexer.AddDefinition(new TokenDefinition(@"//.*?\n", "singlecomment", true));
            lexer.AddDefinition(new TokenDefinition(@"\.", "."));
            lexer.AddDefinition(new TokenDefinition("(?i)(\".*?\")|('.*?')(?-i)", "string"));
            lexer.AddDefinition(new TokenDefinition("=", "="));
            lexer.AddDefinition(new TokenDefinition(@"\+\+", "++"));
            lexer.AddDefinition(new TokenDefinition("--", "--"));
            lexer.AddDefinition(new TokenDefinition(@"\+=", "+="));
            lexer.AddDefinition(new TokenDefinition("-=", "-="));
            lexer.AddDefinition(new TokenDefinition(@"\*=", "*="));
            lexer.AddDefinition(new TokenDefinition("/=", "/="));
            lexer.AddDefinition(new TokenDefinition("%=", "%="));
            lexer.AddDefinition(new TokenDefinition(@"\+", "+"));
            lexer.AddDefinition(new TokenDefinition("-", "-"));
            lexer.AddDefinition(new TokenDefinition(@"\*", "*"));
            lexer.AddDefinition(new TokenDefinition("/", "/"));
            lexer.AddDefinition(new TokenDefinition("%", "%"));

            //In GM "_" is a valid variable name O_O
            lexer.AddDefinition(new TokenDefinition("[_a-zA-Z][_a-zA-Z0-9]*", "id"));
            lexer.AddDefinition(new TokenDefinition(@"(0x[0-9a-fA-F]+)|([0-9]*(\.[0-9]+)?)", "num"));
            lexer.AddDefinition(new TokenDefinition(",", ","));
            lexer.AddDefinition(new TokenDefinition(@"\?", "?"));
            lexer.AddDefinition(new TokenDefinition("#", "#"));
            lexer.AddDefinition(new TokenDefinition(":", ":"));


            return lexer;
        }
    }
}
