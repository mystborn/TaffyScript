namespace GmParser.Syntax
{
    public class EnumNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Enum;

        public EnumNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}