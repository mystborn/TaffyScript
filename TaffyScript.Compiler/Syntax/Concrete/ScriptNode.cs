using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ScriptNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Script;
        public string Name { get; }
        public List<VariableDeclaration> Arguments { get; }
        public ISyntaxElement Body { get; }

        public ScriptNode(string name, List<VariableDeclaration> arguments, ISyntaxElement body, TokenPosition position)
            : base(position)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
