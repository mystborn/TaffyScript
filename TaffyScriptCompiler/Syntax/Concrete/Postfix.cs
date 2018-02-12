namespace TaffyScript.Syntax
{
    public class PostfixNode : SyntaxNode
    {
        public ISyntaxElement Child => Children[0];
        public override SyntaxType Type => SyntaxType.Postfix;

        public PostfixNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
