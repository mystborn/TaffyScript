namespace GmParser.Syntax
{
    public class ArgumentAccessNode : SyntaxNode
    {
        public ISyntaxElement Index => Children[0];
        public override SyntaxType Type => SyntaxType.ArgumentAccess;

        public ArgumentAccessNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
