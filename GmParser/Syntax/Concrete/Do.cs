namespace GmParser.Syntax
{
    public class DoNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[0];
        public ISyntaxElement Until => Children[1];

        public DoNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
