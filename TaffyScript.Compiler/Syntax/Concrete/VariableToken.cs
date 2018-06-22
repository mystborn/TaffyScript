using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class VariableToken : SyntaxToken
    {
        public override SyntaxType Type => SyntaxType.Variable;
        public override string Name { get; }

        public VariableToken(string name, TokenPosition position)
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
