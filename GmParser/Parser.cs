using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myst.LexicalAnalysis;

namespace GmParser
{
    public class Parser
    {
        private Lexer _lexer;

        public Parser()
        {
            _lexer = InitLexer();
        }

        public ISyntaxNode ParseExpression(string expr)
        {
            var tokens = new Queue<Token>(_lexer.Tokenize(expr).Where(t => t.Type != Lexer.EoF));
            return Expression(tokens);
        }

        #region Expressions

        private ISyntaxNode Expression(Queue<Token> tokens)
        {
            SyntaxNode expressions = null;
            var value = AssignmentExpression(tokens);
            /*while(Validate(tokens, "comma"))
            {
                if (expressions == null)
                {
                    expressions = new SyntaxNode("expressions");
                    expressions.AddChild(value);
                }
                expressions.AddChild(AssignmentExpression(tokens));
            }*/
            if (expressions != null)
                return expressions;
            else
                return value;
        }

        private ISyntaxNode AssignmentExpression(Queue<Token> tokens)
        {
            var value = ConditionalExpression(tokens);

            // Extremely hacky. All of the settable values have Access in their type name,
            // so we can assign to it as long as that is true.
            if((value.Type.Contains("Access") || value.Type == "var") && IsAssignment(tokens))
            {
                var assign = new SyntaxNode("assign", tokens.Dequeue().Value);
                assign.AddChild(value);
                assign.AddChild(AssignmentExpression(tokens));
                value = assign;
            }
            return value;
        }

        private ISyntaxNode ConditionalExpression(Queue<Token> tokens)
        {
            var value = LogicalOrExpression(tokens);
            if(Validate(tokens, "question"))
            {
                var conditional = new SyntaxNode("conditional");
                conditional.AddChild(value);
                conditional.AddChild(AssignmentExpression(tokens));
                Confirm(tokens, "colon");
                conditional.AddChild(AssignmentExpression(tokens));
                value = conditional;
            }
            return value;
        }

        private ISyntaxNode LogicalOrExpression(Queue<Token> tokens)
        {
            var value = LogicalAndExpression(tokens);
            while(Try(tokens, "lor", out var token))
            {
                var or = new SyntaxNode("logical", token.Value);
                or.AddChild(value);
                or.AddChild(LogicalAndExpression(tokens));
                value = or;
            }

            return value;
        }

        private ISyntaxNode LogicalAndExpression(Queue<Token> tokens)
        {
            var value = BitwiseOrExpression(tokens);
            while (Try(tokens, "land", out var token))
            {
                var and = new SyntaxNode("logical", token.Value);
                and.AddChild(value);
                and.AddChild(BitwiseOrExpression(tokens));
                value = and;
            }

            return value;
        }

        private ISyntaxNode BitwiseOrExpression(Queue<Token> tokens)
        {
            var value = BitwiseXorExpression(tokens);
            while (Try(tokens, "bor", out var token))
            {
                var or = new SyntaxNode("bitwise", token.Value);
                or.AddChild(value);
                or.AddChild(BitwiseXorExpression(tokens));
                value = or;
            }

            return value;
        }

        private ISyntaxNode BitwiseXorExpression(Queue<Token> tokens)
        {
            var value = BitwiseAndExpression(tokens);
            while (Try(tokens, "xor", out var token))
            {
                var xor = new SyntaxNode("bitwise", token.Value);
                xor.AddChild(value);
                xor.AddChild(BitwiseAndExpression(tokens));
                value = xor;
            }

            return value;
        }

        private ISyntaxNode BitwiseAndExpression(Queue<Token> tokens)
        {
            var value = EqualityExpression(tokens);
            while (Try(tokens, "band", out var token))
            {
                var and = new SyntaxNode("bitwise", token.Value);
                and.AddChild(value);
                and.AddChild(EqualityExpression(tokens));
                value = and;
            }

            return value;
        }

