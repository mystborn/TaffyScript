using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class SymbolNode : ISymbol
    {
        public SymbolType Type { get; }
        public string Name { get; }
        public Dictionary<string, ISymbol> Children { get; } = new Dictionary<string, ISymbol>();
        public bool IsLeaf => false;
        public SymbolNode Parent { get; }
        public List<string> Pending { get; } = new List<string>();

        public SymbolNode(SymbolNode parent, string name, SymbolType type)
        {
            Parent = parent;
            Name = name;
            Type = type;
        }

        public SymbolNode EnterNew(string name, SymbolType type)
        {
            var scope = new SymbolNode(this, name, type);
            Children.Add(name, scope);
            return scope;
        }

        public void AddPending(string name)
        {
            Pending.Add(name);
        }
    }
}