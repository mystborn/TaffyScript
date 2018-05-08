using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler
{
    public class VariableLeaf : SymbolLeaf
    {
        public bool IsCaptured { get; set; } = false;

        public VariableLeaf(SymbolNode parent, string name, SymbolScope scope) 
            : base(parent, name, SymbolType.Variable, scope)
        {
        }
    }
}
