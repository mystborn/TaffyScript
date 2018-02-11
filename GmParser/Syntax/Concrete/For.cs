namespace GmParser.Syntax
{
    public class ForNode : SyntaxNode
    {
        public ISyntaxElement Initializer => Children[0];
        public ISyntaxElement Condition => Children[1];
        public ISyntaxElement Iterator => Children[2];
        public ISyntaxElement Body => Children[3];
        public override SyntaxType Type => SyntaxType.For;

        public ForNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
