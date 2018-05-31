using System;
using System.Collections.Generic;
using System.Linq;

namespace TaffyScript.Compiler.Syntax
{
    public class LambdaNode : SyntaxNode
    {
        private List<ISyntaxElement> _arguments;

        public override SyntaxType Type => SyntaxType.Lambda;
        public ISyntaxElement Body => Children[Children.Count - 1];
        public string Scope { get; set; }

        public IReadOnlyList<ISyntaxElement> Arguments
        {
            get
            {
                if (_arguments is null)
                    _arguments = Children.GetRange(0, Children.Count - 1);
                return _arguments;
            }
        }

        public LambdaNode(string value, TokenPosition position) 
            : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void MarkVariablesAsCaptured(SymbolTable table)
        {
            MarkVariablesAsCaptured((BlockNode)Body, table);
        }

        private void MarkVariablesAsCaptured(ISyntaxNode node, SymbolTable table)
        {
            foreach(var child in node.Children)
            {
                if (child is VariableToken variable && table.Defined(variable.Text, out var symbol) && symbol is VariableLeaf leaf)
                {
                    leaf.IsCaptured = true;
                }
                else if (child is ISyntaxNode sn)
                    MarkVariablesAsCaptured(sn, table);
            }
        }
    }
}
