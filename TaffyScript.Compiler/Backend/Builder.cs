using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaffyScript.Compiler.Backend
{
    /// <summary>
    /// Base class for any TaffyScript project builder.
    /// </summary>
    public abstract class Builder
    {
        protected IErrorLogger _logger;

        public abstract CompilerResult CompileCode(string code, BuildConfig config);
        public abstract CompilerResult CompileCode(string code, string output);
        public abstract CompilerResult CompileProject(string projectDir);

        public Builder(IErrorLogger errorLogger)
        {
            _logger = errorLogger;
        }

        protected BuildConfig GetBuildConfig(string projectDir)
        {
            if (!Directory.Exists(projectDir))
            {
                _logger.Error($"Could not find the project directory {projectDir}", null);
                return null;
            }

            var projectFile = Path.Combine(projectDir, "build.cfg");
            if (!File.Exists(Path.Combine(projectDir, "build.cfg")))
            {
                _logger.Error("Could not find build.cfg", null);
                return null;
            }

            BuildConfig config = null;
            using (var sr = new StreamReader(projectFile))
            {
                var cereal = new XmlSerializer(typeof(BuildConfig));
                try
                {
                    config = (BuildConfig)cereal.Deserialize(sr);
                }
                catch
                {
                    _logger.Error("Could not deserialize build.cfg. Make sure the file is properly formatted.", null);
                    return null;
                }
            }
            return config;
        }

        protected HashSet<string> GetExcludeSet(string projectDir, BuildConfig config)
        {
            var excludes = new HashSet<string>();
            for(var i = 0; i < config.Excludes.Count; i++)
            {
                var file = Path.Combine(projectDir, config.Excludes[i]);
                if (Path.GetExtension(file) == "")
                    file = file + ".tfs";
                excludes.Add(file);
            }
            return excludes;
        }
        
        protected void ParseFilesInProjectDirectory(string directory, Parser parser, HashSet<string> exclude)
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.tfs").Where(f => !exclude.Contains(f)))
                parser.ParseFile(file);

            foreach (var dir in Directory.EnumerateDirectories(directory))
                ParseFilesInProjectDirectory(dir, parser, exclude);
        }

        protected void VerifyReferencesExists(string projectDir, string outputDir, BuildConfig config)
        {
            for (var i = 0; i < config.References.Count; i++)
            {
                // Todo: Determine if reference is a full or relative path.
                //       This currently works because Path.Combine just takes the last parameter if it's a full
                //       path it seems. This will probably result in an error if ran on Linux.
                var find = Path.Combine(projectDir, config.References[i]);
                var output = Path.Combine(outputDir, Path.GetFileName(config.References[i]));
                if (!File.Exists(find))
                {
                    find = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Libraries", config.References[i]);
                    if (!File.Exists(find))
                    {
#if WINDOWS
                        // This searches the Global Assembly Cache for the desired reference.
                        find = null;
                        var folderName = Path.GetFileNameWithoutExtension(config.References[i]);
                        var gacPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.Net", "assembly");
                        var msilPath = Path.Combine(gacPath, "GAC_MSIL", folderName);
                        var archPath = Path.Combine(gacPath, $"GAC_{(config.Target == TargetCpu.x64 ? "64" : "32")}", folderName);

                        if (Directory.Exists(msilPath))
                            find = Path.Combine(msilPath, Directory.EnumerateDirectories(msilPath).First(), config.References[i]);
                        else if (Directory.Exists(archPath))
                            find = Path.Combine(archPath, Directory.EnumerateDirectories(archPath).First(), config.References[i]);

                        if (find != null && File.Exists(find))
                            CopyFileIfNewer(find, output);
                        else
                            _logger.Error($"Could not find the specified reference: {config.References[i]}", null);
#else
                        _logger.Error($"Could not find the specified reference: {config.References[i]}", null);
#endif
                    }
                    else
                        CopyFileIfNewer(find, output);
                }
                else if (find != output)
                    CopyFileIfNewer(find, output);
                config.References[i] = find;
            }
        }

        protected void MoveFile(string source, string dest)
        {
            var ext = Path.GetExtension(source);
            if (ext != ".pdb")
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

        protected void CopyFileIfNewer(string source, string dest)
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
