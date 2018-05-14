namespace TaffyScript.Compiler.Syntax
{
    public class DoNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[0];
        public ISyntaxElement Until => Children[1];
        public override SyntaxType Type => SyntaxType.Do;

        public DoNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
