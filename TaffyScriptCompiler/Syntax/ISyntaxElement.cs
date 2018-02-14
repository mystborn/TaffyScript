using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScriptCompiler.FrontEnd;

namespace TaffyScriptCompiler.Syntax
{
    public interface ISyntaxElement
    {
        //Todo: Element should know file pos.
        ISyntaxNode Parent { get; set; }
        SyntaxType Type { get; }
        TokenPosition Position { get; }
        bool IsToken { get; }
        void Accept(ISyntaxElementVisitor visitor);
    }
}
