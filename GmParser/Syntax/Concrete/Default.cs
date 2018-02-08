namespace GmParser.Syntax
{
    public class DefaultNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Default;

        public DefaultNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
