using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class ForeachNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Foreach;
        public bool VariableDeclared { get; }
        public VariableToken Variable { get; }
        public ISyntaxElement Iterable { get; }
        public BlockNode Body { get; }

        public ForeachNode(bool variableDeclared, VariableToken variable, ISyntaxElement iterable, BlockNode body, TokenPosition position)
            : base(position)
        {
            VariableDeclared = variableDeclared;
            Variable = variable;
            Iterable = iterable;
            Body = body;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
