using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Documentation
{
    public class ObjectDocumentation
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Source { get; set; }
        public List<PropertyDocumentation> Properties { get; } = new List<PropertyDocumentation>();
        public List<FieldDocumentation> Fields { get; } = new List<FieldDocumentation>();
        public List<ScriptDocumentation> Scripts { get; } = new List<ScriptDocumentation>();
        public ConstructorDocumentation Constructor { get; set; }
    }
}
