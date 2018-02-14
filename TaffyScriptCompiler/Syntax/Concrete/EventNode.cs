namespace TaffyScriptCompiler.Syntax
{
    public class EventNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[0];
        public override SyntaxType Type => SyntaxType.Event;

        public EventNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}