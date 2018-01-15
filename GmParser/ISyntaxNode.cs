using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public interface ISyntaxNode
    {
        SyntaxNode Parent { get; set; }
        string Type { get; }
        string Value { get; }
        bool IsToken { get; }
    }
}
