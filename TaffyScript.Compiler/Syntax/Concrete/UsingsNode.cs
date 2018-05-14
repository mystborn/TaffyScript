using System.Collections.Generic;
using System.Linq;

namespace TaffyScript.Compiler.Syntax
{
    public class UsingsNode : SyntaxNode
    {
        private List<ISyntaxToken> _modules = null; 
        public override SyntaxType Type => SyntaxType.Usings;
        public List<ISyntaxToken> Modules
        {
            get
            {
                if (_modules == null)
                    _modules = new List<ISyntaxToken>(Children.GetRange(0, Children.Count - 1).Cast<ISyntaxToken>());
                return _modules;
            }
        }
        public ISyntaxNode Declarations => (ISyntaxNode)Children[Children.Count - 1];

        internal UsingsNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
