namespace GmParser.Syntax
{
    public class ExitToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Exit;

        public ExitToken(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}