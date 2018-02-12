namespace TaffyScript.Syntax
{
    public class DefaultNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Default;
        public ISyntaxElement Expressions => Children[0];

        public DefaultNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
