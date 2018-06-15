using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class WhileNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.While;
        public ISyntaxElement Condition { get; }
        public ISyntaxElement Body { get; }

        public WhileNode(ISyntaxElement condition, ISyntaxElement body, TokenPosition position)
            : base(position)
        {
            Condition = condition;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
