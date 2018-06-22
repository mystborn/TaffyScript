using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ContinueToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Continue;
        public override string Name => "continue";

        public ContinueToken(TokenPosition position)
            : base(position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
