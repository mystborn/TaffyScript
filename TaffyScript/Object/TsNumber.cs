using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Number = System.Single;

namespace TaffyScript
{
    public class TsNumber : TsObject
    {
        public Number Value;
        public override object WeakValue => Value;
        public override VariableType Type => VariableType.Real;

        public TsNumber(bool value)
        {
            Value = value ? 1f : 0f;
        }

        public TsNumber(byte value)
        {
            Value = value;
        }

        public TsNumber(sbyte value)
        {
            Value = value;
        }

        public TsNumber(short value)
        {
            Value = value;
        }

        public TsNumber(ushort value)
        {
            Value = value;
        }

        public TsNumber(int value)
        {
            Value = value;
        }

        public TsNumber(uint value)
        {
            Value = value;
        }

        public TsNumber(long value)
        {
            Value = value;
        }

        public TsNumber(ulong value)
        {
            Value = value;
        }

        public TsNumber(float value)
        {
            Value = value;
        }

        public TsNumber(double value)
        {
            Value = (float)value;
        }

        public override Number GetNumber()
        {
            return Value;
        }

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override ITsInstance GetInstance() => throw new InvalidTsTypeException($"Variable is supposed to be of type Instance, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsNumber num)
                return Value == num.Value;
            else if (obj is Number val)
                return Value == val;

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
