namespace GmParser.Syntax
{
    public class BreakToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Additive;

        public BreakToken(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}