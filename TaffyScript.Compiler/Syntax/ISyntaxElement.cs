using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Compiler.FrontEnd;

namespace TaffyScript.Compiler.Syntax
{
    public interface ISyntaxElement
    {
        ISyntaxNode Parent { get; set; }
        SyntaxType Type { get; }
        TokenPosition Position { get; }
        bool IsToken { get; }

        void Accept(ISyntaxElementVisitor visitor);
    }
}
