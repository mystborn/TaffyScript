using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Documentation
{
    public class ScriptDocumentation
    {
        public MethodInfo Method { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<ArgumentDocumentation> Arguments { get; } = new List<ArgumentDocumentation>();
        public string Source { get; set; }
        public string Returns { get; set; }
    }
}
