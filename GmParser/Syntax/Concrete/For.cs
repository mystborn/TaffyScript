namespace GmParser.Syntax
{
    public class ForNode : SyntaxNode
    {
        public ISyntaxElement Initializer => Children[0];
        public ISyntaxElement Condition => Children[1];
        public ISyntaxElement Iterator => Children[2];
        public ISyntaxElement Body => Children[3];

        public ForNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
