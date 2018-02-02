namespace GmParser.Syntax
{
    public class DeclareNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];

        public DeclareNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
