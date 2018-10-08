using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Wraps a script used to compare two objects.
    /// </summary>
    /// <script name="compare">
    ///     <arg name="left" type="object">The first object to compare.</arg>
    ///     <arg name="right" type="object">The second object to compare.</arg>
    ///     <summary>Compares the two objects, returning a number representing the result. -1 for less than, 0 for equal, 1 for greater than.</summary>
    ///     <returns>number</returns>
    /// </script>
    [TaffyScriptObject]
    public class TsComparer : IComparer<TsObject>, ITsInstance
    {
        public TsDelegate Function { get; }

        public string ObjectType => "TaffyScript.Collections.TsComparer";

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsComparer(TsObject[] args)
        {
            Function = args[0].GetDelegate();
        }

        public int Compare(TsObject x, TsObject y)
        {
            return (int)Function.Invoke(x, y);
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
                case "compare":
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
            switch(scriptName)
            {
                case "compare":
                    return Function.Invoke(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }
    }
}
