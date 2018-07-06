using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaffyScript.Threading
{
    [WeakObject]
    public class TaskCancellationToken : ITsInstance
    {
        private CancellationTokenSource _source;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Threading.TaskCancellationSource";
        public CancellationTokenSource Source => _source;

        public TaskCancellationToken(TsObject[] args)
        {
            _source = args is null || args.Length == 0 ? new CancellationTokenSource() : new CancellationTokenSource((int)args[0]);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "cancel":
                    return cancel(null, args);
                case "cancel_after":
                    return cancel_after(null, args);
                case "dispose":
                    return dispose(null, args);
                case "register":
                    return register(null, args);
                case "throw_if_cancelled":
                    return throw_if_cancelled(null, args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMemberException(ObjectType, delegateName);
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "is_cancellation_requested":
                    return _source.IsCancellationRequested;
                case "can_be_cancelled":
                    return _source.Token.CanBeCanceled;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "cancel":
                    del = new TsDelegate(cancel, "cancel", this);
                    return true;
                case "cancel_after":
                    del = new TsDelegate(cancel_after, "cancel_after", this);
                    return true;
                case "dispose":
                    del = new TsDelegate(dispose, "dispose", this);
                    return true;
                case "register":
                    del = new TsDelegate(register, "register", this);
                    return true;
                case "throw_if_cancelled":
                    del = new TsDelegate(throw_if_cancelled, "throw_if_cancelled", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        private TsObject cancel(ITsInstance inst, TsObject[] args)
        {
            _source.Cancel();
            return TsObject.Empty();
        }

        private TsObject cancel_after(ITsInstance inst, TsObject[] args)
        {
            _source.CancelAfter((int)args[0]);
            return TsObject.Empty();
        }

        private TsObject dispose(ITsInstance inst, TsObject[] args)
        {
            _source.Dispose();
            return TsObject.Empty();
        }

        private TsObject register(ITsInstance inst, TsObject[] args)
        {
            var register = _source.Token.Register(() => args[0].GetDelegate().Invoke());
            var result = new DynamicInstance("TaffyScript.Threading.TaskCancellationTokenRegistration");
            result["unregister"] = new TsDelegate((i, a) => { register.Dispose(); return TsObject.Empty(); }, "unregister", result);
            return result;
        }

        private TsObject throw_if_cancelled(ITsInstance inst, TsObject[] args)
        {
            _source.Token.ThrowIfCancellationRequested();
            return TsObject.Empty();
        }

        public static implicit operator TsObject(TaskCancellationToken token)
        {
            return new TsObject(token);
        }

        public static explicit operator TaskCancellationToken(TsObject obj)
        {
            return (TaskCancellationToken)obj.Value.WeakValue;
        }
    }
}
