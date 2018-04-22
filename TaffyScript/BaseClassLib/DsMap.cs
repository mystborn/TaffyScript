using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript
{
    /// <summary>
    /// Basic Map implementation attempting to keep the same api as the ds_map from Gamemaker.
    /// </summary>
    public class DsMap
    {
        private static readonly ClassBinder<Dictionary<TsObject, TsObject>> _maps = new ClassBinder<Dictionary<TsObject, TsObject>>();

        /// <summary>
        /// Adds a value to a map with the given key.
        /// </summary>
        /// <param name="id">Map id</param>
        /// <param name="key">The key to add the value to</param>
        /// <param name="value">The value to add</param>
        /// <returns></returns>
        public static bool DsMapAdd(int id, TsObject key, TsObject value)
        {
            var map = _maps[id];
            if (map.ContainsKey(key))
                return false;
            map.Add(key, value);
            return true;
        }

        /// <summary>
        /// Clears all of the values from aa map.
        /// </summary>
        /// <param name="id"></param>
        public static void DsMapClear(int id)
        {
            _maps[id].Clear();
        }

        /// <summary>
        /// Copies all values from a source map to a destination map, clearing the destination map first.
        /// </summary>
        /// <param name="id">The destination map id</param>
        /// <param name="source">The source map id</param>
        public static void DsMapCopy(int id, int source)
        {
            var dst = _maps[id];
            var src = _maps[source];

            dst.Clear();
            foreach (var kvp in src)
                dst.Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Creates a new map
        /// </summary>
        /// <returns></returns>
        public static int DsMapCreate()
        {
            return _maps.Add(new Dictionary<TsObject, TsObject>());
        }

        /// <summary>
        /// Deletes the value with the specified key from a map
        /// </summary>
        /// <param name="id">Map id</param>
        /// <param name="key">The key to delete</param>
        public static void DsMapDelete(int id, TsObject key)
        {
            _maps[id].Remove(key);
        }

        /// <summary>
        /// Destroys a previously created map
        /// </summary>
        /// <param name="id">Map id</param>
        public static void DsMapDestroy(int id)
        {
            if (id < 0 || id >= _maps.Count)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (_maps[id] == null)
                throw new DataStructureDestroyedException("ds_map", id);

            _maps.Remove(id);
        }

        /// <summary>
        /// Determines if a map is empty
        /// </summary>
        /// <param name="id">Map id</param>
        /// <returns></returns>
        public static bool DsMapEmpty(int id)
        {
            return _maps[id].Count == 0;
        }

        /// <summary>
        /// Determines if a key exists within a map.
        /// </summary>
        /// <param name="id">Map id</param>
        /// <param name="key">The key to find</param>
        /// <returns></returns>
        public static bool DsMapExists(int id, TsObject key)
        {
            return _maps[id].ContainsKey(key);
        }

        /// <summary>
        /// Finds the value with the specified key within a map.
        /// </summary>
        /// <param name="id">Map id</param>
        /// <param name="key">The key used to find the value</param>
        /// <returns></returns>
        public static TsObject DsMapFindValue(int id, TsObject key)
        {
            if (_maps[id].TryGetValue(key, out var result))
                return result;
            return TsObject.Empty();
        }

        public static Dictionary<TsObject, TsObject> DsMapGet(int id)
        {
            return _maps[id];
        }

        /// <summary>
        /// Gets all of the keys within a map as an array.
        /// </summary>
        /// <param name="id">Map id</param>
        /// <returns></returns>
        public static TsObject[] DsMapKeys(int id)
        {
            return _maps[id].Keys.ToArray();
        }

        /// <summary>
        /// Replaces the value with the given key with a new value.
        /// </summary>
        /// <param name="id">Map id</param>
        /// <param name="key">The key to replace</param>
        /// <param name="value">The new value</param>
        public static void DsMapReplace(int id, TsObject key, TsObject value)
        {
            _maps[id][key] = value;
        }

        /// <summary>
        /// Gets the number of elements within a map
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DsMapSize(int id)
        {
            return _maps[id].Count;
        }
    }
}
