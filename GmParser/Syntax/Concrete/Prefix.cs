namespace GmParser.Syntax
{
    public class PrefixNode : SyntaxNode
    {
        public ISyntaxElement Child => Children[0];

        public PrefixNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
