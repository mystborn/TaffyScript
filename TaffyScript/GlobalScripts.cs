using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// These scripts and objects are available in the global scope. Reserved for the most commonly used functionality.
    /// </summary>
    [TaffyScriptBaseType("")]
    public static class GlobalScripts
    {
        // This class privodes the implementation of all of the
        // scripts in the global namespace. In an effort to not pollute
        // the c# global namespace, all of the scripts are explicitly defined
        // in Resources/SpecialImports.resource

        /// <summary>
        /// Copies a range of elements from one array to another.
        /// </summary>
        /// <arg name="source_array" type="array">The array to copy from.</arg>
        /// <arg name="source_index" type="number">The index of the source array to start copying from.</arg>
        /// <arg name="dest_array" type="array">The array to write to.</arg>
        /// <arg name="dest_index" type="number">The index of the destination array to start writing to.</arg>
        /// <arg name="count" type="number">The number of elements to copy.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.array.copy?view=netframework-4.7</source>
        /// <returns>null</returns>
        [TaffyScriptMethod]
        public static TsObject array_copy(TsObject[] args)
        {
            Array.Copy(args[0].GetArray(), (int)args[1], args[2].GetArray(), (int)args[3], (int)args[4]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates an array of the specified size, and optionally initialized with the specified value.
        /// </summary>
        /// <arg name="size" type="number">The size of the array to create.</arg>
        /// <arg name="[default_element=null]" type="object">The value to set each array index to.</arg>
        /// <returns>array</returns>
        [TaffyScriptMethod]
        public static TsObject array_create(TsObject[] args)
        {
            var size = args[0].GetInt();
            var value = args.Length > 1 ? args[1] : TsObject.Empty;
            var result = new TsObject[size];
            for (var i = 0; i < size; ++i)
                result[i] = value;

            return result;
        }

        /// <summary>
        /// Determines if the elements in two arrays are equivalent.
        /// </summary>
        /// <arg name="array1" type="array">The first array to compare.</arg>
        /// <arg name="array2" type="array">The second array to compare.</arg>
        /// <returns>bool</returns>
        [TaffyScriptMethod]
        public static TsObject array_equals(TsObject[] args)
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

        /// <summary>
        /// Prints a value depending on the arguments to the Standard Output.
        /// </summary>
        /// <arg name="[value]" type="object">A value to write to stdout. If this isn't provided, this script just writes a line terminator.</arg>
        /// <arg name="[..format_args]" type="objects">These arguments will be used to format value.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.console.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        [TaffyScriptMethod]
        public static TsObject print(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    Console.WriteLine();
                    break;
                case 1:
                    Console.WriteLine(args[0]);
                    break;
                case 2:
                    Console.WriteLine((string)args[0], args[1]);
                    break;
                case 3:
                    Console.WriteLine((string)args[0], args[1], args[2]);
                    break;
                case 4:
                    Console.WriteLine((string)args[0], args[1], args[2], args[3]);
                    break;
                default:
                    var arr = new object[args.Length - 1];
                    Array.Copy(args, 1, arr, 0, arr.Length);
                    Console.WriteLine((string)args[0], arr);
                    break;
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Converts a string into a number.
        /// </summary>
        /// <arg name="str" type="string">The string to convert.</arg>
        /// <arg name="[styles]" type="[NumberStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles?view=netframework-4.7)">Indicates the permitted format of str.</arg>
        /// <arg name="[culture]" type="string">The culture used to convert the string.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.single.parse?view=netframework-4.7</source>
        /// <returns>number</returns>
        public static TsObject parse_number(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return float.Parse((string)args[0]);
                case 2:
                    return float.Parse((string)args[0], (NumberStyles)(int)args[1]);
                case 3:
                    return float.Parse((string)args[0], (NumberStyles)(int)args[1], CultureInfo.GetCultureInfo((string)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(parse_number)}");
            }
        }

        /// <summary>
        /// Attempts to convert a string into a number.
        /// </summary>
        /// <arg name="str" type="string">The string to convert.</arg>
        /// <arg name="[styles]" type="[NumberStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles?view=netframework-4.7)">Indicates the permitted format of str.</arg>
        /// <arg name="[culture]" type="string">The culture used to convert the string.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.single.tryparse?view=netframework-4.7</source>
        /// <returns>number</returns>
        public static TsObject try_parse_number(TsObject[] args)
        {
            bool success;
            float result;
            switch(args.Length)
            {
                case 1:
                    success = float.TryParse((string)args[0], out result);
                    break;
                case 2:
                    success = float.TryParse((string)args[0], (NumberStyles)(int)args[1], CultureInfo.CurrentCulture, out result);
                    break;
                case 3:
                    success = float.TryParse((string)args[0], (NumberStyles)(int)args[1], CultureInfo.GetCultureInfo((string)args[2]), out result);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(try_parse_number)}");
            }
            return new ParseResult(success, result);
        }

        /// <summary>
        /// Creates an error with the specified message and either throws it as an exception or writes it to the debug output.
        /// </summary>
        /// <arg name="message" type="string">The error message.</arg>
        /// <arg name="throws" type="bool">Determines whether to throw an exception or just print the error.</arg>
        /// <returns>null</returns>
        [TaffyScriptMethod]
        public static TsObject show_error(TsObject[] args)
        {
            var error = new UserDefinedException((string)args[0]);
            if ((bool)args[1])
                throw error;

            System.Diagnostics.Debug.WriteLine(error);
            return TsObject.Empty;
        }

        /// <summary>
        /// Converts a value to a string.
        /// </summary>
        /// <arg name="value" type="object">The value to convert to a string.</arg>
        /// <returns>string</returns>
        [TaffyScriptMethod]
        public static TsObject @string(TsObject[] args)
        {
            return args[0].ToString();
        }

        /// <summary>
        /// Gets the type of a value.
        /// </summary>
        /// <arg name="value" type="object">The value to get the type of.</arg>
        /// <returns>[Type]({{site.baseurl}}/docs/TaffyScript/Type)</returns>
        [TaffyScriptMethod]
        public static TsObject @typeof(TsObject[] args)
        {
            switch (args[0].Type)
            {
                case VariableType.Array:
                    return new TsType(typeof(TsObject[]), "array");
                case VariableType.Null:
                    return new TsType(typeof(void), "null");
                case VariableType.Real:
                    return new TsType(typeof(float), "number");
                case VariableType.String:
                    return new TsType(typeof(string), "string");
                case VariableType.Delegate:
                    return new TsType(typeof(TsDelegate), "script");
                case VariableType.Instance:
                    var inst = args[0].GetInstance();
                    return new TsType(inst.GetType(), inst.ObjectType);
                default:
                    throw new ArgumentException("Unknown type value");
            }
        }
    }
}
