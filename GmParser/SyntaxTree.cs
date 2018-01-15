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

        public SyntaxTree()
        {
            Root = new SyntaxNode("root");
        }
    }
}
