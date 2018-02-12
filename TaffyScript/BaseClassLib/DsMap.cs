using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public class DsMap
    {
        private readonly static List<Dictionary<GmObject, GmObject>> _maps = new List<Dictionary<GmObject, GmObject>>();
        private readonly static Queue<int> _mapSlots = new Queue<int>();

        public static bool DsMapAdd(int id, GmObject key, GmObject value)
        {
            var map = GetMap(id);
            if (map.ContainsKey(key))
                return false;
            map.Add(key, value);
            return true;
        }

        public static void DsMapClear(int id)
        {
            GetMap(id).Clear();
        }

        public static void DsMapCopy(int id, int source)
        {
            var dst = GetMap(id);
            var src = GetMap(source);

            dst.Clear();
            foreach (var kvp in src)
                dst.Add(kvp.Key, kvp.Value);
        }

        public static int DsMapCreate()
        {
            int index;
            if (_mapSlots.Count == 0)
            {
                index = _mapSlots.Count;
                _maps.Add(new Dictionary<GmObject, GmObject>());
            }
            else
            {
                index = _mapSlots.Dequeue();
                _maps[index] = new Dictionary<GmObject, GmObject>();
            }
            return index;
        }

        public static void DsMapDelete(int id, GmObject key)
        {
            GetMap(id).Remove(key);
        }

        public static void DsMapDestroy(int id)
        {
            if (id < 0 || id >= _maps.Count)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (_maps[id] == null)
                throw new DataStructureDestroyedException("ds_map", id);

            _maps[id] = null;
            _mapSlots.Enqueue(id);
        }

        public static bool DsMapEmpty(int id)
        {
            return GetMap(id).Count == 0;
        }

        public static bool DsMapExists(int id, GmObject key)
        {
            return GetMap(id).ContainsKey(key);
        }

        public static GmObject DsMapFindValue(int id, GmObject key)
        {
            if (GetMap(id).TryGetValue(key, out var result))
                return result;
            return GmObject.Empty();
        }

        public static GmObject[] DsMapKeys(int id)
        {
            return GetMap(id).Keys.ToArray();
        }

        public static void DsMapReplace(int id, GmObject key, GmObject value)
        {
            GetMap(id)[key] = value;
        }

        public static int DsMapSize(int id)
        {
            return GetMap(id).Count;
        }

        private static Dictionary<GmObject, GmObject> GetMap(int id)
        {
            if (id < 0 || id >= _maps.Count || _maps[id] == null)
                throw new ArgumentOutOfRangeException("index");
            return _maps[id];
        }
    }
}
