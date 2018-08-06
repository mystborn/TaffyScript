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
            return args[0].ToString();
        }

        [TaffyScriptMethod]
        public static TsObject array_copy(ITsInstance inst, TsObject[] args)
        {
            Array.Copy(args[0].GetArray(), (int)args[1], args[2].GetArray(), (int)args[3], (int)args[4]);
            return TsObject.Empty;
        }

        [TaffyScriptMethod]
        public static TsObject array_create(ITsInstance target, TsObject[] args)
        {
            var size = args[0].GetInt();
            var value = TsObject.Empty;
            if (args.Length > 1)
                value = args[1];
            var result = new TsObject[size];
            for (var i = 0; i < size; ++i)
                result[i] = value;

            return result;
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
            return TsObject.Empty;
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
            return TsObject.Empty;
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
                    return args[0].GetInstance().ObjectType;
                default:
                    return "unknown";
            }
        }
    }
}
