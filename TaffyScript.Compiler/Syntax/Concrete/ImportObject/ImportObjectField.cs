using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class ImportObjectField
    {
        public TokenPosition Position { get; }
        public string ExternalName { get; }
        public string ImportName { get; }

        public ImportObjectField(string name, TokenPosition position)
        {
            Position = position;
            ImportName = ExternalName = name;
        }

        public ImportObjectField(string externalName, string importName, TokenPosition position)
        {
            Position = position;
            ExternalName = externalName;
            ImportName = importName;
        }
    }
}
