using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public class GmInstance
    {
        const float Start = 100000f;

        private static Dictionary<float, GmInstance> Pool { get; } = new Dictionary<float, GmInstance>();
        private static Queue<float> _availableIds { get; } = new Queue<float>();
        
        private Dictionary<string, GmObject> _vars = new Dictionary<string, GmObject>();

        public static Dictionary<Type, string> ObjectIndexMapping = new Dictionary<Type, string>();
        public static Dictionary<string, Dictionary<string, InstanceEvent>> Events = new Dictionary<string, Dictionary<string, InstanceEvent>>();
        public static Dictionary<string, NativeFunction> Functions = new Dictionary<string, NativeFunction>();

        public GmObject this[string variableName]
        {
            get => _vars[variableName];
            set => _vars[variableName] = value;
        }

        public float Id { get; }
        public string ObjectType { get; }

        private GmInstance(float id, string instanceType)
        {
            Id = id;
            ObjectType = instanceType;
            Pool.Add(id, this);
            if (Events.TryGetValue(instanceType, out var events) && events.TryGetValue("create", out var create))
                create(this);
        }

        public static GmObject InstanceCreate(string instanceType)
        {
            float id;
            if (_availableIds.Count == 0)
                id = Pool.Count + Start;
            else
                id = _availableIds.Dequeue();

            new GmInstance(id, instanceType);

            return new GmObject(id);
        }

        public static void InstanceDestroy(float id)
        {
            if (Pool.TryGetValue(id, out var inst) && Events.TryGetValue(inst.ObjectType, out var events) && events.TryGetValue("destroy", out var destroy))
            {
                destroy(inst);
            }
            Pool.Remove(id);
            _availableIds.Enqueue(id);
        }

        public static bool TryGet(float id, out GmInstance inst)
        {
            return Pool.TryGetValue(id, out inst);
        }

        internal static GmObject InitGlobal()
        {
            var global = new GmInstance(-5f, "");
            return new GmObject(-5f);
        }
    }
}
