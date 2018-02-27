namespace TaffyScriptCompiler.Syntax
{
    public class NamespaceNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Namespace;

        internal NamespaceNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
