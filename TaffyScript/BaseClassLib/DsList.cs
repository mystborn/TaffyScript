using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public static class DsList
    {
        private readonly static List<List<TsObject>> _lists = new List<List<TsObject>>();
        private readonly static Queue<int> _listSlots = new Queue<int>();

        [WeakMethod]
        public static TsObject DsListAdd(TsObject[] args)
        {
            if (args.Length < 2)
                throw new ArgumentNullException("When calling ds_list_add, at least 2 arguments must be provided.");
            var list = GetList(args[0].GetNumAsInt());
            for (var i = 1; i < args.Length; i++)
                list.Add(args[i]);

            return TsObject.Empty();
        }

        public static void DsListClear(int id)
        {
            GetList(id).Clear();
        }

        public static void DsListCopy(int id, int source)
        {
            var dest = GetList(id);
            var src = GetList(source);
            dest.Clear();
            dest.AddRange(src);
        }

        public static int DsListCreate()
        {
            int index;
            if(_listSlots.Count == 0)
            {
                index = _listSlots.Count;
                _lists.Add(new List<TsObject>());
            }
            else
            {
                index = _listSlots.Dequeue();
                _lists[index] = new List<TsObject>();
            }
            return index;
        }

        public static void DsListDelete(int id, int position)
        {
            GetList(id).RemoveAt(position);
        }

        public static void DsListDestroy(int id)
        {
            if (id < 0 || id >= _lists.Count)
                throw new ArgumentOutOfRangeException("id");
            else if (_lists[id] == null)
                throw new DataStructureDestroyedException("list", id);

            _lists[id] = null;
            _listSlots.Enqueue(id);
        }

        public static bool DsListEmpty(int id)
        {
            return GetList(id).Count == 0;
        }

        public static int DsListFindIndex(int id, TsObject value)
        {
            return GetList(id).FindIndex(v => v == value);
        }

        public static TsObject DsListFindValue(int id, int index)
        {
            var list = GetList(id);
            if (index >= list.Count)
                return TsObject.Empty();
            return list[index];
        }

        public static void DsListInsert(int id, int pos, TsObject val)
        {
            GetList(id).Insert(pos, val);
        }

        public static void DsListReplace(int id, int pos, TsObject val)
        {
            GetList(id)[pos] = val;
        }

        [WeakMethod]
        public static TsObject DsListSet(TsObject[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("When calling ds_list_set, at least 3 arguments must be provided.");
            var list = GetList(args[0].GetNumAsInt());
            var pos = args[1].GetNumAsInt();
            var length = pos + args.Length - 2;
            while (list.Count <= length)
                list.Add(new TsObject(0));
            for (var i = 2; i < args.Length; i++)
                list[pos + i - 2] = args[i];

            return TsObject.Empty();
        }

        public static void DsListStrongSet(int id, int pos, TsObject value)
        {
            var list = GetList(id);
            while (list.Count <= pos)
                list.Add(new TsObject(0));
            list[pos] = value;
        }

        public static void DsListShuffle(int id)
        {
            GetList(id).Shuffle();
        }

        public static int DsListSize(int id)
        {
            return GetList(id).Count;
        }

        public static void DsListSort(int id)
        {
            GetList(id).Sort();
        }

        private static List<TsObject> GetList(int id)
        {
            if (id < 0 || id >= _lists.Count || _lists[id] == null)
                throw new ArgumentOutOfRangeException("index");
            return _lists[id];
        }
    }
}
