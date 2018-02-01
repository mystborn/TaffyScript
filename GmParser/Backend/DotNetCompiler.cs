using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using GmExtern;
using GmParser.DotNet;

namespace GmParser.Backend
{
    public abstract class DotNetCompiler : Compiler
    {
        protected DotNetAssemblyLoader _assemblyLoader;
        protected DotNetTypeParser _typeParser;

        protected Type Parent { get; } = typeof(GmObject);
        protected Dictionary<string, MethodInfo> Methods { get; } = new Dictionary<string, MethodInfo>();
        protected BindingFlags MethodFlags { get; } = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        public DotNetCompiler(string asmName, params string[] externalAssemblies)
            : base(asmName)
        {
            _assemblyLoader = new DotNetAssemblyLoader(externalAssemblies);
            _typeParser = new DotNetTypeParser(_assemblyLoader);
        }

        private void FillBaseMethods()
        {
        }

        protected MethodInfo GetMethod(string methodName)
        {
            return Parent.GetMethod(methodName, MethodFlags);
        }

        protected MethodInfo GetMethodToImport(Type owner, string methodName, Type[] argTypes)
        {
            return owner.GetMethod(methodName, MethodFlags, null, argTypes, null);
        }
    }
}
