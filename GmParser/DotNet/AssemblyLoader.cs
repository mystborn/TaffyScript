using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaffyScript.DotNet
{
    public class DotNetAssemblyLoader
    {
        private Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();
        private Dictionary<string, List<Assembly>> _namespaces = new Dictionary<string, List<Assembly>>();

        public Dictionary<string, Assembly> LoadedAssemblies => _loadedAssemblies;
        public Dictionary<string, List<Assembly>> Namespaces => _namespaces;

        public DotNetAssemblyLoader(params string[] assemblies)
        {
            foreach (var asm in assemblies)
                LoadAssembly(asm);
        }

        public Assembly LoadAssembly(string asmPath)
        {
            if (_loadedAssemblies.ContainsKey(asmPath))
                throw new InvalidOperationException();

            var asm = Assembly.LoadFrom(asmPath);

            InitializeAssembly(asm);

            return asm;
        }

        public void InitializeAssembly(Assembly asm)
        {
            _loadedAssemblies[asm.Location] = asm;
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
