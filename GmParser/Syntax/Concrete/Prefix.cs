namespace GmParser.Syntax
{
    public class PrefixNode : SyntaxNode
    {
        public ISyntaxElement Child => Children[0];
        public override SyntaxType Type => SyntaxType.Prefix;

        public PrefixNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
