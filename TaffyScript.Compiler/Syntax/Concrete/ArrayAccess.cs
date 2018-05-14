using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ArrayAccessNode : SyntaxNode
    {
        private List<ISyntaxElement> _indeces = null;

        public ISyntaxElement Left => Children[0];
        public IReadOnlyList<ISyntaxElement> Indeces
        {
            get
            {
                if (_indeces == null)
                    _indeces = Children.GetRange(1, Children.Count - 1);
                return _indeces;
            }
        }
        public override SyntaxType Type => SyntaxType.ArrayAccess;

        public ArrayAccessNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
