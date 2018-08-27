using NDesk.Options;
using System;
using System.Diagnostics;
using System.IO;
using TaffyScript.Compiler;
using TaffyScript.Compiler.Backend;

namespace TaffyScript.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = false;
            bool generateBuild = false;
            bool time = false;

            var options = new OptionSet()
            {
                { "r", v => run = v != null },
                { "build", v => generateBuild = v != null },
                { "t", v => time = v != null }
            };

            var path = Directory.GetCurrentDirectory();
            var extra = options.Parse(args);
            if (extra.Count != 0)
                path = extra[0];

#if DEBUG
            path = @"C:\Users\Chris\Source\TaffyScript\Japanese";
#endif

            if (generateBuild)
            {
                var build = new BuildConfig();
                build.Save(path);
                return;
            }

            Console.WriteLine("Compile Start...");
            Stopwatch sw = null;
            if (time)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            var logger = new ErrorLogger();

            var compiler = new MsilWeakCompiler(logger);

            var result = compiler.CompileProject(path);

            if (result.Errors.Count == 0)
            {
                if(result.Warnings.Count > 0)
                {
                    Console.WriteLine("Warnings:\n");
                    foreach (var warning in result.Warnings)
                        Console.WriteLine(warning);
                    Console.WriteLine('\n');
                }

                Console.WriteLine("Compile succeeded...");
                if(time)
                {
                    sw.Stop();
                    Console.WriteLine($"Compile time: {sw.ElapsedMilliseconds} ms");
                }
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
                    Console.WriteLine(error);
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

            using (var process = new Process())
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
