namespace GmParser.Syntax
{
    public class WhileNode : SyntaxNode
    {
        public ISyntaxElement Condition => Children[0];
        public ISyntaxElement Body => Children[1];

        public WhileNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
