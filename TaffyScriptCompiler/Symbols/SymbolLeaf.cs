using System;

namespace TaffyScript
{
    public class SymbolLeaf : ISymbol
    {
        public SymbolType Type { get; }
        public SymbolScope Scope { get; }
        public bool IsLeaf => true;
        public string Name { get; }
        public SymbolNode Parent { get; }

        public SymbolLeaf(SymbolNode parent, string name, SymbolType type, SymbolScope scope)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            Scope = scope;
        }
    }
}
