using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace TaffyScriptCompiler.Backend
{
    // This class is extremely useful.
    // A) It emits OpCodes in a functional way,
    //    returning itself whenever possible.
    // B) It provides strongly typed intructions.
    // C) It will choose the right instruction for the input, making optimizations easy.
    // D) Keeps a Stack of types that will correspond to the types on the Evaluation Stack.
    //    Unfortunately this functionality isn't perfect (due to branch instructions), but it's very close.
    //    Almost all TaffyScript constructs maintain the stack purity, excluding switch statements.
    //    Those must be handled by the CodeGen, but it isn't that hard. 

    // If you have a question about any of the methods, almost all of them correspond with an OpCode.
    // Make sure to check the Microsoft docs on those to find your answer.
    // If you have further questions, feel free to ask on the Github or on my Discord: https://discord.gg/9Sy9DnD

    /// <summary>
    /// Helper class for emitting MSIL instructions.
    /// </summary>
    public class ILEmitter
    {
        private ILGenerator _generator;
        private Stack<Type> _types = new Stack<Type>();
        private Type[] _paramTypes;

        public MethodBase Method { get; }

        public ILEmitter(MethodBuilder builder, Type[] input)
        {
            _generator = builder.GetILGenerator();
            _paramTypes = input;
            Method = builder;
        }

        public ILEmitter(ConstructorBuilder builder, Type[] input)
        {
            _generator = builder.GetILGenerator();
            _paramTypes = input;
            Method = builder;
        }

        public LocalBuilder DeclareLocal(Type type, string name)
        {
            var local = _generator.DeclareLocal(type);
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

        public ILEmitter MarkSequencePoint(System.Diagnostics.SymbolStore.ISymbolDocumentWriter document, int startLine, int startIndex, int endLine, int endIndex)
        {
            _generator.MarkSequencePoint(document, startLine, startIndex, endLine, endIndex);
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
            if (method == null)
                return this;
            var length = method.GetParameters().Length;
            if (!method.IsStatic)
                length += 1;
            for (var i = 0; i < length; i++)
                _types.Pop();

            if (!method.IsStatic && !method.DeclaringType.IsValueType && (method.IsVirtual || method.IsAbstract))
                _generator.Emit(OpCodes.Callvirt, method);
            else
                _generator.Emit(OpCodes.Call, method);

            if (method.ReturnType != typeof(void))
                _types.Push(method.ReturnType);

            return this;
        }

        public ILEmitter Call(MethodInfo method, int input, Type output)
        {
            if (method == null)
                return this;
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

        public ILEmitter ConvertLong(bool unsigned)
        {
            _types.Pop();
            if (unsigned)
            {
                _generator.Emit(OpCodes.Conv_U8);
                _types.Push(typeof(ulong));
            }
            else
            {
                _generator.Emit(OpCodes.Conv_I8);
                _types.Push(typeof(long));
            }
            return this;
        }

        public ILEmitter Div()
        {
            //Only pop first type, reload second.
            _types.Pop();
            _generator.Emit(OpCodes.Div);
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

        public ILEmitter LdBool(bool value)
        {
            if (value)
                _generator.Emit(OpCodes.Ldc_I4_1);
            else
                _generator.Emit(OpCodes.Ldc_I4_0);
            _types.Push(typeof(bool));
            return this;
        }

        public ILEmitter LdElem(Type type)
        {
            _types.Pop();
            _types.Pop();
            if (!type.IsValueType)
            {
                _generator.Emit(OpCodes.Ldelem_Ref);
                _types.Push(type);
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

        public ILEmitter LdFld(FieldInfo field)
        {
            var code = field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld;
            _generator.Emit(code, field);
            _types.Push(field.FieldType);
            return this;
        }

        public ILEmitter LdFldA(FieldInfo field)
        {
            var code = field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda;
            _generator.Emit(code, field);
            var type = field.FieldType;
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

        public ILEmitter LdFtn(MethodInfo function)
        {
            _generator.Emit(OpCodes.Ldftn, function);
            _types.Push(typeof(MethodInfo));
            return this;
        }

        public ILEmitter LdInt(int value)
        {
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

            _types.Push(typeof(int));
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
                    _generator.Emit(OpCodes.Ldloc, (short)(builder.LocalIndex));
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

        public ILEmitter LdNull()
        {
            _generator.Emit(OpCodes.Ldnull);
            _types.Push(null);
            return this;
        }

        public ILEmitter LdObj(Type type)
        {
            _types.Pop();
            if (!type.IsValueType)
                _generator.Emit(OpCodes.Ldind_Ref);
            else
            {
                if (type == typeof(sbyte))
                    _generator.Emit(OpCodes.Ldind_I1);
                else if (type == typeof(short))
                    _generator.Emit(OpCodes.Ldind_I2);
                else if (type == typeof(int))
                    _generator.Emit(OpCodes.Ldind_I4);
                else if (type == typeof(long))
                    _generator.Emit(OpCodes.Ldind_I8);
                else if (type == typeof(byte))
                    _generator.Emit(OpCodes.Ldind_U1);
                else if (type == typeof(ushort))
                    _generator.Emit(OpCodes.Ldind_U2);
                else if (type == typeof(uint))
                    _generator.Emit(OpCodes.Ldind_U4);
                else if (type == typeof(float))
                    _generator.Emit(OpCodes.Ldind_R4);
                else if (type == typeof(double))
                    _generator.Emit(OpCodes.Ldind_R8);
                else
                    _generator.Emit(OpCodes.Ldobj, type);

            }
            _types.Push(type);
            return this;
        }

        public ILEmitter LdStr(string value)
        {
            _generator.Emit(OpCodes.Ldstr, value);
            _types.Push(typeof(string));
            return this;
        }

        public ILEmitter LdType(Type type)
        {
            _generator.Emit(OpCodes.Ldtoken, type);
            _types.Push(typeof(Type));
            return this;
        }

        public ILEmitter Mul()
        {
            //Only pop first type, reload second.
            _types.Pop();
            _generator.Emit(OpCodes.Mul);
            return this;
        }

        public ILEmitter Neg()
        {
            _generator.Emit(OpCodes.Neg);
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
            _types.Pop();
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

        public ILEmitter Pop(bool removeTop = true)
        {
            if(removeTop)
                _types.Pop();
            _generator.Emit(OpCodes.Pop);
            return this;
        }

        public ILEmitter Rem()
        {
            //Only pop first type, reload second.
            _types.Pop();
            _generator.Emit(OpCodes.Rem);
            return this;
        }

        public ILEmitter Ret()
        {
            if (_types.Count > 0)
                _types.Pop();
            _generator.Emit(OpCodes.Ret);
            return this;
        }

        public ILEmitter StArg(short arg)
        {
            _types.Pop();
            if (arg <= 255)
                _generator.Emit(OpCodes.Starg_S, (byte)arg);
            else
                _generator.Emit(OpCodes.Starg, arg);

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

        public ILEmitter StObj(Type type)
        {
            _types.Pop();
            _types.Pop();
            if (!type.IsValueType)
                _generator.Emit(OpCodes.Stind_Ref);
            else
            {
                if (type == typeof(sbyte))
                    _generator.Emit(OpCodes.Stind_I1);
                else if (type == typeof(short))
                    _generator.Emit(OpCodes.Stind_I2);
                else if (type == typeof(int))
                    _generator.Emit(OpCodes.Stind_I4);
                else if (type == typeof(long))
                    _generator.Emit(OpCodes.Stind_I8);
                else if (type == typeof(float))
                    _generator.Emit(OpCodes.Stind_R4);
                else if (type == typeof(double))
                    _generator.Emit(OpCodes.Stind_R8);
                else
                    _generator.Emit(OpCodes.Stobj, type);
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

        public ILEmitter WriteLine(string value)
        {
            _generator.EmitWriteLine(value);
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

        public ILEmitter PopTop()
        {
            _types.Pop();
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
