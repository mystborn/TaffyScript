using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaffyScript.Threading
{
    [TaffyScriptBaseType]
    public static class Locks
    {
        [TaffyScriptMethod]
        public static TsObject moniter_enter(TsObject[] args)
        {
            Monitor.Enter(args[0].WeakValue);
            return TsObject.Empty;
        }

        [TaffyScriptMethod]
        public static TsObject moniter_try_enter(TsObject[] args)
        {
            if (args.Length < 1)
                throw new ArgumentOutOfRangeException("args", "There must be at least one argument to call moniter_try_enter");
            if (args.Length == 2)
                return Monitor.TryEnter(args[0].WeakValue, (int)args[1]);
            else
                return Monitor.TryEnter(args[0].WeakValue);
        }

        [TaffyScriptMethod]
        public static TsObject moniter_exit(TsObject[] args)
        {
            Monitor.Exit(args[0].WeakValue);
            return TsObject.Empty;
        }
    }
}
