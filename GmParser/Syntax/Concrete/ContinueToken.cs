namespace GmParser.Syntax
{
    public class ContinueToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Continue;

        public ContinueToken(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}