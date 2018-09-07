using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public class SymbolTable
    {
        // Stores other nodes that are in the current scope, 
        // typically entered via the "using name.space" syntax.
        private List<SymbolNode> _others = new List<SymbolNode>();

        public SymbolNode Current { get; set; }

        public IEnumerable<ISymbol> Symbols => Current.Children.Values;

        public SymbolTable()
        {
            Current = new SymbolNode(null, "__0Root", SymbolType.Block);
        }

        /// <summary>
        /// Enters an existing scope.
        /// </summary>
        /// <param name="scopeName">The name of the scope to enter.</param>
        public void Enter(string scopeName)
        {
            var symbol = Current.Children[scopeName];
            if (!symbol.IsLeaf)
                Current = (SymbolNode)symbol;
            else
                throw new InvalidOperationException($"Could not enter scope {scopeName}. Was a leaf node.");
        }

        /// <summary>
        /// Exits the current scope, moving up to its parent.
        /// </summary>
        public void Exit()
        {
            if(Current.Parent != null)
                Current = Current.Parent;
        }

        public void Exit(int count)
        {
            for (var i = 0; i < count; i++)
                Exit();
        }

        /// <summary>
        /// Creates a new scope and enters it.
        /// </summary>
        /// <param name="scopeName">The name of the new scope.</param>
        /// <param name="type">The type of the new scope.</param>
        public void EnterNew(string scopeName, SymbolType type)
        {
            Current = Current.EnterNew(scopeName, type);
        }

        public void EnterNew(string scopeName, SymbolType type, SymbolScope scope)
        {
            Current = Current.EnterNew(scopeName, type, scope);
        }

        public bool TryEnterNew(string scope, SymbolType type)
        {
            if (!Current.Children.TryGetValue(scope, out var symbol))
            {
                Current = Current.EnterNew(scope, type);
                return true;
            }
            else
                return false;
        }

        public bool TryEnterNew(string scopeName, SymbolType type, SymbolScope scope)
        {
            if (!Current.Children.TryGetValue(scopeName, out var symbol))
            {
                Current = Current.EnterNew(scopeName, type, scope);
                return true;
            }
            else
                return false;
        }

        public bool TryAdd(ISymbol symbol)
        {
            if (Current.Children.ContainsKey(symbol.Name))
                return false;
            Current.Children.Add(symbol.Name, symbol);
            return true;
        }

        public int EnterNamespace(string ns)
        {
            if (ns == null || ns == "")
                return 0;
            var parts = ns.Split('.');
            var count = 0;
            foreach (var part in parts)
            {
                if (part == "")
                {
                    Exit(count);
                    throw new ArgumentException("The given namespace was invalid", "ns");
                }
                if (!Current.Children.TryGetValue(part, out var symbol))
                {
                    Current = Current.EnterNew(part, SymbolType.Namespace);
                }
                else if (!symbol.IsLeaf)
                    Current = (SymbolNode)symbol;
                else
                {
                    Exit(count);
                    throw new Backend.NameConflictException($"Could not enter the namespace {ns}. Part {part} had a name conflict with symbol {symbol}");
                }
                count++;
            }
            return count;
        }

        public void ExitNamespace(string ns)
        {
            if (ns == "")
                return;
            var parts = ns.Split('.');
            foreach(var part in parts)
            {
                if(part == "")
                    throw new ArgumentException("The given namespace was invalid", "ns");
                Exit();
            }
        }

        public void AddSymbolToDefinitionLookup(SymbolNode node)
        {
            _others.Add(node);
        }

        public void RemoveSymbolFromDefinitionLookup(SymbolNode node)
        {
            _others.Remove(node);
        }

        /// <summary>
        /// Adds the values in a namespace to the definition lookup mechanism.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public SymbolNode AddNamespaceToDefinitionLookup(string ns)
        {
            var count = EnterNamespace(ns);
            var node = Current;
            Exit(count);
            //if(!_others.Contains(node))
                _others.Add(node);
            return node;
        }

        public void RemoveAllNamespacesFromDefinitionLookup()
        {
            _others.Clear();
        }

        /// <summary>
        /// Determines if a symbol is defined in the current scope.
        /// </summary>
        /// <param name="name">The name of the symbol to lookup.</param>
        /// <param name="symbol">The symbol, it's defined.</param>
        public bool Defined(string name, out ISymbol symbol)
        {
            var current = Current;
            while(current != null)
            {
                if(current.Children.TryGetValue(name, out symbol))
                    return true;
                current = current.Parent;
            }
            for(var i = 0; i < _others.Count; i++)
            {
                if (_others[i].Children.TryGetValue(name, out symbol))
                    return true;
            }
            symbol = default(ISymbol);
            return false;
        }

        /// <summary>
        /// Gets the symbol with the specified name in the current scope.
        /// </summary>
        /// <param name="name">The name of the symbol to lookup.</param>
        /// <returns></returns>
        public ISymbol Defined(string name)
        {
            if (Defined(name, out var symbol))
                return symbol;
            return null;
        }

        /// <summary>
        /// Adds a new <see cref="SymbolLeaf"/> to the current scope.
        /// </summary>
        /// <param name="name">The name of the leaf.</param>
        /// <param name="type">The type of the leaf</param>
        /// <param name="scope">The scope of the leaf.</param>
        /// <returns></returns>
        public bool AddLeaf(string name, SymbolType type, SymbolScope scope)
        {
            if(!Current.Children.TryGetValue(name, out var overwrite))
            {
                if (type == SymbolType.Variable)
                    Current.Children.Add(name, new VariableLeaf(Current, name, scope));
                else
                    Current.Children.Add(name, new SymbolLeaf(Current, name, type, scope));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a new <see cref="EnumLeaf"/> to the current scope.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddLeaf(string name, long value)
        {
            if (!Current.Children.TryGetValue(name, out var overwrite))
            {
                Current.Children.Add(name, new EnumLeaf(Current, name, SymbolType.Variable, SymbolScope.Member, value));
                return true;
            }

            return false;
        }

        public bool AddChild(ISymbol symbol)
        {
            if (!Current.Children.TryGetValue(symbol.Name, out var overwrite))
            {
                Current.Children.Add(symbol.Name, symbol);
                return true;
            }
            else if (symbol != overwrite)
                return false;

            return true;
        }

        /// <summary>
        /// Adds a name that couldn't be found in the current scope to check after all analysis is completed.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        public void AddPending(string name)
        {
            Current.AddPending(name);
        }

        public bool Undefine(string name)
        {
            return Current.Children.Remove(name);
        }

        /// <summary>
        /// Prints this table to the Console.
        /// </summary>
        public void PrintTable()
        {
            var top = Current;
            var node = top;
            while(node != null)
            {
                top = node;
                node = node.Parent;
            }
            var sb = new StringBuilder();
            PrintNode(sb, top, 0);
            Console.WriteLine(sb);
        }

        private void PrintNode(StringBuilder sb, ISymbol symbol, int indent)
        {
            sb.Append(' ', indent);
            sb.AppendLine(symbol.Name);
            if(symbol is SymbolNode node)
                foreach(var child in node.Children.Values)
                    PrintNode(sb, child, indent + 2);
        }
    }
}
