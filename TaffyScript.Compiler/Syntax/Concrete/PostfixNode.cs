using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class PostfixNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Postfix;
        public ISyntaxElement Left { get; }
        public string Op { get; }

        public PostfixNode(ISyntaxElement left, string op, TokenPosition position)
            : base(position)
        {
            Left = left;
            Op = op;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
