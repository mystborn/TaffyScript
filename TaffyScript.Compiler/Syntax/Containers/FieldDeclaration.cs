using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public struct FieldDeclaration
    {
        public string Name { get; }
        public TokenPosition Position { get; }
        public ISyntaxElement DefaultValue { get; }
        public bool HasDefaultValue => DefaultValue != null;

        public FieldDeclaration(string name, TokenPosition position, ISyntaxElement defaultValue)
        {
            Name = name;
            Position = position;
            DefaultValue = defaultValue;
        }
    }
}