        private ISyntaxNode EqualityExpression(Queue<Token> tokens)
        {
            var value = RelationalExpression(tokens);
            while(Try(tokens, "eq", out var token) || Try(tokens, "neq", out token))
            {
                var equal = new SyntaxNode("equality", token.Value);
                equal.AddChild(value);
                equal.AddChild(RelationalExpression(tokens));
                value = equal;
            }
            return value;
        }

        private ISyntaxNode RelationalExpression(Queue<Token> tokens)
        {
            var value = ShiftExpression(tokens);
            while (Try(tokens, "lt", out var token) || Try(tokens, "lte", out token) || Try(tokens, "gt", out token) || Try(tokens, "gte", out token))
            {
                var compare = new SyntaxNode("compare", token.Value);
                compare.AddChild(value);
                compare.AddChild(ShiftExpression(tokens));
                value = compare;
            }

            return value;
        }

        private ISyntaxNode ShiftExpression(Queue<Token> tokens)
        {
            var value = AdditiveExpression(tokens);
            while(Try(tokens, "lshift", out var token) || Try(tokens, "rshift", out token))
            {
                var shift = new SyntaxNode("shift", token.Value);
                shift.AddChild(value);
                shift.AddChild(AdditiveExpression(tokens));
                value = shift;
            }
            return value;
        }

        private ISyntaxNode AdditiveExpression(Queue<Token> tokens)
        {
            var value = MultiplicativeExpression(tokens);
            while(Try(tokens, "add", out var token) || Try(tokens, "sub", out token))
            {
                var add = new SyntaxNode("additive", token.Value);
                add.AddChild(value);
                add.AddChild(MultiplicativeExpression(tokens));
                value = add;
            }
            return value;
        }

        private ISyntaxNode MultiplicativeExpression(Queue<Token> tokens)
        {
            var value = UnaryExpression(tokens);
            while (Try(tokens, "mul", out var token) || Try(tokens, "div", out token) || Try(tokens, "mod", out token))
            {
                var mul = new SyntaxNode("multiplicative", token.Value);
                mul.AddChild(value);
                mul.AddChild(UnaryExpression(tokens));
                value = mul;
            }
            return value;
        }

        private ISyntaxNode UnaryExpression(Queue<Token> tokens)
        {
            ISyntaxNode node = null;
            //Prefix Operators
            if (Try(tokens, "add", out var token) || Try(tokens, "sub", out token) || Try(tokens, "not", out token) ||
                  Try(tokens, "complement", out token) || Try(tokens, "increment", out token) || Try(tokens, "decrement", out token))
            {
                var unary = new SyntaxNode("unary", token.Value);
                unary.AddChild(UnaryExpression(tokens));
                node = unary;
            }
            else
                node = PrimaryExpression(tokens);

            return node;
        }

        private ISyntaxNode PrimaryExpression(Queue<Token> tokens)
        {
            var value = PrimaryExpressionStart(tokens);
            if(Try(tokens, "oparen", out var paren))
            {
                if (value.Type != "var")
                    throw new InvalidTokenException(paren, "Invalid identifier for method call.");
                var method = new SyntaxNode("methodCall");
                method.AddChild(value);
                while(!Try(tokens, "cparen"))
                {
                    method.AddChild(Expression(tokens));
                    Validate(tokens, "comma");
                }
                Confirm(tokens, "cparen");
                value = method;
            }
            bool wasAccessor = false;
            while (true)
            {
                if (Validate(tokens, "dot"))
                {
                    if (!Try(tokens, "id", out var next))
                        next = Confirm(tokens, "this");
                    var temp = new SyntaxNode("memberAccess");
                    temp.AddChild(value);
                    temp.AddChild(new SyntaxToken("var", next.Value));
                    value = temp;
                    wasAccessor = false;
                }
                else if (Validate(tokens, "obracket"))
                {
                    if (wasAccessor)
                        throw new InvalidProgramException("Cannot have two accessors in a row.");
                    SyntaxNode temp;
                    if (Validate(tokens, "bor"))
                        temp = new SyntaxNode("listAccess");
                    else if (Validate(tokens, "sharp"))
                        temp = new SyntaxNode("gridAccess");
                    else if (Validate(tokens, "question"))
                        temp = new SyntaxNode("mapAccess");
                    else if (Validate(tokens, "at"))
                        temp = new SyntaxNode("arrayExplicitAccess");
                    else
                        temp = new SyntaxNode("arrayAccess");

                    temp.AddChild(value);
                    temp.AddChild(Expression(tokens));
                    if (Validate(tokens, "comma"))
                        temp.AddChild(Expression(tokens));
                    Confirm(tokens, "cbracket");
                    value = temp;

                    wasAccessor = true;
                }
                else
                    break;
            }
            if(Try(tokens, "increment", out var post) || Try(tokens, "decrement", out post))
            {
                var postfix = new SyntaxNode("postfix", post.Value);
                postfix.AddChild(value);
            }

            return value;
        }

