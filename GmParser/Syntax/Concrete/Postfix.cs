namespace GmParser.Syntax
{
    public class PostfixNode : SyntaxNode
    {
        public ISyntaxElement Child => Children[0];

        public PostfixNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
