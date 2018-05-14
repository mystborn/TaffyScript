using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class NewNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.New;

        public List<ISyntaxElement> Arguments => Children;

        public NewNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
