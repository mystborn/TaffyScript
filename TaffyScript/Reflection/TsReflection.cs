using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript.Reflection
{
    public static class TsReflection
    {
        /// <summary>
        /// Gets a list of all of the types defined by the loaded TaffyScript assemblies.
        /// </summary>
        public static List<string> Types { get; } = new List<string>();

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
            if(definition.Parent != null)
                Inherits.Add(definition.Name, definition.Parent);
            Constructors.Add(definition.Name, definition.Create);
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

        public static bool ObjectIs(string type, string expectedType)
        {
            do
            {
                if (type == expectedType)
                    return true;
            }
            while (Inherits.TryGetValue(type, out type));
            return false;
        }
    }
}
