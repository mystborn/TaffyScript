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
        public string Value { get; } = null;
        public List<ISyntaxElement> Children { get; } = new List<ISyntaxElement>();
        public bool IsToken => false;
        public SyntaxType Type { get; }

        public SyntaxNode(SyntaxType type)
        {
            Type = type;
        }

        public SyntaxNode(SyntaxType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return Type + (Value == null ? "" : (": " + Value));
        }

        public SyntaxToken AddToken(SyntaxType type, string value)
        {
            var token = new SyntaxToken(this, type, value);
            Children.Add(token);
            return token;
        }

        public SyntaxToken AddToken(ConstantType type, string value)
        {
            var token = SyntaxToken.CreateConstant(type, value, this);
            Children.Add(token);
            return token;
        }

        public void AddToken(SyntaxToken token)
        {
            Children.Add(token);
            token.Parent = this;
        }

        public SyntaxNode AddNode(SyntaxType type)
        {
            var node = new SyntaxNode(type);
            Children.Add(node);
            return node;
        }

        public SyntaxNode AddNode(SyntaxType type, string value)
        {
            var node = new SyntaxNode(type, value);
            Children.Add(node);
            return node;
        }

        public void AddNode(SyntaxNode node)
        {
            Children.Add(node);
            node.Parent = this;
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
