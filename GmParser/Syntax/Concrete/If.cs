namespace GmParser.Syntax
{
    public class IfNode : SyntaxNode
    {
        public ISyntaxElement Condition => Children[0];
        public ISyntaxElement Body => Children[1];

        public IfNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}