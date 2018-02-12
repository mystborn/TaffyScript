using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.FrontEnd;
using TaffyScript.Syntax;

namespace TaffyScript
{
    public class Parser
    {
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

        public Parser()
        {
            _table = new SymbolTable();
            _tree = new SyntaxTree(_table);
            _factory = new SyntaxElementFactory();
        }

        public void Parse(string code)
        {
            using (_stream = new Tokenizer(code))
            {
                _stream.ErrorEncountered += (e) => Errors.Add(e);
                while (!_stream.Finished)
                    _tree.Root.AddChild(DeclarationOrStatement());
            }
        }

        public void ParseFile(string file)
        {
            using (var fs = new System.IO.FileStream(file, System.IO.FileMode.Open))
            {
                using(_stream = new Tokenizer(fs))
                {
                    _stream.ErrorEncountered += (e) => Errors.Add(e);
                    while (!_stream.Finished)
                        _tree.Root.AddChild(DeclarationOrStatement());
                }
            }
        }

        private ISyntaxElement DeclarationOrStatement()
        {
            switch(_stream.Peek().Type)
            {
                case "object":
                    Confirm("object");
                    var objName = Confirm("id");
                    _table.EnterNew(objName.Value, SymbolType.Object);
                    var node = _factory.CreateNode(SyntaxType.Object, objName.Value, objName.Position);
                    Confirm("{");
                    while (!Try("}"))
                    {
                        Confirm("event");
                        var evName = Confirm("id");
                        _table.AddLeaf(evName.Value, SymbolType.Script, SymbolScope.Member);
                        var eventNode = _factory.CreateNode(SyntaxType.Event, evName.Value, evName.Position);
                        eventNode.AddChild(BlockStatement());
                        node.AddChild(eventNode);
                    }
                    Confirm("}");
                    _table.Exit();
                    return node;
                case "enum":
                    Confirm("enum");
                    var enumName = Confirm("id");
                    _table.EnterNew(enumName.Value, SymbolType.Enum);
                    node = _factory.CreateNode(SyntaxType.Enum, enumName.Value, enumName.Position);
                    Confirm("{");
                    if(!Try("}"))
                    {
                        do
                        {
                            var name = Confirm("id");
                            _table.AddLeaf(name.Value, SymbolType.Variable, SymbolScope.Member);
                            ISyntaxNode nameNode;
                            if (Validate("="))
                            {
                                nameNode = _factory.CreateNode(SyntaxType.Assign, name.Value, name.Position);
                                var num = Confirm("num");
                                nameNode.AddChild(_factory.CreateConstant(ConstantType.Real, num.Value, num.Position));
                            }
                            else
                                nameNode = _factory.CreateNode(SyntaxType.Assign, name.Value, name.Position);
                            node.AddChild(nameNode);
                        }
                        while (Validate(","));
                    }
                    Confirm("}");
                    _table.Exit();
                    return node;
                case "import":
                    var import = Confirm("import");
                    node = _factory.CreateNode(SyntaxType.Import, import.Position);
                    var start = Confirm("id");
                    var baseType = new StringBuilder(start.Value);
                    do
                    {
                        baseType.Append(Confirm(".").Value);
                        baseType.Append(Confirm("id").Value);
                    }
                    while (Try("."));
                    node.AddChild(_factory.CreateConstant(ConstantType.String, baseType.ToString(), start.Position));
                    Confirm("(");
                    if(!Try(")"))
                    {
                        do
                        {
                            if (!(Try("id", out var token) || Try("object", out token)))
                            {
                                Throw(new InvalidTokenException(_stream.Read()));
                                continue;
                            }
                            var type = token.Value;
                            if (type == "array")
                                type = "array1d";
                            if (type != "object" && type != "instance" && type != "float" && type != "int" && type != "bool" && type != "string" && type != "array1d" && type != "array2d")
                            {
                                Throw(new InvalidTokenException(token, "Import type must be one of the following: object, float, int, bool, string, instance, array1d, array2d "));
                                node.AddChild(null);
                            }
                            else
                                node.AddChild(_factory.CreateConstant(ConstantType.String, type, token.Position));
                        }
                        while (Validate(","));
                    }
                    Confirm(")");
                    Confirm("as");
                    var importName = Confirm("id");
                    _table.AddLeaf(importName.Value, SymbolType.Script, SymbolScope.Global);
                    node.AddChild(_factory.CreateConstant(ConstantType.String, importName.Value, importName.Position));
                    return node;
                case "script":
                    Confirm("script");
                    var scriptName = Confirm("id");
                    _table.EnterNew(scriptName.Value, SymbolType.Script);
                    node = _factory.CreateNode(SyntaxType.Script, scriptName.Value, scriptName.Position);
                    node.AddChild(Statement());
                    _table.Exit();
                    return node;
                case ";":
                    Confirm(";");
                    return null;
                default:
                    Throw(new InvalidTokenException(_stream.Peek(), $"Expected declaration, got {_stream.Read().Type}"));
                    return null;
            }
        }

