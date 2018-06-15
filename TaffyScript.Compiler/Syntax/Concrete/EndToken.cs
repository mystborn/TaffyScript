using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class EndToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.End;
        public override string Name => ";";

        public EndToken(TokenPosition position)
            : base(position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
