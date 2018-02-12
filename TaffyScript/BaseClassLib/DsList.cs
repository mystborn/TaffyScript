using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public static class DsList
    {
        private readonly static List<List<GmObject>> _lists = new List<List<GmObject>>();
        private readonly static Queue<int> _listSlots = new Queue<int>();

        [WeakMethod]
        public static GmObject DsListAdd(GmObject[] args)
        {
            if (args.Length < 2)
                throw new ArgumentNullException("When calling ds_list_add, at least 2 arguments must be provided.");
            var list = GetList(args[0].GetNumAsInt());
            for (var i = 1; i < args.Length; i++)
                list.Add(args[i]);

            return GmObject.Empty();
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
                _lists.Add(new List<GmObject>());
            }
            else
            {
                index = _listSlots.Dequeue();
                _lists[index] = new List<GmObject>();
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

        public static int DsListFindIndex(int id, GmObject value)
        {
            return GetList(id).FindIndex(v => v == value);
        }

        public static GmObject DsListFindValue(int id, int index)
        {
            return GetList(id)[index];
        }

        public static void DsListInsert(int id, int pos, GmObject val)
        {
            GetList(id).Insert(pos, val);
        }

        public static void DsListReplace(int id, int pos, GmObject val)
        {
            GetList(id)[pos] = val;
        }

        [WeakMethod]
        public static GmObject DsListSet(GmObject[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("When calling ds_list_set, at least 3 arguments must be provided.");
            var list = GetList(args[0].GetNumAsInt());
            var pos = args[1].GetNumAsInt();
            var length = pos + args.Length - 2;
            while (list.Count <= length)
                list.Add(new GmObject(0));
            for (var i = 2; i < args.Length; i++)
                list[pos + i - 2] = args[i];

            return GmObject.Empty();
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

        private static List<GmObject> GetList(int id)
        {
            if (id < 0 || id >= _lists.Count || _lists[id] == null)
                throw new ArgumentOutOfRangeException("index");
            return _lists[id];
        }
    }
}
