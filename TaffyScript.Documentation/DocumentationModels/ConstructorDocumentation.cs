using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Documentation
{
    public class ConstructorDocumentation
    {
        public string Summary { get; set; }
        public List<ArgumentDocumentation> Arguments { get; } = new List<ArgumentDocumentation>();
        public string Source { get; set; }
    }
}
