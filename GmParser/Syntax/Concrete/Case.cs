using System.Collections.Generic;
using System.Linq;

namespace GmParser.Syntax
{
    public class CaseNode : SyntaxNode
    {
        public ISyntaxElement Test => Children[0];
        public IEnumerable<ISyntaxElement> Expressions => Children.Skip(1);
        public override SyntaxType Type => SyntaxType.Case;

        public CaseNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
