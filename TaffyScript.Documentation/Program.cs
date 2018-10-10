using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Documentation
{
    class Program
    {
        static void Main(string[] args)
        {
            Main().GetAwaiter().GetResult();
        }

        public static async Task Main()
        {
            var reader = new DocumentationReader();
            var cache = await reader.ReadDocumentation();
            var writer = new DocumentationWriter(@"C:\Users\Chris\Source\Web\TaffyScriptDocs\docs\");
            try
            {
                await writer.Generate(cache);
            }
            catch(Exception e)
            {

            }
        }
    }
}
