using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Documentation
{
    public class TypeCache
    {
        private Dictionary<string, ObjectDocumentation> _cache = new Dictionary<string, ObjectDocumentation>();
        private Dictionary<string, Type> _baseTypes = new Dictionary<string, Type>();
        private Dictionary<string, NamespaceDocumentation> _namespaces = new Dictionary<string, NamespaceDocumentation>();
        private Dictionary<string, NamespaceDocumentation> _baseTypeNamespaces = new Dictionary<string, NamespaceDocumentation>();

        public TypeCache()
        {
            var asm = typeof(TsObject).Assembly;
            TaffyScriptBaseTypeAttribute attrib;

            foreach(var type in asm.GetTypes())
            {
                if(type != typeof(ITsInstance) && typeof(ITsInstance).IsAssignableFrom(type))
                {
                    _cache.Add(type.FullName, new ObjectDocumentation() { Type = type });
                }
                else if((attrib = type.GetCustomAttribute<TaffyScriptBaseTypeAttribute>()) != null)
                {
                    var ns = attrib.Name ?? type.Namespace;
                    _baseTypes.Add(type.FullName, type);
                    _baseTypeNamespaces.Add(type.FullName, GetOrCreateNamespace(ns));
                }
            }
        }

        public bool TryGetDocumentation(string typeName, out ObjectDocumentation documentation)
        {
            return _cache.TryGetValue(typeName, out documentation);
        }

        public bool TryGetNamespaceFromTypeName(string ns, out NamespaceDocumentation documentation)
        {
            return _baseTypeNamespaces.TryGetValue(ns, out documentation);
        }

        public bool TryGetBaseType(string typeName, out Type baseType)
        {
            return _baseTypes.TryGetValue(typeName, out baseType);
        }

        public NamespaceDocumentation GetNamespace(string ns)
        {
            return GetOrCreateNamespace(ns);
        }

        private NamespaceDocumentation GetOrCreateNamespace(string ns)
        {
            if(!_namespaces.TryGetValue(ns, out var docs))
            {
                docs = new NamespaceDocumentation() { Name = ns };
                _namespaces.Add(ns, docs);
                var index = ns.LastIndexOf('.');
                if (index != -1)
                {
                    var parent = GetOrCreateNamespace(ns.Substring(0, index));
                    parent.Namespaces.Add(docs);
                }
                else if(ns != "")
                {
                    var parent = GetOrCreateNamespace("");
                    parent.Namespaces.Add(docs);
                }
            }

            return docs;
        }
    }
}
