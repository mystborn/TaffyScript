using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class EnumLeaf : SymbolLeaf
    {
        public long Value { get; }

        public EnumLeaf(SymbolNode parent, string name, SymbolType type, SymbolScope scope, long value) : base(parent, name, type, scope)
        {
            Value = value;
        }
    }
}
