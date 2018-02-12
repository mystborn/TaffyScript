namespace TaffyScript.Syntax
{
    public class ExitToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Exit;

        public ExitToken(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}