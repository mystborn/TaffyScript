namespace TaffyScript.Compiler.Syntax
{
    public class RelationalNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];
        public override SyntaxType Type => SyntaxType.Relational;

        public RelationalNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
