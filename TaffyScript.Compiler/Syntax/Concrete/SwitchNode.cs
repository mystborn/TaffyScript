using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class SwitchNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Switch;
        public ISyntaxElement Value { get; }
        public List<SwitchCase> Cases { get; }
        public ISyntaxElement DefaultCase { get; }
        public int DefaultIndex { get; }

        public SwitchNode(ISyntaxElement value, List<SwitchCase> cases, ISyntaxElement defaultCase, int defaultIndex, TokenPosition position)
            : base(position)
        {
            Value = value;
            Cases = cases;
            DefaultCase = defaultCase;
            DefaultIndex = defaultIndex;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
