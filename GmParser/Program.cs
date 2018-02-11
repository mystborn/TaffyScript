using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GmParser.FrontEnd;
using GmParser.Backend;
using GmParser.Syntax;

namespace GmParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //var code = "import Console.WriteLine(object) as show_debug_message; import GmObject.ToString(array) as string; ";

            //var code = "script main { var user = instance_create(obj_user); user.name = 'Chris'; instance_destroy(user); } object obj_user { event create { name = '' } event destroy { show_debug_message('goodbye, ' + name); } }";

            //CompileBcl();

            var compiler = new MsilWeakCompiler();

            compiler.CompileProject(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Source", "GmToSharpSamples", "HelloLanguage"));

            Console.ReadLine();
        }

        static void CompileBcl()
        {
            var code = Bcl.Generate();
            var compiler = new MsilWeakCompiler();
            compiler.CompileCode(code, "gmbcl");
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
