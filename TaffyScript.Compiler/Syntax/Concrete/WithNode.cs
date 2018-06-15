using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class WithNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.With;
        public ISyntaxElement Target { get; }
        public ISyntaxElement Body { get; }

        public WithNode(ISyntaxElement target, ISyntaxElement body, TokenPosition position)
            : base(position)
        {
            Target = target;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
