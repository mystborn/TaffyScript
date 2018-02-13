using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Syntax
{
    public class SyntaxElementFactory : ISyntaxElementFactory
    {
        public ISyntaxToken CreateConstant(ConstantType type, string value, TokenPosition position)
        {
            switch (type)
            {
                case ConstantType.Bool:
                    return new ConstantToken<bool>(value, position, type, value == "true");
                case ConstantType.Real:
                    float real;
                    if (value.StartsWith("0x"))
                    {
                        var temp = value.Remove(0, 2);
                        real = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                    }
                    else if (value.StartsWith("?"))
                    {
                        var temp = value.Remove(0, 1);
                        real = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                        real = float.Parse(value);
                    return new ConstantToken<float>(value, position, type, real);
                case ConstantType.String:
                    return new ConstantToken<string>(value, position, ConstantType.String, value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public ISyntaxNode CreateNode(SyntaxType type, TokenPosition position)
        {
            switch(type)
            {
                case SyntaxType.Locals:
                    return new LocalsNode(null, position);
                case SyntaxType.Import:
                    return new ImportNode(null, position);
                case SyntaxType.Return:
                    return new ReturnNode(null, position);
                case SyntaxType.While:
                    return new WhileNode(null, position);
                case SyntaxType.Repeat:
                    return new RepeatNode(null, position);
                case SyntaxType.With:
                    return new WithNode(null, position);
                case SyntaxType.Do:
                    return new DoNode(null, position);
                case SyntaxType.If:
                    return new IfNode(null, position);
                case SyntaxType.For:
                    return new ForNode(null, position);
                case SyntaxType.Switch:
                    return new SwitchNode(null, position);
                case SyntaxType.Case:
                    return new CaseNode(null, position);
                case SyntaxType.Default:
                    return new DefaultNode(null, position);
                case SyntaxType.Block:
                    return new BlockNode(null, position);
                case SyntaxType.Conditional:
                    return new ConditionalNode(null, position);
                case SyntaxType.MemberAccess:
                    return new MemberAccessNode(null, position);
                case SyntaxType.ListAccess:
                    return new ListAccessNode(null, position);
                case SyntaxType.GridAccess:
                    return new GridAccessNode(null, position);
                case SyntaxType.MapAccess:
                    return new MapAccessNode(null, position);
                case SyntaxType.ExplicitArrayAccess:
                    return new ExplicitArrayAccessNode(null, position);
                case SyntaxType.ArrayAccess:
                    return new ArrayAccessNode(null, position);
                case SyntaxType.ArgumentAccess:
                    return new ArgumentAccessNode(null, position);
                case SyntaxType.ArrayLiteral:
                    return new ArrayLiteralNode(null, position);
                default:
                    throw new InvalidOperationException();
            }
        }

        public ISyntaxNode CreateNode(SyntaxType type, string value, TokenPosition position)
        {
            switch(type)
            {
                case SyntaxType.Assign:
                    return new AssignNode(value, position);
                case SyntaxType.Logical:
                    return new LogicalNode(value, position);
                case SyntaxType.Bitwise:
                    return new BitwiseNode(value, position);
                case SyntaxType.Declare:
                    return new DeclareNode(value, position);
                case SyntaxType.Equality:
                    return new EqualityNode(value, position);
                case SyntaxType.Relational:
                    return new RelationalNode(value, position);
                case SyntaxType.Shift:
                    return new ShiftNode(value, position);
                case SyntaxType.Additive:
                    return new AdditiveNode(value, position);
                case SyntaxType.Multiplicative:
                    return new MultiplicativeNode(value, position);
                case SyntaxType.Prefix:
                    return new PrefixNode(value, position);
                case SyntaxType.FunctionCall:
                    return new FunctionCallNode(value, position);
                case SyntaxType.Postfix:
                    return new PostfixNode(value, position);
                case SyntaxType.Enum:
                    return new EnumNode(value, position);
                case SyntaxType.Script:
                    return new ScriptNode(value, position);
                case SyntaxType.Object:
                    return new ObjectNode(value, position);
                case SyntaxType.Event:
                    return new EventNode(value, position);
                default:
                    throw new InvalidOperationException();
            }
        }

        public ISyntaxToken CreateToken(SyntaxType type, string value, TokenPosition position)
        {
            switch(type)
            {
                case SyntaxType.Variable:
                    return new VariableToken(value, position);
                case SyntaxType.End:
                    return new EndToken(value, position);
                case SyntaxType.Break:
                    return new BreakToken(value, position);
                case SyntaxType.Continue:
                    return new ContinueToken(value, position);
                case SyntaxType.Exit:
                    return new ExitToken(value, position);
                case SyntaxType.ReadOnlyValue:
                    return new ReadOnlyToken(value, position);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
