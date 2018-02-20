using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class DsMap
    {
        private readonly static List<Dictionary<TsObject, TsObject>> _maps = new List<Dictionary<TsObject, TsObject>>();
        private readonly static Queue<int> _mapSlots = new Queue<int>();

        public static bool DsMapAdd(int id, TsObject key, TsObject value)
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
                index = _maps.Count;
                _maps.Add(new Dictionary<TsObject, TsObject>());
            }
            else
            {
                index = _mapSlots.Dequeue();
                _maps[index] = new Dictionary<TsObject, TsObject>();
            }
            return index;
        }

        public static void DsMapDelete(int id, TsObject key)
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

        public static bool DsMapExists(int id, TsObject key)
        {
            return GetMap(id).ContainsKey(key);
        }

        public static TsObject DsMapFindValue(int id, TsObject key)
        {
            if (GetMap(id).TryGetValue(key, out var result))
                return result;
            return TsObject.Empty();
        }

        public static TsObject[] DsMapKeys(int id)
        {
            return GetMap(id).Keys.ToArray();
        }

        public static void DsMapReplace(int id, TsObject key, TsObject value)
        {
            GetMap(id)[key] = value;
        }

        public static int DsMapSize(int id)
        {
            return GetMap(id).Count;
        }

        private static Dictionary<TsObject, TsObject> GetMap(int id)
        {
            if (id < 0 || id >= _maps.Count || _maps[id] == null)
                throw new ArgumentOutOfRangeException("index");
            return _maps[id];
        }
    }
}