        private ISyntaxNode PrimaryExpressionStart(Queue<Token> tokens)
        {
            if (IsLiteral(tokens))
                return Literal(tokens);
            else if (Try(tokens, "id", out var id))
                return new SyntaxToken("var", id.Value);
            else if (Validate(tokens, "oparen"))
            {
                var value = Expression(tokens);
                Confirm(tokens, "cparen");
                return value;
            }
            else if (Try(tokens, "this", out var token))
                return new SyntaxToken("this", token.Value);
            else if (Validate(tokens, "obracket"))
            {
                var array = new SyntaxNode("array");
                while (!Try(tokens, "cbracket"))
                {
                    array.AddChild(Expression(tokens));
                    Validate(tokens, "comma");
                }
                return array;
            }
            else
                throw new InvalidProgramException();
        }

        #endregion

        #region Statements

        private ISyntaxNode Statement(Queue<Token> tokens)
        {
            if (Try(tokens, "local") || Try(tokens, "id"))
                return VariableDeclaration(tokens);
            else
                return EmbeddedStatement(tokens);
        }

        private ISyntaxNode VariableDeclaration(Queue<Token> tokens)
        {
            //members must be set
            if (Try(tokens, "id", out var member))
            {
                Confirm(tokens, "assign");
                var node = new SyntaxNode("assign");
                node.AddChild(new SyntaxToken("var", member.Value));
                node.AddChild(Expression(tokens));
                return node;
            }
            //locals can be set or declared
            else if (Validate(tokens, "local"))
            {
                var node = new SyntaxNode("locals");
                do
                {
                    var id = new SyntaxToken("var", Confirm(tokens, "id").Value);
                    if (Validate(tokens, "assign"))
                    {
                        var temp = new SyntaxNode("assign");
                        temp.AddChild(id);
                        temp.AddChild(Expression(tokens));
                        node.AddChild(temp);
                    }
                    else
                    {
                        var temp = new SyntaxNode("declare");
                        temp.AddChild(id);
                        node.AddChild(temp);
                    }
                }
                while (Validate(tokens, "comma"));
                return node;
            }
            else
                throw new InvalidTokenException(tokens.Peek());
        }

        private ISyntaxNode EmbeddedStatement(Queue<Token> tokens)
        {
            if (Try(tokens, "obrace"))
                return BlockStatement(tokens);
            else
                return SimpleStatement(tokens);
        }

        private ISyntaxNode SimpleStatement(Queue<Token> tokens)
        {
            //Can't dequeue in case of expression.
            var type = tokens.Peek().Type;
            ISyntaxNode result;
            switch(type)
            {
                //Technically valid statement.
                //Does nothing. Should be culled.
                case "end":
                    result = new SyntaxToken("end", Confirm(tokens, "end").Value);
                    break;
                case "if":
                    var temp = new SyntaxNode("if");
                    var paren = Validate(tokens, "oparen");
                    temp.AddChild(Expression(tokens));
                    if (paren)
                        Confirm(tokens, "cparen");
                    temp.AddChild(EmbeddedStatement(tokens));
                    if (Validate(tokens, "else"))
                        temp.AddChild(EmbeddedStatement(tokens));
                    result = temp;
                    break;
                case "switch":
                    temp = new SyntaxNode("switch");
                    paren = Validate(tokens, "oparen");
                    temp.AddChild(Expression(tokens));
                    if (paren)
                        Confirm(tokens, "cparen");
                    Confirm(tokens, "obrace");
                    SyntaxNode caseNode;
                    while(!Try(tokens, "cbrace"))
                    {
                        if (Try(tokens, "case", out var caseType) || Try(tokens, "default", out caseType))
                        {
                            caseNode = new SyntaxNode(caseType.Type);
                            while (!Try(tokens, "case") && !Try(tokens, "default"))
                                caseNode.AddChild(Statement(tokens));
                            temp.AddChild(caseNode);
                        }
                        else
                            throw new InvalidTokenException(tokens.Peek());
                    }
                    Confirm(tokens, "cbrace");
                    result = temp;
                    break;
                
                default:
                    result = Expression(tokens);
                    break;
            }

            return result;
        }

