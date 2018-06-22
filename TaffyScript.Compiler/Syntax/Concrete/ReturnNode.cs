using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ReturnNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Return;
        public ISyntaxElement Result { get; }

        public ReturnNode(ISyntaxElement result, TokenPosition position)
            : base(position)
        {
            Result = result;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
