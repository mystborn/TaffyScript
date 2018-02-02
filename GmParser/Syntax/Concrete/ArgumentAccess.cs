namespace GmParser.Syntax
{
    public class ArgumentAccessNode : SyntaxNode
    {
        public ISyntaxElement Index => Children[0];

        public ArgumentAccessNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
