using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Compiler.FrontEnd;
using TaffyScript.Compiler.Syntax;

namespace TaffyScript.Compiler
{
    public class SymbolResolver
    {
        SymbolTable _table;
        IErrorLogger _logger;

        public SymbolResolver(SymbolTable table, IErrorLogger logger)
        {
            _table = table;
            _logger = logger;
        }

        public string GetAssetNamespace(ISymbol symbol)
        {
            var sb = new StringBuilder();
            var parent = symbol.Parent;
            while(parent != null && parent.Type == SymbolType.Namespace)
            {
                sb.Insert(0, parent.Name + ".");
                parent = parent.Parent;
            }
            return sb.ToString().TrimEnd();
        }

        public string GetAssetFullName(ISymbol symbol)
        {
            var sb = new StringBuilder(symbol.Name);
            symbol = symbol.Parent;
            while(symbol != null && symbol.Type == SymbolType.Namespace)
            {
                sb.Insert(0, symbol.Name + ".");
                symbol = symbol.Parent;
            }
            return sb.ToString().TrimStart('.');
        }

        public bool TryResolveNamespace(MemberAccessNode node, out ISyntaxElement resolved, out SymbolNode namespaceNode)
        {
            namespaceNode = default;
            resolved = default;

            while(node.Left is ISyntaxToken token && _table.Defined(token.Name, out var symbol) && symbol.Type == SymbolType.Namespace)
            {
                namespaceNode = symbol as SymbolNode;
                resolved = node.Right;
                if (node.Right.Type == SyntaxType.MemberAccess)
                    node = (MemberAccessNode)node.Right;
                else
                    break;
            }

            return namespaceNode != null;
        }

        public bool TryResolveType(ISyntaxElement typeElement, out ISymbol typeSymbol)
        {
            if(((typeElement is MemberAccessNode memberAccess &&
                 TryResolveNamespace(memberAccess, out var token, out var ns) &&
                 ns.Children.TryGetValue(((ISyntaxToken)token).Name, out typeSymbol)) ||
                (_table.Defined(((ISyntaxToken)typeElement).Name, out typeSymbol))))
            {
                return true;
            }

            typeSymbol = default(ISymbol);
            return false;
        }
    }
}
