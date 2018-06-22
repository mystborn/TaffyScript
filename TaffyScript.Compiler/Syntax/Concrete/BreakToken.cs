using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class BreakToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Break;
        public override string Name => "break";

        public BreakToken(TokenPosition position)
            : base(position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
