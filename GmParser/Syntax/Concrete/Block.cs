namespace GmParser.Syntax
{
    public class BlockNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Block;

        internal BlockNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
