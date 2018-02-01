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

        public GmObject this[string variableName]
        {
            get => _vars[variableName];
            set => _vars[variableName] = value;
        }

        public static GmObject InstanceCreate()
        {
            float id;
            if (_availableIds.Count == 0)
                id = Pool.Count + Start;
            else
                id = _availableIds.Dequeue();

            Pool.Add(id, new GmInstance());

            return new GmObject(id);
        }

        public static void InstanceDestroy(float id)
        {
            Pool.Remove(id);
        }

        public static bool TryGet(float id, out GmInstance inst)
        {
            return Pool.TryGetValue(id, out inst);
        }
    }
}
