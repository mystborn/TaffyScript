using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript;

namespace TaffyScript.Tests
{
    [TaffyScriptBaseType]
    public static class TestHelper
    {
        public static TsObject @try(TsObject[] args)
        {
            var del = (TsDelegate)args[0];
            TsObject[] delArgs = null;
            if (args.Length > 1)
            {
                delArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, delArgs, 0, delArgs.Length);
            }

            var result = new DynamicInstance("obj_exception");
            try
            {
                del.Invoke(delArgs);
                result["has_error"] = false;
                result["error_type"] = "";
                result["error_msg"] = "";
            }
            catch (Exception e)
            {
                result["has_error"] = true;
                result["error_type"] = e.GetType().Name;
                result["error_msg"] = e.Message;
            }

            return result;
        }
        
        public static TsObject try_expect(TsObject[] args)
        {
            var del = (TsDelegate)args[0];
            TsObject[] delArgs = null;
            if (args.Length > 2)
            {
                delArgs = new TsObject[args.Length - 2];
                Array.Copy(args, 2, delArgs, 0, delArgs.Length);
            }

            try
            {
                del.Invoke(delArgs);
            }
            catch(Exception e)
            {
                return e.GetType().Name == (string)args[1];
            }

            return false;
        }
        
        public static TsObject time_invoke(TsObject[] args)
        {
            var del = (TsDelegate)args[0];
            TsObject[] delArgs = null;
            if(args.Length > 1)
            {
                delArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, delArgs, 0, delArgs.Length);
            }

            var timer = new Stopwatch();
            timer.Start();
            del.Invoke(delArgs);
            timer.Stop();
            var result = new DynamicInstance("obj_timer_result");
            result["ms"] = timer.ElapsedMilliseconds;
            result["ticks"] = timer.ElapsedTicks;
            return result;
        }

        public static TsObject gc_collect(TsObject[] args) 
        {
            GC.Collect();
            return TsObject.Empty;
        }

        public static TsObject gc_get_used_memory(TsObject[] args)
        {
            //return GC.GetTotalMemory(false);
            return Environment.WorkingSet;
        }
    }
}
