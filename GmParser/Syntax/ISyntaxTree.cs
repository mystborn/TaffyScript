using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Syntax
{
    public interface ISyntaxTree
    {
        ISyntaxNode Root { get; }
        SymbolTable Table { get; }
    }
}
