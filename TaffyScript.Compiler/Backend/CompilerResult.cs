using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TaffyScript.Compiler.Backend
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
        public List<string> Errors { get; }

        /// <summary>
        /// Warnings that aren't breaking, but are generally considered bad practice.
        /// </summary>
        public List<string> Warnings { get; }

        /// <summary>
        /// If the compile was successful, contains the path to the output assembly.
        /// </summary>
        public string PathToAssembly { get; }

        public CompilerResult(Assembly asm, string path, IErrorLogger errorLogger)
        {
            CompiledAssebmly = asm;
            PathToAssembly = path;
            Errors = errorLogger.Errors;
            Warnings = errorLogger.Warnings;
        }

        public CompilerResult(IErrorLogger errorLogger)
        {
            CompiledAssebmly = null;
            PathToAssembly = null;
            Errors = errorLogger.Errors;
            Warnings = errorLogger.Warnings;
        }
    }
}
