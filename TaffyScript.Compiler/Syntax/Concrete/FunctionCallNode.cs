using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class FunctionCallNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.FunctionCall;
        public ISyntaxElement Callee { get; }
        public List<ISyntaxElement> Arguments { get; }
        public TokenPosition EndPosition { get; }

        public FunctionCallNode(ISyntaxElement callee, List<ISyntaxElement> arguments, TokenPosition position, TokenPosition endPosition)
            : base(position)
        {
            Callee = callee;
            Arguments = arguments;
            EndPosition = endPosition;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
