using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public abstract class SyntaxToken : ISyntaxToken
    {
        public ISyntaxNode Parent { get; set; }
        public string Text { get; }
        public abstract SyntaxType Type { get; }
        public TokenPosition Position { get; }
        public bool IsToken => true;

        public SyntaxToken(string value, TokenPosition position)
        {
            Position = position;
            Text = value;
        }

        public abstract void Accept(ISyntaxElementVisitor visitor);

        public override string ToString()
        {
            return $"{Type}: {Text}";
        }

        /*public static SyntaxToken CreateConstant(ConstantType type, string value, SyntaxNode parent = null)
        {
            switch (type)
            {
                case ConstantType.Bool:
                    return new ConstantToken<bool>(value, type, value == "true", parent);
                case ConstantType.Real:
                    float real;
                    if (value.StartsWith("0x"))
                    {
                        var temp = value.Remove(0, 2);
                        real = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                        real = float.Parse(value);
                    return new ConstantToken<float>(value, type, real, parent);
                case ConstantType.String:
                    return new ConstantToken<string>(value, ConstantType.String, value, parent);
                default:
                    throw new InvalidOperationException();
            }
        }*/
    }
}
