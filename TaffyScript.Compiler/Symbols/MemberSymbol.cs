using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TaffyScript.Compiler
{
    public interface IMemberSymbol : ISymbol
    {
        AccessModifiers AccessModifiers { get; }
    }
}
