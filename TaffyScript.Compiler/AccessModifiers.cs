using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    [Flags]
    public enum AccessModifiers
    {
        None = 0,
        Public = 1,
        Private = 2,
        Protected = 4,
        Instance = 8,
        Static = 16,
        Global = 32,
        Assembly = 64,
        Virtual = 128,
        Sealed = 256
    }
}
