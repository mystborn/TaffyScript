using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public struct SwitchCase
    {
        public ISyntaxElement Expression { get; }
        public BlockNode Body { get; }

        public SwitchCase(ISyntaxElement expression, BlockNode body)
        {
            Expression = expression;
            Body = body;
        }
    }
}
