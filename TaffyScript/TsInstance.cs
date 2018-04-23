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
    /// Delegate used to represent methods to be triggered when a TS instance is destroyed.
    /// </summary>
    /// <param name="inst">The instance that was destroyed.</param>
    public delegate void DestroyedDelegate(TsInstance inst);

    /// <summary>
    /// Represents an instance of an object in TaffyScript.
    /// </summary>
    public class TsInstance : ITsInstance
    {
        private const string CreateEvent = "create";
        private const string DestroyEvent = "destroy";

        /// <summary>
        /// Event that gets triggered when this instance is destroyed.
        /// </summary>
        public event DestroyedDelegate Destroyed;

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
        /// Gets a stack trace of the currently executing events.
        /// </summary>
        public static Stack<string> EventType { get; } = new Stack<string>();

        /// <summary>
        /// Gets the isntance used to store global variables.
        /// </summary>
        public static TsInstance Global = InitGlobal();
        
        /// <summary>
        /// References the target of the parent scope in a with block.
        /// </summary>
        public static TsInstance Other { get; set; }

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
        /// Gets the id of this instance.
        /// </summary>
        public int Id { get; }

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
#if KeepRef
            References.Register(this);
#endif
            ObjectType = instanceType;
            Init(true, args);
        }

        private TsInstance(string instanceType, bool performEvent = true, params TsObject[] args)
        {
            // Originally, all of the instance events were added onto the object as strings.
            // However at this point in time, I've decided that it's too much of a time sink.
            // This decision is easily reversable if it turns out to be wrong/unneeded.
            // In the meantime, you can still refer to the event by it's string representation.

#if KeepRef
            References.Register(this);
#endif
            ObjectType = instanceType;
            Init(performEvent, args);
        }

        /// <summary>
        /// Creates a new Global instance.
        /// </summary>
        private TsInstance()
        {
            Id = -5;
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
        /// Changes the type of this instance, optionally performing the destroy and create events.
        /// </summary>
        /// <param name="type">The type to change into</param>
        /// <param name="performEvents">Determines whether the events are performed</param>
        public void ChangeType(string type, bool performEvents)
        {
            if (performEvents && TryGetDelegate(DestroyEvent, out var destroy))
                destroy.Invoke(this, null);

            ObjectType = type;
            Init(performEvents);
        }

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        /// <param name="performEvents">Determines whether the create event is performed</param>
        /// <returns></returns>
        public TsInstance Copy(bool performEvents)
        {
            var copy = new TsInstance(ObjectType, false);
            copy._vars = new Dictionary<string, TsObject>(_vars);
            if (performEvents && TryGetDelegate(ObjectType, out var create))
                create.Invoke(copy, null);

            return copy;
        }

        /// <summary>
        /// Destroys this instance in the eyes of TaffyScript
        /// </summary>
        /// <remarks>
        /// Make this object inherit from IDisposable, change this to Dispose method?
        /// </remarks>
        public void Destroy()
        {
            if (TryGetDelegate(DestroyEvent, out var destroy))
                destroy.Invoke(this, null);
            Destroyed?.Invoke(this);
        }

        public override string ToString()
        {
            return $"{ObjectType} {Id}";
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return obj is TsInstance inst && inst.Id == Id;
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
                Inherits.TryGetValue(type, out type);
            }
            while (type != null);

            del = null;
            return false;
        }

        /// <summary>
        /// Changes the currently executing instance into a new type
        /// </summary>
        /// <param name="newObj">The name of the new type</param>
        /// <param name="performEvents">Determines whether or not to perform the destroy and create events when changing.</param>
        [WeakMethod]
        public static TsObject InstanceChange(ITsInstance inst, TsObject[] args)
        {
            ((TsInstance)inst).ChangeType((string)args[0], (bool)args[1]);
            return TsObject.Empty();
        }

        /// <summary>
        /// Copies the currently executing instance and returns the copy.
        /// </summary>
        /// <param name="performEvents">Determines whther or not to perform the create event on the copy.</param>
        /// <returns></returns>
        [WeakMethod]
        public static float InstanceCopy(ITsInstance inst, TsObject[] args)
        {
            return ((TsInstance)inst).Copy((bool)args[0]).Id;
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
            TsInstance inst;
            if (args.Length > 1)
            {
                var ctorArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, ctorArgs, 0, args.Length - 1);
                inst = new TsInstance((string)args[0], ctorArgs);
            }
            else
                inst = new TsInstance((string)args[0]);

            return new TsObject(inst);
        }

        /// <summary>
        /// Destroys a previously created instance. This overload should not be called.
        /// </summary>
        /// <param name="target">Currently executing instance if any.</param>
        /// <param name="args">Optionally contains the id of the instance.</param>
        [WeakMethod]
        public static TsObject InstanceDestroy(ITsInstance target, TsObject[] args)
        {
            if (args == null || args.Length == 0)
                target.Destroy();
            else
                args[0].GetInstance().Destroy();

            return TsObject.Empty();
        }

#if KeepRef
        /// <summary>
        /// Finds the nth occurence of the specified instance.
        /// </summary>
        /// <param name="obj">The object type to find</param>
        /// <param name="n">The index of the object</param>
        /// <returns></returns>
        public static TsObject InstanceFind(string obj, int n)
        {
            var i = 0;
            var iter = References.EnumerateType(obj);
            while(iter.MoveNext())
            {
                if (i++ == n)
                    return new TsObject(iter.Current);
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
            var i = 0;
            var iter = References.EnumerateType(obj);
            while(iter.MoveNext())
            {
                i++;
            }
            return i;
        }

#endif

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value of an instance variable.
        /// </summary>
        /// <param name="inst">The instance to get the value from</param>
        /// <param name="name">The name of the variable</param>
        /// <returns></returns>
        public static TsObject VariableInstanceGet(ITsInstance inst, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the names of all of the variables defined by an instance.
        /// </summary>
        /// <param name="inst">The instance to get the variable names from</param>
        /// <returns></returns>
        public static TsObject[] VariableInstanceGetNames(ITsInstance inst)
        {
            throw new NotImplementedException();
            /*var arr = new TsObject[inst._vars.Count];
            var i = 0;
            foreach (var key in inst._vars.Keys)
                arr[i++] = key;

            return arr;*/
        }

        /// <summary>
        /// Sets a variable on an instance
        /// </summary>
        /// <param name="inst">The instance to set the variable on</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public static void VariableInstanceSet(ITsInstance inst, string name, TsObject value)
        {
            throw new NotImplementedException();
            //inst._vars[name] = value;
        }

        private static TsInstance InitGlobal()
        {
            return new TsInstance();
        }
    }
}
