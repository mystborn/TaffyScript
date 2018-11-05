using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class TryNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Try;
        public BlockNode TryBlock { get; }
        public BlockNode CatchBlock { get; }
        public BlockNode FinallyBlock { get; }

        public TryNode(BlockNode tryBlock, BlockNode catchBlock, BlockNode finallyBlock, TokenPosition position)
            : base(position)
        {
            TryBlock = tryBlock;
            CatchBlock = catchBlock;
            FinallyBlock = finallyBlock;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
