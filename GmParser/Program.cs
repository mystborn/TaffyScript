using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmParser.Backend;

namespace GmParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = Parser.FromCode("enum Color { Red = 0xFF0000, Green = 0x00FF00, Blue = 0x0000FF, Next } import System.Console.WriteLine(string) as show_debug_message; script trace { var value = string(argument[0]); for(var i = 1; i < argument_count; i++) { value += \", \" + string(argument[i]); } show_debug_message(value); }");
            var table = tree.Table;
            WriteSyntax(tree.Root, 0);

            //table.PrintTable();
            //table.PrintPending();

            var compiler = new MsilWeakCompiler("test");
            compiler.Compile(tree);

            Console.ReadLine();
        }

        static void WriteSyntax(ISyntaxElement node, int indent)
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
