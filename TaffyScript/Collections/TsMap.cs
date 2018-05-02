using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TsMap : ITsInstance
    {
        private Dictionary<TsObject, TsObject> _source = new Dictionary<TsObject, TsObject>();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "ds_map";
        public Dictionary<TsObject, TsObject> Source => _source;

        public event DestroyedDelegate Destroyed;

        public TsMap(TsObject[] args)
        {
        }

        public TsMap(Dictionary<TsObject, TsObject> source)
        {
            _source = new Dictionary<TsObject, TsObject>(source);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "get":
                    if (_source.TryGetValue(args[0], out var result))
                        return result;
                    break;
                case "set":
                    _source[args[0]] = args[1];
                    break;
                case "add":
                    if (_source.ContainsKey(args[0]))
                        return false;
                    _source.Add(args[0], args[1]);
                    return true;
                case "clear":
                    _source.Clear();
                    break;
                case "remove":
                    return _source.Remove(args[0]);
                case "contains_key":
                    return _source.ContainsKey(args[0]);
                case "copy":
                    return new TsMap(_source);
                default:
                    throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
            }
            return TsObject.Empty();
        }

        public void Destroy()
        {
            Destroyed?.Invoke(this);
            _source = null;
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;

            throw new MemberAccessException($"The type {ObjectType} does not define a script called {delegateName}");
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "count":
                    return _source.Count;
                case "keys":
                    return _source.Keys.ToArray();
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

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch(delegateName)
            {
                case "get":
                    del = new TsDelegate(get, "get", this);
                    return true;
                case "set":
                    del = new TsDelegate(set, "set", this);
                    return true;
                case "add":
                    del = new TsDelegate(add, "add", this);
                    return true;
                case "clear":
                    del = new TsDelegate(clear, "clear", this);
                    return true;
                case "remove":
                    del = new TsDelegate(remove, "remove", this);
                    return true;
                case "contains_key":
                    del = new TsDelegate(contains_key, "contains_key", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public static explicit operator TsMap(TsObject obj)
        {
            return (TsMap)obj.Value.WeakValue;
        }

        public static implicit operator TsObject(TsMap map)
        {
            return new TsObject(map);
        }

#pragma warning disable IDE1006 // Naming Styles

        public TsObject get(ITsInstance inst, params TsObject[] args)
        {
            return _source[args[0]];
        }

        public TsObject set(ITsInstance inst, params TsObject[] args)
        {
            _source[args[0]] = args[1];
            return TsObject.Empty();
        }

        public TsObject add(ITsInstance inst, params TsObject[] args)
        {
            if (_source.ContainsKey(args[0]))
                return false;

            _source.Add(args[0], args[1]);
            return true;
        }

        public TsObject clear(ITsInstance inst, params TsObject[] args)
        {
            _source.Clear();
            return TsObject.Empty();
        }

        public TsObject remove(ITsInstance inst, params TsObject[] args)
        {
            return _source.Remove(args[0]);
        }

        public TsObject contains_key(ITsInstance inst, params TsObject[] args)
        {
            return _source.ContainsKey(args[0]);
        }

        public TsObject copy(ITsInstance inst, params TsObject[] args)
        {
            return new TsMap(_source);
        }
    }
}
