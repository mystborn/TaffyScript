using System;
using System.Collections.Generic;

namespace TaffyScript
{
    public abstract class TsInstance : ITsInstance
    {
        public static readonly DynamicInstance Global = new DynamicInstance("global");

        internal protected Dictionary<string, TsObject> _members = new Dictionary<string, TsObject>();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public abstract string ObjectType { get; }


        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;

            throw new MissingMethodException(ObjectType, scriptName);
        }

        public override string ToString()
        {
            return ObjectType;
        }

        public abstract TsObject GetMember(string memberName);
        public abstract void SetMember(string memberName, TsObject value);
        public abstract TsObject Call(string scriptName, params TsObject[] args);
        public abstract bool TryGetDelegate(string delegateName, out TsDelegate del);

    }
}