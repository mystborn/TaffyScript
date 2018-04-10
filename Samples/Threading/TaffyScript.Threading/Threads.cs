using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaffyScript;

namespace TaffyScript.Threading.Extern
{
    public static class Threads
    {
        private static Stack<int> _availableIds = new Stack<int>();
        private static ConcurrentDictionary<int, Task<TsObject>> _tasks = new ConcurrentDictionary<int, Task<TsObject>>();
        private static object _key = new object();

        [WeakMethod]
        public static TsObject TaskStart(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args");
            var scriptName = args[0].GetString();
            if(TsInstance.Scripts.TryGetValue(scriptName, out var script))
            {
                var scriptArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, scriptArgs, 0, args.Length - 1);
                return GetId(Task.Run(() => script(scriptArgs)));
            }
            throw new InvalidOperationException($"Could not find a script by the name of {scriptName}");
        }

        [WeakMethod]
        public static TsObject ThreadFire(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args");
            var scriptName = args[0].GetString();
            if(TsInstance.Scripts.TryGetValue(scriptName, out var script))
            {
                var scriptArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, scriptArgs, 0, args.Length - 1);
                ThreadPool.QueueUserWorkItem((obj) => script(scriptArgs));
                return TsObject.Empty();
            }
            throw new InvalidOperationException($"Could not find a script by the name of {scriptName}");
        }

        public static TsInstance TaskResult(int taskId)
        {
            if(_tasks.TryGetValue(taskId, out var task))
            {
                Task.WaitAny(task);
                var inst = new TsInstance("TaffyScript.Threading.thread_result");
                inst["id"] = taskId;
                if(task.Status == TaskStatus.Faulted)
                {
                    inst["exception"] = task.Exception.InnerException.ToString();
                    inst["result"] = TsObject.Empty();
                }
                else
                {
                    inst["exception"] = "";
                    inst["result"] = task.Result;
                }
                ReleaseId(taskId);
                task.Dispose();
                return inst;
            }
            throw new ArgumentException($"Task with id {taskId} has already been disposed of.");
        }

        public static TsObject[] TaskResultArray(int taskId)
        {

            if(_tasks.TryGetValue(taskId, out var task))
            {
                Task.WaitAny(task);
                TsObject[] result = new TsObject[2];
                if (task.Status == TaskStatus.Faulted)
                {
                    result[0] = task.Exception.InnerException.ToString();
                    result[1] = TsObject.Empty();
                }
                else
                {
                    result[0] = "";
                    result[1] = task.Result;
                }
                ReleaseId(taskId);
                task.Dispose();
                return result;
            }
            throw new ArgumentException($"Task with id {taskId} has already been disposed of.");
        }

        public static void ThreadSleep(int ms)
        {
            Thread.Sleep(ms);
        }

        public static int ThreadGetId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        private static int GetId(Task<TsObject> task)
        {
            lock(_key)
            {
                int id;
                if (_availableIds.Count == 0)
                    id = _tasks.Count;
                else
                    id = _availableIds.Pop();
                _tasks[id] = task;

                return id;
            }
        }

        private static Task<TsObject> ReleaseId(int id)
        {
            lock(_key)
            {
                if (_tasks.TryRemove(id, out var result))
                    _availableIds.Push(id);
                return result;
            }
        }
    }
}
