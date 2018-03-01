using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TaffyScriptCompiler.FrontEnd;
using TaffyScriptCompiler.Backend;
using TaffyScriptCompiler.Syntax;
using NDesk.Options;

namespace TaffyScriptCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = false;
            bool generateBcl = false;

            var options = new OptionSet()
            {
                { "r", v => run = v != null },
                { "bcl", v => generateBcl = v != null }
            };

            var extra = options.Parse(args);

            Console.WriteLine("Compile Start...");

            var compiler = new MsilWeakCompiler();
            CompilerResult result;

            if (!generateBcl)
            {
                var path = extra[0];
                result = compiler.CompileProject(path);
            }
            else
                result = compiler.CompileCode(BaseClassLibrary.Generate(), new BuildConfig() { Mode = CompileMode.Release, Output = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Libraries", "TaffyScript.BCL") });

            if (result.Errors.Count == 0)
            {
                Console.WriteLine("Compile succeeded...");
                Console.WriteLine($"Output: {result.PathToAssembly}");
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
    }
}
