using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TaffyScriptCompiler.Backend
{
    public class MsilWeakCompiler : ICompiler
    {
        public MsilWeakCompiler()
        {
        }

        /// <summary>
        /// Compiles a TaffyScript project.
        /// </summary>
        /// <param name="projectDir">The directory of the TaffyScript project.</param>
        /// <returns></returns>
        public CompilerResult CompileProject(string projectDir)
        {
            if (!Directory.Exists(projectDir))
                return new CompilerResult(null, null, new DirectoryNotFoundException($"Could not find the project directory {projectDir}"));

            var projectFile = Path.Combine(projectDir, "build.cfg");
            if (!File.Exists(Path.Combine(projectDir, "build.cfg")))
                return new CompilerResult(null, null, new FileNotFoundException("Could not find the project file."));

            BuildConfig config;
            using(var sr = new StreamReader(projectFile))
            {
                var cereal = new XmlSerializer(typeof(BuildConfig));
                config = (BuildConfig)cereal.Deserialize(sr);
            }

            var dir = Path.Combine(projectDir, Path.GetDirectoryName(config.Output));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            config.Output = Path.GetFileName(config.Output);
            var expectedOutput = Path.Combine(dir, config.Output);

            var errors = new List<Exception>();
            VerifyReferencesExists(projectDir, dir, config, errors);
            if (errors.Count != 0)
                return new CompilerResult(errors);

            var parser = new Parser();
            EnumerateDirectories(projectDir, parser, new HashSet<string>());
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
        public CompilerResult CompileCode(string code, string outputName)
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
        public CompilerResult CompileCode(string code, BuildConfig config)
        {
            var dir = Path.GetDirectoryName(config.Output);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            config.Output = Path.GetFileName(config.Output);
            var expectedOutput = Path.Combine(dir, config.Output);

            var errors = new List<Exception>();
            VerifyReferencesExists(GetType().Assembly.Location, dir, config, errors);
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

        private void EnumerateDirectories(string directory, Parser parser, HashSet<string> exclude)
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.tfs").Where(f => !exclude.Contains(f)))
                parser.ParseFile(file);

            foreach (var dir in Directory.EnumerateDirectories(directory))
                EnumerateDirectories(dir, parser, exclude);
        }

        private void VerifyReferencesExists(string projectDir, string outputDir, BuildConfig config, List<Exception> errors)
        {
            for (var i = 0; i < config.References.Count; i++)
            {
                var find = Path.Combine(projectDir, config.References[i]);
                var output = Path.Combine(outputDir, Path.GetFileName(config.References[i]));
                if (!File.Exists(find))
                {
                    find = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Libraries", config.References[i]);
                    if (!File.Exists(find))
                        errors.Add(new FileNotFoundException($"Could not find the specified reference: {config.References[i]}"));
                    else
                        CopyFileIfNewer(find, output);
                }
                else if (find != output)
                    CopyFileIfNewer(find, output);
                config.References[i] = find;
            }
        }

        private void MoveFile(string source, string dest)
        {
            var ext = Path.GetExtension(source);
            if(ext != ".pdb")
            {
                var pdb = Path.Combine(Path.GetDirectoryName(source), Path.GetFileNameWithoutExtension(source) + ".pdb");
                if (File.Exists(pdb))
                {
                    var destPdb = Path.Combine(Path.GetDirectoryName(dest), Path.GetFileNameWithoutExtension(dest) + ".pdb");
                    MoveFile(pdb, destPdb);
                }
            }
            if (source == dest)
                return;
            if (File.Exists(dest))
                File.Delete(dest);
            File.Move(source, dest);
        }

        private void CopyFileIfNewer(string source, string dest)
        {
            var ext = Path.GetExtension(source);
            if (ext != ".pdb")
            {
                var pdb = Path.Combine(Path.GetDirectoryName(source), Path.GetFileNameWithoutExtension(source) + ".pdb");
                if (File.Exists(pdb))
                {
                    var destPdb = Path.Combine(Path.GetDirectoryName(dest), Path.GetFileNameWithoutExtension(dest) + ".pdb");
                    CopyFileIfNewer(pdb, destPdb);
                }
            }
            if (source == dest)
                return;
            if (File.Exists(dest))
            {
                var srcTime = File.GetLastWriteTime(source);
                var destTime = File.GetLastWriteTime(dest);
                var compare = srcTime.CompareTo(destTime);
                if (srcTime.CompareTo(destTime) != 0)
                {
                    File.Copy(source, dest, true);
                    File.SetLastWriteTime(dest, srcTime);
                }
            }
            else
            {
                File.Copy(source, dest);
            }
        }
    }
}
