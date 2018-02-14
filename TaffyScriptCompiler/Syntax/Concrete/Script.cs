namespace TaffyScriptCompiler.Syntax
{
    public class ScriptNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[0];
        public override SyntaxType Type => SyntaxType.Script;

        public ScriptNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}