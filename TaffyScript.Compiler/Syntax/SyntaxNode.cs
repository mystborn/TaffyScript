using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public abstract class SyntaxNode : ISyntaxNode
    {
        public ISyntaxNode Parent { get; set; }
        public TokenPosition Position { get; }
        public bool IsToken => false;
        public abstract SyntaxType Type { get; }

        public SyntaxNode(TokenPosition position)
        {
            Position = position;
        }

        public abstract void Accept(ISyntaxElementVisitor visitor);

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
