using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public interface IConstantToken : ISyntaxToken
    {
        ConstantType ConstantType { get; }
        object WeakValue { get; }
    }

    public interface IConstantToken<T> : IConstantToken
    {
        T Value { get; }
    }
}
