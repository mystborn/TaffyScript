namespace GmParser.Syntax
{
    public class DoNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[0];
        public ISyntaxElement Until => Children[1];
        public override SyntaxType Type => SyntaxType.Do;

        public DoNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
