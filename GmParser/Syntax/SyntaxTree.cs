using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public class SyntaxTree
    {
        public SyntaxNode Root { get; }
        public SymbolTable Table { get; }

        public SyntaxTree(SymbolTable table)
        {
            Table = table;
            Root = new SyntaxNode(SyntaxType.Root);
        }
    }
}
