using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaffyScript.Collections;
using InternalSource = System.Text.RegularExpressions.Regex;

namespace TaffyScript.Strings.RegularExpressions
{
    /// <summary>
    /// Represents a regular expression.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex</source>
    /// <property name="match_timeout" type="numer" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.matchtimeout</source>
    ///     <summary>Gets the timeout interval of the regex.</summary>
    /// </property>
    /// <property name="options" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.options</source>
    ///     <summary>Gets the options passed into the Regex constructor.</summary>
    /// </property>
    /// <property name="pattern" type="string" access="get">
    ///     <summary>Gets the pattern used to create the regex.</summary>
    /// </property>
    /// <property name="right_to_left" type="bool" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.righttoleft</source>
    ///     <summary>Determines if the regex searches from right to left.</summary>
    /// </property>
    [TaffyScriptObject]
    public sealed class Regex : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Strings.RegularExpressions.Regex";

        public InternalSource Source { get; }

        /// <summary>
        /// Gets or sets the maximum number of entries in the static cache of compiled regular expressions.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.cachesize</source>
        /// <type>number</type>
        public static TsObject cache_size
        {
            get => InternalSource.CacheSize;
            set => InternalSource.CacheSize = (int)value;
        }

        public Regex(InternalSource regex)
        {
            Source = regex;
        }

        /// <summary>
        /// Initializes a new instance of a Regex.
        /// </summary>
        /// <arg name="pattern" type="string">The regular expression pattern.</arg>
        /// <arg name="[options]" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)">A combination of options that modify the regex.</arg>
        /// <arg name="[matchTimeout]" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript.TimeSpan)">A timeout interval.</arg>
        public Regex(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    Source = new InternalSource((string)args[0]);
                    break;
                case 2:
                    Source = new InternalSource((string)args[0], (RegexOptions)(int)args[1]);
                    break;
                case 3:
                    Source = new InternalSource((string)args[0], (RegexOptions)(int)args[1], ((TsTimeSpan)args[2]).Source);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to constructor of {ObjectType}");
            }
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "get_group_names":
                    return get_group_names(args);
                case "get_group_numbers":
                    return get_group_numbers(args);
                case "group_name_from_number":
                    return group_name_from_number(args);
                case "group_number_from_name":
                    return group_number_from_name(args);
                case "is_match":
                    return is_match(args);
                case "match":
                    return match(args);
                case "matches":
                    return matches(args);
                case "replace":
                    return replace(args);
                case "split":
                    return split(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMemberException(ObjectType, scriptName);
        }

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "match_timeout":
                    return new TsTimeSpan(Source.MatchTimeout);
                case "options":
                    return (float)Source.Options;
                case "pattern":
                    return Source.ToString();
                case "right_to_left":
                    return Source.RightToLeft;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "get_group_names":
                    del = new TsDelegate(get_group_names, scriptName);
                    break;
                case "get_group_numbers":
                    del = new TsDelegate(get_group_numbers, scriptName);
                    break;
                case "group_name_from_number":
                    del = new TsDelegate(group_name_from_number, scriptName);
                    break;
                case "group_number_from_name":
                    del = new TsDelegate(group_number_from_name, scriptName);
                    break;
                case "is_match":
                    del = new TsDelegate(is_match, scriptName);
                    break;
                case "match":
                    del = new TsDelegate(match, scriptName);
                    break;
                case "matches":
                    del = new TsDelegate(matches, scriptName);
                    break;
                case "replace":
                    del = new TsDelegate(replace, scriptName);
                    break;
                case "split":
                    del = new TsDelegate(split, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a modified string with certain characters replaced with their escape codes.
        /// </summary>
        /// <arg name="str">The string to escape.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.escape</source>
        /// <returns>string</returns>
        public static TsObject escape(TsObject[] args)
        {
            return InternalSource.Escape((string)args[0]);
        }

        /// <summary>
        /// Returns an array of capture group names for the regex.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.getgroupnames</source>
        /// <returns>array</returns>
        public TsObject get_group_names(TsObject[] args)
        {
            var names = Source.GetGroupNames();
            var result = new TsObject[args.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = names[i];
            return result;
        }

        /// <summary>
        /// Returns an array of capture group numbers for the regex.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.getgroupnumbers</source>
        /// <returns>array</returns>
        public TsObject get_group_numbers(TsObject[] args)
        {
            var numbers = Source.GetGroupNumbers();
            var result = new TsObject[args.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = numbers[i];
            return result;
        }

        /// <summary>
        /// Gets the group name that corresponds to the group number.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.groupnamefromnumber</source>
        /// <returns>string</returns>
        public TsObject group_name_from_number(TsObject[] args)
        {
            return Source.GroupNameFromNumber((int)args[0]);
        }

        /// <summary>
        /// Gets the group number that corresponds to the group name.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.groupnumberfromname</source>
        /// <returns>number</returns>
        public TsObject group_number_from_name(TsObject[] args)
        {
            return Source.GroupNumberFromName((string)args[0]);
        }

        /// <summary>
        /// Determines if the regex finds a match in the specified input string.
        /// </summary>
        /// <arg name="input" type="string">The string to search for a match.</arg>
        /// <arg name="[start_at=0]" type="number">The position to start the search.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch</source>
        /// <returns>bool</returns>
        public TsObject is_match(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return Source.IsMatch((string)args[0]);
                case 2:
                    return Source.IsMatch((string)args[0], (int)args[1]);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(is_match)}");
            }
        }

        /// <summary>
        /// Searches the input string for the last occurrence of the regex.
        /// </summary>
        /// <arg name="input" type="string">The string to search for a match.</arg>
        /// <arg name="[start_at=0]" type="number">The position to start the search.</arg>
        /// <arg name="[length]" type="number">The number of characters to include in the search. Includes all characters by default.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match</source>
        /// <returns>[Match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Match)</returns>
        public TsObject match(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new Match(Source.Match((string)args[0]));
                case 2:
                    return new Match(Source.Match((string)args[0], (int)args[1]));
                case 3:
                    return new Match(Source.Match((string)args[0], (int)args[1], (int)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(match)}");
            }
        }

        /// <summary>
        /// Searches an input string for all occurrences of the regex and returns a list of the matches.
        /// </summary>
        /// <arg name="input" type="string">The string to search for matches.</arg>
        /// <arg name="[start_at=0]" type="number">The position to start the search.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.matches</source>
        /// <returns>[List]({{site.baseurl}}/docs/List)</returns>
        public TsObject matches(TsObject[] args)
        {
            MatchCollection matchCollection;
            switch (args.Length)
            {
                case 1:
                    matchCollection = Source.Matches((string)args[0]);
                    break;
                case 2:
                    matchCollection = Source.Matches((string)args[0], (int)args[1]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(matches)}");
            }
            var list = new List<TsObject>();
            foreach (System.Text.RegularExpressions.Match match in matchCollection)
                list.Add(new Match(match));
            return TsList.Wrap(list);
        }

        /// <summary>
        /// Replaces all occurrences of the regex in an input string with the specified replacement string.
        /// </summary>
        /// <arg name="input" type="string">The string to search for matches.</arg>
        /// <arg name="replacement" type="string">The replacement string.</arg>
        /// <arg name="[count]" type="number">The maximum number of replacements. Defaults to no limit.</arg>
        /// <arg name="[start_at=0]" type="number">The position to start the search.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.replace</source>
        /// <returns>string</returns>
        public TsObject replace(TsObject[] args)
        {
            switch (args.Length)
            {
                case 2:
                    return Source.Replace((string)args[0], (string)args[1]);
                case 3:
                    return Source.Replace((string)args[0], (string)args[1], (int)args[2]);
                case 4:
                    return Source.Replace((string)args[0], (string)args[1], (int)args[2], (int)args[3]);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(replace)}");
            }
        }

