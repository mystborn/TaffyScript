namespace TaffyScriptCompiler.Syntax
{
    public class ConditionalNode : SyntaxNode
    {
        public ISyntaxElement Test => Children[0];
        public ISyntaxElement Left => Children[1];
        public ISyntaxElement Right => Children[2];
        public override SyntaxType Type => SyntaxType.Conditional;

        public ConditionalNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
