using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Defines a method to compare two objects.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icomparer-1?view=netframework-4.7</source>
    [TaffyScriptObject("TaffyScript.Collections.Comparer")]
    public abstract class TsComparer : IComparer<TsObject>, ITsInstance
    {
        public abstract string ObjectType { get; }

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsComparer(TsObject[] args)
        {
        }

        public virtual int Compare(TsObject x, TsObject y)
        {
            return (int)compare(new[] { x, y });
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
                case "compare":
                    del = new TsDelegate(compare, scriptName);
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
            switch(scriptName)
            {
                case "compare":
                    return compare(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        /// <summary>
        /// Compares two objects, returning a number representing the result. -1 for less than, 0 for equal, 1 for greater than.
        /// </summary>
        /// <arg name="left" type="object">The first object to compare.</arg>
        /// <arg name="right" type="object">The second object to compare.</arg>
        /// <returns>number</returns>
        public abstract TsObject compare(TsObject[] args);

        /// <summary>
        /// Creates a Comparer from a script.
        /// </summary>
        /// <arg name="function" type="script">A script that compares two objects.</arg>
        /// <returns>[Comparer]({{site.baseurl}}/docs/TaffyScript/Collections/Comparer)</returns>
        public static TsObject from_script(TsObject[] args)
        {
            return new WrappedScriptComparer((TsDelegate)args[0]);
        }

        public static implicit operator TsObject(TsComparer comparer) => new TsInstanceWrapper(comparer);
        public static explicit operator TsComparer(TsObject obj) => (TsComparer)obj.WeakValue;
    }

    internal class WrappedScriptComparer : TsComparer
    {
        public TsDelegate Function { get; }

        public override string ObjectType => "TaffyScript.Collections.WrappedScriptComparer";

        public WrappedScriptComparer(TsDelegate function)
            : base(null)
        {
            Function = function;
        }

        public override TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "compare":
                    return Function.Invoke(args);
                default:
                    return base.Call(scriptName, args);
            }
        }

        public override int Compare(TsObject x, TsObject y)
        {
            return (int)Function.Invoke(x, y);
        }

        public override bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "compare":
                    del = Function;
                    return true;
                default:
                    return base.TryGetDelegate(scriptName, out del);
            }
        }

        public override TsObject compare(TsObject[] args)
        {
            return Function.Invoke(args);
        }
    }
}
