namespace TaffyScript.Compiler.Syntax
{
    public class ReturnNode : SyntaxNode
    {
        public bool HasReturnValue => Children.Count > 0;
        public ISyntaxElement ReturnValue => Children[0];
        public override SyntaxType Type => SyntaxType.Return;

        public ReturnNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
