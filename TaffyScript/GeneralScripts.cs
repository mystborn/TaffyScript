using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Provides an implementation for unclassified, general purpose scripts.
    /// </summary>
    [TaffyScriptBaseType]
    public static class GeneralScripts
    {
        [TaffyScriptMethod]
        public static TsObject base64_decode(ITsInstance inst, TsObject[] args)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String((string)args[0]));
        }

        [TaffyScriptMethod]
        public static TsObject base64_encode(ITsInstance inst, TsObject[] args)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes((string)args[0]));
        }

        [TaffyScriptMethod]
        public static TsObject environment_get_variable(ITsInstance inst, TsObject[] args)
        {
            return Environment.GetEnvironmentVariable((string)args[0]);
        }
    }
}
