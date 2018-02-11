namespace GmParser.Syntax
{
    public class RepeatNode : SyntaxNode
    {
        public ISyntaxElement Condition => Children[0];
        public ISyntaxElement Body => Children[1];
        public override SyntaxType Type => SyntaxType.Repeat;

        public RepeatNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
