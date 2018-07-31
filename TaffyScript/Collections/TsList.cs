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

        public TsList(TsObject[] args)
        {
            _source = new List<TsObject>();
        }

        public TsList(IEnumerable<TsObject> source)
        {
            _source = new List<TsObject>(source);
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
                    _source.Sort();
                    break;
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
                    del = new TsDelegate(add, "add", this);
                    return true;
                case "clear":
                    del = new TsDelegate(clear, "clear", this);
                    return true;
                case "copy":
                    del = new TsDelegate(copy, "copy", this);
                    return true;
                case "get":
                    del = new TsDelegate(get, "get", this);
                    return true;
                case "insert":
                    del = new TsDelegate(insert, "insert", this);
                    return true;
                case "index_of":
                    del = new TsDelegate(index_of, "index_of", this);
                    return true;
                case "remove":
                    del = new TsDelegate(remove, "remove", this);
                    return true;
                case "set":
                    del = new TsDelegate(set, "set", this);
                    return true;
                case "shuffle":
                    del = new TsDelegate(shuffle, "shuffle", this);
                    return true;
                case "sort":
                    del = new TsDelegate(sort, "sort", this);
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

        public TsObject add(ITsInstance inst, TsObject[] args)
        {
            _source.AddRange(args);
            return TsObject.Empty;
        }

        public TsObject clear(ITsInstance inst, TsObject[] args)
        {
            _source.Clear();
            return TsObject.Empty;
        }

        public TsObject copy(ITsInstance inst, TsObject[] args)
        {
            return new TsList(_source);
        }

        public TsObject get(ITsInstance inst, TsObject[] args)
        {
            return _source[(int)args[0]];
        }

        public TsObject insert(ITsInstance inst, TsObject[] args)
        {
            _source.Insert((int)args[0], args[1]);
            return TsObject.Empty;
        }

        public TsObject index_of(ITsInstance inst, TsObject[] args)
        {
            return _source.IndexOf(args[0]);
        }
        
        public TsObject remove(ITsInstance inst, TsObject[] args)
        {
            _source.RemoveAt((int)args[0]);
            return TsObject.Empty;
        }

        public TsObject set(ITsInstance inst, TsObject[] args)
        {
            var index = (int)args[0];
            while (_source.Count <= index)
                _source.Add(TsObject.Empty);
            _source[index] = args[1];
            return TsObject.Empty;
        }

        public TsObject shuffle(ITsInstance inst, TsObject[] args)
        {
            _source.Shuffle();
            return TsObject.Empty;
        }

        public TsObject sort(ITsInstance inst, TsObject[] args)
        {
            _source.Sort();
            return TsObject.Empty;
        }
    }
}
