using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Myst.Collections;
using GmExtern;
using GmParser.Syntax;

namespace GmParser.Backend
{
    public class MsilWeakCompiler
    {
        public MsilWeakCompiler()
        {
        }

        public void CompileCode(string code, string outputName)
        {
            var config = new MsilWeakBuildConfig()
            {
                Mode = CompileMode.Debug
            };
            var parser = new Parser();
            parser.Parse(code);
            var generator = new MsilWeakCodeGen(outputName, config);
            generator.CompileTree(parser.Tree, parser.Table);
        }
    }
}
