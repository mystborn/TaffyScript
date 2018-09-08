using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class MemberAccessNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.MemberAccess;
        public ISyntaxElement Left { get; }
        public ISyntaxElement Right { get; set; }

        public MemberAccessNode(ISyntaxElement left, ISyntaxElement right, TokenPosition position)
            : base(position)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
