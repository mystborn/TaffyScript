namespace TaffyScriptCompiler.Syntax
{
    public class BlockNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Block;

        internal BlockNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
