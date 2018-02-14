namespace TaffyScriptCompiler.Syntax
{
    public class VariableToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Variable;

        public VariableToken(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}