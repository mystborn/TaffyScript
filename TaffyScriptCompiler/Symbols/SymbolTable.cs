using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler
{
    public class SymbolTable
    {
        private Stack<SymbolNode> _current = new Stack<SymbolNode>();

        private SymbolNode Current
        {
            get => _current.Peek();
            set => _current.Push(value);
        }

        public IEnumerable<ISymbol> Symbols => Current.Children.Values;

        public SymbolTable()
        {
            var root = new SymbolNode(null, "__0Root", SymbolType.Block);
            _current.Push(root);
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
            _current.Pop();
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

        public bool TryCreate(string scope, SymbolType type)
        {
            if (!Current.Children.TryGetValue(scope, out var symbol))
            {
                Current.EnterNew(scope, type);
                return true;
            }
            else
                return false;
        }

        public int EnterNamespace(string ns)
        {
            if (ns == null || ns == "")
                return 0;
            var parts = ns.Split('.');
            foreach (var part in parts)
            {
                if (part == "")
                    throw new ArgumentException("The given namespace was invalid", "ns");
                if (!Current.Children.TryGetValue(part, out var symbol))
                {
                    Current = Current.EnterNew(part, SymbolType.Namespace);
                }
                else if (!symbol.IsLeaf)
                    Current = (SymbolNode)symbol;
                else
                    throw new Backend.NameConflictException($"Could not enter the namespace {ns}. Part {part} had a name conflict with symbol {symbol}");
            }
            return parts.Length;
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

        /// <summary>
        /// Determines if a symbol is defined in the current scope.
        /// </summary>
        /// <param name="name">The name of the symbol to lookup.</param>
        /// <param name="symbol">The symbol, it's defined.</param>
        public bool Defined(string name, out ISymbol symbol)
        {
            var current = new Queue<SymbolNode>(_current);
            symbol = default(ISymbol);
            while(current.Count != 0)
            {
                if(current.Peek().Children.TryGetValue(name, out symbol))
                    return true;
                current.Dequeue();
            }
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
            if(!Defined(name, out var overwrite))
            {
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
        public bool AddLeaf(string name, int value)
        {
            if (!Defined(name, out var overwrite))
            {
                Current.Children.Add(name, new EnumLeaf(Current, name, SymbolType.Variable, SymbolScope.Member, value));
                return true;
            }

            return false;
        }

        public bool AddChild(ISymbol symbol)
        {
            if (!Defined(symbol.Name, out var overwrite))
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
            var top = _current.Reverse().First();
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

        /// <summary>
        /// Prints all pending variables to the Console.
        /// </summary>
        public void PrintPending()
        {
            var top = _current.Reverse().First();
            var sb = new StringBuilder();
            PrintPending(sb, top, 0);
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
