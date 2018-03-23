using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaffyScript.Threading.Extern
{
    public static class Locks
    {
        public static void Lock(TsObject obj)
        {
            try
            {
                Monitor.Enter(obj.GetValue());
            }
            catch(SynchronizationLockException e)
            {
                Console.WriteLine(e.InnerException);
            }
        }

        public static void Unlock(TsObject obj)
        {
            Monitor.Exit(obj.GetValue());
        }
    }
}
