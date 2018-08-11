using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Number = System.Single;

namespace TaffyScript
{
    // Originally this type was a valuetype, which is preferable.
    // Unfortunately, the old way caused a lot of boxing,
    // which makes this version actually faster and
    // it produces less garbage.

    public abstract class TsObject
    {
        private static TsNull _null = new TsNull();

        public static TsObject Empty => _null;

        public abstract VariableType Type { get; }
        public abstract object WeakValue { get; }

        public abstract Number GetNumber();
        public abstract string GetString();
        public abstract TsObject[] GetArray();
        public abstract ITsInstance GetInstance();
        public abstract TsDelegate GetDelegate();

        /// <summary>
        /// Gets the value of this object as a bool.
        /// </summary>
        /// <returns></returns>
        public bool GetBool()
        {
            return GetNumber() > 0;
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
            return (byte)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as an sbyte.
        /// </summary>
        /// <returns></returns>
        public sbyte GetSByte()
        {
            return (sbyte)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as a short.
        /// </summary>
        /// <returns></returns>
        public short GetShort()
        {
            return (short)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as a ushort.
        /// </summary>
        /// <returns></returns>
        public ushort GetUShort()
        {
            return (ushort)GetNumber();
        }

        /// <summary>
        /// Gets the number value held by this object as an int.
        /// </summary>
        public int GetInt()
        {
            return (int)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as a uint.
        /// </summary>
        /// <returns></returns>
        public uint GetUInt()
        {
            return (uint)GetNumber();
        }

        /// <summary>
        /// Gets the number value held by this object as a long.
        /// </summary>
        /// <returns></returns>
        public long GetLong()
        {
            return (long)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as a ulong.
        /// </summary>
        /// <returns></returns>
        public ulong GetULong()
        {
            return (ulong)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as a float.
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            return (float)GetNumber();
        }

        /// <summary>
        /// Gets the value of this object as a float.
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
        {
            return (double)GetNumber();
        }

        public TsObject[] GetArray(int index)
        {
            return GetArray()[index].GetArray();
        }

        public TsObject[] GetArray(int index1, int index2)
        {
            return GetArray()[index1].GetArray()[index2].GetArray();
        }

        public TsObject[] GetArray(params int[] indeces)
        {
            var arr = GetArray();
            for (var i = 0; i < indeces.Length; i++)
                arr = arr[indeces[i]].GetArray();

            return arr;
        }

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

        public void ArraySet(TsObject right, int index)
        {
            GetArray()[index] = right;
        }

        public void ArraySet(TsObject right, int index1, int index2)
        {
            GetArray()[index1].GetArray()[index2] = right;
        }

        public void ArraySet(TsObject right, int index1, int index2, int index3)
        {
            GetArray()[index1].GetArray()[index2].GetArray()[index3] = right;
        }

        public void ArraySet(TsObject right, params int[] indeces)
        {
            var obj = this;
            var last = indeces.Length - 1;
            for (var i = 0; i < last; i++)
                obj = obj.GetArray()[indeces[i]];

            obj.GetArray()[indeces[last]] = right;
        }

        public TsObject ArrayGet(int index)
        {
            return GetArray()[index];
        }

        public TsObject ArrayGet(int index1, int index2)
        {
            return GetArray()[index1].GetArray()[index2];
        }

        public TsObject ArrayGet(int index1, int index2, int index3)
        {
            return GetArray()[index1].GetArray()[index2].GetArray()[index3];
        }

        public TsObject ArrayGet(params int[] indeces)
        {
            var obj = this;
            for (var i = 0; i < indeces.Length; i++)
                obj = obj.GetArray()[indeces[i]];

            return obj;
        }

        #endregion

        #region Delegate Access

        public TsObject DelegateInvoke(params TsObject[] args)
        {
            return GetDelegate().Invoke(args);
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
            return right.GetNumber();
        }

        public static explicit operator double(TsObject right)
        {
            return right.GetNumber();
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

        public static explicit operator TsObject[] (TsObject right)
        {
            return right.GetArray();
        }

        public static implicit operator TsObject(bool right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(char right)
        {
            return new TsString(right);
        }

        public static implicit operator TsObject(byte right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(sbyte right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(short right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(ushort right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(int right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(uint right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(long right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(ulong right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(float right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(double right)
        {
            return new TsNumber(right);
        }

        public static implicit operator TsObject(string right)
        {
            return new TsString(right);
        }

        public static implicit operator TsObject(TsInstance right)
        {
            return new TsInstanceWrapper(right);
        }

        public static implicit operator TsObject(TsDelegate right)
        {
            return new TsDelegateWrapper(right);
        }

        public static implicit operator TsObject(TsObject[] right)
        {
            return new TsArray(right);
        }

        public static TsObject operator +(TsObject obj)
        {
            return new TsNumber(+obj.GetNumber());
        }

        public static TsObject operator -(TsObject obj)
        {
            return new TsNumber(-obj.GetNumber());
        }

        public static TsObject operator !(TsObject obj)
        {
            return new TsNumber(!obj.GetBool());
        }

        public static TsObject operator ~(TsObject obj)
        {
            return new TsNumber(~(int)obj.GetNumber());
        }

        public static TsObject operator ++(TsObject obj)
        {
            return new TsNumber(obj.GetNumber() + 1f);
        }

        public static TsObject operator --(TsObject obj)
        {
            return new TsNumber(obj.GetNumber() - 1f);
        }

        public static bool operator true(TsObject obj) => obj.GetBool();

        public static bool operator false(TsObject obj) => !obj.GetBool();

        public static TsObject operator +(TsObject left, TsObject right)
        {
            if (left.Type == right.Type)
            {
                if (left.Type == VariableType.Real)
                    return new TsNumber(left.GetNumber() + right.GetNumber());
                else if (left.Type == VariableType.String)
                    return new TsString(left.GetString() + right.GetString());
            }
            throw new InvalidTsTypeException($"Cannot add {left.Type} and {right.Type} together.");
        }

        public static TsObject operator +(TsObject left, float right)
        {
            return new TsNumber(left.GetNumber() + right);
        }

        public static TsObject operator +(float left, TsObject right)
        {
            return new TsNumber(left + right.GetNumber());
        }

        public static TsObject operator +(TsObject left, string right)
        {
            return new TsString(left.GetString() + right);
        }

        public static TsObject operator +(string left, TsObject right)
        {
            return new TsString(left + right.GetString());
        }

        public static TsObject operator -(TsObject left, TsObject right)
        {
            return new TsNumber(left.GetNumber() - right.GetNumber());
        }

        public static TsObject operator -(TsObject left, float right)
        {
            return new TsNumber(left.GetNumber() - right);
        }

        public static TsObject operator -(float left, TsObject right)
        {
            return new TsNumber(left - right.GetNumber());
        }

        public static TsObject operator *(TsObject left, TsObject right)
        {
            return new TsNumber(left.GetNumber() * right.GetNumber());
        }

        public static TsObject operator *(TsObject left, float right)
        {
            return new TsNumber(left.GetNumber() * right);
        }

        public static TsObject operator *(float left, TsObject right)
        {
            return new TsNumber(left * right.GetNumber());
        }

        public static TsObject operator /(TsObject left, TsObject right)
        {
            return new TsNumber(left.GetNumber() / right.GetNumber());
        }

        public static TsObject operator /(TsObject left, float right)
        {
            return new TsNumber(left.GetNumber() / right);
        }

        public static TsObject operator /(float left, TsObject right)
        {
            return new TsNumber(left / right.GetNumber());
        }

        public static TsObject operator %(TsObject left, TsObject right)
        {
            return new TsNumber(left.GetNumber() % right.GetNumber());
        }

        public static TsObject operator %(TsObject left, float right)
        {
            return new TsNumber(left.GetNumber() % right);
        }

        public static TsObject operator %(float left, TsObject right)
        {
            return new TsNumber(left % right.GetNumber());
        }

        public static TsObject operator &(TsObject left, TsObject right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) & Convert.ToInt32(right.GetNumber()));
        }

        public static TsObject operator &(TsObject left, float right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) & Convert.ToInt32(right));
        }

        public static TsObject operator &(float left, TsObject right)
        {
            return new TsNumber(Convert.ToInt32(left) & Convert.ToInt32(right.GetNumber()));
        }

        public static TsObject operator |(TsObject left, TsObject right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) | Convert.ToInt32(right.GetNumber()));
        }

        public static TsObject operator |(TsObject left, float right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) | Convert.ToInt32(right));
        }

        public static TsObject operator |(float left, TsObject right)
        {
            return new TsNumber(Convert.ToInt32(left) | Convert.ToInt32(right.GetNumber()));
        }

        public static TsObject operator ^(TsObject left, TsObject right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) ^ Convert.ToInt32(right.GetNumber()));
        }

        public static TsObject operator ^(TsObject left, float right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) ^ Convert.ToInt32(right));
        }

        public static TsObject operator ^(float left, TsObject right)
        {
            return new TsNumber(Convert.ToInt32(left) ^ Convert.ToInt32(right.GetNumber()));
        }

        public static TsObject operator <<(TsObject left, int right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) << right);
        }

        public static TsObject operator >>(TsObject left, int right)
        {
            return new TsNumber(Convert.ToInt32(left.GetNumber()) >> right);
        }

        public static bool operator ==(TsObject left, TsObject right)
        {
            if (left.Type != right.Type)
                return false;
            switch (left.Type)
            {
                case VariableType.Real:
                    return left.GetNumber() == right.GetNumber();
                case VariableType.String:
                    return left.GetString() == right.GetString();
                default:
                    return left.WeakValue == right.WeakValue;
            }
        }

        public static bool operator ==(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                return false;

            return left.GetNumber() == right;
        }

        public static bool operator ==(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                return false;

            return right.GetNumber() == left;
        }

        public static bool operator ==(TsObject left, string right)
        {
            if (left.Type != VariableType.String)
                return false;

            return left.GetString() == right;
        }

        public static bool operator ==(string left, TsObject right)
        {
            if (right.Type != VariableType.String)
                return false;

            return right.GetString() == left;
        }

        public static bool operator !=(TsObject left, TsObject right)
        {
            if (left.Type != right.Type)
                return true;
            switch (left.Type)
            {
                case VariableType.Real:
                    return left.GetNumber() != right.GetNumber();
                case VariableType.String:
                    return left.GetString() != right.GetString();
                default:
                    return left.WeakValue != right.WeakValue;
            }
        }

        public static bool operator !=(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                return true;

            return left.GetNumber() != right;
        }

        public static bool operator !=(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                return true;

            return right.GetNumber() != left;
        }

        public static bool operator !=(TsObject left, string right)
        {
            if (left.Type != VariableType.String)
                return true;

            return left.GetString() != right;
        }

        public static bool operator !=(string left, TsObject right)
        {
            if (right.Type != VariableType.String)
                return true;

            return right.GetString() != left;
        }

        public static bool operator <(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real)
            {
                if (right.Type == VariableType.Real)
                    return left.GetNumber() < right.GetNumber();
                else
                    return left.GetNumber() < right.GetHashCode();
            }
            else if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetNumber();
            else
                return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(TsObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumber() < right;
            return left.GetHashCode() < right;
        }

        public static bool operator <(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left < right.GetNumber();
            return left < right.GetHashCode();
        }

        public static bool operator <(TsObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumber() < right.GetHashCode();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(string left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetNumber();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator >(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real)
            {
                if (right.Type == VariableType.Real)
                    return left.GetNumber() > right.GetNumber();
                else
                    return left.GetNumber() > right.GetHashCode();
            }
            else if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetNumber();
            else
                return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(TsObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumber() > right;
            return left.GetHashCode() > right;
        }

        public static bool operator >(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left > right.GetNumber();
            return left > right.GetHashCode();
        }

        public static bool operator >(TsObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumber() > right.GetHashCode();
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(string left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetNumber();
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
    }
}
