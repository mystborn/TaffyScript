namespace GmParser.Syntax
{
    public class BreakToken : SyntaxToken
    {
        public BreakToken(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}