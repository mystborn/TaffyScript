namespace GmParser.Syntax
{
    public class ReadOnlyToken : SyntaxToken
    {
        public ReadOnlyToken(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}