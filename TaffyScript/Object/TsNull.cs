using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    internal class TsNull : TsObject
    {
        public override VariableType Type => VariableType.Null;
        public override object WeakValue => null;

        public TsNull() { }

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override float GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override ITsInstance GetInstance() => throw new InvalidTsTypeException($"Variable is supposed to be of type Instance, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "null";
        }

        public override bool Equals(object obj)
        {
            if (obj is TsNull || obj is null)
                return true;

            return false;
        }
    }
}
