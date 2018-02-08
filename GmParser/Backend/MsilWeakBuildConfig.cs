using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Backend
{
    public class MsilWeakBuildConfig
    {
        public List<string> References { get; set; } = new List<string>();
        public CompileMode Mode { get; set; } = CompileMode.Debug;
    }
}
