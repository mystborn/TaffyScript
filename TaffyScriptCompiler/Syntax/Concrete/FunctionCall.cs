namespace TaffyScriptCompiler.Syntax
{
    public class FunctionCallNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.FunctionCall;
        public ISyntaxElement Name => Children[0];

        public FunctionCallNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
