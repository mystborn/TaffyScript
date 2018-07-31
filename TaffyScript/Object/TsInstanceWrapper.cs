using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class TsInstanceWrapper : TsObject
    {
        public ITsInstance Value { get; }
        public override VariableType Type => VariableType.Instance;
        public override object WeakValue => Value;

        public TsInstanceWrapper(ITsInstance inst)
        {
            Value = inst;
        }

        public override ITsInstance GetInstance()
        {
            return Value;
        }

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override float GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsInstanceWrapper wrapper)
                return Value == wrapper.Value;
            else if (obj is ITsInstance inst)
                return obj == inst;
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            if (Value.TryGetDelegate("to_string", out var del))
                return (string)del.Invoke();
            return Value.ObjectType;
        }
    }

}
