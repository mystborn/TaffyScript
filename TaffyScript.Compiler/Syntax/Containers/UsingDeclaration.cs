using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public struct UsingDeclaration
    {
        public string Namespace { get; }
        public TokenPosition Position { get; }

        public UsingDeclaration(string nameSpace, TokenPosition position)
        {
            Namespace = nameSpace;
            Position = position;
        }
    }
}
