namespace GmParser.Syntax
{
    public class DefaultNode : SyntaxNode
    {
        public DefaultNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
