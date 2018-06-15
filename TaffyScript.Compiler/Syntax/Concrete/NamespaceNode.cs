using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class NamespaceNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Namespace;
        public string Name { get; }
        public List<ISyntaxElement> Declarations { get; }

        public NamespaceNode(string name, List<ISyntaxElement> declarations, TokenPosition position)
            : base(position)
        {
            Name = name;
            Declarations = declarations;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
