using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public struct VariableDeclaration
    {
        public string Name { get; }
        public ISyntaxElement Value { get; }
        public TokenPosition Position { get; }
        public bool HasValue => Value != null;

        public VariableDeclaration(string name, ISyntaxElement value, TokenPosition position)
        {
            Name = name;
            Value = value;
            Position = position;
        }
    }
}
