using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TaffyScriptCompiler.DotNet
{
    public class DotNetAssemblyLoader
    {
        private Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();
        private Dictionary<string, List<Assembly>> _namespaces = new Dictionary<string, List<Assembly>>();

        /// <summary>
        /// Gets all of the currently loaded assemblies.
        /// </summary>
        public Dictionary<string, Assembly> LoadedAssemblies => _loadedAssemblies;

        /// <summary>
        /// Gets a dictionary that maps namespaces to the assemblies that define the namespace.
        /// </summary>
        public Dictionary<string, List<Assembly>> Namespaces => _namespaces;

        /// <summary>
        /// Creates a new <see cref="DotNetAssemblyLoader"/> and loads all of the given assembly files.
        /// </summary>
        /// <param name="assemblies"></param>
        public DotNetAssemblyLoader(params string[] assemblies)
        {
            foreach (var asm in assemblies)
                LoadAssembly(asm);
        }

        /// <summary>
        /// Loads an assembly from a file and returns the result.
        /// </summary>
        /// <param name="asmPath">The path to an assembly file.</param>
        public Assembly LoadAssembly(string asmPath)
        {
            if (_loadedAssemblies.ContainsKey(asmPath))
            {
                throw new InvalidOperationException($"Duplicate assembly found: {asmPath}");
            }

            var asm = Assembly.LoadFrom(asmPath);

            InitializeAssembly(asm);

            return asm;
        }

        /// <summary>
        /// Initializes a loaded assembly. 
        /// <para>
        /// If the assembly was loaded via the LoadAssembly method, do not call this method.
        /// </para>
        /// </summary>
        /// <param name="asm"></param>
        public void InitializeAssembly(Assembly asm)
        {
            _loadedAssemblies[asm.GetName().Name] = asm;
            var namespaces = asm.GetExportedTypes()
                                .Select(t => t.Namespace)
                                .Where(t => t != null)
                                .Distinct();

            foreach (var ns in namespaces)
            {
                if (!_namespaces.ContainsKey(ns))
                    _namespaces.Add(ns, new List<Assembly>());

                _namespaces[ns].Add(asm);
            }
        }
    }
}
