using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public interface ISyntaxNode : ISyntaxElement
    {
        // Convert this to an enum?
        string Value { get; }
    }
}
