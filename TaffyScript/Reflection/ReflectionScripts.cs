using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Reflection
{
    [TaffyScriptBaseType]
    public static class ReflectionScripts
    {
        [TaffyScriptMethod]
        public static TsObject call_global_script(ITsInstance inst, TsObject[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("You must pass at least a script name to script_execute.");
            var name = args[0].GetString();
            if (!TsReflection.GlobalScripts.TryGetValue(name, out var function))
                throw new ArgumentException($"Tried to execute a non-existant function: {name}");
            var parameters = new TsObject[args.Length - 1];
            if (parameters.Length != 0)
                Array.Copy(args, 1, parameters, 0, parameters.Length);
            return function.Invoke(inst, parameters);
        }

        [TaffyScriptMethod]
        public static TsObject call_instance_script(ITsInstance inst, TsObject[] args)
        {
            if (TsReflection.TryGetScript((string)args[1], (string)args[2], out var ev))
            {
                TsObject[] copy;
                if (args.Length > 3)
                {
                    copy = new TsObject[args.Length - 3];
                    Array.Copy(args, 3, copy, 0, copy.Length);
                }
                else
                    copy = null;

                return ev.Invoke(args[0].GetInstance(), copy);
            }
            return TsObject.Empty();
        }

        [TaffyScriptMethod]
        public static TsObject instance_create(ITsInstance inst, TsObject[] args)
        {
            var type = (string)args[0];
            if (!TsReflection.Constructors.TryGetValue(type, out var ctor))
                throw new ArgumentException($"Cannot create instance of '{type}': Type doesn't exist", "type");

            TsObject[] ctorArgs = null;
            if (args.Length > 1)
            {
                ctorArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, ctorArgs, 0, args.Length - 1);
            }
            return new TsObject(ctor(ctorArgs));
        }

        [TaffyScriptMethod]
        public static TsObject instance_get_name(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetInstance().ObjectType;
        }

        [TaffyScriptMethod]
        public static TsObject instance_get_parent(ITsInstance inst, TsObject[] args)
        {
            if (TsReflection.Inherits.TryGetValue(args[0].GetInstance().ObjectType, out var parent))
                return parent;
            return "";
        }

        [TaffyScriptMethod]
        public static TsObject instance_is(ITsInstance inst, TsObject[] args)
        {
            var type = args[0].GetInstance().ObjectType;
            var expectedType = (string)args[1];

            do
            {
                if (type == expectedType)
                    return true;
            }
            while (TsReflection.Inherits.TryGetValue(type, out type));
            return false;
        }

        public static TsObject is_array(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Array;
        }

        public static TsObject is_instance(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Instance;
        }

        public static TsObject is_null(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Null;
        }

        public static TsObject is_number(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Real;
        }

        public static TsObject is_script(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Delegate;
        }

        public static TsObject is_string(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.String;
        }

        public static TsObject object_is_ancestor(ITsInstance inst, TsObject[] args)
        {
            var type = (string)args[1];
            var parent = (string)args[0];

            while (TsReflection.Inherits.TryGetValue(type, out type))
            {
                if (type == parent)
                    return true;
            }

            return false;
        }

        [TaffyScriptMethod]
        public static TsObject script_exists(ITsInstance inst, TsObject[] args)
        {
            return TsReflection.GlobalScripts.ContainsKey((string)args[0]);
        }

        public static TsObject variable_global_exists(ITsInstance inst, TsObject[] args)
        {
            return TsInstance.Global._members.ContainsKey((string)args[0]);
        }

        public static TsObject variable_global_get(ITsInstance inst, TsObject[] args)
        {
            return TsInstance.Global._members.TryGetValue((string)args[0], out var result) ? result : TsObject.Empty();
        }

        public static TsObject variable_global_get_names(ITsInstance inst, TsObject[] args)
        {
            var arr = new TsObject[TsInstance.Global._members.Count];
            int i = 0;
            foreach (var key in TsInstance.Global._members.Keys)
                arr[i++] = key;

            return arr;
        }

        public static TsObject variable_global_set(ITsInstance inst, TsObject[] args)
        {
            TsInstance.Global._members[(string)args[0]] = args[1];
            return TsObject.Empty();
        }

        public static TsObject variable_instance_exists(ITsInstance inst, TsObject[] args)
        {
            var target = args[0].GetInstance();
            var name = args[1].GetString();
            switch (target)
            {
                case TsInstance ts:
                    return ts._members.ContainsKey(name);
                case DynamicInstance di:
                    return di._members.ContainsKey(name);
                default:
                    try
                    {
                        target.GetMember(name);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
            }
        }

        public static TsObject variable_instance_get(ITsInstance inst, TsObject[] args)
        {
            return args[0].GetInstance().GetMember((string)args[1]);
        }

        public static TsObject variable_instance_get_names(ITsInstance inst, TsObject[] args)
        {
            TsObject[] arr;
            int i;
            switch (args[0].GetInstance())
            {
                case TsInstance ts:
                    arr = new TsObject[ts._members.Count];
                    i = 0;
                    foreach (var key in ts._members.Keys)
                        arr[i++] = key;

                    return arr;
                case DynamicInstance di:
                    arr = new TsObject[di._members.Count];
                    i = 0;
                    foreach (var key in di._members.Keys)
                        arr[i++] = key;

                    return arr;
                default:
                    throw new InvalidOperationException($"Could not get variable names from instance of type {args[0].GetInstance().ObjectType}");
            }
        }

        public static TsObject variable_instance_set(ITsInstance inst, TsObject[] args)
        {
            args[0].GetInstance().SetMember((string)args[1], args[2]);
            return TsObject.Empty();
        }
    }
}
