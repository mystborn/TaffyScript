namespace GmParser.Syntax
{
    public class ReturnNode : SyntaxNode
    {
        public ISyntaxElement ReturnValue => Children[0];

        public ReturnNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
