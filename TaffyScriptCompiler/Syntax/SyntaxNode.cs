using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Syntax
{
    public abstract class SyntaxNode : ISyntaxNode
    {
        public ISyntaxNode Parent { get; set; }
        public string Text { get; } = null;
        public List<ISyntaxElement> Children { get; } = new List<ISyntaxElement>();
        public TokenPosition Position { get; }
        public bool IsToken => false;
        public abstract SyntaxType Type { get; }

        public SyntaxNode(string value, TokenPosition position)
        {
            Position = position;
            Text = value;
        }

        public abstract void Accept(ISyntaxElementVisitor visitor);

        public override string ToString()
        {
            return Type + (Text == null ? "" : (": " + Text));
        }

        public void AddChild(ISyntaxElement child)
        {
            if (child == null)
                return;
            Children.Add(child);
            child.Parent = this;
        }
    }
}
