using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScriptCompiler.Syntax;

namespace TaffyScriptCompiler
{
    public class ImportLeaf : SymbolLeaf
    {
        public ImportNode Node { get; }

        public ImportLeaf(SymbolNode parent, string name, SymbolScope scope, ImportNode node)
            : base(parent, name, SymbolType.Script, scope)
        {
            Node = node;
        }
    }
}
