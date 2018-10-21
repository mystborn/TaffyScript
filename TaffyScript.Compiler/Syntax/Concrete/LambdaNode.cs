using System;
using System.Collections.Generic;
using System.Linq;

namespace TaffyScript.Compiler.Syntax
{
    public class LambdaNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Lambda;
        public string Scope { get; }
        public bool ConstructLocally { get; set; }
        public List<VariableDeclaration> Arguments { get; }
        public BlockNode Body { get; }

        public LambdaNode(string scope, List<VariableDeclaration> arguments, BlockNode body, TokenPosition position) 
            : base(position)
        {
            Scope = scope;
            Arguments = arguments;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
