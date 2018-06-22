using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class EnumNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Enum;
        public string Name { get; }
        public List<EnumValue> Values { get; }

        public EnumNode(string name, List<EnumValue> values, TokenPosition position)
            : base(position)
        {
            Name = name;
            Values = values;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
