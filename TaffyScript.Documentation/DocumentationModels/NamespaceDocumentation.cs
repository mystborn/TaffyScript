using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Documentation
{
    public class NamespaceDocumentation
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<NamespaceDocumentation> Namespaces { get; } = new List<NamespaceDocumentation>();
        public List<ObjectDocumentation> Objects { get; } = new List<ObjectDocumentation>();
        public List<ScriptDocumentation> Scripts { get; } = new List<ScriptDocumentation>();
    }
}
