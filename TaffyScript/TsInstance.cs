using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;
using MethodImpl = System.Runtime.CompilerServices.MethodImplAttribute;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace TaffyScript
{
    /// <summary>
    /// Method signature of a TaffyScript event.
    /// </summary>
    /// <param name="inst">The target of the event</param>
    public delegate void InstanceEvent(TsInstance inst);

    /// <summary>
    /// Method signature of a TaffyScript script.
    /// </summary>
    /// <param name="args">The arguments used to call the script</param>
    public delegate TsObject Script(TsObject[] args);

    /// <summary>
    /// Represents an object instance in TaffyScript.
    /// </summary>
    public class TsInstance
    {
        private const float Start = 100000f;
        private const string CreateEvent = "create";
        private const string DestroyEvent = "destroy";

        // Eventually this should change to not remove object on instance_destroy.
        // When that happens, make sure to change the readonly id instance_count
        // to reflect the change.
        private static Dictionary<float, TsInstance> Pool = new Dictionary<float, TsInstance>();
        private static Queue<float> _availableIds = new Queue<float>();

        private Dictionary<string, TsObject> _vars = new Dictionary<string, TsObject>();

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
        public static LookupTable<string, string, InstanceEvent> Events { get; } = new LookupTable<string, string, InstanceEvent>();

        /// <summary>
        /// Used to get the implementation of a script from it's name (which must include it's namespace).
        /// </summary>
        public static Dictionary<string, Script> Scripts { get; } = new Dictionary<string, Script>();

        /// <summary>
        /// Used to get the parent type of an object type. The object name must include its namespace.
        /// </summary>
        public static Dictionary<string, string> Inherits { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets a stack trace of the currently executing events.
        /// </summary>
        public static Stack<string> EventType { get; } = new Stack<string>();

        /// <summary>
        /// Gets the isntance used to store global variables.
        /// </summary>
        public static TsInstance Global = InitGlobal();

        /// <summary>
        /// Gets or sets a value based on a variable name.
        /// </summary>
        /// <param name="variableName">The name of the variable</param>
        /// <returns></returns>
        public TsObject this[string variableName]
        {
            get => _vars[variableName];
            set => _vars[variableName] = value;
        }

        /// <summary>
        /// Gets the id of this instance.
        /// </summary>
        public float Id { get; }

        /// <summary>
        /// Gets the type of this instance.
        /// </summary>
        public string ObjectType { get; private set; }

        /// <summary>
        /// Gets the parent type of this instance.
        /// </summary>
        public string Parent { get; private set; }

        /// <summary>
        /// Creates a new instance of the specified type.
        /// </summary>
        /// <param name="instanceType">The type of the instance to create</param>
        public TsInstance(string instanceType)
        {
            Id = GetNext();
            ObjectType = instanceType;
            Pool.Add(Id, this);
            Init(true);
        }

        private TsInstance(string instanceType, bool performEvent = true)
        {
            // Originally, all of the instance events were added onto the object as strings.
            // However at this point in time, I've decided that it's too much of a time sink.
            // This decision is easily reversable if it turns out to be wrong/unneeded.
            // In the meantime, you can still refer to the event by it's string representation.

            Id = GetNext();
            ObjectType = instanceType;
            Pool.Add(Id, this);
            Init(performEvent);
        }

        /// <summary>
        /// Creates a new Global instance.
        /// </summary>
        private TsInstance()
        {
            Id = -5;
            ObjectType = "";
        }

        private void Init(bool performEvent)
        {
            if (Inherits.TryGetValue(ObjectType, out var parent))
                Parent = parent;
            else
                Parent = null;

            if (performEvent && TryGetEvent(CreateEvent, out var create))
                create(this);
        }

        /// <summary>
        /// Gets an event defined by this instance.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="instanceEvent">If found, the event.</param>
        /// <returns>True if found, false otherwise.</returns>
        public bool TryGetEvent(string name, out InstanceEvent instanceEvent)
        {
            var inst = ObjectType;
            do
            {
                if (Events.TryGetValue(inst, name, out instanceEvent))
                {
                    if (inst != ObjectType)
                        Events.Add(ObjectType, name, instanceEvent);
                    return true;
                }
                Inherits.TryGetValue(inst, out inst);
            }
            while (inst != null);

            instanceEvent = null;
            return false;
        }

        /// <summary>
        /// Gets an event defined by this instance.
        /// </summary>
        /// <param name="name">The name of the event</param>
        /// <returns></returns>
        public InstanceEvent GetEvent(string name)
        {
            if (!TryGetEvent(name, out var result))
                throw new ArgumentException($"Type {ObjectType} does not define event {name}");
            return result;
        }

        /// <summary>
        /// Gets the value of a variable from this instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetVariable(string name, out TsObject value)
        {
            return _vars.TryGetValue(name, out value);
        }

        /// <summary>
        /// Gets the value of a variable from this instance.
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public TsObject GetVariable(string name)
        {
            return _vars[name];
        }

        /// <summary>
        /// Gets an event defined by the given type.
        /// </summary>
        /// <param name="type">The type that defines the event</param>
        /// <param name="name">The name of the event.</param>
        /// <param name="instanceEvent">If found, the event.</param>
        /// <returns>True if found, otherwise false.</returns>
        public static bool TryGetEvent(string type, string name, out InstanceEvent instanceEvent)
        {
            if (type == null)
            {
                instanceEvent = null;
                return false;
            }
            var origin = type;
            do
            {
                if (Events.TryGetValue(type, name, out instanceEvent))
                {
                    if (type != origin)
                        Events.Add(origin, name, instanceEvent);
                    return true;
                }
                Inherits.TryGetValue(type, out type);
            }
            while (type != null);

            instanceEvent = null;
            return false;
        }

        private static float GetNext()
        {
            if (_availableIds.Count == 0)
                return Pool.Count + Start;
            else
                return _availableIds.Dequeue();
        }

        //Todo: All of the methods that start with Instance should be changed so a non static method implementation can be written to allow other libraries to use the method.

        /// <summary>
        /// Changes the currently executing instance into a new type
        /// </summary>
        /// <param name="newObj">The name of the new type</param>
        /// <param name="performEvents">Determines whether or not to perform the destroy and create events when changing.</param>
        public static void InstanceChange(string newObj, bool performEvents)
        {
            var id = TsObject.Id.Peek();
            Pool.TryGetValue(id.GetNum(), out var inst);
            if (performEvents && inst.TryGetEvent(DestroyEvent, out var destroy))
                destroy(inst);

            inst.ObjectType = newObj;
            inst.Init(performEvents);
        }

        /// <summary>
        /// Copies the currently executing instance and returns the copy.
        /// </summary>
        /// <param name="performEvents">Determines whther or not to perform the create event on the copy.</param>
        /// <returns></returns>
        public static float InstanceCopy(bool performEvents)
        {
            var id = TsObject.Id.Peek();
            Pool.TryGetValue(id.GetNum(), out var inst);
            var next = new TsInstance(inst.ObjectType, false);
            next._vars = new Dictionary<string, TsObject>(inst._vars);
            next.Init(performEvents);
            return next.Id;
        }

        /// <summary>
        /// Creates a new instance of the given type.
        /// </summary>
        /// <param name="instanceType">The name of the TaffyScript object type.</param>
        /// <returns></returns>
        public static TsObject InstanceCreate(string instanceType)
        {
            return new TsObject(new TsInstance(instanceType).Id);
        }

        /// <summary>
        /// Destroys a previously created instance.
        /// </summary>
        /// <param name="id">Instance id</param>
        public static void InstanceDestroy(float id)
        {
            if(Pool.TryGetValue(id, out var inst))
            {
                Pool.Remove(id);
                if (inst.TryGetEvent(DestroyEvent, out var destroy))
                    destroy(inst);
            }
            _availableIds.Enqueue(id);
        }

        /// <summary>
        /// Determines if an instance with the given id exists.
        /// </summary>
        /// <param name="id">Instance id</param>
        /// <returns></returns>
        public static bool InstanceExists(float id)
        {
            return Pool.ContainsKey(id);
        }

        /// <summary>
        /// Finds the nth occurence of the specified instance.
        /// </summary>
        /// <param name="obj">The object type to find</param>
        /// <param name="n">The index of the object</param>
        /// <returns></returns>
        public static TsObject InstanceFind(string obj, int n)
        {
            var i = 0;
            foreach(var inst in Instances(obj))
            {
                if (i++ == n)
                    return inst;
            }
            return TsObject.NooneObject();
        }

        /// <summary>
        /// Gets the number of instances of a given type.
        /// </summary>
        /// <param name="obj">The name of the object type</param>
        /// <returns></returns>
        public static int InstanceNumber(string obj)
        {
            return Instances(obj).Count();
        }

        /// <summary>
        /// Gets the object type of an instance.
        /// </summary>
        /// <param name="inst">Instance to get the type from.</param>
        /// <returns></returns>
        public static string ObjectGetName(TsInstance inst)
        {
            return inst.ObjectType;
        }

        /// <summary>
        /// Gets the parent of the specified instance.
        /// </summary>
        /// <param name="inst">Instance to get the parent type from.</param>
        /// <returns></returns>
        public static string ObjectGetParent(TsInstance inst)
        {
            if (Inherits.TryGetValue(inst.ObjectType, out var parent))
                return parent;
            else
                return "";
        }

        /// <summary>
        /// Determines if an object type inherits from a parent type.
        /// </summary>
        /// <param name="obj">The object type.</param>
        /// <param name="par">The parent type</param>
        /// <returns></returns>
        public static bool ObjectIsAncestor(string obj, string par)
        {
            while(Inherits.TryGetValue(obj, out var inherit))
            {
                if (inherit == par)
                    return true;
                obj = inherit;
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
            return Global._vars.ContainsKey(name);
        }

        /// <summary>
        /// Gets the value of a global variable.
        /// </summary>
        /// <param name="name">The name of the gloable variable</param>
        /// <returns></returns>
        public static TsObject VariableGlobalGet(string name)
        {
            if (Global._vars.TryGetValue(name, out var result))
                return result;
            return TsObject.Empty();
        }

        /// <summary>
        /// Gets the names of all the global variables.
        /// </summary>
        /// <returns></returns>
        public static TsObject[] VariableGlobalGetNames()
        {
            var arr = new TsObject[Global._vars.Count];
            int i = 0;
            foreach (var value in Global._vars.Values)
                arr[i++] = value;

            return arr;
        }

        /// <summary>
        /// Sets the value of a global variable.
        /// </summary>
        /// <param name="name">The name of the global variable</param>
        /// <param name="value">The value to set</param>
        public static void VariableGlobalSet(string name, TsObject value)
        {
            Global._vars[name] = value;
        }

        /// <summary>
        /// Determines if an instance has a variable with the given name
        /// </summary>
        /// <param name="inst">The instance to check</param>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public static bool VariableInstanceExists(TsInstance inst, string name)
        {
            return inst._vars.ContainsKey(name);
        }

        /// <summary>
        /// Gets the value of an instance variable.
        /// </summary>
        /// <param name="inst">The instance to get the value from</param>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public static TsObject VariableInstanceGet(TsInstance inst, string name)
        {
            if (inst._vars.TryGetValue(name, out var result))
                return result;

            return TsObject.Empty();
        }

        /// <summary>
        /// Gets the names of all of the variables defined by an instance.
        /// </summary>
        /// <param name="inst">The instance to get the variable names from</param>
        /// <returns></returns>
        public static TsObject[] VariableInstanceGetNames(TsInstance inst)
        {
            var arr = new TsObject[inst._vars.Count];
            var i = 0;
            foreach (var value in inst._vars.Values)
                arr[i++] = value;

            return arr;
        }

        /// <summary>
        /// Sets a variable on an instance
        /// </summary>
        /// <param name="inst">The instance to set the variable on</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public static void VariableInstanceSet(TsInstance inst, string name, TsObject value)
        {
            inst._vars[name] = value;
        }

        /// <summary>
        /// Attempts to get an instance from an id
        /// </summary>
        /// <param name="id">Instance id</param>
        /// <param name="inst">If it exists, the instance with the given id</param>
        /// <returns></returns>
        public static bool TryGetInstance(float id, out TsInstance inst)
        {
            return Pool.TryGetValue(id, out inst);
        }

        /// <summary>
        /// Gets a collection of all of the current instances
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TsObject> Instances()
        {
            foreach (var inst in Pool.Keys)
            {
                yield return new TsObject(inst);
            }
        }

        /// <summary>
        /// Gets a collection of all of the instances of a specified type.
        /// </summary>
        /// <param name="type">The type to get the instances of</param>
        /// <returns></returns>
        public static IEnumerable<TsObject> Instances(string type)
        {
            foreach (var inst in Pool.Values)
                if (inst.ObjectType == type || ObjectIsAncestor(inst.ObjectType, type))
                    yield return new TsObject(inst.Id);
        }

        internal static TsInstance InitGlobal()
        {
            return new TsInstance();
        }
    }
}
