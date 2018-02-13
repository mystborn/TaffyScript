using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TaffyScript.Backend
{
    public class CompilerResult
    {
        /// <summary>
        /// If the compile was successful, contains the output assembly.
        /// </summary>
        public Assembly CompiledAssebmly { get; }

        /// <summary>
        /// If the compile failed, contains a list of the encountered errors.
        /// </summary>
        public List<Exception> Errors { get; }

        /// <summary>
        /// If the compile was successful, contains the path to the output assembly.
        /// </summary>
        public string PathToAssembly { get; }

        public CompilerResult(Assembly asm, string path, params Exception[] errors)
        {
            CompiledAssebmly = asm;
            PathToAssembly = path;
            Errors = errors == null ? new List<Exception>() : new List<Exception>(errors);
        }

        public CompilerResult(IEnumerable<Exception> errors)
        {
            CompiledAssebmly = null;
            PathToAssembly = null;
            Errors = new List<Exception>(errors);
        }
    }
}
