using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class TsString : TsObject
    {
        public string Value { get; }
        public override VariableType Type => VariableType.String;
        public override object WeakValue => Value;

        public TsString(string value)
        {
            Value = value;
        }

        public TsString(char value)
        {
            Value = value.ToString();
        }

        public override string GetString()
        {
            return Value;
        }

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override float GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override ITsInstance GetInstance() => throw new InvalidTsTypeException($"Variable is supposed to be of type Instance, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsString str)
                return Value == str.Value;
            else if (obj is string val)
                return Value == val;
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
