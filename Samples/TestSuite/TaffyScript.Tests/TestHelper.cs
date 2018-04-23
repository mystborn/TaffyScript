using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript;

namespace TaffyScript.Tests
{
    public static class TestHelper
    {
        [WeakMethod]
        public static TsObject Try(ITsInstance target, TsObject[] args)
        {
            var del = (TsDelegate)args[0];
            TsObject[] delArgs = null;
            if (args.Length > 1)
            {
                delArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, delArgs, 0, delArgs.Length);
            }

            var result = new TsInstance("obj_exception");
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

        [WeakMethod]
        public static TsObject TryExpect(ITsInstance target, TsObject[] args)
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
    }
}
