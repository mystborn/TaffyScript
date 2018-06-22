using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public struct ObjectDefinition
    {
        public string Name;
        public string Parent;
        public Dictionary<string, TsDelegate> Scripts;
        public Func<TsObject[], ITsInstance> Create;

        public ObjectDefinition(string name, string parent, Dictionary<string, TsDelegate> scripts, Func<TsObject[], ITsInstance> create)
        {
            Name = name;
            Parent = parent;
            Scripts = scripts;
            Create = create;
        }
    }
}
