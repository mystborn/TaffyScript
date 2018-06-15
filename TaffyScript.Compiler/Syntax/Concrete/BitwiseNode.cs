using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class BitwiseNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Bitwise;
        public ISyntaxElement Left { get; }
        public string Op { get; }
        public ISyntaxElement Right { get; }

        public BitwiseNode(ISyntaxElement left, string op, ISyntaxElement right, TokenPosition position)
            : base(position)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
