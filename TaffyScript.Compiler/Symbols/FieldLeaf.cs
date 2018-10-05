using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class FieldLeaf : SymbolLeaf, IMemberSymbol
    {
        public AccessModifiers AccessModifiers { get; }

        public FieldLeaf(SymbolNode parent, string name, SymbolScope scope, AccessModifiers accessModifiers) 
            : base(parent, name, SymbolType.Field, scope)
        {
            AccessModifiers = accessModifiers;
        }
    }
}
