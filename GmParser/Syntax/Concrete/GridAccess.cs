namespace GmParser.Syntax
{
    public class GridAccessNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];
        public override SyntaxType Type => SyntaxType.GridAccess;

        public GridAccessNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
