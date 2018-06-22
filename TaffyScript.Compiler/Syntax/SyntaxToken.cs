using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public abstract class SyntaxToken : ISyntaxToken
    {
        public ISyntaxNode Parent { get; set; }
        public abstract SyntaxType Type { get; }
        public abstract string Name { get; }
        public TokenPosition Position { get; }
        public bool IsToken => true;

        public SyntaxToken(TokenPosition position)
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
