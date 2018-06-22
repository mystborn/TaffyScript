using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class ImportObjectMethod
    {
        public string ExternalName { get; }
        public string ImportName { get; }
        public List<string> Generics { get; }
        public List<string> Arguments { get; }
        public TokenPosition Position { get; }

        public ImportObjectMethod(string externalName, string importName, List<string> generics, List<string> arguments, TokenPosition position)
        {
            ExternalName = externalName;
            ImportName = importName;
            Generics = generics;
            Arguments = arguments;
            Position = position;
        }
    }
}
