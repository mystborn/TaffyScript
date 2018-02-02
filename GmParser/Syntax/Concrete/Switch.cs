using System.Collections.Generic;
using System.Linq;

namespace GmParser.Syntax
{
    public class SwitchNode : SyntaxNode
    {
        public ISyntaxElement Test => Children[0];
        public IEnumerable<ISyntaxElement> Cases => Children.Skip(1);

        public SwitchNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
