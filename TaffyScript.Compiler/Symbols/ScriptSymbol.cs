using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class ScriptSymbol : SymbolNode, IMemberSymbol
    {
        public AccessModifiers AccessModifiers { get; }

        public ScriptSymbol(SymbolNode parent, string name, SymbolScope scope, AccessModifiers accessModifiers)
            : base(parent, name, SymbolType.Script, scope)
        {
            AccessModifiers = accessModifiers;
        }
    }
}
