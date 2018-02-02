namespace GmParser.Syntax
{
    public class ConditionalNode : SyntaxNode
    {
        public ISyntaxElement Test => Children[0];
        public ISyntaxElement Left => Children[1];
        public ISyntaxElement Right => Children[2];

        public ConditionalNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
