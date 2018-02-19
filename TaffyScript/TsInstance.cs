using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public delegate void InstanceEvent(TsInstance inst);
    public delegate TsObject TaffyFunction(TsObject[] args);

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

        public static Dictionary<Type, string> ObjectIndexMapping = new Dictionary<Type, string>();
        public static Dictionary<string, Dictionary<string, InstanceEvent>> Events = new Dictionary<string, Dictionary<string, InstanceEvent>>();
        public static Dictionary<string, TaffyFunction> Functions = new Dictionary<string, TaffyFunction>();
        public static Dictionary<string, string> Inherits = new Dictionary<string, string>();
        public static Stack<string> EventType = new Stack<string>();
        public static TsInstance Global = InitGlobal();

        public TsObject this[string variableName]
        {
            get => _vars[variableName];
            set => _vars[variableName] = value;
        }

        public float Id { get; }
        public string ObjectType { get; private set; }
        public string Parent { get; private set; }

        private TsInstance(float id, string instanceType, bool performEvent = true)
        {
            // Originally, all of the instance events were added onto the object as strings.
            // However at this point in time, I've decided that it's too much of a time sink.
            // This decision is easily reversable if it turns out to be wrong/unneeded.
            // In the meantime, you can still refer to the event by it's string representation.

            Id = id;
            ObjectType = instanceType;
            Pool.Add(id, this);
            Init(performEvent);
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

        private bool TryGetEvent(string name, out InstanceEvent instanceEvent)
        {
            var inst = ObjectType;
            do
            {
                if (Events.TryGetValue(inst, out var events) && events.TryGetValue(name, out instanceEvent))
                {
                    if (inst != ObjectType)
                    {
                        if (!Events.TryGetValue(ObjectType, out events))
                        {
                            events = new Dictionary<string, InstanceEvent>();
                            Events.Add(ObjectType, events);
                        }
                        events.Add(name, instanceEvent);
                    }
                    return true;
                }
                Inherits.TryGetValue(inst, out inst);
            }
            while (inst != null);

            instanceEvent = null;
            return false;
        }

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
                if (Events.TryGetValue(type, out var events) && events.TryGetValue(name, out instanceEvent))
                {
                    if (type != origin)
                    {
                        if (!Events.TryGetValue(origin, out events))
                        {
                            events = new Dictionary<string, InstanceEvent>();
                            Events.Add(origin, events);
                        }
                        events.Add(name, instanceEvent);
                    }
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

        public static void InstanceChange(string newObj, bool performEvents)
        {
            var id = TsObject.Id.Peek();
            Pool.TryGetValue(id.GetNum(), out var inst);
            if (performEvents && inst.TryGetEvent(DestroyEvent, out var destroy))
                destroy(inst);

            inst.ObjectType = newObj;
            inst.Init(performEvents);
        }

        public static float InstanceCopy(bool performEvents)
        {
            var id = TsObject.Id.Peek();
            Pool.TryGetValue(id.GetNum(), out var inst);
            var result = GetNext();
            var next = new TsInstance(result, inst.ObjectType, false);
            next._vars = new Dictionary<string, TsObject>(inst._vars);
            next.Init(performEvents);
            return result;
        }

        public static TsObject InstanceCreate(string instanceType)
        {
            var id = GetNext();

            new TsInstance(id, instanceType);

            return new TsObject(id);
        }

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

        public static bool InstanceExists(float id)
        {
            return Pool.ContainsKey(id);
        }

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

        public static int InstanceNumber(string obj)
        {
            return Instances(obj).Count();
        }

        public static string ObjectGetName(TsInstance inst)
        {
            return inst.ObjectType;
        }

        public static string ObjectGetParent(TsInstance inst)
        {
            if (Inherits.TryGetValue(inst.ObjectType, out var parent))
                return parent;
            else
                return "";
        }

        public static bool ObjectIsAncestor(string obj, string par)
        {
            if (Inherits.TryGetValue(obj, out var test))
                return test == par;

            return false;
        }

        public static bool TryGet(float id, out TsInstance inst)
        {
            return Pool.TryGetValue(id, out inst);
        }

        public static IEnumerable<TsObject> Instances()
        {
            foreach (var inst in Pool.Keys)
            {
                yield return new TsObject(inst);
            }
        }

        public static IEnumerable<TsObject> Instances(string type)
        {
            foreach (var inst in Pool.Values)
                if (inst.ObjectType == type)
                    yield return new TsObject(inst.Id);
        }

        internal static TsInstance InitGlobal()
        {
            var global = new TsInstance(-5f, "");
            Pool.Remove(-5f);
            return global;
        }
    }
}
