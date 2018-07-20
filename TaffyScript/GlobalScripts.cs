using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Provides an implementation for all of the global scripts in TaffyScript.
    /// </summary>
    public static class GlobalScripts
    {
        // This class privodes the implementation of all of the
        // scripts in the global namespace. In an effort to not pollute
        // the c# global namespace, all of the scripts are explicitly defined
        // in Resources/SpecialImports.resource

        [TaffyScriptMethod]
        public static TsObject ToString(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type != VariableType.String ? new TsObject(args[0].ToString()) : args[0];
        }

        [TaffyScriptMethod]
        public static TsObject array_copy(ITsInstance inst, TsObject[] args)
        {
            var srcWrapper = args[0].Value as TsMutableValue<TsObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "src");
            var srcIndex = (int)args[1];
            var dstWrapper = args[2].Value as TsMutableValue<TsObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "dest");
            var dstIndex = (int)args[3];
            var length = (int)args[4];
            var src = srcWrapper.StrongValue;
            var dst = dstWrapper.StrongValue;
            if(dstIndex + length >= dst.Length)
            {
                var temp = new TsObject[dstIndex + length + 1];
                Array.Copy(dst, 0, temp, 0, dst.Length);
                dst = temp;
                dstWrapper.StrongValue = dst;
            }
            Array.Copy(src, srcIndex, dst, dstIndex, length);
            return TsObject.Empty();
        }

        [TaffyScriptMethod]
        public static TsObject array_create(ITsInstance target, TsObject[] args)
        {
            var size = args[0].GetInt();
            var value = TsObject.Empty();
            if (args.Length > 1)
                value = args[1];
            var result = new TsObject[size];
            for (var i = 0; i < size; ++i)
                result[i] = value;

            return new TsObject(result);
        }

        [TaffyScriptMethod]
        public static TsObject array_equals(ITsInstance inst, TsObject[] args)
        {
            var left = args[0].GetArray();
            var right = args[1].GetArray();
            if (left.Length != right.Length)
                return false;
            for(var i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                    return false;
            }

            return true;
        }

        [TaffyScriptMethod]
        public static TsObject array_length(ITsInstance inst, TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return args[0].GetArray().Length;
                case 2:
                    return args[0].GetArray((int)args[1]).Length;
                case 3:
                    return args[0].GetArray((int)args[1], (int)args[2]).Length;
                default:
                    var indeces = new int[args.Length - 1];
                    var i = 0;
                    while (i < indeces.Length)
                        indeces[i++] = (int)args[i];
                    return args[0].GetArray(indeces).Length;
            }
        }

        [TaffyScriptMethod]
        public static TsObject print(ITsInstance inst, TsObject[] args)
        {
            Console.WriteLine(args[0]);
            return TsObject.Empty();
        }

        [TaffyScriptMethod]
        public static TsObject real(ITsInstance inst, TsObject[] args)
        {
            return float.Parse((string)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject show_error(ITsInstance inst, TsObject[] args)
        {
            var error = new UserDefinedException((string)args[0]);
            if ((bool)args[1])
                throw error;

            Console.WriteLine(error);
            return TsObject.Empty();
        }

        [TaffyScriptMethod]
        public static TsObject string_char_at(ITsInstance inst, TsObject[] args)
        {
            var str = (string)args[0];
            var index = (int)args[1];
            return index >= str.Length || index < 0 ? "" : str[index].ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_copy(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().Substring((int)args[1], (int)args[2]);
        }

        [TaffyScriptMethod]
        public static TsObject string_count(ITsInstance inst, TsObject[] args)
        {
            var str = (string)args[0];
            var subString = (string)args[1];

            // Code found here:
            // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string
            return (str.Length - str.Replace(subString, "").Length) / subString.Length;
        }

        [TaffyScriptMethod]
        public static TsObject string_delete(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().Remove((int)args[1], (int)args[2]);
        }

        [TaffyScriptMethod]
        public static TsObject string_digits(ITsInstance inst, TsObject[] args)
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
        public static TsObject string_insert(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().Insert((int)args[2], (string)args[1]);
        }

        [TaffyScriptMethod]
        public static TsObject string_length(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().Length;
        }

        [TaffyScriptMethod]
        public static TsObject string_letters(ITsInstance inst, TsObject[] args)
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
        public static TsObject string_letters_digits(ITsInstance inst, TsObject[] args)
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
        public static TsObject string_lower(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().ToLower();
        }

        [TaffyScriptMethod]
        public static TsObject string_ord_at(ITsInstance inst, TsObject[] args)
        {
            return (float)args[0].GetString()[(int)args[1]];
        }

        [TaffyScriptMethod]
        public static TsObject string_pos(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().IndexOf((string)args[1]);
        }

        [TaffyScriptMethod]
        public static TsObject string_repeat(ITsInstance inst, TsObject[] args)
        {
            var str = (string)args[0];
            var count = (int)args[1];

            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
                sb.Append(str);

            return sb.ToString();
        }

        [TaffyScriptMethod]
        public static TsObject string_replace(ITsInstance inst, TsObject[] args)
        {
            var str = (string)args[0];
            var subString = (string)args[1];
            var newString = (string)args[2];

            var index = str.IndexOf(subString);
            return index != -1 ? str.Substring(0, index) + newString + str.Substring(index + subString.Length) : str;
        }

        [TaffyScriptMethod]
        public static TsObject string_replace_all(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().Replace((string)args[1], (string)args[2]);
        }

        [TaffyScriptMethod]
        public static TsObject string_upper(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetString().ToUpper();
        }

        [TaffyScriptMethod]
        public static TsObject Typeof(ITsInstance inst, TsObject[] args)
        {
            switch (args[0].Type)
            {
                case VariableType.Array:
                    return "array";
                case VariableType.Null:
                    return "null";
                case VariableType.Real:
                    return "real";
                case VariableType.String:
                    return "string";
                case VariableType.Delegate:
                    return "script";
                case VariableType.Instance:
                    return args[0].GetInstanceUnchecked().ObjectType;
                default:
                    return "unknown";
            }
        }
    }
}
