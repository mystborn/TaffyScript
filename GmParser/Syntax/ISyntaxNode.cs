using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Syntax
{
    public interface ISyntaxNode : ISyntaxElement
    {
        // Convert this to an enum?
        string Value { get; }
        List<ISyntaxElement> Children { get; }

        void AddChild(ISyntaxElement child);
    }
}
