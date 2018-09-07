using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public struct ObjectField
    {
        public string Name { get; }
        public TokenPosition Position { get; }
        public ISyntaxElement DefaultValue { get; }
        public bool HasDefaultValue => DefaultValue != null;

        public ObjectField(string name, TokenPosition position, ISyntaxElement defaultValue)
        {
            Name = name;
            Position = position;
            DefaultValue = defaultValue;
        }
    }
}
