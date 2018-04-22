using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaffyScript.Threading.Extern
{
    public static class Locks
    {
        public static void MoniterEnter(TsObject obj)
        {
            Monitor.Enter(obj.GetValue());
        }

        [WeakMethod]
        public static TsObject MoniterTryEnter(TsInstance inst, TsObject[] args)
        {
            if (args.Length < 1)
                throw new ArgumentOutOfRangeException("args", "There must be at least one argument to call moniter_try_enter");
            if (args.Length == 2)
                return Monitor.TryEnter(args[0].GetValue(), (int)args[1]);
            else
                return Monitor.TryEnter(args[0].GetValue());
        }

        public static void MoniterExit(TsObject obj)
        {
            Monitor.Exit(obj.GetValue());
        }
    }
}
