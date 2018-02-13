using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public struct TsObject
    {
        public const float All = -3f;
        public static Stack<TsObject> Id { get; } = new Stack<TsObject>();

        /// <summary>
        /// Global GM object. DO NOT SET.
        /// </summary>
        public static TsObject Global = TsInstance.InitGlobal();

        public VariableType Type { get; private set; }
        public ITsValue Value { get; private set; }

        private TsObject(VariableType type, ITsValue value)
        {
            Type = type;
            Value = value;
        }

        public TsObject(bool value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value ? 1 : 0);
        }

        public TsObject(int value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        public TsObject(float value)
        {
            Type = VariableType.Real;
            Value = new TsValue<float>(value);
        }

        public TsObject(string value)
        {
            Type = VariableType.String;
            Value = new TsValue<string>(value);
        }

        public TsObject(TsObject[] array)
        {
            Type = VariableType.Array1;
            Value = new TsValueArray<TsObject[]>(array);
        }

        public TsObject(TsObject[][] array)
        {
            Type = VariableType.Array2;
            Value = new TsValueArray<TsObject[][]>(array);
        }

        public static TsObject Empty()
        {
            return new TsObject(VariableType.Null, new TsValue<object>(null));
        }

        #region Raw Values

        public float GetNum()
        {
            if (Type == VariableType.Null)
                return 0;
            if (Type != VariableType.Real)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Real, is {Type} instead.");
            return ((TsValue<float>)Value).StrongValue;
        }

        /// <summary>
        /// Particularly useful when accessing native array indices.
        /// </summary>
        public int GetNumAsInt()
        {
            return (int)GetNum();
        }

        /// <summary>
        /// Gets this object as a long. Used for bitwise operations.
        /// </summary>
        /// <returns></returns>
        public long GetNumAsLong()
        {
            return (long)GetNum();
        }

        public float GetNumUnchecked()
        {
            return ((TsValue<float>)Value).StrongValue;
        }

        public string GetString()
        {
            if (Type == VariableType.Null)
                return "";
            if (Type != VariableType.String)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Real, is {Type} instead.");
            return ((TsValue<string>)Value).StrongValue;
        }

        public string GetStringUnchecked()
        {
            return ((TsValue<string>)Value).StrongValue;
        }

        public bool GetBool()
        {
            return GetNum() >= .5f;
        }

        public TsInstance GetInstance()
        {
            if (!TsInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            return inst;
        }

        public TsObject[] GetArray1D()
        {
            if (Type != VariableType.Array1)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Array1D, is {Type} instead.");
            return ((TsValueArray<TsObject[]>)Value).StrongValue;
        }

        public TsObject[][] GetArray2D()
        {
            if (Type != VariableType.Array2)
                throw new InvalidTsTypeException($"Variable is supposed to be of type Array2D, is {Type} instead.");
            return ((TsValueArray<TsObject[][]>)Value).StrongValue;
        }

        public object GetValue()
        {
            if (Value == null)
                return 0;
            return Value.WeakValue;
        }

        #endregion

        #region Member Access

        public static TsObject GetId()
        {
            return Id.Peek();
        }

        public void MemberSet(string name, float value)
        {
            if (!TsInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = new TsObject(value);
        }

        public void MemberSet(string name, string value)
        {
            if (!TsInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = new TsObject(value);
        }

        public void MemberSet(string name, TsObject value)
        {
            if (!TsInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = value;
        }

        public TsObject MemberGet(string name)
        {
            if (!TsInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            return inst[name];
        }

        #endregion

        #region Array Access

        public void ArraySet(TsObject index, TsObject right)
            => ArraySet(index.GetNum(), right);

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

        public void ArraySet(TsObject index1, TsObject index2, TsObject right)
            => ArraySet(index1.GetNum(), index2.GetNum(), right);

        public void ArraySet(float index1, TsObject index2, TsObject right)
            => ArraySet(index1, index2.GetNum(), right);

        public void ArraySet(TsObject index1, float index2, TsObject right)
            => ArraySet(index1.GetNum(), index2, right);

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

        public TsObject ArrayGet(TsObject index)
            => ArrayGet(index.GetNum());

        public TsObject ArrayGet(float index)
        {
            var real = (int)index;
            var arr = GetArray1D();
            if (real < 0 || real >= arr.Length)
                throw new IndexOutOfRangeException();
            return arr[real];
        }

        public TsObject ArrayGet(TsObject index1, TsObject index2)
            => ArrayGet((float)index1, (float)index2);

        public TsObject ArrayGet(TsObject index1, float index2)
            => ArrayGet((float)index1, index2);

        public TsObject ArrayGet(float index1, TsObject index2)
            => ArrayGet(index1, (float)index2);

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
            return right.GetNum();
        }

        public static explicit operator int(TsObject right)
        {
            return (int)right.GetNum();
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
            return new TsObject(+obj.GetNum());
        }

        public static TsObject operator -(TsObject obj)
        {
            return new TsObject(-obj.GetNum());
        }

        public static TsObject operator !(TsObject obj)
        {
            return new TsObject(!obj.GetBool());
        }

        public static TsObject operator ~(TsObject obj)
        {
            return new TsObject(~(int)obj.GetNum());
        }

        public static TsObject operator ++(TsObject obj)
        {
            return new TsObject(obj.GetNum() + 1f);
        }

        public static TsObject operator --(TsObject obj)
        {
            return new TsObject(obj.GetNum() - 1f);
        }

        public static bool operator true(TsObject obj) => obj.GetBool();

        public static bool operator false(TsObject obj) => !obj.GetBool();

        public static TsObject operator +(TsObject left, TsObject right)
        {
            if (left.Type == right.Type)
            {
                if (left.Type == VariableType.Real)
                    return new TsObject(left.GetNum() + right.GetNum());
                else if (left.Type == VariableType.String)
                    return new TsObject(left.GetString() + right.GetString());
            }
            throw new InvalidOperationException($"Cannot add {left.Type} and {right.Type} together.");
        }

        public static TsObject operator +(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot add {left.Type} and Real together.");
            return new TsObject(left.GetNum() + right);
        }

        public static TsObject operator +(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot add Real and {right.Type} together.");
            return new TsObject(left + right.GetNum());
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
                return new TsObject(left.GetNum() - right.GetNum());
            throw new InvalidOperationException($"Cannot subtract {left.Type} and {right.Type}.");
        }

        public static TsObject operator -(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot subtract {left.Type} and Real together.");
            return new TsObject(left.GetNum() - right);
        }

        public static TsObject operator -(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot subtract Real and {right.Type} together.");
            return new TsObject(left - right.GetNum());
        }

        public static TsObject operator *(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetNum() * right.GetNum());
            throw new InvalidOperationException($"Cannot multiply {left.Type} and {right.Type}.");
        }

        public static TsObject operator *(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot multiply {left.Type} and Real together.");
            return new TsObject(left.GetNum() * right);
        }

        public static TsObject operator *(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot multiply Real and {right.Type} together.");
            return new TsObject(left * right.GetNum());
        }

        public static TsObject operator /(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetNum() / right.GetNum());
            throw new InvalidOperationException($"Cannot divide {left.Type} and {right.Type}.");
        }

        public static TsObject operator /(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot divide {left.Type} and Real together.");
            return new TsObject(left.GetNum() / right);
        }

        public static TsObject operator /(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot divide Real and {right.Type} together.");
            return new TsObject(left / right.GetNum());
        }

        public static TsObject operator %(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new TsObject(left.GetNum() % right.GetNum());
            throw new InvalidOperationException($"Cannot modulo {left.Type} and {right.Type}.");
        }

        public static TsObject operator %(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot modulo {left.Type} and Real together.");
            return new TsObject(left.GetNum() % right);
        }

        public static TsObject operator %(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot modulo Real and {right.Type} together.");
            return new TsObject(left % right.GetNum());
        }

        public static TsObject operator &(TsObject left, TsObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and {right.Type}.");
            return new TsObject(Convert.ToInt32(left.GetNum()) & Convert.ToInt32(right.GetNum()));
        }

        public static TsObject operator &(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetNum()) & Convert.ToInt32(right));
        }

        public static TsObject operator &(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & Real and {right.Type} together.");
            return new TsObject(Convert.ToInt32(left) & Convert.ToInt32(right.GetNum()));
        }

        public static TsObject operator |(TsObject left, TsObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | {left.Type} and {right.Type}.");
            return new TsObject(Convert.ToInt32(left.GetNum()) | Convert.ToInt32(right.GetNum()));
        }

        public static TsObject operator |(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetNum()) | Convert.ToInt32(right));
        }

        public static TsObject operator |(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | Real and {right.Type} together.");
            return new TsObject(Convert.ToInt32(left) | Convert.ToInt32(right.GetNum()));
        }

        public static TsObject operator ^(TsObject left, TsObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ {left.Type} and {right.Type}.");
            return new TsObject(Convert.ToInt32(left.GetNum()) ^ Convert.ToInt32(right.GetNum()));
        }

        public static TsObject operator ^(TsObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetNum()) ^ Convert.ToInt32(right));
        }

        public static TsObject operator ^(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ Real and {right.Type} together.");
            return new TsObject(Convert.ToInt32(left) ^ Convert.ToInt32(right.GetNum()));
        }

        public static TsObject operator <<(TsObject left, int right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetNum()) << right);
        }

        public static TsObject operator >>(TsObject left, int right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new TsObject(Convert.ToInt32(left.GetNum()) >> right);
        }

        public static bool operator ==(TsObject left, TsObject right)
        {
            if (left.Type != right.Type)
                return false;
            switch (left.Type)
            {
                case VariableType.Real:
                    return left.GetNumUnchecked() == right.GetNumUnchecked();
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

            return left.GetNumUnchecked() == right;
        }

        public static bool operator ==(float left, TsObject right)
        {
            if (right.Type != VariableType.Real)
                return false;

            return right.GetNumUnchecked() == left;
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
                    return left.GetNumUnchecked() != right.GetNumUnchecked();
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
            if (left.Type == VariableType.Real)
                return false;

            return left.GetNumUnchecked() != right;
        }

        public static bool operator !=(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return false;

            return right.GetNumUnchecked() != left;
        }

        public static bool operator !=(TsObject left, string right)
        {
            if (left.Type == VariableType.String)
                return false;

            return left.GetStringUnchecked() != right;
        }

        public static bool operator !=(string left, TsObject right)
        {
            if (right.Type == VariableType.String)
                return false;

            return right.GetStringUnchecked() != left;
        }

        public static bool operator <(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real)
            {
                if (right.Type == VariableType.Real)
                    return left.GetNumUnchecked() < right.GetNumUnchecked();
                else
                    return left.GetNumUnchecked() < right.GetHashCode();
            }
            else if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetNumUnchecked();
            else
                return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(TsObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() < right;
            return left.GetHashCode() < right;
        }

        public static bool operator <(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left < right.GetNumUnchecked();
            return left < right.GetHashCode();
        }

        public static bool operator <(TsObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() < right.GetHashCode();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(string left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetNumUnchecked();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator >(TsObject left, TsObject right)
        {
            if (left.Type == VariableType.Real)
            {
                if (right.Type == VariableType.Real)
                    return left.GetNumUnchecked() > right.GetNumUnchecked();
                else
                    return left.GetNumUnchecked() > right.GetHashCode();
            }
            else if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetNumUnchecked();
            else
                return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(TsObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() > right;
            return left.GetHashCode() > right;
        }

        public static bool operator >(float left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left > right.GetNumUnchecked();
            return left > right.GetHashCode();
        }

        public static bool operator >(TsObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() > right.GetHashCode();
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(string left, TsObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetNumUnchecked();
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

        public static string ToString(object obj)
        {
            return obj.ToString();
        }

        public static void ShowDebugMessage(TsInstance obj)
        {
            Console.WriteLine(obj);
        }

        public static void ConditionalTest()
        {
            var list = new List<int>();
            list.Add(1);
            list[0]++;
        }

        #endregion

    }
}
