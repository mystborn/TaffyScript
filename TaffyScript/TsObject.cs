using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Variant representing a TaffyScript value
    /// </summary>
    public struct TsObject
    {
        public const float All = -3f;
        public const float Noone = -4f;

        /// <summary>
        /// Gets a stack trace of the currently executing instances.
        /// </summary>
        public static Stack<TsObject> Id { get; } = new Stack<TsObject>();

        public VariableType Type { get; private set; }
        public ITsValue Value { get; private set; }

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
            Value = new TsValue<float>(value ? 1 : 0);
        }

        /// <summary>
        /// Creates a string TaffyScript object from a char.
        /// </summary>
        /// <param name="value"></param>
        public TsObject(char value)
        {
            Type = VariableType.String;
            Value = new TsValue<string>(value.ToString());
        }

        /// <summary>
        /// Creates a TaffyScript object from a byte.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(byte value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from an sbyte.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(sbyte value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a short.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(short value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a ushort.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(ushort value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from an int.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(int value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a uint.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(uint value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a long.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(long value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a ulong.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(ulong value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a float.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(float value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a double.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(double value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>((float)value);
        }

        /// <summary>
        /// Creates a TaffyScript object from a string.
        /// </summary>
        /// <param name="value">The value of the object.</param>
        public TsObject(string value)
        {
            Type = VariableType.String;
            Value = new TsValue<string>(value);
        }

        /// <summary>
        /// Creates a TaffyScript object from an instances id.
        /// </summary>
        /// <param name="instance">The instance to get the id from.</param>
        public TsObject(TsInstance instance)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(instance.Id);
        }

        /// <summary>
        /// Creates a TaffyScript object from a 1D array.
        /// </summary>
        /// <param name="array">The value of the object.</param>
        public TsObject(TsObject[] array)
        {
            Type = VariableType.Array1;
            Value = new TsValueArray<TsObject[]>(array);
        }

        /// <summary>
        /// Creates a TaffyScript object from a 2D array
        /// </summary>
        /// <param name="array">The value of the object.</param>
        public TsObject(TsObject[][] array)
        {
            Type = VariableType.Array2;
            Value = new TsValueArray<TsObject[][]>(array);
        }

        /// <summary>
        /// Creates an empty TaffyScript object.
        /// </summary>
        public static TsObject Empty()
        {
            return new TsObject(VariableType.Null, new TsValue<object>(null));
        }

        public static TsObject NooneObject()
        {
            return new TsObject(Noone);
        }

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
            return ((TsValue<float>)Value).StrongValue;
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
            return ((TsValue<float>)Value).StrongValue;
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
            return ((TsValue<string>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the string value held by this object without checking its type.
        /// </summary>
        /// <returns></returns>
        public string GetStringUnchecked()
        {
            return ((TsValue<string>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the instance that has an id matching the value held by this object.
        /// </summary>
        /// <returns></returns>
        public TsInstance GetInstance()
        {
            if (!TsInstance.TryGetInstance(GetFloat(), out var inst))
                throw new InvalidInstanceException();
            return inst;
        }

        /// <summary>
        /// Gets the 1D array held by this object.
        /// </summary>
        /// <returns></returns>
        public TsObject[] GetArray1D()
        {
            if (Type != VariableType.Array1)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Array1D, is {Type} instead.");
            return ((TsValueArray<TsObject[]>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the 2D array held by this object.
        /// </summary>
        /// <returns></returns>
        public TsObject[][] GetArray2D()
        {
            if (Type != VariableType.Array2)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Array2D, is {Type} instead.");
            return ((TsValueArray<TsObject[][]>)Value).StrongValue;
        }

        /// <summary>
        /// Gets the untyped value held by this object.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (Value == null)
                return 0;
            return Value.WeakValue;
        }

        #endregion

        #region Member Access

        /// <summary>
        /// Gets the id of the currently executing instance
        /// </summary>
        /// <returns></returns>
        public static TsObject GetId()
        {
            return Id.Peek();
        }

        /// <summary>
        /// Attempts to get the id of the currently executing instance.
        /// </summary>
        /// <param name="id">If found, the instance id</param>
        /// <returns></returns>
        public static bool TryGetId(out TsObject id)
        {
            if(Id.Count != 0)
            {
                id = Id.Peek();
                return true;
            }
            id = Empty();
            return false;
        }

        /// <summary>
        /// Sets a variable of the given name on the instance with an id that matches the value of this object to a float value.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void MemberSet(string name, float value)
        {
            if (!TsInstance.TryGetInstance(GetFloat(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = new TsObject(value);
        }

        /// <summary>
        /// Sets a variable of the given name on the instance with an id that matches the value of this object to a string value.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void MemberSet(string name, string value)
        {
            if (!TsInstance.TryGetInstance(GetFloat(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = new TsObject(value);
        }

        /// <summary>
        /// Sets a variable of the given name on the instance with an id that matches the value of this object.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void MemberSet(string name, TsObject value)
        {
            if (!TsInstance.TryGetInstance(GetFloat(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = value;
        }

        /// <summary>
        /// Gets the value of a variable with the given name from the instance with an id that matches the value of this object.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns></returns>
        public TsObject MemberGet(string name)
        {
            if (!TsInstance.TryGetInstance(GetFloat(), out var inst))
                throw new InvalidInstanceException();
            return inst[name];
        }

        #endregion

        #region Array Access

        /// <summary>
        /// Sets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The array index.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(TsObject index, TsObject right)
            => ArraySet(index.GetFloat(), right);

        /// <summary>
        /// Sets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The array index.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(float index, TsObject right)
        {
            var real = (int)index;
            if (real < 0)
                throw new ArgumentOutOfRangeException("index");
            if (Type != VariableType.Array1)
            {
                Type = VariableType.Array1;
                var temp = new TsObject[real + 1];
                temp[real] = right;
                Value = new TsValueArray<TsObject[]>(temp);
                return;
            }
            var self = (TsValueArray<TsObject[]>)Value;
            var arr = self.StrongValue;
            if (index >= arr.Length)
            {
                var temp = new TsObject[real + 1];
                Array.Copy(arr, 0, temp, 0, arr.Length);
                arr = temp;
                self.StrongValue = temp;
            }
            arr[real] = right;
        }

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(TsObject index1, TsObject index2, TsObject right)
            => ArraySet(index1.GetFloat(), index2.GetFloat(), right);

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(float index1, TsObject index2, TsObject right)
            => ArraySet(index1, index2.GetFloat(), right);

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(TsObject index1, float index2, TsObject right)
            => ArraySet(index1.GetFloat(), index2, right);

        /// <summary>
        /// Sets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <param name="right">The value of the index.</param>
        public void ArraySet(float index1, float index2, TsObject right)
        {
            int real1 = (int)index1;
            int real2 = (int)index2;
            if (real1 < 0 || index2 < 0)
                throw new IndexOutOfRangeException();
            if (Type != VariableType.Array2)
            {
                Type = VariableType.Array2;
                var temp = new TsObject[real1 + 1][];
                var inner = new TsObject[real2 + 1];
                inner[real2] = right;
                temp[real1] = inner;
                Value = new TsValueArray<TsObject[][]>(temp);
                return;
            }
            var self = (TsValueArray<TsObject[][]>)Value;
            if(real1 >= self.StrongValue.Length)
            {
                var temp = new TsObject[real1 + 1][];
                Array.Copy(self.StrongValue, 0, temp, 0, self.StrongValue.Length);
                self.StrongValue = temp;
            }
            if (self.StrongValue[real1] == null)
                self.StrongValue[real1] = new TsObject[real2 + 1];
            else if(real2 >= self.StrongValue[real1].Length)
            {
                var temp = new TsObject[real2 + 1];
                Array.Copy(self.StrongValue[real1], 0, temp, 0, self.StrongValue[real2].Length);
                self.StrongValue[real1] = temp;
            }
            self.StrongValue[real1][real2] = right;
        }

        /// <summary>
        /// Gets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns></returns>
        public TsObject ArrayGet(TsObject index)
            => ArrayGet(index.GetFloat());

        /// <summary>
        /// Gets the value at the given index in the 1D array held by this object.
        /// </summary>
        /// <param name="index">The index of the value.</param>
        /// <returns></returns>
        public TsObject ArrayGet(float index)
        {
            var real = (int)index;
            var arr = GetArray1D();
            if (real < 0 || real >= arr.Length)
                throw new IndexOutOfRangeException();
            return arr[real];
        }

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(TsObject index1, TsObject index2)
            => ArrayGet((float)index1, (float)index2);

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(TsObject index1, float index2)
            => ArrayGet((float)index1, index2);

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(float index1, TsObject index2)
            => ArrayGet(index1, (float)index2);

        /// <summary>
        /// Gets the value at the given indeces in the 2D array held by this object.
        /// </summary>
        /// <param name="index1">The index of the first dimension.</param>
        /// <param name="index2">The index of the second dimension.</param>
        /// <returns></returns>
        public TsObject ArrayGet(float index1, float index2)
        {
            var real1 = (int)index1;
            var real2 = (int)index2;
            var arr = GetArray2D();
            if (real1 < 0 || real1 >= arr.Length || real2 < 0 || real2 >= arr[real1].Length)
                throw new IndexOutOfRangeException();
            return arr[real1][real2];
        }

        #endregion

        public override int GetHashCode()
        {
            if (Type == VariableType.Null)
                return 0;
            return GetValue().GetHashCode();
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public override bool Equals(object obj)
        {
            if(obj != null)
            {
                if (obj is string str)
                    return this == str;
                else if (obj is float f)
                    return this == f;
                else if (obj is TsObject other)
                    return this == other;
            }
            return false;
        }

        #region Operators

        public static explicit operator float(TsObject right)
        {
            return right.GetFloat();
        }

        public static explicit operator int(TsObject right)
        {
            return (int)right.GetFloat();
        }

        public static explicit operator string(TsObject right)
        {
            return right.GetString();
        }

        public static explicit operator bool(TsObject right)
        {
            return right.GetBool();
        }

        public static explicit operator TsObject(float right)
        {
            return new TsObject(right);
        }

        public static explicit operator TsObject(int right)
        {
            return new TsObject(right);
        }

        public static explicit operator TsObject(string right)
        {
            return new TsObject(right);
        }

        public static explicit operator TsObject(bool right)
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

        #endregion


        //The unsafe block works just GM does, but it's extremely volatile.
        //The safe block won't work exactly like GM, but it's close enough.
        //If absolutely necessary, use the unsafe flag.

#if Unsafe

        private static unsafe int GetMemAddress(object obj)
        {
            TypedReference reference = __makeref(obj);
            var ptr = **(IntPtr**)(&reference);
            return ptr.ToInt32();
        }

#else

        private static int GetMemAddress(object obj)
        {
            if (obj is null)
                return 0;
            return obj.GetHashCode();
        }

#endif

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
                default:
                    return "unknown";
            }
        }

        public static void ConditionalTest()
        {
            float f = -1;
            var result = +f;
        }

        #endregion

    }
}
