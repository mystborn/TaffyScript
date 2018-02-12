﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.FrontEnd;
using TaffyScript.Backend;
using TaffyScript.Syntax;
using NDesk.Options;

namespace TaffyScript
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = false;
            bool isDebug = true;

            var options = new OptionSet()
            {
                { "r", v => run = v != null },
                { "mode=", v => isDebug = v == null || v == "release" }
            };

            var extra = options.Parse(args);
            var path = extra[0];

            Console.WriteLine("Compile Start...");

            var compiler = new MsilWeakCompiler();

            var result = compiler.CompileProject(path);

            if (result.Errors.Count == 0)
            {
                Console.WriteLine("Compile succeeded...");
                if (run && result.PathToAssembly.EndsWith(".exe"))
                {
                    Console.WriteLine("Running...\n");
                    RunOutput(result.PathToAssembly);
                }
            }
            else
            {
                Console.WriteLine("Compile failed...");
                Console.WriteLine("Errors: \n");
                foreach (var error in result.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        private static void RunOutput(string location)
        {
            var psi = new ProcessStartInfo(location)
            {
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            using(var process = new Process())
            {
                process.StartInfo = psi;
                process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        static void CompileBcl()
        {
            var code = Bcl.Generate();
            var compiler = new MsilWeakCompiler();
            compiler.CompileCode(code, "gmbcl");
        }

        static void WriteSyntax(ISyntaxElement node, int indent)
        {
            Console.WriteLine(new string(' ', indent) + node.ToString());
            if(node is SyntaxNode parent)
            {
                foreach (var child in parent.Children)
                    WriteSyntax(child, indent + 2);
            }
        }
    }
}