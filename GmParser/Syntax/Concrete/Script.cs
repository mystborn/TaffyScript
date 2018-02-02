namespace GmParser.Syntax
{
    public class ScriptNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[0];

        public ScriptNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}