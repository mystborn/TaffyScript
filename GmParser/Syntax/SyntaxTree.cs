using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Syntax
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
    }
}
