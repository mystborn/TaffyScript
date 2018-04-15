using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript
{
    /// <summary>
    /// Basic List implementation attempting to keep the same api as the ds_list from Gamemaker.
    /// </summary>
    public static class DsList
    {
        private readonly static ClassBinder<List<TsObject>> _lists = new ClassBinder<List<TsObject>>();

        /// <summary>
        /// Adds a value to a list.
        /// </summary>
        [WeakMethod]
        public static TsObject DsListAdd(TsInstance target, TsObject[] args)
        {
            if (args.Length < 2)
                throw new ArgumentNullException("When calling ds_list_add, at least 2 arguments must be provided.");
            var list = _lists[(int)args[0]];
            for (var i = 1; i < args.Length; i++)
                list.Add(args[i]);

            return TsObject.Empty();
        }

        /// <summary>
        /// Clears all values from a list.
        /// </summary>
        /// <param name="id"></param>
        public static void DsListClear(int id)
        {
            _lists[id].Clear();
        }

        /// <summary>
        /// Copies the contents from a source list to a destination list, clearing all previous contents in the destination.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="source"></param>
        public static void DsListCopy(int id, int source)
        {
            var dest = _lists[id];
            var src = _lists[source];
            dest.Clear();
            dest.AddRange(src);
        }

        /// <summary>
        /// Creates a new list.
        /// </summary>
        /// <returns></returns>
        public static int DsListCreate()
        {
            return _lists.Add(new List<TsObject>());
        }

        /// <summary>
        /// Removes the value at the specified position within a list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        public static void DsListDelete(int id, int position)
        {
            _lists[id].RemoveAt(position);
        }

        /// <summary>
        /// Destroys a previously created list.
        /// </summary>
        /// <param name="id"></param>
        public static void DsListDestroy(int id)
        {
            if (id < 0 || id >= _lists.Count)
                throw new ArgumentOutOfRangeException("id");
            else if (_lists[id] == null)
                throw new DataStructureDestroyedException("list", id);

            _lists.Remove(id);
        }

        /// <summary>
        /// Determines if a list is empty.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DsListEmpty(int id)
        {
            return _lists[id].Count == 0;
        }

        /// <summary>
        /// Finds the index of the given value within a list.
        /// </summary>
        /// <param name="id">List id</param>
        /// <param name="value">The value to find.</param>
        /// <returns></returns>
        public static int DsListFindIndex(int id, TsObject value)
        {
            return _lists[id].FindIndex(v => v == value);
        }

        /// <summary>
        /// Gets the value stored in specified list index.
        /// </summary>
        /// <param name="id">List id</param>
        /// <param name="index">The index of the value.</param>
        /// <returns></returns>
        public static TsObject DsListFindValue(int id, int index)
        {
            var list = _lists[id];
            if (index >= list.Count)
                return TsObject.Empty();
            return list[index];
        }

        /// <summary>
        /// Inserts a value at the specified location.
        /// </summary>
        /// <param name="id">List id</param>
        /// <param name="pos">Position index</param>
        /// <param name="val">Value to insert</param>
        public static void DsListInsert(int id, int pos, TsObject val)
        {
            _lists[id].Insert(pos, val);
        }

        /// <summary>
        /// Replaces a value at the specfied index.
        /// </summary>
        /// <param name="id">List id</param>
        /// <param name="pos">Position index</param>
        /// <param name="val">Value to replace with</param>
        public static void DsListReplace(int id, int pos, TsObject val)
        {
            _lists[id][pos] = val;
        }

        /// <summary>
        /// Sets a value at the specified position within a list.
        /// </summary>
        /// <remarks>
        /// Do not change the signature of this method. It is used within the compiler when using the following syntax:
        /// list[| 0] = "foo";
        /// </remarks>
        [WeakMethod]
        public static TsObject DsListSet(TsInstance target, TsObject[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("When calling ds_list_set, at least 3 arguments must be provided.");
            var list = _lists[(int)args[0]];
            var pos = (int)args[1];
            var length = pos + args.Length - 2;
            while (list.Count <= length)
                list.Add(new TsObject(0));
            for (var i = 2; i < args.Length; i++)
                list[pos + i - 2] = args[i];

            return TsObject.Empty();
        }
        
        /// <summary>
        /// Sets a value at the specified position within a list.
        /// </summary>
        /// <param name="id">The id of the list.</param>
        /// <param name="pos">The index of the value.</param>
        /// <param name="value">The new value.</param>
        public static void DsListSet(int id, int pos, TsObject value)
        {
            var list = _lists[id];
            while (list.Count <= pos)
                list.Add(new TsObject(0));
            list[pos] = value;
        }

        /// <summary>
        /// Shuffles all of the values within a list.
        /// </summary>
        /// <param name="id"></param>
        public static void DsListShuffle(int id)
        {
            _lists[id].Shuffle();
        }

        /// <summary>
        /// Gets the number of elements within a list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DsListSize(int id)
        {
            return _lists[id].Count;
        }

        /// <summary>
        /// Sorts all of the elements within a list.
        /// </summary>
        /// <param name="id"></param>
        public static void DsListSort(int id)
        {
            _lists[id].Sort();
        }
    }
}
