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
        public Closure Parent { get; set; }
        public ConstructorBuilder Constructor { get; set; }
        public Dictionary<string, FieldInfo> Fields { get; set; } = new Dictionary<string, FieldInfo>();
        public LocalBuilder Self { get; set; } = null;
        public FieldInfo Target { get; set; } = null;
        public FieldInfo ParentField { get; set; } = null;
        public bool Constructed { get; set; } = false;

        public Closure(TypeBuilder type, ConstructorBuilder ctor)
        {
            Type = type;
            Constructor = ctor;
        }

        public Closure LoadVariablesClosure(string name, ILEmitter emit, out FieldInfo field)
        {
            if (emit.Method.DeclaringType == Type)
                emit.LdArg(0);
            else
                emit.LdLocal(Self);

            var closure = this;
            field = default;
            while (!closure.Fields.TryGetValue(name, out field) && closure.Parent != null)
            {
                emit.LdFld(closure.ParentField);
                closure = closure.Parent;
            }

            if (field is null)
                throw new InvalidOperationException($"Tried to get captured variable '{name}' that doesn't exist.");

            return closure;
        }
    }
}
