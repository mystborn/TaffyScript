namespace GmParser.Syntax
{
    public class VariableToken : SyntaxToken
    {
        public VariableToken(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}