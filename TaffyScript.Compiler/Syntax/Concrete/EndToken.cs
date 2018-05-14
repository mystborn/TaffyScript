namespace TaffyScript.Compiler.Syntax
{
    public class EndToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.End;

        public EndToken(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}