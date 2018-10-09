using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Defines a key/value pair.
    /// </summary>
    [TaffyScriptObject("TaffyScript.Collections.KeyValuePair")]
    public sealed class TsKeyValuePair : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Collections.KeyValuePair";

        /// <summary>
        /// The key of the pair.
        /// </summary>
        /// <type>object</type>
        public TsObject key { get; }

        /// <summary>
        /// The value of the pair.
        /// </summary>
        /// <type>object</type>
        public TsObject value { get; }

        public TsKeyValuePair(TsObject key, TsObject value)
        {
            this.key = key;
            this.value = value;
        }

        public TsKeyValuePair(KeyValuePair<TsObject, TsObject> kvp)
        {
            key = kvp.Key;
            value = kvp.Value;
        }

        public TsKeyValuePair(TsObject[] args)
        {
            key = args[0];
            value = args[1];
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is TsKeyValuePair tkvp)
                return key == tkvp.key && value == tkvp.value;
            else if (obj is KeyValuePair<TsObject, TsObject> kvp)
                return key == kvp.Key && value == kvp.Value;

            return false;
        }

        public override string ToString()
        {
            return $"{{ {key}, {value} }}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (key.GetHashCode() * 23) ^ value.GetHashCode();
            }
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "key":
                    return key.GetDelegate().Invoke(args);
                case "value":
                    return value.GetDelegate().Invoke(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "key":
                    return key;
                case "value":
                    return value;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "key":
                    del = (TsDelegate)key;
                    return true;
                case "value":
                    del = (TsDelegate)value;
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public static implicit operator TsObject(TsKeyValuePair kvp) => new TsInstanceWrapper(kvp);
        public static explicit operator TsKeyValuePair(TsObject obj) => (TsKeyValuePair)obj.WeakValue;
    }
}
