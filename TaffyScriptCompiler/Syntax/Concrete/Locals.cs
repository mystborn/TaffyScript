namespace TaffyScriptCompiler.Syntax
{
    public class LocalsNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Locals;

        public LocalsNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
