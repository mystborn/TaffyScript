using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public class SyntaxNode : ISyntaxNode
    {
        public SyntaxNode Parent { get; set; }
        public string Type { get; }
        public string Value { get; } = null;
        public List<ISyntaxNode> Children { get; } = new List<ISyntaxNode>();
        public bool IsToken => false;

        public SyntaxNode(string type)
        {
            Type = type;
        }

        public SyntaxNode(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return Type + (Value == null ? "" : (": " + Value));
        }

        public SyntaxToken AddToken(string type, string value)
        {
            var token = new SyntaxToken(this, type, value);
            Children.Add(token);
            return token;
        }

        public void AddToken(SyntaxToken token)
        {
            Children.Add(token);
            token.Parent = this;
        }

        public SyntaxNode AddNode(string type)
        {
            var node = new SyntaxNode(type);
            Children.Add(node);
            return node;
        }

        public void AddNode(SyntaxNode node)
        {
            Children.Add(node);
            node.Parent = this;
        }

        public void AddChild(ISyntaxNode child)
        {
            Children.Add(child);
            child.Parent = this;
        }
    }
}
