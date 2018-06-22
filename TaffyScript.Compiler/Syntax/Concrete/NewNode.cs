using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class NewNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.New;
        public ISyntaxElement TypeName { get; }
        public List<ISyntaxElement> Arguments { get; }
        public TokenPosition EndPosition { get; }

        public NewNode(ISyntaxElement typeName, List<ISyntaxElement> arguments, TokenPosition endPosition, TokenPosition position)
            : base(position)
        {
            TypeName = typeName;
            Arguments = arguments;
            EndPosition = endPosition;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
