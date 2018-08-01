using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Number = System.Single;

namespace TaffyScript
{
    public class TsArray : TsObject, ITsInstance
    {
        public TsObject[] Value { get; set; }
        public override VariableType Type => VariableType.Array;
        public override object WeakValue => Value;
        public string ObjectType => "Array";

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsArray(TsObject[] value)
        {
            Value = value;
        }

        public override TsObject[] GetArray()
        {
            return Value;
        }

        public override ITsInstance GetInstance() => this;

        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override Number GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsArray array)
                return Value == array.Value;
            else if (obj is TsObject[] val)
                return Value == val;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("[");
            if (Value.Length != 0)
            {
                sb.Append(Value[0].ToString());
                for (var i = 1; i < Value.Length; i++)
                    sb.Append(", ").Append(Value[i].ToString());
            }
            return sb.Append("]").ToString();
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "get":
                    return Value[(int)args[0]];
                case "get_length":
                    return get_length(null, args);
                case "set":
                    Value[(int)args[0]] = args[1];
                    return Empty;
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, delegateName);
        }

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "length":
                    return Value.Length;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "get":
                    del = new TsDelegate(get, delegateName, this);
                    return true;
                case "get_length":
                    del = new TsDelegate(get, delegateName, this);
                    return true;
                case "set":
                    del = new TsDelegate(set, delegateName, this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsObject get(ITsInstance inst, TsObject[] args)
        {
            return Value[(int)args[0]];
        }

        public TsObject get_length(ITsInstance inst, TsObject[] args)
        {
            if(args is null)
                return Value.Length;

            switch(args.Length)
            {
                case 0:
                    return Value.Length;
                case 1:
                    return Value[(int)args[0]].GetArray().Length;
                case 2:
                    return Value[(int)args[0]].GetArray()[(int)args[1]].GetArray().Length;
                default:
                    var array = Value;
                    for (var i = 0; i < args.Length; i++)
                        array = array[(int)args[i]].GetArray();
                    return array.Length;
            }
        }

        public TsObject set(ITsInstance inst, TsObject[] args)
        {
            Value[(int)args[0]] = args[1];
            return Empty;
        }
    }
}
