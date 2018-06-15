using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class IfNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.If;
        public ISyntaxElement Condition { get; }
        public ISyntaxElement ThenBrach { get; }
        public ISyntaxElement ElseBranch { get; }

        public IfNode(ISyntaxElement condition, ISyntaxElement thenBrach, ISyntaxElement elseBranch, TokenPosition position)
            : base(position)
        {
            Condition = condition;
            ThenBrach = thenBrach;
            ElseBranch = elseBranch;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
