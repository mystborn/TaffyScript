using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;
using MethodImpl = System.Runtime.CompilerServices.MethodImplAttribute;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace TaffyScript
{
    public abstract class TsInstance : ITsInstance
    {
        public static readonly DynamicInstance Global = new DynamicInstance("global");
        public static ITsInstance Other { get; set; }

        internal protected Dictionary<string, TsObject> _members = new Dictionary<string, TsObject>();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => _members[memberName] = value;
        }

        public abstract string ObjectType { get; }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del.Invoke(args);

            throw new MemberAccessException();
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;

            throw new MemberAccessException();
        }

        public TsObject GetMember(string memberName)
        {
            if (_members.TryGetValue(memberName, out var member))
                return member;
            if (TryGetDelegate(memberName, out var del))
                return del;
            throw new MemberAccessException();
        }

        public void SetMember(string memberName, TsObject value)
        {
            _members[memberName] = value;
        }

        public override string ToString()
        {
            return ObjectType;
        }

        public abstract bool TryGetDelegate(string delegateName, out TsDelegate del);
    }
}