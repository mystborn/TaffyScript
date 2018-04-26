using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Syntax
{
    public class ImportObjectConstructor
    {
        public List<string> ArgumentTypes { get; } = new List<string>();
        public TokenPosition Position { get; }

        public ImportObjectConstructor(TokenPosition position)
        {
            Position = position;
        }
    }
}
