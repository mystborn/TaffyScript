namespace GmParser.Syntax
{
    public class MemberAccessNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];
        public override SyntaxType Type => SyntaxType.MemberAccess;

        public MemberAccessNode(string value) : base(value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
