using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class BaseNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Base;
        public List<ISyntaxElement> Arguments { get; }
        public TokenPosition EndPosition { get; }

        public BaseNode(List<ISyntaxElement> arguments, TokenPosition endPosition, TokenPosition position)
            : base(position)
        {
            Arguments = arguments;
            EndPosition = endPosition;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
