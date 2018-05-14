using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class WithNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.With;
        public ISyntaxElement Target => Children[0];
        public ISyntaxElement Body => Children[1];

        public WithNode(string value, TokenPosition position) 
            : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}