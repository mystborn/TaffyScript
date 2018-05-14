namespace TaffyScriptCompiler.Syntax
{
    public class NewNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.New;

        public NewNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
