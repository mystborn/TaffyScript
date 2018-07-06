using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaffyScript.Threading
{
    [WeakBaseType]
    public static class Locks
    {
        [WeakMethod]
        public static TsObject moniter_enter(ITsInstance inst, TsObject[] args)
        {
            Monitor.Enter(args[0].GetValue());
            return TsObject.Empty();
        }

        [WeakMethod]
        public static TsObject moniter_try_enter(ITsInstance inst, TsObject[] args)
        {
            if (args.Length < 1)
                throw new ArgumentOutOfRangeException("args", "There must be at least one argument to call moniter_try_enter");
            if (args.Length == 2)
                return Monitor.TryEnter(args[0].GetValue(), (int)args[1]);
            else
                return Monitor.TryEnter(args[0].GetValue());
        }

        [WeakMethod]
        public static TsObject moniter_exit(ITsInstance inst, TsObject[] args)
        {
            Monitor.Exit(args[0].GetValue());
            return TsObject.Empty();
        }
    }
}
