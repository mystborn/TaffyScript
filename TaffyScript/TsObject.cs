using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript
{
    /// <summary>
    /// Variant representing a TaffyScript value
    /// </summary>
    public struct TsObject
    {
        #region Constants

        public const float All = -3f;
        public const float Noone = -4f;

        #endregion

        #region Properties

        public VariableType Type { get; private set; }
        public ITsValue Value { get; private set; }

        #endregion

        #region Constructors

        private TsObject(VariableType type, ITsValue value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Creates a TaffyScript object from a bool.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(bool value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value ? 1 : 0);
        }

        /// <summary>
        /// Creates a string TaffyScript object from a char.
        /// </summary>
        /// <param name="value"></param>
        public TsObject(char value)
        {
            Type = VariableType.String;
            Value = new TsImmutableValue<string>(value.ToString());
        }

        /// <summary>
        /// Creates a TaffyScript object from a byte.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(byte value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from an sbyte.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(sbyte value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a short.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(short value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a ushort.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(ushort value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from an int.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(int value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a uint.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(uint value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a long.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(long value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a ulong.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(ulong value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a float.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(float value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a double.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(double value)
        {
            Type = VariableType.Real;
            Value = new TsImmutableValue<float>((float)value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a string.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(string value)
        {
            Type = VariableType.String;
            Value = new TsImmutableValue<string>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from an instances id.
        /// </summary>
        /// <param name="instance">The instance to get the id from.</param>
        public TsObject(ITsInstance instance)
        {
            Type = VariableType.Instance;
            Value = new TsImmutableValue<ITsInstance>(instance);
        }

        /// <summary>
        /// Creates a TaffyScript object from a 1D array.
        /// </summary>
        /// <param name="array">The value of the object.</param>
        public TsObject(TsObject[] array)
        {
            Type = VariableType.Array1;
            Value = new TsMutableValue<TsObject[]>(array);
        }

        /// <summary>
        /// Creates a TaffyScript object from a 2D array.
        /// </summary>
        /// <param name="array">The value of the object.</param>
        public TsObject(TsObject[][] array)
        {
            Type = VariableType.Array2;
            Value = new TsMutableValue<TsObject[][]>(array);
        }

        /// <summary>
        /// Creates a TaffyScript object from a <see cref="TsDelegate"/>.
        /// </summary>
        /// <param name="script">The value of the object.</param>
        public TsObject(TsDelegate script)
        {
            Type = VariableType.Delegate;
            Value = new TsImmutableValue<TsDelegate>(script);
        }

        /// <summary>
        /// Creates an empty TaffyScript object.
        /// </summary>
        public static TsObject Empty()
        {
            return new TsObject(VariableType.Null, new TsImmutableValue<object>(null));
        }

        public static TsObject NooneObject()
        {
            return new TsObject(Noone);
        }

        #endregion

        #region Raw Values

        /// <summary>
        /// Gets the value of this object as a bool.
        /// </summary>
        /// <returns></returns>
        public bool GetBool()
        {
            return GetFloat() >= .5f;
        }

        /// <summary>
        /// Gets the value of this object as a char.
        /// </summary>
        /// <returns></returns>
        public char GetChar()
        {
            return GetString()[0];
        }

        /// <summary>
        /// Gets the value of this object as a byte.
        /// </summary>
        /// <returns></returns>
        public byte GetByte()
        {
            return (byte)GetFloat();
        }

        /// <summary>
        /// Gets the value of this object as an sbyte.
        /// </summary>
        /// <returns></returns>
        public sbyte GetSByte()
        {
            return (sbyte)GetFloat();
        }

        /// <summary>
        /// Gets the value of this object as a short.
        /// </summary>
        /// <returns></returns>
        public short GetShort()
        {
            return (short)GetFloat();
        }

        /// <summary>
        /// Gets the value of this object as a ushort.
        /// </summary>
        /// <returns></returns>
        public ushort GetUShort()
        {
            return (ushort)GetFloat();
        }

        /// <summary>
        /// Gets the number value held by this object as an int.
        /// </summary>
        public int GetInt()
        {
            return (int)GetFloat();
        }

        /// <summary>
        /// Gets the value of this object as a uint.
        /// </summary>
        /// <returns></returns>
        public uint GetUInt()
        {
            return (uint)GetFloat();
        }

        /// <summary>
        /// Gets the number value held by this object as a long.
        /// </summary>
        /// <returns></returns>
        public long GetLong()
        {
            return (long)GetFloat();
        }

        /// <summary>
        /// Gets the value of this object as a ulong.
        /// </summary>
        /// <returns></returns>
        public ulong GetULong()
        {
            return (ulong)GetFloat();
        }

        /// <summary>
        /// Gets the number value held by this object.
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            if (Type == VariableType.Null)
                return 0;
            if (Type != VariableType.Real)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Real, is {Type} instead.");
            return ((TsImmutableValue<float>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the value of this object as a double.
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
        {
            return GetFloat();
        }

        /// <summary>
        /// Gets the number held by this object without checking it's type.
        /// </summary>
        /// <returns></returns>
        public float GetFloatUnchecked()
        {
            return ((TsImmutableValue<float>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the string value held by this object.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (Type == VariableType.Null)
                return "";
            if (Type != VariableType.String)
                throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");
            return ((TsImmutableValue<string>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the string value held by this object without checking its type.
        /// </summary>
        /// <returns></returns>
        public string GetStringUnchecked()
        {
            return ((TsImmutableValue<string>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the instance held by this object.
        /// </summary>
        /// <returns></returns>
        public ITsInstance GetInstance()
        {
            if (Type != VariableType.Instance)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Instance, is {Type} instead.");

            return (ITsInstance)Value.WeakValue;
        }

        /// <summary>
        /// Gets the instance held by this object without checking its type.
        /// </summary>
        /// <returns></returns>
        public ITsInstance GetInstanceUnchecked()
        {
            return (ITsInstance)Value.WeakValue;
        }

        /// <summary>
        /// Gets the 1D array held by this object.
        /// </summary>
        /// <returns></returns>
        public TsObject[] GetArray1D()
        {
            if (Type != VariableType.Array1)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Array1D, is {Type} instead.");
            return ((TsMutableValue<TsObject[]>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the 2D array held by this object.
        /// </summary>
        /// <returns></returns>
        public TsObject[][] GetArray2D()
        {
            if (Type != VariableType.Array2)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Array2D, is {Type} instead.");
            return ((TsMutableValue<TsObject[][]>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the delegate held by this object.
        /// </summary>
        /// <returns></returns>
        public TsDelegate GetDelegate()
        {
            if (Type != VariableType.Delegate)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Delegate, is {Type} instead.");
            return (TsDelegate)Value.WeakValue;
        }

        /// <summary>
        /// Gets the delegate held by this object without checking its type.
        /// </summary>
        /// <returns></returns>
        public TsDelegate GetDelegateUnchecked()
        {
            return (TsDelegate)Value.WeakValue;
        }

        /// <summary>
        /// Gets the untyped value held by this object.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (Value == null)
                return null;
            return Value.WeakValue;
        }

        #endregion

        #region Member Access

        /// <summary>
        /// Sets a variable of the given name on the instance with an id that matches the value of this object to a float value.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void MemberSet(string name, float value)
        {
            GetInstance()[name] = value;
        }

        /// <summary>
        /// Sets a variable of the given name on the instance with an id that matches the value of this object to a string value.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void MemberSet(string name, string value)
        {
            GetInstance()[name] = value;
        }

        /// <summary>
        /// Sets a variable of the given name on the instance with an id that matches the value of this object.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void MemberSet(string name, TsObject value)
        {
            GetInstance()[name] = value;
        }

        /// <summary>
        /// Gets the value of a variable with the given name from the instance with an id that matches the value of this object.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns></returns>
        public TsObject MemberGet(string name)
        {
            return GetInstance()[name];
        }

        #endregion

        #region Array Access

        /// <summary>
        /// Sets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The array index.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(TsObject index, TsObject right)
            => ArraySet((int)index, right);

        /// <summary>
        /// Sets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The array index.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(int index, TsObject right)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (Type != VariableType.Array1)
            {
                Type = VariableType.Array1;
                var temp = new TsObject[index + 1];
                temp[index] = right;
                Value = new TsMutableValue<TsObject[]>(temp);
                return;
            }
            var self = (TsMutableValue<TsObject[]>)Value;
            var arr = self.StrongValue;
            if (index >= arr.Length)
            {
                var temp = new TsObject[index + 1];
                Array.Copy(arr, 0, temp, 0, arr.Length);
                arr = temp;
                self.StrongValue = temp;
            }
            arr[index] = right;
        }

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(TsObject index1, TsObject index2, TsObject right)
            => ArraySet((int)index1, (int)index2, right);

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(int index1, TsObject index2, TsObject right)
            => ArraySet(index1, (int)index2, right);

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(TsObject index1, int index2, TsObject right)
            => ArraySet((int)index1, index2, right);

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(int index1, int index2, TsObject right)
        {
            if (index1 < 0 || index2 < 0)
                throw new ArgumentOutOfRangeException($"{(index1 < 0 ? nameof(index1) : nameof(index2))}");

            if (Type != VariableType.Array2)
            {
                Type = VariableType.Array2;
                var temp = new TsObject[index1 + 1][];
                var inner = new TsObject[index2 + 1];
                inner[index2] = right;
                temp[index1] = inner;
                Value = new TsMutableValue<TsObject[][]>(temp);
                return;
            }

            var self = (TsMutableValue<TsObject[][]>)Value;
            if(index1 >= self.StrongValue.Length)
            {
                var temp = new TsObject[index1 + 1][];
                Array.Copy(self.StrongValue, 0, temp, 0, self.StrongValue.Length);
                self.StrongValue = temp;
            }
            if (self.StrongValue[index1] == null)
                self.StrongValue[index1] = new TsObject[index2 + 1];
            else if(index2 >= self.StrongValue[index1].Length)
            {
                var temp = new TsObject[index2 + 1];
                Array.Copy(self.StrongValue[index1], 0, temp, 0, self.StrongValue[index1].Length);
                self.StrongValue[index1] = temp;
            }
            self.StrongValue[index1][index2] = right;
        }

        /// <summary>
        /// Gets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns></returns>
        public TsObject ArrayGet(TsObject index)
            => ArrayGet((int)index);

        /// <summary>
        /// Gets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns></returns>
        public TsObject ArrayGet(int index)
        {
            return GetArray1D()[index];
        }

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(TsObject index1, TsObject index2)
            => ArrayGet((int)index1, (int)index2);

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(TsObject index1, int index2)
            => ArrayGet((int)index1, index2);

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(int index1, TsObject index2)
            => ArrayGet(index1, (int)index2);

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(int index1, int index2)
        {
            return GetArray2D()[index1][index2];
        }

        #endregion

        #region Delegate Access

        public TsObject DelegateInvoke(params TsObject[] args)
        {
            return GetDelegate().Invoke(args);
        }

        public TsObject DelegateInvoke(TsInstance target, params TsObject[] args)
        {
            return GetDelegate().Invoke(target, args);
        }

        #endregion

        #region Object Overrides

        /// <summary>
        /// Gets the hash code of the underlying value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Type == VariableType.Null)
                return 0;
            return GetValue().GetHashCode();
        }

        /// <summary>
        /// Converts the held value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Type == VariableType.Null)
                return "undefined";
            return GetValue().ToString();
        }

        /// <summary>
        /// Determines if the held value is equal to an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var held = Value.WeakValue;
            if (held is null)
                return obj is null;
            else if (obj is TsObject other)
                return this == other;
            else
                return held.Equals(obj);
        }

        #endregion

        #region Operators

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static explicit operator bool(TsObject right)
        {
            return right.GetBool();
        }

        public static explicit operator char(TsObject right)
        {
            return right.GetChar();
        }

        public static explicit operator byte(TsObject right)
        {
            return right.GetByte();
        }

        public static explicit operator sbyte(TsObject right)
        {
            return right.GetSByte();
        }

        public static explicit operator short(TsObject right)
        {
            return right.GetShort();
        }

        public static explicit operator ushort(TsObject right)
        {
            return right.GetUShort();
        }

        public static explicit operator int(TsObject right)
        {
            return right.GetInt();
        }

        public static explicit operator uint(TsObject right)
        {
            return right.GetUInt();
        }

        public static explicit operator long(TsObject right)
        {
            return right.GetLong();
        }

        public static explicit operator ulong(TsObject right)
        {
            return right.GetULong();
        }

        public static explicit operator float(TsObject right)
        {
            return right.GetFloat();
        }

        public static explicit operator double(TsObject right)
        {
            return right.GetFloat();
        }

        public static explicit operator string(TsObject right)
        {
            return right.GetString();
        }

        public static explicit operator TsInstance(TsObject right)
        {
            return (TsInstance)right.GetInstance();
        }

        public static explicit operator TsDelegate(TsObject right)
        {
            return right.GetDelegate();
        }

        public static explicit operator TsObject[](TsObject right)
        {
            return right.GetArray1D();
        }

        public static explicit operator TsObject[][](TsObject right)
        {
            return right.GetArray2D();
        }

        public static implicit operator TsObject(bool right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(char right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(byte right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(sbyte right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(short right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(ushort right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(int right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(uint right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(long right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(ulong right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(float right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(double right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(string right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(TsInstance right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(TsDelegate right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(TsObject[] right)
        {
            return new TsObject(right);
        }

        public static implicit operator TsObject(TsObject[][] right)
        {
            return new TsObject(right);
        }

        public static TsObject operator +(TsObject obj)
        {
            return new TsObject(+obj.GetFloat());
        }

        public static TsObject operator -(TsObject obj)
        {
            return new TsObject(-obj.GetFloat());
        }

        public static TsObject operator !(TsObject obj)
        {
            return new TsObject(!obj.GetBool());
        }

        public static TsObject operator ~(TsObject obj)
        {
            return new TsObject(~(int)obj.GetFloat());
        }

        public static TsObject operator ++(TsObject obj)
        {
            return new TsObject(obj.GetFloat() + 1f);
        }

        public static TsObject operator --(TsObject obj)
        {
            return new TsObject(obj.GetFloat() - 1f);
        }

        public static bool operator true(TsObject obj) => obj.GetBool();

        public static bool operator false(TsObject obj) => !obj.GetBool();

        public static TsObject operator +(TsObject left, TsObject right)
        {
            if (left.Type == right.Type)
            {
                if (left.Type == VariableType.Real)
                    return new TsObject(left.GetFloat() + right.GetFloat());
                else if (left.Type == VariableType.String)
                    return new TsObject(left.GetString() + right.GetString());
            }
            throw new InvalidOperationException($"Cannot add {left.Type} and {right.Type} together.");
        }

        public static TsObject operator +(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot add {left.Type} and Real together.");
            return new TsObject(left.GetFloat() + right);
        }

        public static TsObject operator +(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot add Real and {right.Type} together.");
            return new TsObject(left + right.GetFloat());
        }

        public static TsObject operator +(TsObject left, string right)
        {
            if (left.Type != VariableType.String)
                throw new InvalidOperationException($"Cannot add {left.Type} and String together.");
            return new TsObject(left.GetString() + right);
        }

        public static TsObject operator +(string left, TsObject right)
        {
            if (right.Type != VariableType.String)
                throw new InvalidOperationException($"Cannot add String and {right.Type} together.");
            return new TsObject(left + right.GetString());
        }

        public static TsObject operator -(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetFloat() - right.GetFloat());
            throw new InvalidOperationException($"Cannot subtract {left.Type} and {right.Type}.");
        }

        public static TsObject operator -(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot subtract {left.Type} and Real together.");
            return new TsObject(left.GetFloat() - right);
        }

        public static TsObject operator -(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot subtract Real and {right.Type} together.");
            return new TsObject(left - right.GetFloat());
        }

        public static TsObject operator *(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetFloat() * right.GetFloat());
            throw new InvalidOperationException($"Cannot multiply {left.Type} and {right.Type}.");
        }

        public static TsObject operator *(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot multiply {left.Type} and Real together.");
            return new TsObject(left.GetFloat() * right);
        }

        public static TsObject operator *(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot multiply Real and {right.Type} together.");
            return new TsObject(left * right.GetFloat());
        }

        public static TsObject operator /(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetFloat() / right.GetFloat());
            throw new InvalidOperationException($"Cannot divide {left.Type} and {right.Type}.");
        }

        public static TsObject operator /(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot divide {left.Type} and Real together.");
            return new TsObject(left.GetFloat() / right);
        }

        public static TsObject operator /(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot divide Real and {right.Type} together.");
            return new TsObject(left / right.GetFloat());
        }

        public static TsObject operator %(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetFloat() % right.GetFloat());
            throw new InvalidOperationException($"Cannot modulo {left.Type} and {right.Type}.");
        }

        public static TsObject operator %(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot modulo {left.Type} and Real together.");
            return new TsObject(left.GetFloat() % right);
        }

        public static TsObject operator %(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot modulo Real and {right.Type} together.");
            return new TsObject(left % right.GetFloat());
        }

        public static TsObject operator &(TsObject left, TsObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and {right.Type}.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) & Convert.ToInt32(right.GetFloat()));
        }

        public static TsObject operator &(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) & Convert.ToInt32(right));
        }

        public static TsObject operator &(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & Real and {right.Type} together.");
            return new TsObject(Convert.ToInt32(left) & Convert.ToInt32(right.GetFloat()));
        }

        public static TsObject operator |(TsObject left, TsObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | {left.Type} and {right.Type}.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) | Convert.ToInt32(right.GetFloat()));
        }

        public static TsObject operator |(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) | Convert.ToInt32(right));
        }

        public static TsObject operator |(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | Real and {right.Type} together.");
            return new TsObject(Convert.ToInt32(left) | Convert.ToInt32(right.GetFloat()));
        }

        public static TsObject operator ^(TsObject left, TsObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ {left.Type} and {right.Type}.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) ^ Convert.ToInt32(right.GetFloat()));
        }

        public static TsObject operator ^(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) ^ Convert.ToInt32(right));
        }

        public static TsObject operator ^(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ Real and {right.Type} together.");
            return new TsObject(Convert.ToInt32(left) ^ Convert.ToInt32(right.GetFloat()));
        }

        public static TsObject operator <<(TsObject left, int right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) << right);
        }

        public static TsObject operator >>(TsObject left, int right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetFloat()) >> right);
        }

        public static bool operator ==(TsObject left, TsObject right)
        {
            if (left.Type != right.Type)
                return false;
            switch (left.Type)
            {
                case VariableType.Real:
                    return left.GetFloatUnchecked() == right.GetFloatUnchecked();
                case VariableType.String:
                    return left.GetStringUnchecked() == right.GetStringUnchecked();
                case VariableType.Array1:
                case VariableType.Array2:
                default:
                    return left.GetValue() == right.GetValue();
            }
        }

        public static bool operator ==(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                return false;

            return left.GetFloatUnchecked() == right;
        }

        public static bool operator ==(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                return false;

            return right.GetFloatUnchecked() == left;
        }

        public static bool operator ==(TsObject left, string right)
        {
            if (left.Type != VariableType.String)
                return false;

            return left.GetStringUnchecked() == right;
        }

        public static bool operator ==(string left, TsObject right)
        {
            if (right.Type != VariableType.String)
                return false;

            return right.GetStringUnchecked() == left;
        }

        public static bool operator !=(TsObject left, TsObject right)
        {
            if (left.Type != right.Type)
                return true;
            switch (left.Type)
            {
                case VariableType.Real:
                    return left.GetFloatUnchecked() != right.GetFloatUnchecked();
                case VariableType.String:
                    return left.GetStringUnchecked() != right.GetStringUnchecked();
                case VariableType.Array1:
                case VariableType.Array2:
                default:
                    return left.GetValue() != right.GetValue();
            }
        }

        public static bool operator !=(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                return true;

            return left.GetFloatUnchecked() != right;
        }

        public static bool operator !=(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                return true;

            return right.GetFloatUnchecked() != left;
        }

        public static bool operator !=(TsObject left, string right)
        {
            if (left.Type != VariableType.String)
                return true;

            return left.GetStringUnchecked() != right;
        }

        public static bool operator !=(string left, TsObject right)
        {
            if (right.Type != VariableType.String)
                return true;

            return right.GetStringUnchecked() != left;
        }

        public static bool operator <(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real)
            {
                if (right.Type == VariableType.Real)
                    return left.GetFloatUnchecked() < right.GetFloatUnchecked();
                else
                    return left.GetFloatUnchecked() < right.GetHashCode();
            }
            else if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetFloatUnchecked();
            else
                return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(TsObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetFloatUnchecked() < right;
            return left.GetHashCode() < right;
        }

        public static bool operator <(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left < right.GetFloatUnchecked();
            return left < right.GetHashCode();
        }

        public static bool operator <(TsObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetFloatUnchecked() < right.GetHashCode();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(string left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetFloatUnchecked();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator >(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real)
            {
                if (right.Type == VariableType.Real)
                    return left.GetFloatUnchecked() > right.GetFloatUnchecked();
                else
                    return left.GetFloatUnchecked() > right.GetHashCode();
            }
            else if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetFloatUnchecked();
            else
                return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(TsObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetFloatUnchecked() > right;
            return left.GetHashCode() > right;
        }

        public static bool operator >(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left > right.GetFloatUnchecked();
            return left > right.GetHashCode();
        }

        public static bool operator >(TsObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetFloatUnchecked() > right.GetHashCode();
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(string left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetFloatUnchecked();
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator <=(TsObject left, TsObject right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(TsObject left, float right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(float left, TsObject right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(TsObject left, string right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(string left, TsObject right)
        {
            return left == right || left < right;
        }

        public static bool operator >=(TsObject left, TsObject right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(TsObject left, float right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(float left, TsObject right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(TsObject left, string right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(string left, TsObject right)
        {
            return left == right || left > right;
        }

        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion

        #region Base Class Library

        public static bool IsArray(TsObject obj)
        {
            return obj.Type == VariableType.Array1 || obj.Type == VariableType.Array2;
        }

        public static bool IsReal(TsObject obj)
        {
            return obj.Type == VariableType.Real;
        }

        public static bool IsString(TsObject obj)
        {
            return obj.Type == VariableType.String;
        }

        public static bool IsDelegate(TsObject obj)
        {
            return obj.Type == VariableType.Delegate;
        }

        public static bool IsUndefined(TsObject obj)
        {
            return obj.Type == VariableType.Null;
        }

        public static string Typeof(TsObject obj)
        {
            switch(obj.Type)
            {
                case VariableType.Array1:
                case VariableType.Array2:
                    return "array";
                case VariableType.Null:
                    return "undefined";
                case VariableType.Real:
                    return "real";
                case VariableType.String:
                    return "string";
                case VariableType.Delegate:
                    return "script";
                default:
                    return "unknown";
            }
        }

        #endregion
    }
}
