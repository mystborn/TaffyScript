namespace GmParser.Syntax
{
    public class ReadOnlyToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.ReadOnlyValue;

        public ReadOnlyToken(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}