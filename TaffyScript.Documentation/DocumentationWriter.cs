using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Documentation
{
    public class DocumentationWriter
    {
        private const string LeftColumnWidth = "width=\"20%\"";
        private const string FieldColumnWidth = "width=\"15%\"";
        private string _baseDir;

        public DocumentationWriter(string outputDirectory)
        {
            _baseDir = outputDirectory;
        }

        public async Task Generate(TypeCache cache)
        {
            var globalNamespace = cache.GetNamespace("");
            await WriteNamespace(globalNamespace);
        }

        private async Task WriteNamespace(NamespaceDocumentation documentation)
        {
            var directory = Path.Combine(_baseDir, documentation.Name.Replace('.', '/'));

            Directory.CreateDirectory(directory);

            using(var writer = new StreamWriter(Path.Combine(directory, "index.md")))
            {
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync("layout: default");
                await writer.WriteLineAsync($"title: {(string.IsNullOrEmpty(documentation.Name) ? "Documentation" : documentation.Name)}");
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"# {(string.IsNullOrEmpty(documentation.Name) ? "Global" : documentation.Name)}");
                await writer.WriteLineAsync();
                await WritePathLink(writer, directory);

                if (documentation.Summary != null)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync(documentation.Summary);
                }

                if(documentation.Namespaces.Count != 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Namespaces");
                    await writer.WriteLineAsync();
                    await WriteNamespaceTable(writer, GetChildNamespaces(documentation));
                }

                if(documentation.Objects.Count != 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Objects");
                    await writer.WriteLineAsync();
                    await WriteObjectTable(writer, documentation.Objects, documentation.Name);
                }

                if(documentation.Scripts.Count != 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Scripts");
                    await writer.WriteLineAsync();
                    await WriteScriptTable(writer, "Signature", "Description", documentation.Scripts, documentation.Name);
                }
            }

            foreach (var obj in documentation.Objects)
                await WriteObject(obj, directory);

            foreach (var script in documentation.Scripts)
                await WriteScript(script, directory, "");

            foreach (var ns in documentation.Namespaces)
                await WriteNamespace(ns);
        }

        private async Task WriteObject(ObjectDocumentation documentation, string parentDirectory)
        {
            var directory = Path.Combine(parentDirectory, documentation.Name);
            Directory.CreateDirectory(directory);

            var fullName = GetQualifiedName(directory);

            using(var writer = new StreamWriter(Path.Combine(directory, "index.md")))
            {
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync("layout: default");
                await writer.WriteLineAsync($"title: {documentation.Name}");
                await writer.WriteLineAsync("---");

                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"# {documentation.Name}");
                await writer.WriteLineAsync();
                await WritePathLink(writer, directory);
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"_{documentation.Summary}_");

                if (documentation.Fields.Count != 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Fields");
                    await writer.WriteLineAsync();
                    await WriteFieldTable(writer, "Name", "Type", documentation.Fields, fullName);
                }

                if (documentation.Properties.Count != 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Properties");
                    await writer.WriteLineAsync();
                    await WritePropertyTable(writer, "Name", "Type", documentation.Properties, fullName);
                }

                if (documentation.Constructor != null)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Constructor");
                    await writer.WriteLineAsync();
                    await WriteConstructorTable(writer, documentation.Constructor, fullName);
                }

                if (documentation.Scripts.Count != 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync("## Scripts");
                    await writer.WriteLineAsync();
                    await WriteScriptTable(writer, "Signature", "Description", documentation.Scripts, fullName);
                }
            }

            foreach (var field in documentation.Fields)
                await WriteField(field, directory, documentation.Name);

            foreach (var property in documentation.Properties)
                await WriteProperty(property, directory, documentation.Name);

            if (documentation.Constructor != null)
                await WriteConstructor(documentation.Constructor, directory, documentation.Name);

            foreach (var script in documentation.Scripts)
                await WriteScript(script, directory, documentation.Name);
        }

        private async Task WriteField(FieldDocumentation documentation, string parentDirectory, string parentType)
        {
            using (var writer = new StreamWriter(Path.Combine(parentDirectory, documentation.Name + ".md")))
            {
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync("layout: default");
                await writer.WriteLineAsync($"title: {parentType}.{documentation.Name}");
                await writer.WriteLineAsync("---");

                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"# {documentation.Name}");
                await writer.WriteLineAsync();
                await WritePathLink(writer, Path.Combine(parentDirectory, documentation.Name));
                await writer.WriteLineAsync();
                await writer.WriteLineAsync("```cs");
                await writer.WriteLineAsync($"{parentType}.{documentation.Name}");
                await writer.WriteLineAsync("```");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"**Type:** {documentation.Type}");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"**Description:** {documentation.Summary}");
            }
        }

        private async Task WriteProperty(PropertyDocumentation documentation, string parentDirectory, string parentType)
        {
            using (var writer = new StreamWriter(Path.Combine(parentDirectory, documentation.Name + ".md")))
            {
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync("layout: default");
                await writer.WriteLineAsync($"title: {parentType}.{documentation.Name}");
                await writer.WriteLineAsync("---");

                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"# {documentation.Name}");
                await writer.WriteLineAsync();
                await WritePathLink(writer, Path.Combine(parentDirectory, documentation.Name));
                await writer.WriteLineAsync();
                await writer.WriteLineAsync("```cs");
                await writer.WriteLineAsync($"{parentType}.{documentation.Name} {{ {(documentation.Access == "both" ? "get; set;" : (documentation.Access == "get" ? "get;" : "set;"))} }}");
                await writer.WriteLineAsync("```");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"**Type:** {documentation.Type}");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"**Description:** {documentation.Summary}");
            }
        }

        private async Task WriteConstructor(ConstructorDocumentation documentation, string parentDirectory, string parentType)
        {
            var title = $"{parentType}.create";
            using (var writer = new StreamWriter(Path.Combine(parentDirectory, "create.md")))
            {
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync("layout: default");
                await writer.WriteLineAsync($"title: {title}");
                await writer.WriteLineAsync("---");

                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"# {parentType} Constructor");
                await writer.WriteLineAsync();
                await WritePathLink(writer, Path.Combine(parentDirectory, "create"));
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"_{documentation.Summary}_");
                await writer.WriteLineAsync();

                var args = string.Join(", ", documentation.Arguments.Select(a => a.Name));
                await writer.WriteLineAsync("```cs");
                await writer.WriteLineAsync($"new {title}({args})");
                await writer.WriteLineAsync("```");


                if(documentation.Arguments.Count > 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync($"## Arguments");
                    await writer.WriteLineAsync();
                    await WriteArgumentTable(writer, "Argument", "Type", documentation.Arguments);
                }
            }
        }

        private async Task WriteScript(ScriptDocumentation documentation, string parentDirectory, string parentType)
        {
            var title = $"{parentType}.{documentation.Name}".TrimStart('.');
            using (var writer = new StreamWriter(Path.Combine(parentDirectory, documentation.Name + ".md")))
            {
                await writer.WriteLineAsync("---");
                await writer.WriteLineAsync("layout: default");
                await writer.WriteLineAsync($"title: {title}");
                await writer.WriteLineAsync("---");

                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"# {title}");
                await writer.WriteLineAsync();
                await WritePathLink(writer, Path.Combine(parentDirectory, documentation.Name));
                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"_{documentation.Summary}_");
                await writer.WriteLineAsync();

                var args = string.Join(", ", documentation.Arguments.Select(a => a.Name));
                await writer.WriteLineAsync("```cs");
                await writer.WriteLineAsync($"{title}({args})");
                await writer.WriteLineAsync("```");

                if (documentation.Arguments.Count > 0)
                {
                    await writer.WriteLineAsync();
                    await writer.WriteLineAsync($"## Arguments");
                    await writer.WriteLineAsync();
                    await WriteArgumentTable(writer, "Argument", "Type", documentation.Arguments);
                }

                await writer.WriteLineAsync();
                await writer.WriteLineAsync($"**Returns:** {documentation.Returns}");
            }
        }

        private async Task WriteNamespaceTable(StreamWriter writer, SortedDictionary<string, string> namespaces)
        {
            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {LeftColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync("      <th>Name</th>");
            await writer.WriteLineAsync("      <th>Description</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");
            foreach (var ns in namespaces)
            {
                await writer.WriteLineAsync($"    <tr>");
                await writer.WriteLineAsync($"      <td><a href=\"{{{{site.baseurl}}}}/docs/{ns.Key.Replace('.', '/')}/\">{ns.Key}</a></td>");
                await writer.WriteLineAsync($"      <td>{ns.Value}</td>");
                await writer.WriteLineAsync($"    </tr>");
            }
            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private async Task WriteObjectTable(StreamWriter writer, List<ObjectDocumentation> objects, string parent)
        {
            var link = $"{{{{site.baseurl}}}}/docs/{parent.Replace('.', '/')}".Trim('/');
            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {LeftColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync("      <th>Name</th>");
            await writer.WriteLineAsync("      <th>Description</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");
            foreach (var obj in objects)
            {
                await writer.WriteLineAsync("    <tr>");
                await writer.WriteLineAsync($"      <td><a href=\"{link}/{obj.Name}\">{obj.Name}</a></td>");
                await writer.WriteLineAsync($"      <td>{obj.Summary}</td>");
                await writer.WriteLineAsync("    </tr>");
            }
            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private async Task WriteFieldTable(StreamWriter writer, string left, string middle, List<FieldDocumentation> fields, string parent)
        {
            var link = parent is null ? null : $"{{{{site.baseurl}}}}/docs/{parent.Replace('.', '/')}".Trim('/');
            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {FieldColumnWidth}>");
            await writer.WriteLineAsync($"  <col {FieldColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync($"      <th>{left}</th>");
            await writer.WriteLineAsync($"      <th>{middle}</th>");
            await writer.WriteLineAsync("      <th>Description</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");

            foreach (var field in fields)
            {
                await writer.WriteLineAsync("    <tr>");
                if (link is null)
                    await writer.WriteLineAsync($"      <td>{field.Name}</td>");
                else
                    await writer.WriteLineAsync($"      <td><a href=\"{link}/{field.Name}/\">{field.Name}</a></td>");
                await writer.WriteLineAsync($"      <td>{field.Type}</td>");
                await writer.WriteLineAsync($"      <td>{field.Summary}</td>");
                await writer.WriteLineAsync("    </tr>");
            }

            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private async Task WritePropertyTable(StreamWriter writer, string left, string middle, List<PropertyDocumentation> properties, string parent)
        {
            var link = parent is null ? null : $"{{{{site.baseurl}}}}/docs/{parent.Replace('.', '/')}".Trim('/');
            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {FieldColumnWidth}>");
            await writer.WriteLineAsync($"  <col {FieldColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync($"      <th>{left}</th>");
            await writer.WriteLineAsync($"      <th>{middle}</th>");
            await writer.WriteLineAsync("      <th>Description</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");

            foreach (var property in properties)
            {
                await writer.WriteLineAsync("    <tr>");
                if (link is null)
                    await writer.WriteLineAsync($"      <td>{property.Name}</td>");
                else
                    await writer.WriteLineAsync($"      <td><a href=\"{link}/{property.Name}/\">{property.Name}</a></td>");
                await writer.WriteLineAsync($"      <td>{property.Type}</td>");
                await writer.WriteLineAsync($"      <td>{property.Summary}</td>");
                await writer.WriteLineAsync("    </tr>");
            }

            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private async Task WriteScriptTable(StreamWriter writer, string left, string right, List<ScriptDocumentation> scripts, string parent)
        {
            var link = $"{{{{site.baseurl}}}}/docs/{parent.Replace('.', '/')}".Trim('/');

            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {LeftColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync($"      <th>{left}</th>");
            await writer.WriteLineAsync($"      <th>{right}</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");
            foreach (var script in scripts)
            {
                var args = string.Join(", ", script.Arguments.Select(a => a.Name));
                await writer.WriteLineAsync("    <tr>");
                await writer.WriteLineAsync($"      <td><a href=\"{link}/{script.Name}\">{script.Name}({args})</a></td>");
                await writer.WriteLineAsync($"      <td>{script.Summary}</td>");
                await writer.WriteLineAsync("    </tr>");
            }
            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private async Task WriteConstructorTable(StreamWriter writer, ConstructorDocumentation constructor, string parent)
        {
            var link = $"{{{{site.baseurl}}}}/docs/{parent.Replace('.', '/')}".Trim('/');
            var args = string.Join(", ", constructor.Arguments.Select(a => a.Name));
            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {LeftColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync("      <th>Name</th>");
            await writer.WriteLineAsync("      <th>Description</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");
                await writer.WriteLineAsync("    <tr>");
                await writer.WriteLineAsync($"      <td><a href=\"{link}/create/\">create({args})</a></td>");
                await writer.WriteLineAsync($"      <td>{constructor.Summary}</td>");
                await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private async Task WriteArgumentTable(StreamWriter writer, string left, string middle, List<ArgumentDocumentation> arguments)
        {
            await writer.WriteLineAsync("<table>");
            await writer.WriteLineAsync($"  <col {FieldColumnWidth}>");
            await writer.WriteLineAsync($"  <col {FieldColumnWidth}>");
            await writer.WriteLineAsync("  <thead>");
            await writer.WriteLineAsync("    <tr>");
            await writer.WriteLineAsync($"      <th>{left}</th>");
            await writer.WriteLineAsync($"      <th>{middle}</th>");
            await writer.WriteLineAsync("      <th>Description</th>");
            await writer.WriteLineAsync("    </tr>");
            await writer.WriteLineAsync("  </thead>");
            await writer.WriteLineAsync("  <tbody>");

            foreach (var arg in arguments)
            {
                await writer.WriteLineAsync("    <tr>");
                await writer.WriteLineAsync($"      <td>{arg.Name}</td>");
                await writer.WriteLineAsync($"      <td>{arg.Type}</td>");
                await writer.WriteLineAsync($"      <td>{arg.Summary}</td>");
                await writer.WriteLineAsync("    </tr>");
            }

            await writer.WriteLineAsync("  </tbody>");
            await writer.WriteLineAsync("</table>");
        }

        private SortedDictionary<string, string> GetChildNamespaces(NamespaceDocumentation documentation)
        {
            SortedDictionary<string, string> map = new SortedDictionary<string, string>();
            Stack<NamespaceDocumentation> stack = new Stack<NamespaceDocumentation>(documentation.Namespaces);
            while(stack.Count > 0)
            {
                var ns = stack.Pop();
                foreach (var child in ns.Namespaces)
                    stack.Push(child);
                map.Add(ns.Name, ns.Summary);
            }
            return map;
        }

        private async Task WritePathLink(StreamWriter writer, string file)
        {
            var name = GetQualifiedName(file);
            var link = "{{site.baseurl}}/docs/";

            await writer.WriteAsync($"[\\[global\\]]({link})");

            if(name == "")
            {
                await writer.WriteLineAsync();
                return;
            }

            var parts = name.Split(new[] { '.', '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var part in parts)
            {
                link += part + "/";
                await writer.WriteAsync($".[{part}]({link})");
            }

            await writer.WriteLineAsync();
        }

        private string GetQualifiedName(string link)
        {
            if (link.StartsWith(_baseDir))
                link = link.Remove(0, _baseDir.Length);

            if (link.EndsWith(".md"))
                link = link.Remove(link.Length - 3);

            return link.Replace('\\', '.');
        }
    }
}
