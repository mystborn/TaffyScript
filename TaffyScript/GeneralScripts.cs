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
        public static TsObject environment_get_variable(TsObject[] args)
        {
            return Environment.GetEnvironmentVariable((string)args[0]);
        }
    }
}
