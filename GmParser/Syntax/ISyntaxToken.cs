using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Syntax
{
    public interface ISyntaxToken : ISyntaxElement
    {
        string Text { get; }
    }
}
