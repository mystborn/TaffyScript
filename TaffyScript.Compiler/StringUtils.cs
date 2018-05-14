using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
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

                sb.Append(split[i].Substring(1, split[i].Length - 1));
            }
            return sb.ToString();
        }

        public static string ConvertToPascalCase(string value)
        {
            var sb = new StringBuilder();
            var split = value.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < split.Length; i++)
            {
                sb.Append(char.ToUpper(split[i][0]));
                sb.Append(split[i].Substring(1, split[i].Length - 1));
            }
            return sb.ToString();
        }

        public static string ConvertToSnakeCase(string value)
        {
            return string.Concat(value.Select((x, i) => i == 0 ? char.ToLower(x).ToString() : char.IsUpper(x) ? "_" + char.ToLower(x) : x.ToString()));
        }
    }
}
