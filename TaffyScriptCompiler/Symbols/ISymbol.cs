using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public interface ISymbol
    {
        SymbolType Type { get; }
        bool IsLeaf { get; }
        string Name { get; }
        SymbolNode Parent { get; }
    }
}
