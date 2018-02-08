using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmParser.Backend;
using GmParser.Syntax;

namespace GmParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //var code = "import System.Console.WriteLine(string) as show_debug_message; script test { var arr; arr[0] = 20; arr[0] = arr[0] + 20; show_debug_message(arr[0]); }";
            /*var code = "import GmExtern.GmObject.ToString(object) as string; " +
                "script trace { var out = string(argument[0]); for(var i = 1; i < argument_count; i = i + 1) { out = out + \" \" + string(argument[i + 0]); } show_debug_message(out); } " +
                "import System.Console.WriteLine(string) as show_debug_message;";*/

            var code = "import Console.WriteLine(string) as show_debug_message; script main { show_debug_message(\"Hello, World!\"); }";
            var compiler = new MsilWeakCompiler();
            compiler.CompileCode(code, "test");
            //TestAsmBuilder.Test();

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
