namespace GmParser.Syntax
{
    public class ShiftNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];

        public ShiftNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
