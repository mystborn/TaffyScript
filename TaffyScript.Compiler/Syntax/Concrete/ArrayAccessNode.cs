using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ArrayAccessNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.ArrayAccess;
        public ISyntaxElement Left { get; }
        public List<ISyntaxElement> Arguments { get; }

        public ArrayAccessNode(ISyntaxElement left, List<ISyntaxElement> arguments, TokenPosition position)
            : base(position)
        {
            Left = left;
            Arguments = arguments;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
