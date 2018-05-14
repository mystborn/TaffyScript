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
        public abstract CompilerResult CompileCode(string code, BuildConfig config);
        public abstract CompilerResult CompileCode(string code, string output);
        public abstract CompilerResult CompileProject(string projectDir);

        protected BuildConfig GetBuildConfig(string projectDir, out Exception exception)
        {
            if (!Directory.Exists(projectDir))
            {
                exception = new DirectoryNotFoundException($"Could not find the project directory {projectDir}");
                return null;
            }

            var projectFile = Path.Combine(projectDir, "build.cfg");
            if (!File.Exists(Path.Combine(projectDir, "build.cfg")))
            {
                exception = new FileNotFoundException("Could not find the project file.");
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
                catch(Exception e)
                {
                    exception = e;
                    return null;
                }
            }

            exception = null;

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

        protected List<Exception> VerifyReferencesExists(string projectDir, string outputDir, BuildConfig config)
        {
            var errors = new List<Exception>();
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
            return errors;
        }

        protected List<Exception> VerifyReferencesExists(string projectDir, Action<string> onReference, BuildConfig config)
        {
            //Todo: Look for assemblies in the global assembly cache
            //Update: After some tinkering, it seems this is not easily achieved.
            //        The best way to determine if an assembly exists is to use gacutil,
            //        however, I cannot figure out a way to find a path to the asm.
            //        You could just force search %windir%\Microsoft.NET\assembly
            //        but you'd have top at least guess that one folder will be the correct one.
            //        Still, here is some psuedo code to do so.
            //        I'm not currently using it becuase it's not stable enough.
            //        Still, it could be useful. Maybe used when a compile option is specified

            /*
            var cpuArchitecture = "64";
            var folderName = Path.GetFileNameWithoutExtension(config.References[i]);
            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "assembly");
            var archPath = Path.Combine(basePath, $"GAC_{cpuArchitecture}", folderName);
            var msilPath = Path.Combine(basePath, "GAC_MSIL", folderName);
            if (Directory.Exists(archPath))
            {
                asmExpectedLocation = Path.Combine(archPath, Directory.EnumerateDirectories(archPath).First(), config.References[i]);
            }
            else if(Directory.Exists(msilPath))
            {
                asmExpectedLocation = Path.Combine(msilPath, Directory.EnumerateDirectories(msilPath).First(), config.References[i]);
            }
            */

            var errors = new List<Exception>();
            for(var i = 0; i < config.References.Count; i++)
            {
                var asmExpectedLocation = Path.Combine(projectDir, config.References[i]);
                if (!File.Exists(asmExpectedLocation))
                {
                    asmExpectedLocation = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Libraries", config.References[i]);
                    if (!File.Exists(asmExpectedLocation))
                        errors.Add(new FileNotFoundException($"Could not find the specified reference: {config.References[i]}"));
                    else
                        onReference?.Invoke(asmExpectedLocation);
                }
                else
                    onReference?.Invoke(asmExpectedLocation);
            }
            return errors;
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
