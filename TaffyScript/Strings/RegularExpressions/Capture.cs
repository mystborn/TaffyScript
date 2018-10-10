using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalSource = System.Text.RegularExpressions.Capture;

namespace TaffyScript.Strings.RegularExpressions
{
    /// <summary>
    /// Represents the result of a successfully captured subexpression from a Regex.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.capture</source>
    /// <property name="index" type="number" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.capture.index</source>
    ///     <summary>Gets the position in the original string where the last character of the capture was found.</summary>
    /// </property>
    /// <property name="length" type="number" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.capture.length</source>
    ///     <summary>Gets the length of the captured string.</summary>
    /// </property>
    /// <property name="value" type="string" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.capture.value</source>
    ///     <summary>Gets the captured string.</summary>
    /// </property>
    public class Capture : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public virtual string ObjectType => "TaffyScript.Strings.RegularExpressions.Capture";
        public virtual InternalSource WrappedCapture { get; }

        protected Capture() { }

        public Capture(InternalSource capture)
        {
            WrappedCapture = capture;
        }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public virtual TsObject GetMember(string name)
        {
            switch(name)
            {
                case "index":
                    return WrappedCapture.Index;
                case "length":
                    return WrappedCapture.Length;
                case "value":
                    return WrappedCapture.Value;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public virtual void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            del = null;
            return false;
        }

        public static implicit operator TsObject(Capture capture) => new TsInstanceWrapper(capture);
        public static explicit operator Capture(TsObject obj) => (Capture)obj.WeakValue;
    }
}
