using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class BlockNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Block;
        public List<ISyntaxElement> Body { get; }

        public BlockNode(List<ISyntaxElement> body, TokenPosition position)
            : base(position)
        {
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
