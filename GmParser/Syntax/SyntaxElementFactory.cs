using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Syntax
{
    public class SyntaxElementFactory : ISyntaxElementFactory
    {
        public ISyntaxToken CreateConstant(ConstantType type, string value)
        {
            switch (type)
            {
                case ConstantType.Bool:
                    return new ConstantToken<bool>(value, type, value == "true");
                case ConstantType.Real:
                    float real;
                    if (value.StartsWith("0x"))
                    {
                        var temp = value.Remove(0, 2);
                        real = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                        real = float.Parse(value);
                    return new ConstantToken<float>(value, type, real);
                case ConstantType.String:
                    return new ConstantToken<string>(value, ConstantType.String, value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public ISyntaxNode CreateNode(SyntaxType type)
        {
            switch(type)
            {
                case SyntaxType.Locals:
                    return new LocalsNode(null);
                case SyntaxType.Import:
                    return new ImportNode(null);
                case SyntaxType.Declare:
                    return new DeclareNode(null);
                case SyntaxType.Return:
                    return new ReturnNode(null);
                case SyntaxType.While:
                    return new WhileNode(null);
                case SyntaxType.With:
                    return new WithNode(null);
                case SyntaxType.Do:
                    return new DoNode(null);
                case SyntaxType.If:
                    return new IfNode(null);
                case SyntaxType.For:
                    return new ForNode(null);
                case SyntaxType.Switch:
                    return new SwitchNode(null);
                case SyntaxType.Case:
                    return new CaseNode(null);
                case SyntaxType.Default:
                    return new DefaultNode(null);
                case SyntaxType.Block:
                    return new BlockNode(null);
                case SyntaxType.Conditional:
                    return new ConditionalNode(null);
                case SyntaxType.MemberAccess:
                    return new MemberAccessNode(null);
                case SyntaxType.ListAccess:
                    return new ListAccessNode(null);
                case SyntaxType.GridAccess:
                    return new GridAccessNode(null);
                case SyntaxType.MapAccess:
                    return new MapAccessNode(null);
                case SyntaxType.ExplicitArrayAccess:
                    return new ExplicitArrayAccessNode(null);
                case SyntaxType.ArrayAccess:
                    return new ArrayAccessNode(null);
                case SyntaxType.ArgumentAccess:
                    return new ArgumentAccessNode(null);
                case SyntaxType.ArrayLiteral:
                    return new ArrayLiteralNode(null);
                default:
                    throw new InvalidOperationException();
            }
        }

        public ISyntaxNode CreateNode(SyntaxType type, string value)
        {
            switch(type)
            {
                case SyntaxType.Assign:
                    return new AssignNode(value);
                case SyntaxType.Logical:
                    return new LogicalNode(value);
                case SyntaxType.Bitwise:
                    return new BitwiseNode(value);
                case SyntaxType.Equality:
                    return new EqualityNode(value);
                case SyntaxType.Relational:
                    return new RelationalNode(value);
                case SyntaxType.Shift:
                    return new ShiftNode(value);
                case SyntaxType.Additive:
                    return new AdditiveNode(value);
                case SyntaxType.Multiplicative:
                    return new MultiplicativeNode(value);
                case SyntaxType.Prefix:
                    return new PrefixNode(value);
                case SyntaxType.FunctionCall:
                    return new FunctionCallNode(value);
                case SyntaxType.Postfix:
                    return new PostfixNode(value);
                case SyntaxType.Enum:
                    return new EnumNode(value);
                case SyntaxType.Script:
                    return new ScriptNode(value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public ISyntaxToken CreateToken(SyntaxType type, string value)
        {
            switch(type)
            {
                case SyntaxType.Variable:
                    return new VariableToken(value);
                case SyntaxType.End:
                    return new EndToken(value);
                case SyntaxType.Break:
                    return new BreakToken(value);
                case SyntaxType.Continue:
                    return new ContinueToken(value);
                case SyntaxType.Exit:
                    return new ExitToken(value);
                case SyntaxType.ReadOnlyValue:
                    return new ReadOnlyToken(value);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
