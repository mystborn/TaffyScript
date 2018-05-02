using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler
{
    public static class StringUtils
    {
        public static string ConvertToCamelCase(string value)
        {
            var sb = new StringBuilder();
            var split = value.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            for(var i = 0; i < split.Length; i++)
            {
                if (i == 0)
                    sb.Append(char.ToLower(split[0][0]));
                else
                    sb.Append(char.ToUpper(split[i][0]));

                sb.Append(split[0].Substring(1, split[0].Length - 1));
            }
            return sb.ToString();
        }

        public static string ConvertToPascalCase(string value)
        {
            return value.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).Select(s => char.ToUpper(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string ConvertToSnakeCase(string value)
        {
            return string.Concat(value.Select((x, i) => i == 0 ? char.ToLower(x).ToString() : char.IsUpper(x) ? "_" + char.ToLower(x) : x.ToString()));
        }
    }
}
