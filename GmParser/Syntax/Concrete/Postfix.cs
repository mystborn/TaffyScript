namespace GmParser.Syntax
{
    public class PostfixNode : SyntaxNode
    {
        public ISyntaxElement Child => Children[0];
        public override SyntaxType Type => SyntaxType.Postfix;

        public PostfixNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
