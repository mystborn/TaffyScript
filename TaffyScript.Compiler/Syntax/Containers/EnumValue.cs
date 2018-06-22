using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public struct EnumValue
    {
        public string Name { get; }
        public long Value { get; }

        public EnumValue(string name, long value)
        {
            Name = name;
            Value = value;
        }
    }
}