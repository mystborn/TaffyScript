namespace TaffyScript.Compiler.Syntax
{
    public class EnumNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Enum;

        public EnumNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}