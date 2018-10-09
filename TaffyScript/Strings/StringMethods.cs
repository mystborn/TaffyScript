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
        /// <summary>
        /// Decodes a 64-bit encoded string.
        /// </summary>
        /// <arg name="str" type="string">The string to decode.</arg>
        /// <returns>string</returns>
        public static TsObject base64_decode(TsObject[] args)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String((string)args[0]));
        }
        
        /// <summary>
        /// Converts a string to a base64 encoded string.
        /// </summary>
        /// <arg name="str" type="string">The string to encode.</arg>
        /// <returns>string</returns>
        public static TsObject base64_encode(TsObject[] args)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes((string)args[0]));
        }
        
        /// <summary>
        /// Converts each argument into a string and concatenates them together with the specified seperator.
        /// </summary>
        /// <arg name="seperator" type="string">The string to put in between each argument.</arg>
        /// <arg name="..args" type="objects">Any number of arguments to convert to a string.</arg>
        /// <returns>string</returns>
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
        public static TsObject to_char(TsObject[] args)
        {
            return (char)(int)args[0];
        }
    }
}
