namespace GmParser.Syntax
{
    public class ObjectNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Object;

        public ObjectNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}