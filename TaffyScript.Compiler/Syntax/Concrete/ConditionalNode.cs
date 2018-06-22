using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ConditionalNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Conditional;
        public ISyntaxElement Condition { get; }
        public ISyntaxElement Left { get; }
        public ISyntaxElement Right { get; }

        public ConditionalNode(ISyntaxElement condition, ISyntaxElement left, ISyntaxElement right, TokenPosition position)
            : base(position)
        {
            Condition = condition;
            Left = left;
            Right = right;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
