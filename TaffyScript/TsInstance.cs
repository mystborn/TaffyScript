using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public delegate void InstanceEvent(TsInstance inst);
    public delegate TsObject TaffyFunction(TsObject[] args);

    public class TsInstance
    {
        const float Start = 100000f;

        private static Dictionary<float, TsInstance> Pool { get; } = new Dictionary<float, TsInstance>();
        private static Queue<float> _availableIds { get; } = new Queue<float>();

        private Dictionary<string, TsObject> _vars = new Dictionary<string, TsObject>();

        public static Dictionary<Type, string> ObjectIndexMapping = new Dictionary<Type, string>();
        public static Dictionary<string, Dictionary<string, InstanceEvent>> Events = new Dictionary<string, Dictionary<string, InstanceEvent>>();
        public static Dictionary<string, TaffyFunction> Functions = new Dictionary<string, TaffyFunction>();

        public TsObject this[string variableName]
        {
            get => _vars[variableName];
            set => _vars[variableName] = value;
        }

        public float Id { get; }
        public string ObjectType { get; }

        private TsInstance(float id, string instanceType)
        {
            Id = id;
            ObjectType = instanceType;
            Pool.Add(id, this);
            if(Events.TryGetValue(instanceType, out var events))
            {
                foreach (var ev in events.Keys)
                    _vars.Add(ev, new TsObject(ev));

                if (events.TryGetValue("create", out var create))
                    create(this);
            }
        }

        public static TsObject InstanceCreate(string instanceType)
        {
            float id;
            if (_availableIds.Count == 0)
                id = Pool.Count + Start;
            else
                id = _availableIds.Dequeue();

            new TsInstance(id, instanceType);

            return new TsObject(id);
        }

        public static void InstanceDestroy(float id)
        {
            Pool.Remove(id);
            if (Pool.TryGetValue(id, out var inst) && Events.TryGetValue(inst.ObjectType, out var events) && events.TryGetValue("destroy", out var destroy))
            {
                destroy(inst);
            }
            _availableIds.Enqueue(id);
        }

        public static bool InstanceExists(float id)
        {
            return Pool.ContainsKey(id);
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

        internal static TsObject InitGlobal()
        {
            var global = new TsInstance(-5f, "");
            return new TsObject(-5f);
        }
    }
}
