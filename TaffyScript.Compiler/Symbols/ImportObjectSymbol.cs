using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Compiler.Syntax;

namespace TaffyScript.Compiler
{
    public class ImportObjectSymbol : SymbolNode
    {
        public ImportObjectNode ImportObject { get; }
        public System.Reflection.ConstructorInfo Constructor { get; set; }
        public System.Reflection.MethodInfo TryGetDelegate { get; set; }
        public bool HasImportedObject { get; set; } = false;


        public ImportObjectSymbol(SymbolNode parent, string name, ImportObjectNode importObject) 
            : base(parent, name, SymbolType.Object, SymbolScope.Global)
        {
            ImportObject = importObject;
        }
    }
}
