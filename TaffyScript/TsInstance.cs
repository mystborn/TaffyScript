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
    /// Represents an instance of an object in TaffyScript.
    /// </summary>
    public class TsInstance : ITsInstance
    {
        private const string CreateEvent = "create";

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
        public static LookupTable<string, string, TsDelegate> InstanceScripts { get; } = new LookupTable<string, string, TsDelegate>();

        /// <summary>
        /// Used to get the implementation of a script from it's name (which must include it's namespace).
        /// </summary>
        public static Dictionary<string, TsDelegate> GlobalScripts { get; } = new Dictionary<string, TsDelegate>();

        /// <summary>
        /// Used to get the parent type of an object type. The object name must include its namespace.
        /// </summary>
        public static Dictionary<string, string> Inherits { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Maps imported types to a func that will construct an instance of the type.
        /// </summary>
        public static Dictionary<string, Func<TsObject[], ITsInstance>> WrappedConstructors { get; } = new Dictionary<string, Func<TsObject[], ITsInstance>>();

        /// <summary>
        /// Gets the isntance used to store global variables.
        /// </summary>
        public static TsInstance Global = InitGlobal();
        
        /// <summary>
        /// References the target of the parent scope in a with block.
        /// </summary>
        public static ITsInstance Other { get; set; }

        /// <summary>
        /// Gets or sets a value based on a variable name.
        /// </summary>
        /// <param name="variableName">The name of the variable</param>
        /// <returns></returns>
        /// <remarks>
        /// During code generation, the backing methods of this indexer are referred to explicitly.
        /// We make sure that it will always have the name of Item so that compiler implementations
        /// give it the correct name.
        /// </remarks>
        [System.Runtime.CompilerServices.IndexerName("Item")]
        public TsObject this[string variableName]
        {
            get => GetMember(variableName);
            set => _vars[variableName] = value;
        }

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
        /// <param name="args">Any arguments passed to the create event.</param>
        public TsInstance(string instanceType, params TsObject[] args)
        {
            ObjectType = instanceType;
            Init(true, args);
        }

        private TsInstance(string instanceType, bool performEvent = true, params TsObject[] args)
        {
            // Originally, all of the instance events were added onto the object as strings.
            // However at this point in time, I've decided that it's too much of a time sink.
            // This decision is easily reversable if it turns out to be wrong/unneeded.
            // In the meantime, you can still refer to the event by it's string representation.
            
            ObjectType = instanceType;
            Init(performEvent, args);
        }

        /// <summary>
        /// Creates a new Global instance.
        /// </summary>
        private TsInstance()
        {
            ObjectType = "";
        }

        private void Init(bool performEvent, params TsObject[] args)
        {
            if (Inherits.TryGetValue(ObjectType, out var parent))
                Parent = parent;
            else
                Parent = null;

            if (performEvent && TryGetDelegate(CreateEvent, out var create))
                create.Script(this, args);
        }

        /// <summary>
        /// Gets an event defined by this instance.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="del">If found, the event.</param>
        /// <returns>True if found, false otherwise.</returns>
        public bool TryGetDelegate(string name, out TsDelegate del)
        {
            if (_vars.TryGetValue(name, out var wrapper) && wrapper.Type == VariableType.Delegate)
            {
                del = wrapper.GetDelegateUnchecked();
                return true;
            }

            return InternalTryGetDelegate(name, out del);
        }

        private bool InternalTryGetDelegate(string name, out TsDelegate del)
        {
            var type = ObjectType;
            do
            {
                if (InstanceScripts.TryGetValue(type, name, out del))
                {
                    //If the script is inherited, cache it in the child's script lookup.
                    if (type != ObjectType)
                        InstanceScripts.Add(ObjectType, name, del);

                    del = new TsDelegate(del, this);

                    if (!_vars.ContainsKey(name))
                        _vars[name] = new TsObject(del);

                    return true;
                }
                Inherits.TryGetValue(type, out type);
            }
            while (type != null);

            del = null;
            return false;
        }

        /// <summary>
        /// Gets an event defined by this instance.
        /// </summary>
        /// <param name="name">The name of the event</param>
        /// <returns></returns>
        public TsDelegate GetDelegate(string name)
        {
            if (!TryGetDelegate(name, out var result))
                throw new ArgumentException($"Type {ObjectType} does not define event {name}");
            return result;
        }

        /// <summary>
        /// Calls a script defined or assigned to the instance.
        /// </summary>
        /// <param name="scriptName">The name of the script to call.</param>
        /// <param name="args">Any arguments to pass to the script.</param>
        /// <returns>Script result.</returns>
        public TsObject Call(string scriptName, params TsObject[] args)
        {
            return GetDelegate(scriptName).Invoke(args);
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
            if (!_vars.TryGetValue(name, out value))
            {
                if (InternalTryGetDelegate(name, out var del))
                {
                    value = new TsObject(del);
                    _vars[name] = value;
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the value of a variable from this instance.
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TsObject GetMember(string name)
        {
            if (TryGetVariable(name, out var result))
                return result;
            else
                throw new MissingFieldException($"Tried to access non-existant variable {name} on object of type {ObjectType}");
        }

        public void SetMember(string name, TsObject value)
        {
            _vars[name] = value;
        }

        /// <summary>
        /// Changes the type of this instance, optionally performing the create event of the new type.
        /// </summary>
        /// <param name="type">The type to change into</param>
        /// <param name="performEvents">Determines whether the events are performed</param>
        /// <param name="args">The arguments to pass to the create event if performEvents is true.</param>
        public void ChangeType(string type, bool performEvents, params TsObject[] args)
        {
            // Instances cache scripts after the first call to make executing them faster.
            // Here we need to make sure that the cached scripts get cleared from the cache.
            // Otherwise name conflicts will cause the od types script to be called in
            // certain circumstances.
            var events = InstanceScripts.GetRow(ObjectType);
            var values = _vars.ToArray();
            for(var i = 0; i < values.Length; i++)
            {
                if (values[i].Value.Type == VariableType.Delegate && events.TryGetValue(values[i].Key, out var del))
                {
                    var wrapper = values[i].Value.GetDelegateUnchecked();
                    if (wrapper.Target == this && wrapper.Script == del.Script)
                        _vars.Remove(values[i].Key);
                }
            }
            ObjectType = type;
            Init(performEvents, args);
        }

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        /// <param name="performEvents">Determines whether the create event is performed</param>
        /// <param name="args">The arguments to pass to the create event if performEvents is true.</param>
        /// <returns></returns>
        public TsInstance Copy(bool performEvents, params TsObject[] args)
        {
            var copy = new TsInstance(ObjectType, false);
            copy._vars = new Dictionary<string, TsObject>(_vars);
            if (performEvents && copy.TryGetDelegate(CreateEvent, out var create))
                create.Invoke(copy, args);

            return copy;
        }

        public override string ToString()
        {
            return ObjectType;
        }

        /// <summary>
        /// Gets an event defined by the given type.
        /// </summary>
        /// <param name="type">The type that defines the event</param>
        /// <param name="name">The name of the event.</param>
        /// <param name="del">If found, the event.</param>
        /// <returns>True if found, otherwise false.</returns>
        public static bool TryGetDelegate(string type, string name, out TsDelegate del)
        {
            if (type == null)
            {
                del = null;
                return false;
            }
            var origin = type;
            do
            {
                if (InstanceScripts.TryGetValue(type, name, out del))
                {
                    //If the script is inherited, cache it in the child's script lookup.
                    if (type != origin)
                        InstanceScripts.Add(origin, name, del);
                    return true;
                }
            }
            while (Inherits.TryGetValue(type, out type));

            del = null;
            return false;
        }

        public static TsDelegate GetDelegate(string type, string name)
        {
            var origin = type;
            TsDelegate result;
            do
            {
                if (InstanceScripts.TryGetValue(type, name, out result))
                {
                    if (type != origin)
                        InstanceScripts.Add(origin, name, result);
                    return result;
                }
            }
            while (Inherits.TryGetValue(type, out type));
            throw new ArgumentException($"Type {type} does not define the script {name}.");
        }

        /// <summary>
        /// Changes the currently executing instance into a new type
        /// </summary>
        /// <param name="newObj">The name of the new type</param>
        /// <param name="performEvents">Determines whether or not to perform the destroy and create events when changing.</param>
        [WeakMethod]
        public static TsObject InstanceChange(ITsInstance inst, TsObject[] args)
        {
            switch(args.Length)
            {
                case 0:
                case 1:
                    throw new ArgumentOutOfRangeException("At least two argument must be passed to InstanceChange.");
                case 2:
                    ((TsInstance)args[0]).ChangeType((string)args[1], false);
                    break;
                case 3:
                    ((TsInstance)args[0]).ChangeType((string)args[1], (bool)args[2]);
                    break;
                default:
                    var copy = new TsObject[args.Length - 3];
                    Array.Copy(args, 3, copy, 0, copy.Length);
                    ((TsInstance)args[0]).ChangeType((string)args[1], (bool)args[2], copy);
                    break;

            }
            return args[0];
        }

        /// <summary>
        /// Copies the currently executing instance and returns the copy.
        /// </summary>
        /// <param name="performEvents">Determines whther or not to perform the create event on the copy.</param>
        /// <returns></returns>
        [WeakMethod]
        public static TsObject InstanceCopy(ITsInstance inst, TsObject[] args)
        {
            switch(args.Length)
            {
                case 0:
                    throw new ArgumentOutOfRangeException("At least one arguments must be passed to InstanceCopy.");
                case 1:
                    return ((TsInstance)args[0]).Copy(false);
                case 2:
                    return ((TsInstance)args[0]).Copy((bool)args[1]);
                default:
                    var copy = new TsObject[args.Length - 2];
                    Array.Copy(args, 2, copy, 0, copy.Length);
                    return ((TsInstance)args[0]).Copy((bool)args[1], copy);
            }
        }

        /// <summary>
        /// Creates a new TS instance. Should not be called directly.
        /// </summary>
        /// <param name="target">Currently executing instance if any.</param>
        /// <param name="args">Arguments used to call this method.
        /// <para>
        /// The first argument should be a string of the object name. Anything else will be passed to the ctor.
        /// </para>
        /// </param>
        /// <returns></returns>
        [WeakMethod]
        public static TsObject InstanceCreate(ITsInstance target, TsObject[] args)
        {
            var type = (string)args[0];
            TsObject[] ctorArgs = null;
            if(args.Length > 1)
            {
                ctorArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, ctorArgs, 0, args.Length - 1);
            }

            if (WrappedConstructors.TryGetValue(type, out var ctor))
                return new TsObject(ctor(ctorArgs));
            else
                return new TsInstance(type, ctorArgs);
        }

        /// <summary>
        /// Gets the object type of an instance.
        /// </summary>
        /// <param name="inst">Instance to get the type from.</param>
        /// <returns></returns>
        public static string ObjectGetName(ITsInstance inst)
        {
            return inst.ObjectType;
        }

        /// <summary>
        /// Gets the parent of the specified instance.
        /// </summary>
        /// <param name="inst">Instance to get the parent type from.</param>
        /// <returns></returns>
        public static string ObjectGetParent(ITsInstance inst)
        {
            if (inst is TsInstance ts)
                return ts.Parent ?? "";
            /*if (Inherits.TryGetValue(inst.ObjectType, out var parent))
                return parent;*/
            else
                return "";
        }

        /// <summary>
        /// Determines if an object type inherits from a parent type.
        /// </summary>
        /// <param name="obj">The object type.</param>
        /// <param name="par">The parent type</param>
        /// <returns></returns>
        public static bool ObjectIsAncestor(string par, string obj)
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
            foreach (var key in Global._vars.Keys)
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
            Global._vars[name] = value;
        }

        /// <summary>
        /// Determines if an instance has a variable with the given name
        /// </summary>
        /// <param name="inst">The instance to check</param>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public static bool VariableInstanceExists(ITsInstance inst, string name)
        {
            if (inst is TsInstance ts)
                return ts._vars.ContainsKey(name);
            else
                throw new NotImplementedException("Currently imported types don't support determining if a variable exists.");
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
            if(inst is TsInstance ts)
            {
                var arr = new TsObject[ts._vars.Count];
                var i = 0;
                foreach (var key in ts._vars.Keys)
                    arr[i++] = key;

                return arr;
            }
            else
            {
                throw new NotImplementedException("Currenyly imported types don't support getting the names of variables.");
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

        private static TsInstance InitGlobal()
        {
            return new TsInstance();
        }
    }
}
