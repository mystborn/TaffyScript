using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class BlockNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Block;
        public string Id { get; }
        public List<ISyntaxElement> Body { get; }
        public List<VariableLeaf> Variables { get; } = new List<VariableLeaf>();

        public BlockNode(List<ISyntaxElement> body, string id, TokenPosition position)
            : base(position)
        {
            Id = id;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
