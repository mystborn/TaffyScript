namespace GmParser.Syntax
{
    public class PrefixNode : SyntaxNode
    {
        public ISyntaxElement Child => Children[0];
        public override SyntaxType Type => SyntaxType.Prefix;

        public PrefixNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
