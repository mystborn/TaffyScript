using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaffyScript.Threading
{
    [TaffyScriptObject]
    public class ThreadLock : ITsInstance
    {
        private object _key = new object();

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Threading.ThreadLock";

        public ThreadLock(TsObject[] args)
        {
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "lock":
                    return _lock(null, null);
                case "unlock":
                    return unlock(null, null);
                case "try_lock":
                    return try_lock(null, args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, delegateName);
        }

        public TsObject GetMember(string name)
        {
            if (TryGetDelegate(name, out var del))
                return del;
            throw new MissingMemberException(ObjectType, name);
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "lock":
                    del = new TsDelegate(_lock, "lock", this);
                    return true;
                case "unlock":
                    del = new TsDelegate(unlock, "unlock", this);
                    return true;
                case "try_lock":
                    del = new TsDelegate(try_lock, "try_lock", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        private TsObject _lock(ITsInstance inst, TsObject[] args)
        {
            Monitor.Enter(_key);
            return TsObject.Empty();
        }

        private TsObject unlock(ITsInstance inst, TsObject[] args)
        {
            Monitor.Exit(_key);
            return TsObject.Empty();
        }

        private TsObject try_lock(ITsInstance inst, TsObject[] args)
        {
            return args != null && args.Length > 0 ? Monitor.TryEnter(_key, (int)args[0]) : Monitor.TryEnter(_key);
        }
    }
}
