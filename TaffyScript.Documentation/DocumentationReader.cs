using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Documentation
{
    public class DocumentationReader
    {
        const string CorrectArguments = "(TaffyScript.TsObject[])";
        const string ConstructorDecorator = ".#ctor";

        TypeCache _cache;

        public DocumentationReader()
        {
            _cache = new TypeCache();
        }

        public async Task<TypeCache> ReadDocumentation()
        {
            var settings = new XmlReaderSettings()
            {
                Async = true,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
            };

            using (var reader = XmlReader.Create(@"C:\Users\Chris\Source\Repos\GmParser\TaffyScript\bin\Release\TaffyScript.xml", settings))
            {
                try
                {
                    reader.ReadToFollowing("members");
                    reader.ReadStartElement();
                    int index;
                    NamespaceDocumentation ns;
                    while (reader.Name == "member")
                    {
                        var name = reader["name"];
                        if (name is null)
                            continue;

                        var memberType = name.Substring(0, 1);
                        var memberName = name.Remove(0, 2);

                        switch (memberType)
                        {
                            case "T":
                                if (_cache.TryGetDocumentation(memberName, out var documentation))
                                {
                                    var nameAttrib = documentation.Type.GetCustomAttribute<TaffyScriptObjectAttribute>();
                                    string fullName;
                                    if (nameAttrib?.Name is null)
                                        fullName = documentation.Type.FullName;
                                    else
                                        fullName = nameAttrib.Name;
                                    index = fullName.LastIndexOf('.');

                                    if (index == -1)
                                        ns = _cache.GetNamespace("");
                                    else
                                        ns = _cache.GetNamespace(fullName.Substring(0, index));

                                    documentation.Name = fullName.Substring(++index, fullName.Length - index);

                                    reader.ReadStartElement();
                                    while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "member")
                                    {
                                        switch (reader.Name)
                                        {
                                            case "summary":
                                                documentation.Summary = await ReadSummary(reader);
                                                break;
                                            case "source":
                                                documentation.Source = await reader.ReadElementContentAsStringAsync();
                                                break;
                                            case "property":
                                                var pName = reader["name"];
                                                var pType = reader["type"];
                                                var access = reader["access"];
                                                string pSummary = null, pSource = null;
                                                reader.ReadStartElement();
                                                while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "property")
                                                {
                                                    switch (reader.Name)
                                                    {
                                                        case "summary":
                                                            pSummary = await ReadSummary(reader);
                                                            break;
                                                        case "source":
                                                            pSource = await reader.ReadElementContentAsStringAsync();
                                                            break;
                                                        default:
                                                            reader.Skip();
                                                            break;
                                                    }
                                                }
                                                reader.ReadEndElement();
                                                documentation.Properties.Add(new PropertyDocumentation()
                                                {
                                                    Name = pName,
                                                    Type = pType,
                                                    Access = access,
                                                    Summary = pSummary,
                                                    Source = pSource
                                                });
                                                break;
                                            case "field":
                                                var fName = reader["name"];
                                                var fType = reader["type"];
                                                string fSummary = null, fSource = null;
                                                reader.ReadStartElement();
                                                while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "field")
                                                {
                                                    switch (reader.Name)
                                                    {
                                                        case "summary":
                                                            fSummary = await ReadSummary(reader);
                                                            break;
                                                        case "source":
                                                            fSource = await reader.ReadElementContentAsStringAsync();
                                                            break;
                                                        default:
                                                            reader.Skip();
                                                            break;
                                                    }
                                                }
                                                reader.ReadEndElement();
                                                documentation.Fields.Add(new FieldDocumentation()
                                                {
                                                    Name = fName,
                                                    Type = fType,
                                                    Summary = fSummary,
                                                    Source = fSource
                                                });
                                                break;
                                        }
                                    }
                                    reader.ReadEndElement();
                                    ns.Objects.Add(documentation);
                                }
                                else if (_cache.TryGetNamespaceFromTypeName(memberName, out ns))
                                {
                                    reader.ReadStartElement();
                                    while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "member")
                                    {
                                        switch (reader.Name)
                                        {
                                            case "summary":
                                                ns.Summary = await ReadSummary(reader);
                                                break;
                                            default:
                                                reader.Skip();
                                                break;
                                        }
                                    }
                                    reader.ReadEndElement();
                                }
                                else
                                {
                                    reader.Skip();
                                    break;
                                }
                                break;
                            case "P":
                                index = memberName.LastIndexOf('.');
                                name = memberName.Substring(0, index);
                                if (!_cache.TryGetDocumentation(name, out documentation))
                                {
                                    reader.Skip();
                                    break;
                                }
                                name = memberName.Substring(++index, memberName.Length - index);
                                var propertyInfo = documentation.Type.GetProperty(name,
                                                                                  BindingFlags.Public |
                                                                                      BindingFlags.NonPublic |
                                                                                      BindingFlags.Static |
                                                                                      BindingFlags.Instance,
                                                                                  null,
                                                                                  typeof(TsObject),
                                                                                  Type.EmptyTypes,
                                                                                  null);
                                if (propertyInfo is null)
                                {
                                    reader.Skip();
                                    break;
                                }

                                var property = new PropertyDocumentation() { Name = name };
                                property.Access = propertyInfo.CanRead && propertyInfo.CanWrite ? "both" : (propertyInfo.CanRead ? "get" : "set");
                                reader.ReadStartElement();
                                while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "member")
                                {
                                    switch (reader.Name)
                                    {
                                        case "summary":
                                            property.Summary = await ReadSummary(reader);
                                            break;
                                        case "source":
                                            property.Source = await reader.ReadElementContentAsStringAsync();
                                            break;
                                        case "type":
                                            property.Type = await reader.ReadElementContentAsStringAsync();
                                            break;
                                        default:
                                            reader.Skip();
                                            break;
                                    }
                                }
                                reader.ReadEndElement();
                                documentation.Properties.Add(property);
                                break;
                            case "M":
                                if (!memberName.EndsWith(CorrectArguments))
                                {
                                    reader.Skip();
                                    break;
                                }
                                memberName = memberName.Remove(memberName.Length - CorrectArguments.Length);
                                if (memberName.EndsWith(ConstructorDecorator))
                                {
                                    name = memberName.Remove(memberName.Length - ConstructorDecorator.Length);
                                    if (!_cache.TryGetDocumentation(name, out documentation))
                                    {
                                        reader.Skip();
                                        break;
                                    }
                                    var constructorInfo = documentation.Type.GetConstructor(new[] { typeof(TsObject[]) });
                                    if (constructorInfo is null)
                                    {
                                        reader.Skip();
                                        break;
                                    }
                                    var constructor = new ConstructorDocumentation();
                                    reader.ReadStartElement();
                                    while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "member")
                                    {
                                        switch (reader.Name)
                                        {
                                            case "summary":
                                                constructor.Summary = await ReadSummary(reader);
                                                break;
                                            case "arg":
                                                constructor.Arguments.Add(await ReadArgument(reader));
                                                break;
                                            case "source":
                                                constructor.Source = await reader.ReadElementContentAsStringAsync();
                                                break;
                                            default:
                                                reader.Skip();
                                                break;
                                        }
                                    }
                                    reader.ReadEndElement();
                                    documentation.Constructor = constructor;
                                }
                                else
                                {
                                    index = memberName.LastIndexOf('.');
                                    name = memberName.Substring(0, index);
                                    if (_cache.TryGetDocumentation(name, out documentation))
                                    {
                                        name = memberName.Substring(++index, memberName.Length - index);
                                        var methodInfo = documentation.Type.GetMethod(name,
                                                                                      BindingFlags.Public |
                                                                                          BindingFlags.NonPublic |
                                                                                          BindingFlags.Static |
                                                                                          BindingFlags.Instance,
                                                                                      null,
                                                                                      new[] { typeof(TsObject[]) },
                                                                                      null);
                                        if (methodInfo is null)
                                        {
                                            reader.Skip();
                                            break;
                                        }
                                        documentation.Scripts.Add(await ReadScript(reader, methodInfo));
                                    }
                                    else if (_cache.TryGetNamespaceFromTypeName(name, out ns) && _cache.TryGetBaseType(name, out var baseType))
                                    {
                                        name = memberName.Substring(++index, memberName.Length - index);
                                        var methodInfo = baseType.GetMethod(name,
                                                                            BindingFlags.Public |
                                                                                BindingFlags.Static,
                                                                            null,
                                                                            new[] { typeof(TsObject[]) },
                                                                            null);

                                        if (methodInfo is null)
                                        {
                                            reader.Skip();
                                            break;
                                        }
                                        ns.Scripts.Add(await ReadScript(reader, methodInfo));
                                    }
                                    else
                                    {
                                        reader.Skip();
                                        break;
                                    }
                                }
                                break;
                            case "F":
                                reader.Skip();
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            HandleInheritance();
            return _cache;
        }

        private async Task<ScriptDocumentation> ReadScript(XmlReader reader, MethodInfo method)
        {
            var script = new ScriptDocumentation()
            {
                Name = method.Name,
                Method = method
            };

            reader.ReadStartElement();
            while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "member")
            {
                switch (reader.Name)
                {
                    case "summary":
                        script.Summary = await ReadSummary(reader);
                        break;
                    case "arg":
                        script.Arguments.Add(await ReadArgument(reader));
                        break;
                    case "source":
                        script.Source = await reader.ReadElementContentAsStringAsync();
                        break;
                    case "returns":
                        script.Returns = await reader.ReadElementContentAsStringAsync();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
            reader.ReadEndElement();
            return script;
        }

        private async Task<string> ReadSummary(XmlReader reader)
        {
            var summary = await reader.ReadElementContentAsStringAsync();
            return summary.Trim();
        }

        private async Task<ArgumentDocumentation> ReadArgument(XmlReader reader)
        {
            var name = reader["name"];
            var type = reader["type"];
            var summary = await ReadSummary(reader);
            return new ArgumentDocumentation()
            {
                Name = name,
                Type = type,
                Summary = summary
            };
        }

        private void HandleInheritance()
        {
            var globalNamespace = _cache.GetNamespace("");
            var processed = new HashSet<ObjectDocumentation>();
            var stack = new Stack<NamespaceDocumentation>();
            stack.Push(globalNamespace);
            while(stack.Count > 0)
            {
                var ns = stack.Pop();
                processed.UnionWith(ns.Objects);
                foreach (var child in ns.Namespaces)
                    stack.Push(child);
            }

            while(processed.Count > 0)
            {
                var obj = processed.First();
                HandleInheritance(processed, obj);
            }
        }

        private void HandleInheritance(HashSet<ObjectDocumentation> processed, ObjectDocumentation obj)
        {
            var type = obj.Type.BaseType;
            if (type == typeof(object) || type == typeof(TsObject) || type == typeof(TsInstance))
            {
                processed.Remove(obj);
                return;
            }

            if(_cache.TryGetDocumentation(type.FullName, out var parent))
            {
                if (processed.Contains(parent))
                    HandleInheritance(processed, parent);

                foreach(var field in parent.Fields)
                {
                    if (obj.Fields.Find(fd => fd.Name == field.Name) == null)
                        obj.Fields.Add(field);
                }

                foreach(var property in parent.Properties)
                {
                    if (obj.Properties.Find(pd => pd.Name == property.Name) == null)
                        obj.Properties.Add(property);
                }

                foreach (var script in parent.Scripts)
                {
                    if (obj.Scripts.Find(sd => sd.Name == script.Name) == null)
                        obj.Scripts.Add(script);
                }
            }

            processed.Remove(obj);
        }
    }
}
