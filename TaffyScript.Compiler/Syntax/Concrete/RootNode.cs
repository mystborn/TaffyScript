using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class RootNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Root;
        public List<UsingsNode> CompilationUnits { get; } = new List<UsingsNode>();

        public RootNode()
            : base(null)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
