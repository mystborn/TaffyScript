using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace GmParser
{
    public class TestAsmBuilder
    {
        public static void Test()
        {
            var asmName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Save);
            var mb = ab.DefineDynamicModule(asmName.Name, asmName.Name + ".dll");
            var tb = mb.DefineType("MyDynamicType", TypeAttributes.Public);
            var fb = tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

            var ctorTypes = new[] { typeof(int) };
            var cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, ctorTypes);
            var il = cb1.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fb);
            il.Emit(OpCodes.Ret);

            var cb0 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            il = cb0.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4_S, 42);
            il.Emit(OpCodes.Call, cb1);
            il.Emit(OpCodes.Ret);

            tb.CreateType();
            ab.Save(asmName.Name + ".dll");
        }
    }
}
