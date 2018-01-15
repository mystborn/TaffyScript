using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myst.LexicalAnalysis;

namespace GmParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var node = parser.ParseExpression("x[0, 2] = (2 + 3) * 5 % 3 << 3");
            WriteSyntax(node, 0);
            Console.ReadLine();
        }

        static void WriteSyntax(ISyntaxNode node, int indent)
        {
            Console.WriteLine(new string(' ', indent) + node.ToString());
            if(node is SyntaxNode parent)
            {
                foreach (var child in parent.Children)
                    WriteSyntax(child, indent + 2);
            }
        }
    }
}
