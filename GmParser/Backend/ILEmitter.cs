using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace GmParser.Backend
{
    public class ILEmitter
    {
        private ILGenerator _generator;
        private bool _isDebug;
        private Stack<Type> _types = new Stack<Type>();
        private Type[] _paramTypes;

        public ILEmitter(MethodBuilder builder, Type[] input, bool isDebug)
        {
            _generator = builder.GetILGenerator();
            _isDebug = isDebug;
            _paramTypes = input;
        }

        public LocalBuilder DeclareLocal(Type type, string name)
        {
            var local = _generator.DeclareLocal(type);
            if(_isDebug)
                local.SetLocalSymInfo(name);
            return local;
        }

        public Label DefineLabel()
        {
            return _generator.DefineLabel();
        }

        public ILEmitter MarkLabel(Label label)
        {
            _generator.MarkLabel(label);
            return this;
        }

        public ILEmitter Add()
        {
            var first = _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Add);

            if (first == typeof(int))
                _types.Push(typeof(int));
            else if (first == typeof(float))
                _types.Push(typeof(float));
            else
                throw new NotImplementedException();
            return this;
        }

        public ILEmitter And()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.And);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Beq(Label label)
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Beq, label);
            return this;
        }

        public ILEmitter Bge(Label label)
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Bge, label);
            return this;
        }

        public ILEmitter Bgt(Label label)
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Bgt, label);
            return this;
        }

        public ILEmitter Ble(Label label)
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Ble, label);
            return this;
        }

        public ILEmitter Blt(Label label)
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Blt, label);
            return this;
        }

        public ILEmitter Box(Type valueType)
        {
            _types.Pop();
            _generator.Emit(OpCodes.Box, valueType);
            _types.Push(typeof(object));
            return this;
        }

        public ILEmitter Br(Label label)
        {
            _generator.Emit(OpCodes.Br, label);
            return this;
        }

        public ILEmitter BrFalse(Label label)
        {
            _types.Pop();
            _generator.Emit(OpCodes.Brfalse, label);
            return this;
        }

        public ILEmitter BrTrue(Label label)
        {
            _types.Pop();
            _generator.Emit(OpCodes.Brtrue, label);
            return this;
        }

        public ILEmitter Call(MethodInfo method)
        {
            var length = method.GetParameters().Length;
            for (var i = 0; i < length; i++)
                _types.Pop();

            if (!method.IsStatic && (method.IsVirtual || method.IsAbstract))
                _generator.Emit(OpCodes.Callvirt, method);
            else
                _generator.Emit(OpCodes.Call, method);

            if (method.ReturnType != typeof(void))
                _types.Push(method.ReturnType);

            return this;
        }

        public ILEmitter Call(MethodInfo method, int input, Type output)
        {
            for (var i = 0; i < input; i++)
                _types.Pop();

            if (!method.IsStatic && (method.IsVirtual || method.IsAbstract))
                _generator.Emit(OpCodes.Callvirt, method);
            else
                _generator.Emit(OpCodes.Call, method);

            if (output != null && output != typeof(void))
                _types.Push(output);

            return this;
        }

        public ILEmitter Ceq()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Ceq);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Cge()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Clt);
            _generator.Emit(OpCodes.Ldc_I4_0);
            _generator.Emit(OpCodes.Ceq);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Cgt()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Cgt);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Cle()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Cgt);
            _generator.Emit(OpCodes.Ldc_I4_0);
            _generator.Emit(OpCodes.Ceq);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Clt()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Clt);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Cne()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Ceq);
            _generator.Emit(OpCodes.Ldc_I4_0);
            _generator.Emit(OpCodes.Ceq);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter ConvertInt(bool unsigned)
        {
            _types.Pop();
            if (unsigned)
            {
                _generator.Emit(OpCodes.Conv_U4);
                _types.Push(typeof(uint));
            }
            else
            {
                _generator.Emit(OpCodes.Conv_I4);
                _types.Push(typeof(int));
            }
            return this;
        }

        public ILEmitter ConvertFloat()
        {
            _types.Pop();
            _generator.Emit(OpCodes.Conv_R4);
            _types.Push(typeof(float));
            return this;
        }

        public ILEmitter Dup()
        {
            _generator.Emit(OpCodes.Dup);
            _types.Push(_types.Peek());
            return this;
        }

        public ILEmitter LdArg(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            switch(index)
            {
                case 0:
                    _generator.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    _generator.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    _generator.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    _generator.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    _generator.Emit(OpCodes.Ldarg, (short)index);
                    break;
            }

            _types.Push(_paramTypes[index]);

            return this;
        }

        public ILEmitter LdArgA(int index)
        {
            _generator.Emit(OpCodes.Ldarga, (short)index);
            var type = _paramTypes[index];
            if (type.IsValueType)
                _types.Push(type.MakePointerType());
            else
                _types.Push(type);
            return this;
        }

        public ILEmitter LdElem(Type type)
        {
            _types.Pop();
            _types.Pop();
            if (!type.IsValueType)
            {
                _generator.Emit(OpCodes.Ldelem_Ref);
                return this;
            }

            if (type == typeof(int))
                _generator.Emit(OpCodes.Ldelem_I4);
            else if (type == typeof(float))
                _generator.Emit(OpCodes.Ldelem_R4);
            else if (type == typeof(byte))
                _generator.Emit(OpCodes.Ldelem_U1);
            else if (type == typeof(uint))
                _generator.Emit(OpCodes.Ldelem_U4);
            else if (type == typeof(double))
                _generator.Emit(OpCodes.Ldelem_R8);
            else if (type == typeof(long))
                _generator.Emit(OpCodes.Ldelem_I8);
            else if (type == typeof(short))
                _generator.Emit(OpCodes.Ldelem_I2);
            else if (type == typeof(sbyte))
                _generator.Emit(OpCodes.Ldelem_I1);
            else if (type == typeof(ushort))
                _generator.Emit(OpCodes.Ldelem_U2);
            else
                _generator.Emit(OpCodes.Ldelem, type);

            _types.Push(type);

            return this;
        }

        public ILEmitter LdElemA(Type type)
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Ldelema, type);
            if (type.IsValueType)
                type = type.MakePointerType();
            _types.Push(type);
            return this;
        }

        public ILEmitter LdFloat(float value)
        {
            _generator.Emit(OpCodes.Ldc_R4, value);
            _types.Push(typeof(float));
            return this;
        }

        public ILEmitter LdInt(int value)
        {
            _types.Push(typeof(int));
            switch(value)
            {
                case -1:
                    _generator.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    _generator.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    _generator.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    _generator.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    _generator.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    _generator.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    _generator.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    _generator.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    _generator.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    _generator.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    _generator.Emit(OpCodes.Ldc_I4, value);
                    break;
            }

            return this;
        }

        public ILEmitter LdLen()
        {
            _types.Pop();
            _generator.Emit(OpCodes.Ldlen);
            _types.Push(typeof(int));
            return this;
        }

        public ILEmitter LdLocal(LocalBuilder builder)
        {
            switch (builder.LocalIndex)
            {
                case 0:
                    _generator.Emit(OpCodes.Ldloc_0);
                    break;
                case 1:
                    _generator.Emit(OpCodes.Ldloc_1);
                    break;
                case 2:
                    _generator.Emit(OpCodes.Ldloc_2);
                    break;
                case 3:
                    _generator.Emit(OpCodes.Ldloc_3);
                    break;
                default:
                    _generator.Emit(OpCodes.Ldloc, (builder.LocalIndex));
                    break;
            }
            _types.Push(builder.LocalType);
            return this;
        }

        public ILEmitter LdLocalA(LocalBuilder builder)
        {
            _generator.Emit(OpCodes.Ldloca, (short)builder.LocalIndex);
            var type = builder.LocalType;
            if (type.IsValueType)
                type = type.MakePointerType();
            _types.Push(type);
            return this;
        }

        public ILEmitter LdLong(long value)
        {
            _generator.Emit(OpCodes.Ldc_I8, value);
            _types.Push(typeof(long));
            return this;
        }

        public ILEmitter LdStr(string value)
        {
            _generator.Emit(OpCodes.Ldstr, value);
            _types.Push(typeof(string));
            return this;
        }

        public ILEmitter Mul()
        {
            //Only pop first type, reload second.
            _types.Pop();
            _generator.Emit(OpCodes.Mul);
            return this;
        }

        public ILEmitter Neq()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Ceq);
            _generator.Emit(OpCodes.Ldc_I4_0);
            _generator.Emit(OpCodes.Ceq);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter New(ConstructorInfo info)
        {
            var length = info.GetParameters().Length;
            for (var i = 0; i < length; i++)
                _types.Pop();

            _generator.Emit(OpCodes.Newobj, info);

            _types.Push(info.DeclaringType);
            return this;
        }

        public ILEmitter NewArr(Type elementType)
        {
            _generator.Emit(OpCodes.Newarr, elementType);
            _types.Push(elementType.MakeArrayType());
            return this;
        }

        public ILEmitter Nop()
        {
            _generator.Emit(OpCodes.Nop);
            return this;
        }

        public ILEmitter Or()
        {
            _types.Pop();
            _types.Pop();
            _generator.Emit(OpCodes.Or);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter Pop()
        {
            _types.Pop();
            _generator.Emit(OpCodes.Pop);
            return this;
        }

        public ILEmitter Ret()
        {
            if (_types.Count > 0)
                _types.Pop();
            _generator.Emit(OpCodes.Ret);
            return this;
        }

        public ILEmitter StElem(Type elementType)
        {
            _types.Pop();
            _types.Pop();
            _types.Pop();
            if (!elementType.IsValueType)
            {
                _generator.Emit(OpCodes.Stelem_Ref);
                return this;
            }

            if (elementType == typeof(int))
                _generator.Emit(OpCodes.Stelem_I4);
            else if (elementType == typeof(float))
                _generator.Emit(OpCodes.Stelem_R4);
            else if (elementType == typeof(double))
                _generator.Emit(OpCodes.Stelem_R8);
            else if (elementType == typeof(long))
                _generator.Emit(OpCodes.Stelem_I8);
            else if (elementType == typeof(short))
                _generator.Emit(OpCodes.Stelem_I2);
            else if (elementType == typeof(sbyte))
                _generator.Emit(OpCodes.Stelem_I1);
            else
                _generator.Emit(OpCodes.Stelem, elementType);
            return this;
        }

        public ILEmitter StLocal(LocalBuilder local)
        {
            _types.Pop();
            switch(local.LocalIndex)
            {
                case 0:
                    _generator.Emit(OpCodes.Stloc_0);
                    break;
                case 1:
                    _generator.Emit(OpCodes.Stloc_1);
                    break;
                case 2:
                    _generator.Emit(OpCodes.Stloc_2);
                    break;
                case 3:
                    _generator.Emit(OpCodes.Stloc_3);
                    break;
                default:
                    _generator.Emit(OpCodes.Stloc, local);
                    break;
            }

            return this;
        }

        public ILEmitter Sub()
        {
            //Load this type back into the stack.
            //int - int -> int;
            //Therefore, only pop one, leaving other ont top.
            _types.Pop();
            _generator.Emit(OpCodes.Sub);
            return this;
        }

        public ILEmitter Xor()
        {
            //Load this type back on top.
            _types.Pop();
            _generator.Emit(OpCodes.Xor);
            return this;
        }

        public Type GetTop()
        {
            if (_types.Count == 0)
                throw new IndexOutOfRangeException();
            return _types.Peek();
        }

        public bool TryGetTop(out Type top)
        {
            top = default(Type);
            if(_types.Count != 0)
            {
                top = _types.Peek();
                return true;
            }
            return false;
        }
    }
}
