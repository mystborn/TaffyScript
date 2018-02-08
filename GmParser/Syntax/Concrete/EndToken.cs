namespace GmParser.Syntax
{
    public class EndToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.End;

        public EndToken(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}