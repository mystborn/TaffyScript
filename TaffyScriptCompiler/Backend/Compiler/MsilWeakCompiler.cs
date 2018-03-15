using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TaffyScriptCompiler.Backend
{
    public class MsilWeakCompiler : Builder
    {
        public MsilWeakCompiler()
        {
        }

        /// <summary>
        /// Compiles a TaffyScript project.
        /// </summary>
        /// <param name="projectDir">The directory of the TaffyScript project.</param>
        /// <returns></returns>
        public override CompilerResult CompileProject(string projectDir)
        {
            var config = GetBuildConfig(projectDir, out var exception);
            if(config == null)
                return new CompilerResult(null, null, exception);

            var dir = Path.Combine(projectDir, Path.GetDirectoryName(config.Output));
            Directory.CreateDirectory(dir);

            config.Output = Path.GetFileName(config.Output);
            var expectedOutput = Path.Combine(dir, config.Output);

            var errors = VerifyReferencesExists(projectDir, dir, config);
            if (errors.Count != 0)
                return new CompilerResult(errors);

            var parser = new Parser();
            ParseFilesInProjectDirectory(projectDir, parser, new HashSet<string>());
            if(parser.Errors.Count != 0)
                return new CompilerResult(parser.Errors);

            var generator = new MsilWeakCodeGen(parser.Table, config);
            var result = generator.CompileTree(parser.Tree);
            if (result.Errors.Count > 0)
                return result;
            else
            {
                expectedOutput += Path.GetExtension(result.PathToAssembly);
                CopyFileIfNewer(typeof(TaffyScript.TsObject).Assembly.Location, Path.Combine(dir, typeof(TaffyScript.TsObject).Assembly.GetName().Name + ".dll"));
                if (result.PathToAssembly != expectedOutput)
                {
                    if (File.Exists(expectedOutput))
                        File.Delete(expectedOutput);
                    MoveFile(result.PathToAssembly, expectedOutput);
                    return new CompilerResult(result.CompiledAssebmly, expectedOutput, result.Errors.ToArray());
                }
            }
            return result;
        }

        /// <summary>
        /// Compiles a string comprised of TaffyScript code using the default settings.
        /// </summary>
        /// <param name="code">The TaffyScript code to compile.</param>
        /// <param name="outputName">The name of the output file. Can be a path.</param>
        /// <returns></returns>
        public override CompilerResult CompileCode(string code, string outputName)
        {
            var config = new BuildConfig()
            {
                Mode = CompileMode.Debug,
                Output = outputName
            };
            return CompileCode(code, config);
        }

        /// <summary>
        /// Compiles a string comprised of TaffyScript code using the config settings.
        /// </summary>
        /// <param name="code">The TaffyScript code to compile.</param>
        /// <param name="config">The config to use.</param>
        /// <returns></returns>
        public override CompilerResult CompileCode(string code, BuildConfig config)
        {
            var dir = Path.GetDirectoryName(config.Output);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            config.Output = Path.GetFileName(config.Output);
            var expectedOutput = Path.Combine(dir, config.Output);

            //var errors = new List<Exception>();
            var errors = VerifyReferencesExists(GetType().Assembly.Location, dir, config);
            if (errors.Count != 0)
                return new CompilerResult(errors);

            var parser = new Parser();
            parser.Parse(code);
            if (parser.Errors.Count != 0)
                return new CompilerResult(parser.Errors);

            var generator = new MsilWeakCodeGen(parser.Table, config);
            var result = generator.CompileTree(parser.Tree);
            if (result.Errors.Count > 0)
                return result;
            else
            {
                expectedOutput += Path.GetExtension(result.PathToAssembly);
                CopyFileIfNewer(typeof(TaffyScript.TsObject).Assembly.Location, Path.Combine(dir, typeof(TaffyScript.TsObject).Assembly.GetName().Name + ".dll"));
                if (result.PathToAssembly != expectedOutput)
                {
                    MoveFile(result.PathToAssembly, expectedOutput);
                    return new CompilerResult(result.CompiledAssebmly, expectedOutput, result.Errors.ToArray());
                }
            }
            return result;
        }
    }
}
