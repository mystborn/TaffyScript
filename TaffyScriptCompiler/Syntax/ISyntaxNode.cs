using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Syntax
{
    public interface ISyntaxNode : ISyntaxElement
    {
        // Convert this to an enum?
        List<ISyntaxElement> Children { get; }

        void AddChild(ISyntaxElement child);
    }
}
