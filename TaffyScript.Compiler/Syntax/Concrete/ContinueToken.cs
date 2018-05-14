namespace TaffyScript.Compiler.Syntax
{
    public class ContinueToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Continue;

        public ContinueToken(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}