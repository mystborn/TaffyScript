using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ReadOnlyToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.ReadOnly;
        public override string Name { get; }

        public ReadOnlyToken(string name, TokenPosition position)
            : base(position)
        {
            Name = name;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
