using System.Collections.Generic;
using System.Linq;

namespace GmParser.Syntax
{
    public class CaseNode : SyntaxNode
    {
        public ISyntaxElement Test => Children[0];
        public IEnumerable<ISyntaxElement> Expressions => Children.Skip(1);

        public CaseNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
