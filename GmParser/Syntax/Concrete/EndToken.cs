namespace GmParser.Syntax
{
    public class EndToken : SyntaxToken
    {
        public EndToken(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}