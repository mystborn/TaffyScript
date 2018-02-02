namespace GmParser.Syntax
{
    public class EnumNode : SyntaxNode
    {
        public EnumNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}