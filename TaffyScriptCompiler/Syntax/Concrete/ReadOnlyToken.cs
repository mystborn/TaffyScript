namespace TaffyScriptCompiler.Syntax
{
    public class ReadOnlyToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.ReadOnlyValue;

        public ReadOnlyToken(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}