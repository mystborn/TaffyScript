using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    public class TsList : ObjectWrapper, ITsInstance
    {
        private List<TsObject> _source = new List<TsObject>();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "ds_list";

        public event DestroyedDelegate Destroyed;

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "add":
                    if (args.Length == 1)
                        _source.Add(args[0]);
                    else
                        _source.AddRange(args);
                    break;
                case "clear":
                    _source.Clear();
                    break;
                case "delete":
                    _source.RemoveAt((int)args[0]);
                    break;
                case "_get":
                    return _source[(int)args[0]];
                case "_set":
                    _source[(int)args[0]] = args[1];
                    break;
                case "insert":
                    _source.Insert((int)args[0], args[1]);
                    break;
                default:
                    if (Members.TryGetValue(scriptName, out var member) && member.Type == VariableType.Delegate)
                        return member.GetDelegateUnchecked().Invoke(args);
                    else
                        throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
            }
            return TsObject.Empty();
        }

        public void Destroy()
        {
            Destroyed?.Invoke(this);
            _source = null;
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
                case "size":
                    return _source.Count;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;

                    if (Members.TryGetValue(name, out var member))
                        return member;

                    throw new MemberAccessException($"Couldn't find member with the name {name}");
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch(name)
            {
                case "size":
                case "add":
                case "clear":
                case "delete":
                case "_get":
                case "_set":
                case "insert":
                    throw new MemberAccessException($"Member {name} on type {ObjectType} is readonly");
                default:
                    Members[name] = value;
                    break;
            }
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName) {
                case "add":
                    del = new TsDelegate(add, "add", this);
                    return true;
                case "clear":
                    del = new TsDelegate((i, a) => { _source.Clear(); return TsObject.Empty(); }, "clear", this);
                    return true;
                case "delete":
                    del = new TsDelegate((i, a) => { _source.RemoveAt((int)a[0]); return TsObject.Empty(); }, "delete", this);
                    return true;
                case "_get":
                    del = new TsDelegate((i, a) => _source[(int)a[0]], "_get", this);
                    return true;
                case "_set":
                    del = new TsDelegate((i, a) => { _source[(int)a[0]] = a[1]; return TsObject.Empty(); }, "_set", this);
                    return true;
                case "insert":
                    del = new TsDelegate((i, a) => { _source.Insert((int)a[0], a[1]); return TsObject.Empty(); }, "insert", this);
                    return true;
                default:
                    if(Members.TryGetValue(scriptName, out var member) && member.Type == VariableType.Delegate)
                    {
                        del = member.GetDelegateUnchecked();
                        return true;
                    }
                    del = null;
                    return false;
            }
        }

        public static TsList New(params TsObject[] args)
        {
            return new TsList();
        }

        public TsObject add(ITsInstance isnt, TsObject[] args)
        {
            _source.AddRange(args);
            return TsObject.Empty();
        }
    }
}
