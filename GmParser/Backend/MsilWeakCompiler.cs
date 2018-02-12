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
                throw new DirectoryNotFoundException($"Could not find the project directory {projectDir}");

            var projectFile = Path.Combine(projectDir, "config.cfg");
            if (!File.Exists(Path.Combine(projectDir, "config.cfg")))
                throw new FileNotFoundException("Could not find the project file.");

            MsilWeakBuildConfig config;
            using(var sr = new StreamReader(projectFile))
            {
                var cereal = new XmlSerializer(typeof(MsilWeakBuildConfig));
                config = (MsilWeakBuildConfig)cereal.Deserialize(sr);
            }

            VerifyReferencesExists(projectDir, config);
            var parser = new Parser();
            EnumerateDirectories(projectDir, parser, new HashSet<string>());
            if(parser.Errors.Count != 0)
            {
                return new CompilerResult(null, null, parser.Errors.ToArray());
            }

            var generator = new MsilWeakCodeGen(parser.Table, config);
            return generator.CompileTree(parser.Tree);
        }

        private void EnumerateDirectories(string directory, Parser parser, HashSet<string> exclude)
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.tf").Where(f => !exclude.Contains(f)))
                parser.ParseFile(file);

            foreach (var dir in Directory.EnumerateDirectories(directory))
                EnumerateDirectories(dir, parser, exclude);
        }

        private void VerifyReferencesExists(string projectDir, MsilWeakBuildConfig config)
        {
            for(var i = 0; i < config.References.Count; i++)
            {
                var find = Path.Combine(projectDir, config.References[i]);
                if (!File.Exists(find))
                {
                    find = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Taffy", "Libraries", config.References[i]);
                    if(!File.Exists(find))
                        throw new FileNotFoundException($"Could not find the specified reference: {config.References[i]}.");
                }
                config.References[i] = find;
            }
        }

        public void CompileCode(string code, string outputName)
        {
            var config = new MsilWeakBuildConfig()
            {
                Mode = CompileMode.Debug,
                Output = outputName
            };
            var parser = new Parser();
            parser.Parse(code);
            var generator = new MsilWeakCodeGen(parser.Table, config);
            generator.CompileTree(parser.Tree);
        }

        public void CompileCode(string code, MsilWeakBuildConfig config)
        {
            var parser = new Parser();
            parser.Parse(code);
            var generator = new MsilWeakCodeGen(parser.Table, config);
            generator.CompileTree(parser.Tree);
        }
    }
}
