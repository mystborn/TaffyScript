using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ForNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.For;
        public ISyntaxElement Initialize { get; }
        public ISyntaxElement Condition { get; }
        public ISyntaxElement Increment { get; }
        public ISyntaxElement Body { get; }

        public ForNode(ISyntaxElement initialize, ISyntaxElement condition, ISyntaxElement increment, ISyntaxElement body, TokenPosition position)
            : base(position)
        {
            Initialize = initialize;
            Condition = condition;
            Increment = increment;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
