﻿using System;
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
        public static TsObject string_char_at(TsObject[] args)
        {
            var str = (string)args[0];
            var index = (int)args[1];
            return index >= str.Length || index < 0 ? "" : str[index].ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_copy(TsObject[] args)
        {
            return args[0].GetString().Substring((int)args[1], (int)args[2]);
        }

        [TaffyScriptMethod]
        public static TsObject string_count(TsObject[] args)
        {
            var str = (string)args[0];
            var subString = (string)args[1];

            // Code found here:
            // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string
            return (str.Length - str.Replace(subString, "").Length) / subString.Length;
        }

        [TaffyScriptMethod]
        public static TsObject string_delete(TsObject[] args)
        {
            return args[0].GetString().Remove((int)args[1], (int)args[2]);
        }

        [TaffyScriptMethod]
        public static TsObject string_digits(TsObject[] args)
        {
            var str = (string)args[0];

            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(str, @"[^\d]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                //Good ol fashioned C trick.
                if (str[i] >= '0' && str[i] <= '9')
                    sb.Append(str[i]);
            }
            return sb.ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_insert(TsObject[] args)
        {
            return args[0].GetString().Insert((int)args[2], (string)args[1]);
        }

        [TaffyScriptMethod]
        public static TsObject string_join(TsObject[] args)
        {
            if (args.Length == 1)
                return "";

            var seperator = (string)args[0];
            var sb = new StringBuilder(args[1].ToString());
            for(var i = 2; i < args.Length; i++)
                sb.Append(seperator).Append(args[i].ToString());

            return sb.ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_length(TsObject[] args)
        {
            return args[0].GetString().Length;
        }

        [TaffyScriptMethod]
        public static TsObject string_letters(TsObject[] args)
        {
            var str = (string)args[0];

            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'A' && str[i] <= 'Z'))
                    sb.Append(str[i]);
            }
            return sb.ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_letters_digits(TsObject[] args)
        {
            var str = (string)args[0];

            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z\d]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= '0' && str[i] <= '9'))
                    sb.Append(str[i]);
            }
            return sb.ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_lower(TsObject[] args)
        {
            return args[0].GetString().ToLower();
        }

        [TaffyScriptMethod]
        public static TsObject string_ord_at(TsObject[] args)
        {
            return (float)args[0].GetString()[(int)args[1]];
        }

        [TaffyScriptMethod]
        public static TsObject string_pos(TsObject[] args)
        {
            return args[0].GetString().IndexOf((string)args[1]);
        }

        [TaffyScriptMethod]
        public static TsObject string_repeat(TsObject[] args)
        {
            var str = (string)args[0];
            var count = (int)args[1];

            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
                sb.Append(str);

            return sb.ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_replace(TsObject[] args)
        {
            var str = (string)args[0];
            var subString = (string)args[1];
            var newString = (string)args[2];

            var index = str.IndexOf(subString);
            return index != -1 ? str.Substring(0, index) + newString + str.Substring(index + subString.Length) : str;
        }

        [TaffyScriptMethod]
        public static TsObject string_replace_all(TsObject[] args)
        {
            return args[0].GetString().Replace((string)args[1], (string)args[2]);
        }

        [TaffyScriptMethod]
        public static TsObject string_split(TsObject[] args)
        {
            string[] parts;
            if(args[args.Length - 1].Type == VariableType.Real)
            {
                var sso = (StringSplitOptions)args[args.Length - 1].GetNumber();
                var seperators = new string[args.Length - 2];
                for (var i = 1; i < seperators.Length; i++)
                    seperators[i] = (string)args[i];
                parts = args[0].GetString().Split(seperators, sso);
            }
            else
            {
                var seperators = new string[args.Length - 1];
                for (var i = 1; i < seperators.Length; i++)
                    seperators[i] = (string)args[i];
                parts = args[0].GetString().Split(seperators, StringSplitOptions.None);
            }
            var result = new TsObject[parts.Length];
            for (var i = 0; i < parts.Length; i++)
                result[i] = parts[i];

            return result;
        }

        [TaffyScriptMethod]
        public static TsObject string_upper(TsObject[] args)
        {
            return args[0].GetString().ToUpper();
        }
    }
}
