using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Reflection
{
    public struct ObjectDefinition
    {
        public string Name;
        public string Parent;
        public Func<TsObject[], ITsInstance> Create;

        public ObjectDefinition(string name, string parent, Func<TsObject[], ITsInstance> create)
        {
            Name = name;
            Parent = parent;
            Create = create;
        }
    }
}
