namespace GmParser.Syntax
{
    public class FunctionCallNode : SyntaxNode
    {
        public FunctionCallNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
