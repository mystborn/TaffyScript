using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ArrayLiteralNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.ArrayLiteral;
        public List<ISyntaxElement> Elements { get; }

        public ArrayLiteralNode(List<ISyntaxElement> elements, TokenPosition position)
            : base(position)
        {
            Elements = elements;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