        private ISyntaxElement Statement()
        {
            if (Try("local", out var localToken))
            {
                var locals = _factory.CreateNode(SyntaxType.Locals, localToken.Position);
                do
                {
                    var localName = Confirm("id");
                    if (!_table.Defined(localName.Value, out var symbol))
                        _table.AddLeaf(localName.Value, SymbolType.Variable, SymbolScope.Local);
                    else if (symbol.Type != SymbolType.Variable)
                        Throw(new InvalidTokenException(localName, $"Id already defined for higher priority type: {localName.Value} = {symbol.Type}"));
                    
                    if (Try("=", out var equalToken))
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
            var type = _stream.Peek();
            ISyntaxElement result;
            switch (type.Value)
            {
                case ";":
                    Confirm(";");
                    result = null;
                    break;
                case "break":
                    result = _factory.CreateToken(SyntaxType.Break, Confirm("break").Value, type.Position);
                    break;
                case "continue":
                    result = _factory.CreateToken(SyntaxType.Continue, Confirm("continue").Value, type.Position);
                    break;
                case "exit":
                    result = _factory.CreateToken(SyntaxType.Exit, Confirm("exit").Value, type.Position);
                    break;
                case "return":
                    Confirm("return");
                    var temp = _factory.CreateNode(SyntaxType.Return, type.Position);
                    temp.AddChild(Expression());
                    result = temp;
                    break;
                case "repeat":
                    Confirm("repeat");
                    temp = _factory.CreateNode(SyntaxType.Repeat, type.Position);
                    var paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "while":
                    Confirm("while");
                    temp = _factory.CreateNode(SyntaxType.While, type.Position);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "with":
                    Confirm("with");
                    temp = _factory.CreateNode(SyntaxType.With, type.Position);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "do":
                    Confirm("do");
                    temp = _factory.CreateNode(SyntaxType.Do, type.Position);
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
                    temp = _factory.CreateNode(SyntaxType.If, type.Position);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    temp.AddChild(BodyStatement());
                    while (Validate(";")) ;
                    if (Validate("else"))
                        temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "for":
                    Confirm("for");
                    Confirm("(");
                    temp = _factory.CreateNode(SyntaxType.For, type.Position);
                    if (!Try(";"))
                        temp.AddChild(BodyStatement());
                    else
                        temp.AddChild(_factory.CreateNode(SyntaxType.Block, type.Position));
                    Confirm(";");
                    if (Try(";"))
                        Throw(new InvalidTokenException(_stream.Peek(), "Expected expression in for declaration"));
                    else
                        temp.AddChild(Expression());
                    Confirm(";");
                    temp.AddChild(BodyStatement());
                    Confirm(")");
                    temp.AddChild(BodyStatement());
                    result = temp;
                    break;
                case "switch":
                    Confirm("switch");
                    temp = _factory.CreateNode(SyntaxType.Switch, type.Position);
                    paren = Validate("(");
                    temp.AddChild(Expression());
                    if (paren)
                        Confirm(")");
                    Confirm("{");
                    ISyntaxNode caseNode;
                    while(!Try("}"))
                    {
                        if (Try("case", out var caseToken))
                        {
                            caseNode = _factory.CreateNode(SyntaxType.Case, caseToken.Position);
                            caseNode.AddChild(Expression());
                        }
                        else if (Try("default", out var defaultToken))
                            caseNode = _factory.CreateNode(SyntaxType.Default, defaultToken.Position);
                        else
                        {
                            Throw(new InvalidTokenException(_stream.Peek(), $"Expected case declaration, got {_stream.Read().Value}"));
                            continue;
                        }
                        var blockStart = Confirm(":");
                        var block = _factory.CreateNode(SyntaxType.Block, blockStart.Position);
                        while (!Try("case") && !Try("default") && !Try("}"))
                            block.AddChild(Statement());
                        caseNode.AddChild(block);
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
            var blockStart = Confirm("{");
            var result = _factory.CreateNode(SyntaxType.Block, blockStart.Position);
            while (!_stream.Finished && !Try("}"))
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
            _canAssign++;
            var value = ConditionalExpression();
            if (value == null)
                return null;

            //value = 10;
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

            //true == false ? "hello" : "henlo"
            if (Try("?", out var token))
            {
                var conditional = _factory.CreateNode(SyntaxType.Conditional, token.Position);
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
            while (Try("&&", out var token))
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
            while (Try("|", out var token))
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
            while (Try("^", out var token))
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
            while (Try("&", out var token))
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

            while(Try("==", out var token) || Try("!=", out token) || (_canAssign > 1 && Try("=", out token)))
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

            while(Try("<", out var token) || Try("<=", out token) || Try(">", out token) || Try(">=", out token))
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

            while(Try("<<", out var token) || Try(">>", out token))
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

            while (Try("+", out var token) || Try("-", out token))
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

            while (Try("*", out var token) || Try("/", out token) || Try("%", out token))
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
            if (Try("+", out var token) || Try("-", out token) || Try("!", out token) ||
                Try("~", out token) || Try("++", out token) || Try("--", out token))
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

            if(Try("(", out var paren))
            {
                if (value.Type != SyntaxType.Variable)
                {
                    Throw(new InvalidTokenException(paren, "Invalid identifier for a function call."));
                    return null;
                }
                var function = _factory.CreateNode(SyntaxType.FunctionCall, ((SyntaxToken)value).Text, value.Position);
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
                    {
                        Throw(new InvalidTokenException(next, "The value after a period in an access expression must be a variable."));
                        return null;
                    }
                    var temp = _factory.CreateNode(SyntaxType.MemberAccess, value.Position);
                    temp.AddChild(value);
                    temp.AddChild(_factory.CreateToken(SyntaxType.Variable, next.Value, next.Position));
                    value = temp;
                    wasAccessor = false;
                }
                else if (Try("[", out var accessToken))
                {
                    if (wasAccessor)
                    {
                        Throw(new InvalidTokenException(accessToken, "Cannot have two accessors in a row."));
                    }
                    ISyntaxNode access;
                    if (Validate("|"))
                        access = _factory.CreateNode(SyntaxType.ListAccess, value.Position);
                    else if (Validate("#"))
                        access = _factory.CreateNode(SyntaxType.GridAccess, value.Position);
                    else if (Validate("?"))
                        access = _factory.CreateNode(SyntaxType.MapAccess, value.Position);
                    else if (Validate("@"))
                        access = _factory.CreateNode(SyntaxType.ExplicitArrayAccess, value.Position);
                    else
                        access = _factory.CreateNode(SyntaxType.ArrayAccess, value.Position);

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
            else if (Try("readonly", out var token))
                return _factory.CreateToken(SyntaxType.ReadOnlyValue, token.Value, token.Position);
            else if (Try("argument", out token))
            {
                var arg = _factory.CreateNode(SyntaxType.ArgumentAccess, token.Position);
                if (token.Value != "argument")
                    arg.AddChild(_factory.CreateConstant(ConstantType.Real, token.Value.Remove(0, 8),
                        new TokenPosition(token.Position.Index + 8, token.Position.Line, token.Position.Column + 8, token.Position.File)));
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
                return _factory.CreateToken(SyntaxType.Variable, token.Value, token.Position);
            }
            else if (Validate("("))
            {
                var value = Expression();
                Confirm(")");
                return value;
            }
            else if (Try("[", out token))
            {
                var array = _factory.CreateNode(SyntaxType.ArrayLiteral, token.Position);
                while (!Try("]"))
                {
                    array.AddChild(Expression());
                    Validate(",");
                }
                Confirm("]");
                return array;
            }
            else
            {
                Throw(new InvalidTokenException(_stream.Read()));
                return null;
            }
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
            return type == "num" || type == "string" || type == "bool";
        }

        private bool IsConstant(ISyntaxElement element)
        {
            return element.Type == SyntaxType.Constant;
        }

        private ISyntaxToken Constant()
        {
            if (Try("num", out var token))
                return _factory.CreateConstant(ConstantType.Real, token.Value, token.Position);
            else if (Try("string", out token))
                return _factory.CreateConstant(ConstantType.String, token.Value.Trim('"', '\''),
                    new TokenPosition(token.Position.Index + 1, token.Position.Line, token.Position.Column + 1, token.Position.File));
            else if (Try("bool", out token))
                return _factory.CreateConstant(ConstantType.Bool, token.Value, token.Position);
            else
                Throw(new InvalidTokenException(_stream.Peek(), $"Expected literal, got {_stream.Peek().Type}"));

            return null;
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

        private void Throw(Exception exception)
        {
            Errors.Add(exception);
        }
    }
}
