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
        public Assembly CompiledAssebmly { get; }
        public List<Exception> Errors { get; }
        public string PathToAssembly { get; }

        internal CompilerResult(Assembly asm, string path, params Exception[] errors)
        {
            CompiledAssebmly = asm;
            PathToAssembly = path;
            Errors = new List<Exception>(errors);
        }
    }
}
