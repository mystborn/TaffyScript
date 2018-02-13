using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using GmExtern;

namespace TaffyScript.Backend
{
    public interface ICompiler
    {
        CompilerResult CompileProject(string projectDir);
        CompilerResult CompileCode(string code, BuildConfig config);
        CompilerResult CompileCode(string code, string output);
    }
}
