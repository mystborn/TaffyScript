using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Threading
{
    [WeakObject]
    public class Task : ITsInstance
    {
        private Task<TsObject> _task;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public Task<TsObject> Source => _task;
        public string ObjectType => "TaffyScript.Threading.Task";

        public Task(TsObject[] args)
        {
            Func<TsObject> task = () => args[0].GetDelegate().Invoke();
            _task = args.Length > 1 ? new Task<TsObject>(task, ((TaskCancellationToken)args[1]).Source.Token) : new Task<TsObject>(task);
        }

        public Task(Task<TsObject> task)
        {
            _task = task;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "continue_with":
                    return continue_with(null, args);
                case "dispose":
                    return dispose(null, null);
                case "start":
                    return start(null, null);
                case "wait":
                    return wait(null, null);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            throw new NotImplementedException();
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "id":
                    return _task.Id;
                case "is_canceled":
                    return _task.IsCanceled;
                case "is_completed":
                    return _task.IsCompleted;
                case "is_faulted":
                    return _task.IsFaulted;
                case "result":
                    var result = new DynamicInstance("TaskResult");
                    try
                    {
                        var value = _task.Result;
                        return Tasks.CreateTaskResult(value, "");
                    }
                    catch(AggregateException e)
                    {
                        return Tasks.CreateTaskResult(TsObject.Empty(), e.InnerException.ToString());
                    }
                case "status":
                    return (int)_task.Status;
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
                case "continue_with":
                    del = new TsDelegate(continue_with, "continue_with", this);
                    return true;
                case "dispose":
                    del = new TsDelegate(dispose, "dispose", this);
                    return true;
                case "start":
                    del = new TsDelegate(start, "start", this);
                    return true;
                case "wait":
                    del = new TsDelegate(wait, "wait", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        private TsObject continue_with(ITsInstance inst, TsObject[] args)
        {
            var del = args[0].GetDelegate();
            return new Task(_task.ContinueWith((result) => del.Invoke(result.Result)));
        }

        private TsObject dispose(ITsInstance inst, TsObject[] args)
        {
            _task.Dispose();
            return TsObject.Empty();
        }

        private TsObject start(ITsInstance inst, TsObject[] args)
        {
            _task.Start();
            return this;
        }

        private TsObject wait(ITsInstance inst, TsObject[] args)
        {
            _task.Wait();
            return this;
        }

        public static implicit operator TsObject(Task task)
        {
            return new TsObject(task);
        }

        public static explicit operator Task(TsObject obj)
        {
            return (Task)obj.Value.WeakValue;
        }
    }
}
