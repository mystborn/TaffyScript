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
    /// <script name="equals">
    ///     <arg name="left" type="object">The first object to compare.</arg>
    ///     <arg name="right" type="object">The second object to compare.</arg>
    ///     <summary>Checks two objects for equality.</summary>
    ///     <returns>bool</returns>
    /// </script>
    [TaffyScriptObject]
    public class TsEqualityComparer : IEqualityComparer<TsObject>, ITsInstance
    {
        public TsDelegate Function { get; }

        public string ObjectType => "TaffyScript.Collections.TsEqualityComparer";

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsEqualityComparer(TsObject[] args)
        {
            Function = args[0].GetDelegate();
        }

        public bool Equals(TsObject x, TsObject y)
        {
            return (bool)Function.Invoke(x, y);
        }

        public int GetHashCode(TsObject obj)
        {
            return obj.GetHashCode();
        }

        public TsObject GetMember(string name)
        {
            if (TryGetDelegate(name, out var del))
                return del;
            throw new MissingMemberException(ObjectType, name);
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "equals":
                    del = Function;
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

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "equals":
                    return Function.Invoke(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }
    }
}
