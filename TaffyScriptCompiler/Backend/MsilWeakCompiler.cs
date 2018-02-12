using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TaffyScript.Backend
{
    public class MsilWeakCompiler
    {
        public MsilWeakCompiler()
        {
        }

        public CompilerResult CompileProject(string projectDir)
        {
            if (!Directory.Exists(projectDir))
                return new CompilerResult(null, null, new DirectoryNotFoundException($"Could not find the project directory {projectDir}"));

            var projectFile = Path.Combine(projectDir, "config.cfg");
            if (!File.Exists(Path.Combine(projectDir, "config.cfg")))
                return new CompilerResult(null, null, new FileNotFoundException("Could not find the project file."));

            MsilWeakBuildConfig config;
            using(var sr = new StreamReader(projectFile))
            {
                var cereal = new XmlSerializer(typeof(MsilWeakBuildConfig));
                config = (MsilWeakBuildConfig)cereal.Deserialize(sr);
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
                CopyFileIfNewer(typeof(GmExtern.GmObject).Assembly.Location, Path.Combine(dir, typeof(GmExtern.GmObject).Assembly.GetName().Name + ".dll"));
                if (result.PathToAssembly != expectedOutput)
                {
                    if (File.Exists(expectedOutput))
                        File.Delete(expectedOutput);
                    File.Move(result.PathToAssembly, expectedOutput);
                    return new CompilerResult(result.CompiledAssebmly, expectedOutput, result.Errors.ToArray());
                }
            }
            return result;
        }

        public CompilerResult CompileCode(string code, string outputName)
        {
            var config = new MsilWeakBuildConfig()
            {
                Mode = CompileMode.Debug,
                Output = outputName
            };
            return CompileCode(code, config);
        }

        public CompilerResult CompileCode(string code, MsilWeakBuildConfig config)
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
                CopyFileIfNewer(typeof(GmExtern.GmObject).Assembly.Location, Path.Combine(dir, typeof(GmExtern.GmObject).Assembly.GetName().Name + ".dll"));
                if (result.PathToAssembly != expectedOutput)
                {
                    if (File.Exists(expectedOutput))
                        File.Delete(expectedOutput);
                    File.Move(result.PathToAssembly, expectedOutput);
                    return new CompilerResult(result.CompiledAssebmly, expectedOutput, result.Errors.ToArray());
                }
            }
            return result;
        }

        private void EnumerateDirectories(string directory, Parser parser, HashSet<string> exclude)
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.tf").Where(f => !exclude.Contains(f)))
                parser.ParseFile(file);

            foreach (var dir in Directory.EnumerateDirectories(directory))
                EnumerateDirectories(dir, parser, exclude);
        }

        private void VerifyReferencesExists(string projectDir, string outputDir, MsilWeakBuildConfig config, List<Exception> errors)
        {
            for (var i = 0; i < config.References.Count; i++)
            {
                var find = Path.Combine(projectDir, config.References[i]);
                var output = Path.Combine(outputDir, Path.GetFileName(config.References[i]));
                if (!File.Exists(find))
                {
                    find = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TaffyScript", "Libraries", config.References[i]);
                    if (!File.Exists(find))
                        errors.Add(new FileNotFoundException($"Could not find the specified reference: {config.References[i]}."));
                    else
                        CopyFileIfNewer(find, output);
                }
                else if (find != output)
                    CopyFileIfNewer(find, output);
                config.References[i] = find;
            }
        }

        private void CopyFileIfNewer(string source, string dest)
        {
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
