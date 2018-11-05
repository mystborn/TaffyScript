using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    [TaffyScriptObject]
    public class Set : TsEnumerable
    {
        public override string ObjectType => "TaffyScript.Collections.Set";
        public HashSet<TsObject> Source { get; }

        private Set(HashSet<TsObject> set)
        {
            Source = set;
        }

        public Set(IEnumerable<TsObject> set)
        {
            Source = new HashSet<TsObject>(set);
        }

        public Set(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    Source = new HashSet<TsObject>();
                    break;
                case 1:
                    Source = new HashSet<TsObject>((IEqualityComparer<TsObject>)args[0].WeakValue);
                    break;
            }
        }

        public static Set Wrap(HashSet<TsObject> set)
        {
            return new Set(set);
        }

        public override TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "add":
                    return add(args);
                case "clear":
                    return clear(args);
                case "contains":
                    return contains(args);
                case "except_with":
                    return except_with(args);
                case "intersect_with":
                    return intersect_with(args);
                case "is_proper_subset_of":
                    return is_proper_subset_of(args);
                case "is_proper_superset_of":
                    return is_proper_superset_of(args);
                case "is_subset_of":
                    return is_subset_of(args);
                case "is_superset_of":
                    return is_superset_of(args);
                case "overlaps":
                    return overlaps(args);
                case "remove":
                    return remove(args);
                case "remove_where":
                    return remove_where(args);
                case "set_equals":
                    return set_equals(args);
                case "trim_excess":
                    return trim_excess(args);
                case "union_with":
                    return union_with(args);
                default:
                    return base.Call(scriptName, args);
            }
        }

        public override IEnumerator<TsObject> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        public override TsObject GetMember(string name)
        {
            switch(name)
            {
                case "comparer":
                    return TsEqualityComparer.Wrap(Source.Comparer);
                case "count":
                    return Source.Count;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public override void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public override bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "add":
                    del = new TsDelegate(add, scriptName);
                    break;
                case "clear":
                    del = new TsDelegate(clear, scriptName);
                    break;
                case "contains":
                    del = new TsDelegate(contains, scriptName);
                    break;
                case "except_with":
                    del = new TsDelegate(except_with, scriptName);
                    break;
                case "intersect_with":
                    del = new TsDelegate(intersect_with, scriptName);
                    break;
                case "is_proper_subset_of":
                    del = new TsDelegate(is_proper_subset_of, scriptName);
                    break;
                case "is_proper_superset_of":
                    del = new TsDelegate(is_proper_superset_of, scriptName);
                    break;
                case "is_subset_of":
                    del = new TsDelegate(is_subset_of, scriptName);
                    break;
                case "is_superset_of":
                    del = new TsDelegate(is_superset_of, scriptName);
                    break;
                case "overlaps":
                    del = new TsDelegate(overlaps, scriptName);
                    break;
                case "remove":
                    del = new TsDelegate(remove, scriptName);
                    break;
                case "remove_where":
                    del = new TsDelegate(remove_where, scriptName);
                    break;
                case "set_equals":
                    del = new TsDelegate(set_equals, scriptName);
                    break;
                case "trim_excess":
                    del = new TsDelegate(trim_excess, scriptName);
                    break;
                case "union_with":
                    del = new TsDelegate(union_with, scriptName);
                    break;
                default:
                    return base.TryGetDelegate(scriptName, out del);
            }
            return true;
        }

        public TsObject add(TsObject[] args)
        {
            return Source.Add(args[0]);
        }

        public TsObject clear(TsObject[] args)
        {
            Source.Clear();
            return TsObject.Empty;
        }

        public override TsObject contains(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return Source.Contains(args[0]);
                case 2:
                    return Source.Contains(args[0], (IEqualityComparer<TsObject>)args[1].WeakValue);
                default:
                    throw new ArgumentException($"Invalid number of argumnets passed to {ObjectType}.{nameof(contains)}");
            }
        }

        public TsObject except_with(TsObject[] args)
        {
            Source.ExceptWith((IEnumerable<TsObject>)args[0].WeakValue);
            return TsObject.Empty;
        }

        public override TsObject get_enumerator(TsObject[] args)
        {
            return TsEnumerator.Wrap(GetEnumerator());
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }

        public TsObject intersect_with(TsObject[] args)
        {
            Source.IntersectWith((IEnumerable<TsObject>)args[0].WeakValue);
            return TsObject.Empty;
        }

        public TsObject is_proper_subset_of(TsObject[] args)
        {
            return Source.IsProperSubsetOf((IEnumerable<TsObject>)args[0].WeakValue);
        }

        public TsObject is_proper_superset_of(TsObject[] args)
        {
            return Source.IsProperSupersetOf((IEnumerable<TsObject>)args[0].WeakValue);
        }

        public TsObject is_subset_of(TsObject[] args)
        {
            return Source.IsSubsetOf((IEnumerable<TsObject>)args[0].WeakValue);
        }

        public TsObject is_superset_of(TsObject[] args)
        {
            return Source.IsSupersetOf((IEnumerable<TsObject>)args[0].WeakValue);
        }

        public TsObject overlaps(TsObject[] args)
        {
            return Source.Overlaps((IEnumerable<TsObject>)args[0].WeakValue);
        }

        public TsObject remove(TsObject[] args)
        {
            return Source.Remove(args[0]);
        }

        public TsObject remove_where(TsObject[] args)
        {
            var script = args[0].GetDelegate();
            return Source.RemoveWhere(o => (bool)script.Invoke(o));
        }

        public TsObject set_equals(TsObject[] args)
        {
            return Source.SetEquals((IEnumerable<TsObject>)args[0].WeakValue);
        }

        public TsObject trim_excess(TsObject[] args)
        {
            Source.TrimExcess();
            return TsObject.Empty;
        }

        public TsObject union_with(TsObject[] args)
        {
            Source.UnionWith((IEnumerable<TsObject>)args[0].WeakValue);
            return TsObject.Empty;
        }
    }
}
