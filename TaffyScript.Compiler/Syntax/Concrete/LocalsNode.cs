using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class LocalsNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Locals;
        public List<VariableDeclaration> Locals { get; }

        public LocalsNode(List<VariableDeclaration> locals, TokenPosition position)
            : base(position)
        {
            Locals = locals;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
