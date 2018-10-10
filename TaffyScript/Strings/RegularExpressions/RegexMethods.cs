using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealRegex = System.Text.RegularExpressions.Regex;
using TaffyScript.Collections;

namespace TaffyScript.Strings.RegularExpressions
{
    /// <summary>
    /// Contains objects and scripts that provide access to the .NET regular expression engine.
    /// </summary>
    [TaffyScriptBaseType]
    public static class RegexMethods
    {
        /// <summary>
        /// Determines if the regex finds a match in the specified input string.
        /// </summary>
        /// <arg name="input" type="string">The string to search for a match.</arg>
        /// <arg name="regex" type="string">The regular expression pattern.</arg>
        /// <arg name="[options]" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)">The options to use while matching.</arg>
        /// <arg name="[matchTimeout]" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">A timeout interval.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch#System_Text_RegularExpressions_Regex_IsMatch_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_</source>
        /// <returns>bool</returns>
        public static TsObject regex_is_match(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    return RealRegex.IsMatch((string)args[0], (string)args[1]);
                case 3:
                    return RealRegex.IsMatch((string)args[0], (string)args[1], (RegexOptions)(int)args[2]);
                case 4:
                    return RealRegex.IsMatch((string)args[0], (string)args[1], (RegexOptions)(int)args[2], ((TsTimeSpan)args[3]).Source);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(regex_is_match)}");
            }
        }

        /// <summary>
        /// Searches the input string for the last occurrence of the specified regex.
        /// </summary>
        /// <arg name="input" type="string">The string to search for a match.</arg>
        /// <arg name="regex" type="string">The regular expression pattern.</arg>
        /// <arg name="[options]" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)">The options to use while matching.</arg>
        /// <arg name="[matchTimeout]" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">A timeout interval.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match#System_Text_RegularExpressions_Regex_Match_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_</source>
        /// <returns>[Match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Match)</returns>
        public static TsObject regex_match(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    return new Match(RealRegex.Match((string)args[0], (string)args[1]));
                case 3:
                    return new Match(RealRegex.Match((string)args[0], (string)args[1], (RegexOptions)(int)args[2]));
                case 4:
                    return new Match(RealRegex.Match((string)args[0], (string)args[1], (RegexOptions)(int)args[2], ((TsTimeSpan)args[3]).Source));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(regex_match)}");
            }
        }

        /// <summary>
        /// Searches the input string for all occurrences of the specified regex.
        /// </summary>
        /// <arg name="input" type="string">The string to search for matches.</arg>
        /// <arg name="regex" type="string">The regular expression pattern.</arg>
        /// <arg name="[options]" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)">The options to use while matching.</arg>
        /// <arg name="[matchTimeout]" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">A timeout interval.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.matches#System_Text_RegularExpressions_Regex_Matches_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_</source>
        /// <returns>[List]({{site.baseurl}}/docs/List)</returns>
        public static TsObject regex_matches(TsObject[] args)
        {
            MatchCollection matchCollection;
            switch (args.Length)
            {
                case 2:
                    matchCollection = RealRegex.Matches((string)args[0], (string)args[1]);
                    break;
                case 3:
                    matchCollection = RealRegex.Matches((string)args[0], (string)args[1], (RegexOptions)(int)args[2]);
                    break;
                case 4:
                    matchCollection = RealRegex.Matches((string)args[0], (string)args[1], (RegexOptions)(int)args[2], ((TsTimeSpan)args[3]).Source);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(regex_match)}");
            }
            var list = new List<TsObject>();
            foreach (System.Text.RegularExpressions.Match match in matchCollection)
                list.Add(new Match(match));
            return TsList.Wrap(list);
        }

        /// <summary>
        /// Replaces all instances of a regex in an input string with the specified replacement string.
        /// </summary>
        /// <arg name="input" type="string">The string to search for matches.</arg>
        /// <arg name="regex" type="string">The regular expression pattern.</arg>
        /// <arg name="replacement" type="string">The replacement string.</arg>
        /// <arg name="[options]" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)">The options to use while matching.</arg>
        /// <arg name="[matchTimeout]" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">A timeout interval.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.replace#System_Text_RegularExpressions_Regex_Replace_System_String_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_</source>
        /// <returns>string</returns>
        public static TsObject regex_replace(TsObject[] args)
        {
            switch (args.Length)
            {
                case 3:
                    return RealRegex.Replace((string)args[0], (string)args[1], (string)args[2]);
                case 4:
                    return RealRegex.Replace((string)args[0], (string)args[1], (string)args[2], (RegexOptions)(int)args[3]);
                case 5:
                    return RealRegex.Replace((string)args[0], (string)args[1], (string)args[2], (RegexOptions)(int)args[3], ((TsTimeSpan)args[4]).Source);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(regex_replace)}");
            }
        }

        /// <summary>
        /// Splits a string into an array at the positions defined by a regex.
        /// </summary>
        /// <arg name="input" type="string">The string to search to split.</arg>
        /// <arg name="regex" type="string">The regular expression pattern.</arg>
        /// <arg name="[options]" type="[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)">The options to use while matching.</arg>
        /// <arg name="[matchTimeout]" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">A timeout interval.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.split#System_Text_RegularExpressions_Regex_Split_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_</source>
        /// <returns>array</returns>
        public static TsObject regex_split(TsObject[] args)
        {
            string[] split;
            switch (args.Length)
            {
                case 2:
                    split = RealRegex.Split((string)args[0], (string)args[1]);
                    break;
                case 3:
                    split = RealRegex.Split((string)args[0], (string)args[1], (RegexOptions)(int)args[2]);
                    break;
                case 4:
                    split = RealRegex.Split((string)args[0], (string)args[1], (RegexOptions)(int)args[2], ((TsTimeSpan)args[3]).Source);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(regex_match)}");
            }
            var result = new TsObject[split.Length];
            for (var i = 0; i < split.Length; i++)
                result[i] = split[i];
            return result;
        }
    }
}
