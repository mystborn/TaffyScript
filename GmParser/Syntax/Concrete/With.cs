namespace GmParser.Syntax
{
    public class WithNode : SyntaxNode
    {
        public ISyntaxElement Condition => Children[0];
        public ISyntaxElement Body => Children[1];
        public override SyntaxType Type => SyntaxType.With;

        public WithNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
