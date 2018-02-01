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
        private Stack<Type> _types;
        private Type[] _paramTypes;

        public ILEmitter(MethodBuilder builder, bool isDebug)
        {
            var paramTypes = builder.GetParameters();
            _paramTypes = new Type[_paramTypes.Length];
            for (var i = 0; i < paramTypes.Length; i++)
                _paramTypes[i] = paramTypes[i].ParameterType;
        }

        public ILEmitter(ILGenerator il, bool isDebug)
        {
            _generator = il;
            _isDebug = isDebug;
        }

        public LocalBuilder DeclareLocal(Type type, string name)
        {
            var local = _generator.DeclareLocal(type);
            if(_isDebug)
                local.SetLocalSymInfo(name);
            return local;
        }

        public ILEmitter Box(Type valueType)
        {
            _types.Pop();
            _types.Push(typeof(object));
            _generator.Emit(OpCodes.Box, valueType);
            return this;
        }

        public ILEmitter Call(MethodInfo method)
        {
            var length = method.GetParameters().Length;
            for (var i = 0; i < length; i++)
                _types.Pop();

            if (!method.IsStatic && (method.IsVirtual || method.IsAbstract))
                _generator.EmitCall(OpCodes.Callvirt, method, null);
            else
                _generator.EmitCall(OpCodes.Call, method, null);

            if (method.ReturnType != typeof(void))
                _types.Push(method.ReturnType);

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
                    _generator.Emit(OpCodes.Ldarg, index);
                    break;
            }

            _types.Push(_paramTypes[index]);

            return this;
        }

        public ILEmitter LdElem(Type type)
        {
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
            _types.Push(type);
            _generator.Emit(OpCodes.Ldelema, type);
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

        public ILEmitter LdFloat(float value)
        {
            _types.Push(typeof(float));
            _generator.Emit(OpCodes.Ldc_R4, value);
            return this;
        }

        public ILEmitter LdStr(string value)
        {
            _types.Push(typeof(string));
            _generator.Emit(OpCodes.Ldstr, value);
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

        public ILEmitter Nop()
        {
            _generator.Emit(OpCodes.Nop);
            return this;
        }

        public ILEmitter Ret()
        {
            if (_types.Count > 0)
                _types.Pop();
            _generator.Emit(OpCodes.Ret);
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
