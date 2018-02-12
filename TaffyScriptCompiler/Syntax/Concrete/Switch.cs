using System.Collections.Generic;
using System.Linq;

namespace TaffyScript.Syntax
{
    public class SwitchNode : SyntaxNode
    {
        public ISyntaxElement Test => Children[0];
        public IEnumerable<ISyntaxElement> Cases => Children.Skip(1);
        public override SyntaxType Type => SyntaxType.Switch;

        public SwitchNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
