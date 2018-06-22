using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class ObjectSymbol : SymbolNode
    {
        public ISymbol Inherits { get; set; }

        public ObjectSymbol(SymbolNode parent, string name) 
            : base(parent, name, SymbolType.Object)
        {
        }
    }
}