        private ISyntaxNode BlockStatement(Queue<Token> tokens)
        {
            Confirm(tokens, "obrace");
            var result = new SyntaxNode("block");
            while (!Try(tokens, "cbrace"))
                result.AddChild(Statement(tokens));

            //Make sure we didn't just run out of tokens.
            Confirm(tokens, "cbrace");
            return result;
        }

        #endregion

        #region Helpers

        private SyntaxToken Literal(Queue<Token> tokens)
        {
            if (Try(tokens, "num"))
                return Number(tokens);
            else if (Try(tokens, "bool"))
                return Bool(tokens);
            else if (Try(tokens, "string"))
                return String(tokens);
            else
                throw new InvalidTokenException(tokens.Peek(), $"Expected a literal, got {tokens.Peek().Type}");
        }

        private SyntaxToken Bool(Queue<Token> tokens)
        {
            var token = Confirm(tokens, "bool");
            return new SyntaxToken("bool", token.Value);
        }

        private SyntaxToken String(Queue<Token> tokens)
        {
            var token = Confirm(tokens, "string");
            return new SyntaxToken("string", token.Value);
        }

        private SyntaxToken Number(Queue<Token> tokens)
        {
            if(tokens.Peek().Type == "num")
            {
                var value = tokens.Dequeue().Value;
                if (value.StartsWith("0x"))
                    value.Remove(0, 2);
                return new SyntaxToken("number", value);
            }
            throw new InvalidTokenException(tokens.Peek(), $"Expected number, got {tokens.Peek().Type}");
        }

        private bool Try(Queue<Token> tokens, string next)
        {
            if (tokens.Count > 0 && tokens.Peek().Type != next)
                return false;
            return true;
        }

        private bool Try(Queue<Token> tokens, string next, out Token result)
        {
            result = default(Token);
            if (tokens.Count <= 0 || tokens.Peek().Type != next)
                return false;

            result = tokens.Dequeue();
            return true;
        }

        private bool Validate(Queue<Token> tokens, string next)
        {
            if(tokens.Count > 0 && tokens.Peek().Type == next)
            {
                tokens.Dequeue();
                return true;
            }

            return false;
        }

        private bool Expect(Queue<Token> tokens, string next)
        {
            if (tokens.Peek().Type != next)
                throw new InvalidTokenException(tokens.Peek(), $"Expected {next}, got {tokens.Peek().Type}");
            return true;
        }

        private Token Confirm(Queue<Token> tokens, string next)
        {
            var result = tokens.Dequeue();
            if(result.Type != next)
                throw new InvalidTokenException(tokens.Peek(), $"Expected {next}, got {tokens.Peek().Type}");
            return result;
        }

        private bool IsLiteral(Queue<Token> tokens)
        {
            var type = tokens.Peek().Type;
            return type == "num" || type == "string" || type == "bool";
        }

        private bool IsLiteral(ISyntaxNode node)
        {
            if(node is SyntaxToken token)
                return token.Type == "number" || token.Type == "bool" || token.Type == "string";
            return false;
        }

        private bool IsAssignment(Queue<Token> tokens)
        {
            var type = tokens.Peek().Type;
            return type == "assign" ||
                   type == "addAssign" ||
                   type == "subAssign" ||
                   type == "mulAssign" ||
                   type == "divAssign" ||
                   type == "modAssign" ||
                   type == "andAssign" ||
                   type == "xorAssign" ||
                   type == "orAssign";
        }

