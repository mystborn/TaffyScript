using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Syntax
{
    public abstract class SyntaxNode : ISyntaxNode
    {
        public SyntaxNode Parent { get; set; }
        public string Value { get; } = null;
        public List<ISyntaxElement> Children { get; } = new List<ISyntaxElement>();
        public bool IsToken => false;
        public SyntaxType Type { get; }

        public SyntaxNode(SyntaxType type, string value)
        {
            Type = type;
            Value = value;
        }

        public abstract void Accept(ISyntaxElementVisitor visitor);

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

        public void AddChild(ISyntaxElement child)
        {
            if (child == null)
                return;
            Children.Add(child);
            child.Parent = this;
        }
    }
}
