namespace GmParser.Syntax
{
    public class LocalsNode : SyntaxNode
    {
        public LocalsNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
