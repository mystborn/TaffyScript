using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript
{
    public class TsReflection
    {
        /// <summary>
        /// Gets a list of all of the types defined by the loaded TaffyScript assemblies.
        /// </summary>
        public static List<string> Types { get; } = new List<string>();

        /// <summary>
        /// Used to find and call the events defined by the loaded TaffyScript assemblies.
        /// <para>
        /// Row = ObjectType (including namespace),  Column = EventName
        /// </para>
        /// </summary>
        public static LookupTable<string, string, TsDelegate> InstanceScripts { get; } = new LookupTable<string, string, TsDelegate>();

        /// <summary>
        /// Used to get the implementation of a script from it's name (which must include it's namespace).
        /// </summary>
        public static Dictionary<string, TsDelegate> GlobalScripts { get; } = new Dictionary<string, TsDelegate>();

        /// <summary>
        /// Used to get the parent type of an object type. The object name must include its namespace.
        /// </summary>
        public static Dictionary<string, string> Inherits { get; } = new Dictionary<string, string>();

        public static Dictionary<string, Func<TsObject[], ITsInstance>> Constructors { get; } = new Dictionary<string, Func<TsObject[], ITsInstance>>();

        public static void ProcessObjectDefinition(ObjectDefinition definition)
        {
            Types.Add(definition.Name);
            if (definition.Parent != null)
                Inherits.Add(definition.Name, definition.Parent);
            InstanceScripts.AddRow(definition.Name, definition.Scripts);
        }

        public static bool TryGetScript(string typeName, string scriptName, out TsDelegate del)
        {
            if (typeName == null)
            {
                del = null;
                return false;
            }
            var origin = typeName;
            do
            {
                if (InstanceScripts.TryGetValue(typeName, scriptName, out del))
                {
                    //If the script is inherited, cache it in the child's script lookup.
                    if (typeName != origin)
                        InstanceScripts.Add(origin, scriptName, del);
                    return true;
                }
            }
            while (Inherits.TryGetValue(typeName, out typeName));

            del = null;
            return false;
        }

        [WeakMethod]
        public static TsObject InstanceCreate(ITsInstance inst, TsObject[] args)
        {
            var type = (string)args[0];
            if (!Constructors.TryGetValue(type, out var ctor))
                throw new ArgumentException($"Cannot create instance of '{type}': Type doesn't exist", "type");

            TsObject[] ctorArgs = null;
            if (args.Length > 1)
            {
                ctorArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, ctorArgs, 0, args.Length - 1);
            }
            return new TsObject(ctor(ctorArgs));
        }

        public static string ObjectGetName(ITsInstance inst)
        {
            return inst.ObjectType;
        }

        public static string ObjectGetParent(ITsInstance inst)
        {
            if (Inherits.TryGetValue(inst.ObjectType, out var parent))
                return parent;
            return "";
        }

        public static bool ObjectIsAncestor(string parent, string type)
        {
            while(Inherits.TryGetValue(type, out type))
            {
                if (type == parent)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if a global variable with the specified name exists.
        /// </summary>
        /// <param name="name">The name of the global variable</param>
        /// <returns></returns>
        public static bool VariableGlobalExists(string name)
        {
            return TsInstanceTemp.Global._members.ContainsKey(name);
        }

        /// <summary>
        /// Gets the value of a global variable.
        /// </summary>
        /// <param name="name">The name of the gloable variable</param>
        /// <returns></returns>
        public static TsObject VariableGlobalGet(string name)
        {
            if (TsInstanceTemp.Global._members.TryGetValue(name, out var result))
                return result;
            return TsObject.Empty();
        }

        /// <summary>
        /// Gets the names of all the global variables.
        /// </summary>
        /// <returns></returns>
        public static TsObject[] VariableGlobalGetNames()
        {
            var arr = new TsObject[TsInstanceTemp.Global._members.Count];
            int i = 0;
            foreach (var key in TsInstanceTemp.Global._members.Keys)
                arr[i++] = key;

            return arr;
        }

        /// <summary>
        /// Sets the value of a global variable.
        /// </summary>
        /// <param name="name">The name of the global variable</param>
        /// <param name="value">The value to set</param>
        public static void VariableGlobalSet(string name, TsObject value)
        {
            TsInstanceTemp.Global._members[name] = value;
        }

        /// <summary>
        /// Determines if an instance has a variable with the given name
        /// </summary>
        /// <param name="inst">The instance to check</param>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public static bool VariableInstanceExists(ITsInstance inst, string name)
        {
            switch(inst)
            {
                case TsInstanceTemp ts:
                    return ts._members.ContainsKey(name);
                case DynamicInstance di:
                    return di._members.ContainsKey(name);
                default:
                    try
                    {
                        inst.GetMember(name);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// Gets the value of an instance variable.
        /// </summary>
        /// <param name="inst">The instance to get the value from</param>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public static TsObject VariableInstanceGet(ITsInstance inst, string name)
        {
            return inst.GetMember(name);
        }

        /// <summary>
        /// Gets the names of all of the variables defined by an instance.
        /// </summary>
        /// <param name="inst">The instance to get the variable names from</param>
        /// <returns></returns>
        public static TsObject[] VariableInstanceGetNames(ITsInstance inst)
        {
            TsObject[] arr;
            int i;
            switch(inst)
            {
                case TsInstanceTemp ts:
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
                    throw new InvalidOperationException($"Could not get variable names from instance of type {inst.ObjectType}");
            }
        }

        /// <summary>
        /// Sets a variable on an instance
        /// </summary>
        /// <param name="inst">The instance to set the variable on</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public static void VariableInstanceSet(ITsInstance inst, string name, TsObject value)
        {
            inst.SetMember(name, value);
        }
    }
}
