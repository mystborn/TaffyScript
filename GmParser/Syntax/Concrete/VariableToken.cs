namespace GmParser.Syntax
{
    public class VariableToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Variable;

        public VariableToken(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}