using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class PropertySymbol : SymbolNode, IMemberSymbol
    {
        public AccessModifiers AccessModifiers { get; }

        public PropertySymbol(SymbolNode parent, string name, SymbolScope scope, AccessModifiers accessModifiers) 
            : base(parent, name, SymbolType.Property, scope)
        {
            AccessModifiers = accessModifiers;
        }
    }
}
