using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public abstract class ObjectWrapper
    {
        private Dictionary<string, TsObject> _members = null;
        protected Dictionary<string, TsObject> Members
        {
            get
            {
                if (_members is null)
                    _members = new Dictionary<string, TsObject>();
                return _members;
            }
        }
    }

    public abstract class TsInstanceTemp : ITsInstance
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

    public class DynamicInstance : ITsInstance
    {
        internal Dictionary<string, TsObject> _members = new Dictionary<string, TsObject>();

        public TsObject this[string memberName]
        {
            get => _members[memberName];
            set => _members[memberName] = value;
        }

        public string ObjectType { get; }

        public DynamicInstance(string typeName)
        {
            ObjectType = typeName;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del.Invoke(args);

            throw new MemberAccessException();
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;

            throw new MemberAccessException();
        }

        public TsObject GetMember(string name)
        {
            return _members[name];
        }

        public void SetMember(string name, TsObject value)
        {
            _members[name] = value;
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            if(_members.TryGetValue(delegateName, out var member) && member.Type == VariableType.Delegate)
            {
                del = member.GetDelegateUnchecked();
                return true;
            }

            del = null;
            return false;
        }

        public static implicit operator TsObject(DynamicInstance instance)
        {
            return new TsObject(instance);
        }

        public static explicit operator DynamicInstance(TsObject obj)
        {
            return (DynamicInstance)obj.Value.WeakValue;
        }
    }
}
