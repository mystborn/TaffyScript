using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class TsDelegateWrapper : TsObject
    {
        public TsDelegate Value { get; }
        public override VariableType Type => VariableType.Delegate;
        public override object WeakValue => Value;

        public TsDelegateWrapper(TsDelegate value)
        {
            Value = value;
        }

        public override TsDelegate GetDelegate()
        {
            return Value;
        }

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override float GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override ITsInstance GetInstance() => throw new InvalidTsTypeException($"Variable is supposed to be of type Instance, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsDelegateWrapper wrapper)
                return Value == wrapper.Value;
            else if (obj is TsDelegate del)
                return Value == del;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
