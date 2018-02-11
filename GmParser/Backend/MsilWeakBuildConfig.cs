using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmParser.Backend
{
    [XmlRoot(ElementName = "Config")]
    public class MsilWeakBuildConfig
    {
        public string Output { get; set; }
        [XmlArray]
        [XmlArrayItem(ElementName = "Reference")]
        public List<string> References { get; set; } = new List<string>();
        public CompileMode Mode { get; set; } = CompileMode.Debug;
    }
}
