using NDesk.Options;
using System;
using System.Diagnostics;
using System.IO;
using TaffyScriptCompiler.Backend;

namespace TaffyScriptCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = false;
            bool generateBcl = false;
            bool generateBuild = false;

            var options = new OptionSet()
            {
                { "r", v => run = v != null },
                { "bcl", v => generateBcl = v != null },
                { "build", v => generateBuild = v != null }
            };

            var path = Directory.GetCurrentDirectory();
            var extra = options.Parse(args);
            if (extra.Count != 0)
                path = extra[0];

            Console.WriteLine("Compile Start...");

            var compiler = new MsilWeakCompiler();
            CompilerResult result;

            if(generateBuild)
            {
                var build = new BuildConfig();
                build.Save(path);
                return;
            }

            if (!generateBcl)
            {
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
