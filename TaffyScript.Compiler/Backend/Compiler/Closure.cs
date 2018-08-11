using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace TaffyScript.Compiler.Backend
{
    public class Closure
    {
        public TypeBuilder Type { get; }
        public ConstructorBuilder Constructor { get; set; }
        public Dictionary<string, FieldInfo> Fields { get; set; } = new Dictionary<string, FieldInfo>();
        public LocalBuilder Self { get; set; } = null;
        public FieldInfo Target { get; set; } = null;

        public Closure(TypeBuilder type, ConstructorBuilder ctor)
        {
            Type = type;
            Constructor = ctor;
        }
    }
}
