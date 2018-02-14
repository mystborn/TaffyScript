using System.Collections.Generic;
using System.Linq;

namespace TaffyScriptCompiler.Syntax
{
    public class CaseNode : SyntaxNode
    {
        public ISyntaxElement Condition => Children[0];
        public ISyntaxElement Expressions => Children[1];
        public override SyntaxType Type => SyntaxType.Case;

        public CaseNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
