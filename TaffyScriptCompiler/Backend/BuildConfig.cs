using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaffyScriptCompiler.Backend
{
    [XmlRoot(ElementName = "Build")]
    public class BuildConfig
    {
        public string Output { get; set; } = "bin/Output";

        [XmlArray]
        [XmlArrayItem(ElementName = "Reference")]
        public List<string> References { get; set; } = new List<string>();

        [XmlArray]
        [XmlArrayItem(ElementName = "Exclude")]
        public List<string> Excludes { get; set; } = new List<string>();

        public string EntryPoint { get; set; } = "main";
        public string Product { get; set; } = "";
        public string Version { get; set; } = "0.0.0";
        public string Company { get; set; } = "";
        public string Copyright { get; set; } = "";
        public string Trademark { get; set; } = "";
        public string Description { get; set; } = "";

        public CompileMode Mode { get; set; } = CompileMode.Debug;

        public void Save(string path)
        {
            path = Path.Combine(path, "build.cfg");
            using(var sw = new StreamWriter(path))
            {
                var serializer = new XmlSerializer(typeof(BuildConfig));
                serializer.Serialize(sw, this);
            }
        }
    }
}
