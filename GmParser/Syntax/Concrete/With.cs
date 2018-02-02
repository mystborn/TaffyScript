namespace GmParser.Syntax
{
    public class WithNode : SyntaxNode
    {
        public ISyntaxElement Condition => Children[0];
        public ISyntaxElement Body => Children[1];

        public WithNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
