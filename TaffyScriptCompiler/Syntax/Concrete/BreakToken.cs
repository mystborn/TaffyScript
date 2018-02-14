namespace TaffyScriptCompiler.Syntax
{
    public class BreakToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Additive;

        public BreakToken(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}