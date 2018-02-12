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
        CompilerResult CompileFiles(string output, params string[] files);
        CompilerResult CompileCode(string output, string code);
    }

    public abstract class Compiler
    {
        public abstract CompilerResult CompileFiles(params string[] files);
        public abstract CompilerResult CompileCode(string code);
    }
}
