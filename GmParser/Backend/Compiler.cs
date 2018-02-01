using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using GmExtern;

namespace GmParser
{
    public abstract class Compiler
    {
        protected string _asmName;
        protected AssemblyBuilder _asm;
        protected ModuleBuilder _module;
        protected TypeBuilder _baseType;
        protected bool _isDebug = false;

        public Compiler(string asmName)
        {
            var name = new AssemblyName(asmName);
            _asmName = name.Name;
            Init(name);
            _isDebug = true;
        }

        public abstract void Compile(SyntaxTree tree);

        private void Init(AssemblyName name)
        {
            _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
            _module = _asm.DefineDynamicModule(_asmName, _asmName + ".dll", true);
            _baseType = _module.DefineType("GmModuleBaseType");
        }
    }
}
