namespace TaffyScript.Compiler.Syntax
{
    public class ArgumentAccessNode : SyntaxNode
    {
        public ISyntaxElement Index => Children[0];
        public override SyntaxType Type => SyntaxType.ArgumentAccess;

        public ArgumentAccessNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}