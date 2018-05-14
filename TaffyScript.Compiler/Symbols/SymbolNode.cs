using System.Collections.Generic;

namespace TaffyScript.Compiler
{
    public class SymbolNode : ISymbol
    {
        public SymbolType Type { get; }
        public SymbolScope Scope { get; }
        public string Name { get; }
        public Dictionary<string, ISymbol> Children { get; } = new Dictionary<string, ISymbol>();
        public bool IsLeaf => false;
        public SymbolNode Parent { get; }
        public List<string> Pending { get; } = new List<string>();

        public SymbolNode(SymbolNode parent, string name, SymbolType type)
            : this(parent, name, type, SymbolScope.Global)
        {
        }

        public SymbolNode(SymbolNode parent, string name, SymbolType type, SymbolScope scope)
        {
            Parent = parent;
            Name = name;
            Type = type;
            Scope = scope;
        }

        public SymbolNode EnterNew(string name, SymbolType type) => EnterNew(name, type, SymbolScope.Global);

        public SymbolNode EnterNew(string name, SymbolType type, SymbolScope scope)
        {
            var child = new SymbolNode(this, name, type, scope);
            Children.Add(name, child);
            return child;
        }

        public void AddPending(string name)
        {
            Pending.Add(name);
        }

        public override string ToString()
        {
            return $"SymbolNode {Type} {Name}";
        }
    }
}