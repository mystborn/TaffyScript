using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class ImportObjectConstructor
    {
        public List<string> Arguments { get; }
        public TokenPosition Position { get; }

        public ImportObjectConstructor(List<string> arguments, TokenPosition position)
        {
            Arguments = arguments;
            Position = position;
        }
    }
}
