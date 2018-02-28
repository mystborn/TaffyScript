using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler
{
    public class EnumLeaf : SymbolLeaf
    {
        public int Value { get; }

        public EnumLeaf(SymbolNode parent, string name, SymbolType type, SymbolScope scope, int value) : base(parent, name, type, scope)
        {
            Value = value;
        }
    }
}
