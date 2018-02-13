using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class SymbolTable
    {
        private SymbolNode _root;
        private SymbolNode _current;

        public IEnumerable<ISymbol> Symbols => _current.Children.Values;

        public SymbolTable()
        {
            _root = new SymbolNode(null, "__0Root", SymbolType.Block);
            _current = _root;
        }

        /// <summary>
        /// Enters an existing scope.
        /// </summary>
        /// <param name="scopeName">The name of the scope to enter.</param>
        public void Enter(string scopeName)
        {
            var symbol = _current.Children[scopeName];
            if (!symbol.IsLeaf)
                _current = (SymbolNode)symbol;
            else
                throw new InvalidOperationException($"Could not enter scope {scopeName}. Was a leaf node.");
        }

        /// <summary>
        /// Exits the current scope, moving up to its parent.
        /// </summary>
        public void Exit()
        {
            _current = _current.Parent;
        }

        /// <summary>
        /// Creates a new scope and enters it.
        /// </summary>
        /// <param name="scopeName">The name of the new scope.</param>
        /// <param name="type">The type of the new scope.</param>
        public void EnterNew(string scopeName, SymbolType type)
        {
            _current = _current.EnterNew(scopeName, type);
        }

        /// <summary>
        /// Determines if a symbol is defined in the current scope.
        /// </summary>
        /// <param name="name">The name of the symbol to lookup.</param>
        /// <param name="symbol">The symbol, it's defined.</param>
        public bool Defined(string name, out ISymbol symbol)
        {
            var current = _current;
            symbol = default(ISymbol);
            while(current != null)
            {
                if(current.Children.TryGetValue(name, out symbol))
                    return true;
                current = current.Parent;
            }
            return false;
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
            if(!Defined(name, out var overwrite))
            {
                _current.Children.Add(name, new SymbolLeaf(_current, name, type, scope));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a name that couldn't be found in the current scope to check after all analysis is completed.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        public void AddPending(string name)
        {
            _current.AddPending(name);
        }

        /// <summary>
        /// Prints this table to the Console.
        /// </summary>
        public void PrintTable()
        {
            var sb = new StringBuilder();
            PrintNode(sb, _root, 0);
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

        /// <summary>
        /// Prints all pending variables to the Console.
        /// </summary>
        public void PrintPending()
        {
            var sb = new StringBuilder();
            PrintPending(sb, _root, 0);
            Console.WriteLine(sb);
        }

        private void PrintPending(StringBuilder sb, ISymbol symbol, int indent)
        {
            if(symbol is SymbolNode node)
            {
                foreach(var pending in node.Pending)
                {
                    sb.Append(' ', indent);
                    sb.AppendLine(pending);
                }
                foreach (var child in node.Children.Values)
                    PrintPending(sb, child, indent);
            }
        }
    }
}
