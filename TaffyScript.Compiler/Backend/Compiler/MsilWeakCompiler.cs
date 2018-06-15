using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TaffyScript.Compiler.FrontEnd;
using TaffyScript.Compiler.Syntax;

namespace TaffyScript.Compiler.Backend
{
    public class MsilWeakCompiler : Builder
    {
        public MsilWeakCompiler(IErrorLogger errorLogger)
            : base(errorLogger)
        {
        }

        /// <summary>
        /// Compiles a TaffyScript project.
        /// </summary>
        /// <param name="projectDir">The directory of the TaffyScript project.</param>
        /// <returns></returns>
        public override CompilerResult CompileProject(string projectDir)
        {
            var config = GetBuildConfig(projectDir);
            if(config == null)
                return new CompilerResult(_logger);

            if (string.IsNullOrEmpty(config.Output))
            {
                _logger.Error("The config file must contain an output path.", null);
                return new CompilerResult(_logger);
            }

            var dir = Path.Combine(projectDir, Path.GetDirectoryName(config.Output));
            Directory.CreateDirectory(dir);

            config.Output = Path.GetFileName(config.Output);
            var expectedOutput = Path.Combine(dir, config.Output);

            VerifyReferencesExists(projectDir, dir, config);
            if (_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            var table = new SymbolTable();
            var root = new RootNode();
            var parser = new Parser(_logger, table, root);
            ParseFilesInProjectDirectory(projectDir, parser, GetExcludeSet(projectDir, config));
            if(_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            var resolver = new Resolver(_logger, table);
            resolver.Resolve(root);
            if (_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            var generator = new MsilWeakCodeGen(table, config, _logger);
            var result = generator.CompileTree(root);
            if (result.Errors.Count > 0)
                return result;
            else
            {
                expectedOutput += Path.GetExtension(result.PathToAssembly);
                CopyFileIfNewer(typeof(TsObject).Assembly.Location, Path.Combine(dir, typeof(TsObject).Assembly.GetName().Name + ".dll"));
                if (result.PathToAssembly != expectedOutput)
                {
                    if (File.Exists(expectedOutput))
                        File.Delete(expectedOutput);
                    MoveFile(result.PathToAssembly, expectedOutput);
                    return new CompilerResult(result.CompiledAssebmly, expectedOutput, _logger);
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
            VerifyReferencesExists(GetType().Assembly.Location, dir, config);
            if (_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            var table = new SymbolTable();
            var root = new RootNode();
            var parser = new Parser(_logger, table, root);
            parser.Parse(code);
            if (_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            var resolver = new Resolver(_logger, table);
            resolver.Resolve(root);
            if (_logger.Errors.Count != 0)
                return new CompilerResult(_logger);

            var generator = new MsilWeakCodeGen(table, config, _logger);
            var result = generator.CompileTree(root);
            if (result.Errors.Count > 0)
                return result;
            else
            {
                expectedOutput += Path.GetExtension(result.PathToAssembly);
                CopyFileIfNewer(typeof(TaffyScript.TsObject).Assembly.Location, Path.Combine(dir, typeof(TaffyScript.TsObject).Assembly.GetName().Name + ".dll"));
                if (result.PathToAssembly != expectedOutput)
                {
                    MoveFile(result.PathToAssembly, expectedOutput);
                    return new CompilerResult(result.CompiledAssebmly, expectedOutput, _logger);
                }
            }
            return result;
        }
    }
}
