using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ArgumentAccessNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.ArgumentAccess;
        public ISyntaxElement Index { get; }

        public ArgumentAccessNode(ISyntaxElement index, TokenPosition position)
            : base(position)
        {
            Index = index;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
