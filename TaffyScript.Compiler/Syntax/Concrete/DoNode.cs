using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class DoNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Do;
        public ISyntaxElement Body { get; }
        public ISyntaxElement Condition { get; }

        public DoNode(ISyntaxElement body, ISyntaxElement condition, TokenPosition position)
            : base(position)
        {
            Body = body;
            Condition = condition;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
