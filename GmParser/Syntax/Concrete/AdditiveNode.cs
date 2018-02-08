namespace GmParser.Syntax
{
    public class AdditiveNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];
        public override SyntaxType Type => SyntaxType.Additive;

        public AdditiveNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
