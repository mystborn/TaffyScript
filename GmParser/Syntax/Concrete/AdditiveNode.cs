namespace GmParser.Syntax
{
    public class AdditiveNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];

        public AdditiveNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
