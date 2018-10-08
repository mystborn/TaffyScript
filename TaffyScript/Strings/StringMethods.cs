using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Strings
{
    [TaffyScriptBaseType]
    public static class StringMethods
    {
        [TaffyScriptMethod]
        public static TsObject base64_decode(TsObject[] args)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String((string)args[0]));
        }

        [TaffyScriptMethod]
        public static TsObject base64_encode(TsObject[] args)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes((string)args[0]));
        }

        [TaffyScriptMethod]
        public static TsObject string_join(TsObject[] args)
        {
            if (args.Length == 1)
                return "";

            var seperator = (string)args[0];
            var sb = new System.Text.StringBuilder(args[1].ToString());
            for(var i = 2; i < args.Length; i++)
                sb.Append(seperator).Append(args[i].ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Takes an ordinal value and converts it to a character.
        /// </summary>
        /// <returns>string</returns>
        [TaffyScriptMethod]
        public static TsObject to_char(TsObject[] args)
        {
            return (char)(int)args[0];
        }
    }
}