        /// <summary>
        /// Splits the input string into an array at the positions where the regex matches.
        /// </summary>
        /// <arg name="input" type="string">The string to split.</arg>
        /// <arg name="[count]" type="number">The maximum number of splits. Defaults to no limit.</arg>
        /// <arg name="[start_at=0]" type="number">The position to start the search.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.split</source>
        /// <returns>array</returns>
        public TsObject split(TsObject[] args)
        {
            string[] split;
            switch (args.Length)
            {
                case 1:
                    split = Source.Split((string)args[0]);
                    break;
                case 2:
                    split = Source.Split((string)args[0], (int)args[1]);
                    break;
                case 3:
                    split = Source.Split((string)args[0], (int)args[1], (int)args[2]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(split)}");
            }
            var result = new TsObject[split.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = split[i];
            return result;
        }

        /// <summary>
        /// Returns a modified string with all escaped characters replaced with their normal counterparts.
        /// </summary>
        /// <arg name="str" type="string">The input string to modify.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.unescape</source>
        /// <returns>string</returns>
        public static TsObject unescape(TsObject[] args)
        {
            return InternalSource.Unescape((string)args[0]);
        }

        public static implicit operator TsObject(Regex regex) => new TsInstanceWrapper(regex);
        public static explicit operator Regex(TsObject obj) => (Regex)obj.WeakValue;
    }
}
