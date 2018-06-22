using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class RepeatNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Repeat;
        public ISyntaxElement Count { get; }
        public ISyntaxElement Body { get; }

        public RepeatNode(ISyntaxElement count, ISyntaxElement body, TokenPosition position)
            : base(position)
        {
            Count = count;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
