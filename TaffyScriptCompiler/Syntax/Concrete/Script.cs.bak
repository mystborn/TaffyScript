namespace TaffyScriptCompiler.Syntax
{
    public class ScriptNode : SyntaxNode
    {
        public ISyntaxElement Body => Children[Children.Count - 1];
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