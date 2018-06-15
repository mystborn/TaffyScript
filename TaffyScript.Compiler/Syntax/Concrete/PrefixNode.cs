using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class PrefixNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Prefix;
        public string Op { get; }
        public ISyntaxElement Right { get; }

        public PrefixNode(string op, ISyntaxElement right, TokenPosition position)
            : base(position)
        {
            Op = op;
            Right = right;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
