using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Syntax
{
    /// <summary>
    /// Default implementation of a syntax tree.
    /// </summary>
    public class SyntaxTree : ISyntaxTree
    {
        public ISyntaxNode Root { get; }
        public SymbolTable Table { get; }

        public SyntaxTree(SymbolTable table)
        {
            Table = table;
            Root = new RootNode(null, null);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            PrintElement(sb, Root, 0);
            return sb.ToString();
        }

        private void PrintElement(StringBuilder sb, ISyntaxElement element, int indent)
        {
            sb.Append(' ', indent);
            sb.Append(element.Type);
            if(element.Text != null)
            {
                sb.Append(" : ");
                sb.Append(element.Text);
            }
            sb.AppendLine();
            if(element is ISyntaxNode node)
            {
                foreach (var child in node.Children)
                    PrintElement(sb, child, indent + 2);
            }
        }
    }
}
