using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaffyScript.Collections;
using InternalSource = System.Text.RegularExpressions.Match;

namespace TaffyScript.Strings.RegularExpressions
{
    /// <summary>
    /// Represents the results from a single Regex match.
    /// </summary>
    /// <property name="groups" type="[List]({{site.baseurl}}/docs/List)" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.match.groups</source>
    ///     <summary>Gets a list of groups matched by the regex.</summary>
    /// </property>
    public class Match : Group
    {
        private TsList _groups;

        public override string ObjectType => "TaffyScript.Strings.RegularExpressions.Match";

        public override System.Text.RegularExpressions.Capture WrappedCapture => WrappedMatch;
        public override System.Text.RegularExpressions.Group WrappedGroup => WrappedMatch;

        public InternalSource WrappedMatch { get; }

        public TsList Groups
        {
            get
            {
                if(_groups is null)
                {
                    var groups = new List<TsObject>();
                    foreach (System.Text.RegularExpressions.Group group in WrappedMatch.Groups)
                        groups.Add(new Group(group));
                    _groups = TsList.Wrap(groups);
                }
                return _groups;
            }
        }

        public Match(InternalSource match)
        {
            WrappedMatch = match;
        }

        public override TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "next_match":
                    return next_match(args);
                case "result":
                    return result(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public override TsObject GetMember(string name)
        {
            switch (name)
            {
                case "captures":
                    return Captures;
                case "groups":
                    return Groups;
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

        public override bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "next_match":
                    del = new TsDelegate(next_match, scriptName);
                    return true;
                case "result":
                    del = new TsDelegate(result, scriptName);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        /// <summary>
        /// Returns a new Match with the results for the next match, starting at the position where the last match ended.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.match.nextmatch</source>
        /// <returns>[Match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Match)</returns>
        public TsObject next_match(TsObject[] args)
        {
            return new Match(WrappedMatch.NextMatch());
        }

        /// <summary>
        /// Returns the expansion of the specified replacement pattern.
        /// </summary>
        /// <arg name="replacement">The replacement pattern.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.match.result</source>
        /// <returns>string</returns>
        public TsObject result(TsObject[] args)
        {
            return WrappedMatch.Result((string)args[0]);
        }

        public static explicit operator Match(TsObject obj) => (Match)obj.WeakValue;
    }
}
