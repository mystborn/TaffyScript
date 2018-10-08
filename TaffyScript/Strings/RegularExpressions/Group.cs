using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaffyScript.Collections;
using InternalSource = System.Text.RegularExpressions.Group;

namespace TaffyScript.Strings.RegularExpressions
{
    /// <summary>
    /// Represents the result of a single capturing group from a Regex.
    /// </summary>
    /// <property name="captures" type="[List]({{site.baseurl}}/docs/List)" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.group.captures</source>
    ///     <summary>Gets a list of the captures matched by the capturing group.</summary>
    /// </property>
    /// <property name="name" type="string" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.group.name</source>
    ///     <summary>Gets the name of the capturing group.</summary>
    /// </property>
    /// <property name="success" type="bool" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.group.success</source>
    ///     <summary>Determines whether the match was successful</summary>
    /// </property>
    public class Group : Capture
    {
        private TsList _captures;

        public override string ObjectType => "TaffyScript.Strings.RegularExpressions.Group";
        public override System.Text.RegularExpressions.Capture WrappedCapture => WrappedGroup;

        public virtual InternalSource WrappedGroup { get; }
        public TsList Captures
        {
            get
            {
                if(_captures is null)
                {
                    var captures = new List<TsObject>();
                    foreach (System.Text.RegularExpressions.Capture capture in WrappedGroup.Captures)
                        captures.Add(new Capture(capture));
                    _captures = TsList.Wrap(captures);
                }
                return _captures;
            }
        }

        protected Group() { }

        public Group(InternalSource group)
        {
            WrappedGroup = group;
        }

        public override TsObject GetMember(string name)
        {
            switch (name)
            {
                case "captures":
                    return Captures;
                case "index":
                    return WrappedGroup.Index;
                case "length":
                    return WrappedGroup.Length;
#if NET_4_7
                case "name":
                    return WrappedGroup.Name;
#endif
                case "success":
                    return WrappedGroup.Success;
                case "value":
                    return WrappedGroup.Value;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public static explicit operator Group(TsObject obj) => (Group)obj.WeakValue;
    }
}
