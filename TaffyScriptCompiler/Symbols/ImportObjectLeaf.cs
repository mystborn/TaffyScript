using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScriptCompiler.Syntax;

namespace TaffyScriptCompiler
{
    public class ImportObjectLeaf : SymbolLeaf
    {
        public ImportObjectNode ImportObject { get; }
        public System.Reflection.ConstructorInfo Constructor { get; set; }
        public bool HasImportedObject { get; set; } = false;


        public ImportObjectLeaf(SymbolNode parent, string name, ImportObjectNode importObject) 
            : base(parent, name, SymbolType.Object, SymbolScope.Global)
        {
            ImportObject = importObject;
        }
    }
}
