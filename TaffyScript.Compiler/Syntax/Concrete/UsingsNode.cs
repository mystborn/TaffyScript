using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class UsingsNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Usings;
        public List<UsingDeclaration> Usings { get; }
        public List<ISyntaxElement> Declarations { get; }

        public UsingsNode(List<UsingDeclaration> usings, List<ISyntaxElement> declarations, TokenPosition position)
            : base(position)
        {
            Usings = usings;
            Declarations = declarations;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
