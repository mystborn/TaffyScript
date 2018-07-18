using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RealTask = System.Threading.Tasks.Task;

namespace TaffyScript.Threading
{
    [WeakBaseType]
    public static class Tasks
    {
        [WeakMethod]
        public static TsObject task_run(ITsInstance inst, TsObject[] args)
        {
            return args.Length == 1
                ? new Task(RealTask.Run(() => args[0].GetDelegate().Invoke()))
                : new Task(RealTask.Run(() => args[0].GetDelegate().Invoke(), ((TaskCancellationToken)args[1]).Source.Token));
        }

        [WeakMethod]
        public static TsObject task_wait_all(ITsInstance inst, TsObject[] args)
        {
            var tasks = new RealTask[args.Length];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = ((Task)args[i]).Source;

            try
            {
                RealTask.WaitAll(tasks);
                return "";
            }
            catch(AggregateException e)
            {
                return e.InnerException.ToString();
            }
        }

        [WeakMethod]
        public static TsObject task_wait_any(ITsInstance inst, TsObject[] args)
        {
            var tasks = new RealTask[args.Length];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = ((Task)args[i]).Source;

            try
            {
                var result = RealTask.WaitAny(tasks);
                return CreateTaskResult(result, "");
            }
            catch(AggregateException e)
            {
                return CreateTaskResult(-1, e.InnerException.ToString());
            }
        }

        [WeakMethod]
        public static TsObject thread_pool_queue_item(ITsInstance inst, TsObject[] args)
        {
            return ThreadPool.QueueUserWorkItem((obj) => args[0].GetDelegate().Invoke());
        }

        [WeakMethod]
        public static TsObject thread_sleep(ITsInstance inst, TsObject[] args)
        {
            Thread.Sleep((int)args[0]);
            return TsObject.Empty();
        }

        [WeakMethod]
        public static TsObject thread_get_id(ITsInstance inst, TsObject[] args)
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        public static TsObject CreateTaskResult(TsObject result, string exception)
        {
            var inst = new DynamicInstance("TaffyScript.Threading.TaskResult");
            inst["value"] = result;
            inst["exception"] = exception;
            return inst;
        }
    }
}
