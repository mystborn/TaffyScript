namespace TaffyScript.Syntax
{
    public class GridAccessNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement X => Children[1];
        public ISyntaxElement Y => Children[2];
        public override SyntaxType Type => SyntaxType.GridAccess;

        public GridAccessNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
