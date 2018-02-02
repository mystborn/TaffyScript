namespace GmParser.Syntax
{
    public class ExitToken : SyntaxToken
    {
        public ExitToken(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}