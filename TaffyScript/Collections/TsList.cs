using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TsList : ITsInstance
    {
        private List<TsObject> _source;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "List";
        public List<TsObject> Source => _source;

        public TsList()
        {
            _source = new List<TsObject>();
        }

        public TsList(TsObject[] args)
        {
            if (args is null)
                _source = new List<TsObject>();
            else
                _source = new List<TsObject>(args);
        }

        public TsList(IEnumerable<TsObject> source)
        {
            _source = new List<TsObject>(source);
        }

        private TsList(List<TsObject> source)
        {
            _source = source;
        }

        public static TsList Wrap(List<TsObject> source)
        {
            return new TsList(source);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "add":
                    _source.AddRange(args);
                    break;
                case "clear":
                    _source.Clear();
                    break;
                case "copy":
                    return new TsList(_source);
                case "get":
                    return _source[(int)args[0]];
                case "insert":
                    _source.Insert((int)args[0], args[1]);
                    break;
                case "index_of":
                    return _source.IndexOf(args[0]);
                case "remove":
                    _source.RemoveAt((int)args[0]);
                    break;
                case "set":
                    var index = (int)args[0];
                    while (_source.Count <= index)
                        _source.Add(TsObject.Empty);
                    _source[index] = args[1];
                    break;
                case "shuffle":
                    _source.Shuffle();
                    break;
                case "sort":
                    return sort(args);
                default:
                    throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
            }
            return TsObject.Empty;
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;

            throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "count":
                    return _source.Count;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;

                    throw new MemberAccessException($"Couldn't find member with the name {name}");
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MemberAccessException($"Member {name} on type {ObjectType} is readonly");
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName) {
                case "add":
                    del = new TsDelegate(add, "add");
                    return true;
                case "clear":
                    del = new TsDelegate(clear, "clear");
                    return true;
                case "copy":
                    del = new TsDelegate(copy, "copy");
                    return true;
                case "get":
                    del = new TsDelegate(get, "get");
                    return true;
                case "insert":
                    del = new TsDelegate(insert, "insert");
                    return true;
                case "index_of":
                    del = new TsDelegate(index_of, "index_of");
                    return true;
                case "remove":
                    del = new TsDelegate(remove, "remove");
                    return true;
                case "set":
                    del = new TsDelegate(set, "set");
                    return true;
                case "shuffle":
                    del = new TsDelegate(shuffle, "shuffle");
                    return true;
                case "sort":
                    del = new TsDelegate(sort, "sort");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public static explicit operator TsList(TsObject obj)
        {
            return (TsList)obj.WeakValue;
        }

        public static implicit operator TsObject(TsList list)
        {
            return new TsInstanceWrapper(list);
        }

#pragma warning disable IDE1006 // Naming Styles

        public TsObject add(TsObject[] args)
        {
            _source.AddRange(args);
            return TsObject.Empty;
        }

        public TsObject clear(TsObject[] args)
        {
            _source.Clear();
            return TsObject.Empty;
        }

        public TsObject copy(TsObject[] args)
        {
            return new TsList(_source);
        }

        public TsObject get(TsObject[] args)
        {
            return _source[(int)args[0]];
        }

        public TsObject insert(TsObject[] args)
        {
            _source.Insert((int)args[0], args[1]);
            return TsObject.Empty;
        }

        public TsObject index_of(TsObject[] args)
        {
            return _source.IndexOf(args[0]);
        }
        
        public TsObject remove(TsObject[] args)
        {
            _source.RemoveAt((int)args[0]);
            return TsObject.Empty;
        }

        public TsObject set(TsObject[] args)
        {
            var index = (int)args[0];
            while (_source.Count <= index)
                _source.Add(TsObject.Empty);
            _source[index] = args[1];
            return TsObject.Empty;
        }

        public TsObject shuffle(TsObject[] args)
        {
            _source.Shuffle();
            return TsObject.Empty;
        }

        public TsObject sort(TsObject[] args)
        {
            if(args is null)
                _source.Sort();
            switch(args.Length)
            {
                case 0:
                    _source.Sort();
                    break;
                case 1:
                    var script = args[0].GetDelegate();
                    _source.Sort((x, y) => (int)script.Invoke(x, y));
                    break;
            }
            return TsObject.Empty;
        }
    }
}