        #endregion

        #region Lexer

        private static Lexer InitLexer()
        {
            // Todo:
            // self/id/this

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
            lexer.AddDefinition(new TokenDefinition(";", "end"));
            lexer.AddDefinition(new TokenDefinition("||", "lor"));
            lexer.AddDefinition(new TokenDefinition("&&", "land"));
            lexer.AddDefinition(new TokenDefinition("!=|<>", "neq"));
            lexer.AddDefinition(new TokenDefinition("==", "eq"));
            lexer.AddDefinition(new TokenDefinition("<=", "lte"));
            lexer.AddDefinition(new TokenDefinition(">=", "gte"));
            lexer.AddDefinition(new TokenDefinition("<", "lt"));
            lexer.AddDefinition(new TokenDefinition(">", "gt"));
            lexer.AddDefinition(new TokenDefinition("!", "not"));
            lexer.AddDefinition(new TokenDefinition("~", "complement"));
            lexer.AddDefinition(new TokenDefinition(@"\^", "xor"));
            lexer.AddDefinition(new TokenDefinition("|", "bor"));
            lexer.AddDefinition(new TokenDefinition("&", "band"));
            lexer.AddDefinition(new TokenDefinition("<<", "lshift"));
            lexer.AddDefinition(new TokenDefinition(">>", "rshift"));
            lexer.AddDefinition(new TokenDefinition(@"\(", "oparen"));
            lexer.AddDefinition(new TokenDefinition(@"\)", "cparen"));
            lexer.AddDefinition(new TokenDefinition(@"\[", "obracket"));
            lexer.AddDefinition(new TokenDefinition(@"\]", "cbracket"));
            lexer.AddDefinition(new TokenDefinition(@"\{", "obrace"));
            lexer.AddDefinition(new TokenDefinition(@"\}", "cbrace"));
            lexer.AddDefinition(new TokenDefinition(@"/\*(?i).*?\*/(?-i)", "multicomment"));
            lexer.AddDefinition(new TokenDefinition(@"//.*?\n", "singlecomment"));
            lexer.AddDefinition(new TokenDefinition(@"\.", "dot"));
            lexer.AddDefinition(new TokenDefinition("(?i)(\".*?\")|('.*?')(?-i)", "string"));
            lexer.AddDefinition(new TokenDefinition("=", "assign"));
            lexer.AddDefinition(new TokenDefinition(@"\+\+", "increment"));
            lexer.AddDefinition(new TokenDefinition("--", "decrement"));
            lexer.AddDefinition(new TokenDefinition(@"\+=", "addAssign"));
            lexer.AddDefinition(new TokenDefinition("-=", "subAssign"));
            lexer.AddDefinition(new TokenDefinition(@"\*=", "mulAssign"));
            lexer.AddDefinition(new TokenDefinition("/=", "divAssign"));
            lexer.AddDefinition(new TokenDefinition("%=", "modAssign"));
            lexer.AddDefinition(new TokenDefinition(@"\+", "add"));
            lexer.AddDefinition(new TokenDefinition("-", "sub"));
            lexer.AddDefinition(new TokenDefinition(@"\*", "mul"));
            lexer.AddDefinition(new TokenDefinition("/", "div"));
            lexer.AddDefinition(new TokenDefinition("%", "mod"));

            //In GM "_" is a valid variable name O_O
            lexer.AddDefinition(new TokenDefinition("(_)|(_*[a-zA-Z][_a-zA-Z0-9]*)", "id"));
            lexer.AddDefinition(new TokenDefinition(@"(0x[0-9a-fA-F]+)|([0-9]*?\.?[0-9]+?)", "num"));
            lexer.AddDefinition(new TokenDefinition(",", "comma"));
            lexer.AddDefinition(new TokenDefinition(@"\?", "question"));
            lexer.AddDefinition(new TokenDefinition("#", "sharp"));


            return lexer;
        }

        #endregion
    }
}
