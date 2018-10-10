using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Container namespace for basic TaffyScript functionality.
    /// </summary>
    [TaffyScriptBaseType]
    public static class GeneralScripts
    {
        /// <summary>
        /// Retrieves the value of an environment variable from the current process.
        /// </summary>
        /// <arg name="variable" type="string">The name of the environment variable.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable?view=netframework-4.7</source>
        /// <returns>string</returns>
        [TaffyScriptMethod]
        public static TsObject environment_get_variable(TsObject[] args)
        {
            return Environment.GetEnvironmentVariable((string)args[0]);
        }
    }
}
