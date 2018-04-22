using System.Collections.Generic;

namespace TaffyScriptCompiler.Syntax
{
    public class ScriptNode : SyntaxNode
    {
        private List<ISyntaxElement> _args;

        public ISyntaxElement Body => Children[Children.Count - 1];

        public IReadOnlyList<ISyntaxElement> Arguments
        {
            get
            {
                if (_args == null)
                    _args = Children.GetRange(0, Children.Count - 1);
                return _args;
            }
        }


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