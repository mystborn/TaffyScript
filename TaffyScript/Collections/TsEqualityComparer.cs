using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Wraps a script that is used to check two objects for equality.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1?view=netframework-4.7</source>
    [TaffyScriptObject("TaffyScript.Collections.EqualityComparer")]
    public abstract class TsEqualityComparer : IEqualityComparer<TsObject>, ITsInstance
    {
        public abstract string ObjectType { get; }

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsEqualityComparer(TsObject[] args)
        {
        }

        public virtual bool Equals(TsObject x, TsObject y)
        {
            return (bool)equals(new[] { x, y });
        }

        public virtual int GetHashCode(TsObject obj)
        {
            return (int)get_hash_code(new[] { obj });
        }

        public virtual TsObject GetMember(string name)
        {
            if (TryGetDelegate(name, out var del))
                return del;
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "equals":
                    del = new TsDelegate(equals, scriptName);
                    return true;
                case "get_hash_code":
                    del = new TsDelegate(get_hash_code, scriptName);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "equals":
                    return equals(args);
                case "get_hash_code":
                    return get_hash_code(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        /// <summary>
        /// Checks two objects for equality.
        /// </summary>
        /// <arg name="left" type="object">The first object to compare.</arg>
        /// <arg name="right" type="object">The second object to compare.</arg>
        /// <returns>bool</returns>
        public abstract TsObject equals(TsObject[] args);
        public virtual TsObject get_hash_code(TsObject[] args)
        {
            return args[0].GetHashCode();
        }

        public static TsObject from_script(TsObject[] args)
        {
            return new WrappedScriptEqualityComparer((TsDelegate)args[0]);
        }

        public static implicit operator TsObject(TsEqualityComparer comparer) => new TsInstanceWrapper(comparer);
        public static explicit operator TsEqualityComparer(TsObject obj) => (TsEqualityComparer)obj.WeakValue;
    }

    internal class WrappedScriptEqualityComparer : TsEqualityComparer
    {
        public override string ObjectType => "TaffyScript.Collections.WrappedScriptEqualityComparer";
        public TsDelegate Function { get; }

        public WrappedScriptEqualityComparer(TsDelegate function)
            : base(null)
        {
            Function = function;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            else if (obj is WrappedScriptEqualityComparer comparer)
                return Function == comparer.Function;
            else if (obj is TsDelegate function)
                return Function == function;

            return false;
        }

        public override bool Equals(TsObject x, TsObject y)
        {
            return (bool)Function.Invoke(x, y);
        }

        public override TsObject equals(TsObject[] args)
        {
            return Function.Invoke(args);
        }

        public override int GetHashCode()
        {
            return Function.GetHashCode();
        }

        public override int GetHashCode(TsObject obj)
        {
            return obj.GetHashCode();
        }

        public override TsObject get_hash_code(TsObject[] args)
        {
            return args[0].GetHashCode();
        }
    }
}
