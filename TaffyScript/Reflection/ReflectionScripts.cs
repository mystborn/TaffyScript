using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace TaffyScript.Reflection
{
    /// <summary>
    /// Provides scripts to dynamically retrieve information about loaded assemblies.
    /// </summary>
    [TaffyScriptBaseType]
    public static class ReflectionScripts
    {
        private static Dictionary<Type, Func<TsObject[], ITsInstance>> _constructors = new Dictionary<Type, Func<TsObject[], ITsInstance>>();

        [TaffyScriptMethod]
        public static TsObject call_global_script(TsObject[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("You must pass at least a script name to script_execute.");
            var name = args[0].GetString();
            if (!TsReflection.GlobalScripts.TryGetValue(name, out var function))
                throw new ArgumentException($"Tried to execute a non-existant function: {name}");
            var parameters = new TsObject[args.Length - 1];
            if (parameters.Length != 0)
                Array.Copy(args, 1, parameters, 0, parameters.Length);
            return function.Invoke(parameters);
        }

        [TaffyScriptMethod]
        public static TsObject call_instance_script(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    return args[0].GetInstance().Call((string)args[1]);
                default:
                    var scriptArgs = new TsObject[args.Length - 2];
                    Array.Copy(args, 2, scriptArgs, 0, scriptArgs.Length);
                    return args[0].GetInstance().Call((string)args[1], scriptArgs);
            }
        }

        [TaffyScriptMethod]
        public static TsObject instance_create(TsObject[] args)
        {
            var type = ((TsType)args[0]).Source;
            if (!typeof(ITsInstance).IsAssignableFrom(type))
                throw new ArgumentException($"Tried to create an instance of non-TaffyScript type '{type.Name}'");

            if(!_constructors.TryGetValue(type, out var constructor))
            {
                var ctor = type.GetConstructor(new[] { typeof(TsObject[]) });
                var create = new DynamicMethod($"<>create_{type.Name}", typeof(ITsInstance), new[] { typeof(TsObject[]) });
                var cgen = create.GetILGenerator();
                cgen.Emit(OpCodes.Ldarg_0);
                cgen.Emit(OpCodes.Newobj, ctor);
                cgen.Emit(OpCodes.Ret);
                constructor = (Func<TsObject[], ITsInstance>)create.CreateDelegate(typeof(Func<TsObject[], ITsInstance>));
                _constructors.Add(type, constructor);
            }

            TsObject[] ctorArgs = null;
            if (args.Length > 1)
            {
                ctorArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, ctorArgs, 0, args.Length - 1);
            }
            return new TsInstanceWrapper(constructor(ctorArgs));
        }

        [TaffyScriptMethod]
        public static TsObject instance_is(TsObject[] args)
        {
            return ((TsType)args[1]).Source.IsAssignableFrom(args[0].WeakValue.GetType());
        }

        public static TsObject is_array(TsObject[] args)
        {
            return args[0].Type == VariableType.Array;
        }

        public static TsObject is_instance(TsObject[] args)
        {
            return args[0].Type == VariableType.Instance;
        }

        public static TsObject is_null(TsObject[] args)
        {
            return args[0].Type == VariableType.Null;
        }

        public static TsObject is_number(TsObject[] args)
        {
            return args[0].Type == VariableType.Real;
        }

        public static TsObject is_script(TsObject[] args)
        {
            return args[0].Type == VariableType.Delegate;
        }

        public static TsObject is_string(TsObject[] args)
        {
            return args[0].Type == VariableType.String;
        }

        [TaffyScriptMethod]
        public static TsObject script_exists(TsObject[] args)
        {
            return TsReflection.GlobalScripts.ContainsKey((string)args[0]);
        }

        public static TsObject variable_global_exists(TsObject[] args)
        {
            return TsInstance.Global._members.ContainsKey((string)args[0]);
        }

        public static TsObject variable_global_get(TsObject[] args)
        {
            return TsInstance.Global._members.TryGetValue((string)args[0], out var result) ? result : TsObject.Empty;
        }

        public static TsObject variable_global_get_names(TsObject[] args)
        {
            var arr = new TsObject[TsInstance.Global._members.Count];
            int i = 0;
            foreach (var key in TsInstance.Global._members.Keys)
                arr[i++] = key;

            return arr;
        }

        public static TsObject variable_global_set(TsObject[] args)
        {
            TsInstance.Global._members[(string)args[0]] = args[1];
            return TsObject.Empty;
        }

        public static TsObject variable_instance_exists(TsObject[] args)
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

        public static TsObject variable_instance_get(TsObject[] args)
        {
            return args[0].GetInstance().GetMember((string)args[1]);
        }

        public static TsObject variable_instance_get_names(TsObject[] args)
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

        public static TsObject variable_instance_set(TsObject[] args)
        {
            args[0].GetInstance().SetMember((string)args[1], args[2]);
            return TsObject.Empty;
        }
    }
}
