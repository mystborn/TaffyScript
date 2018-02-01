using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public struct GmObject
    {
        public VariableType Type { get; }
        public IGmValue Value { get; }

        private GmObject(VariableType type, IGmValue value)
        {
            Type = type;
            Value = value;
        }

        public GmObject(bool value)
        {
            Type = VariableType.Real;
            Value = new GmValue<float>(value ? 1 : 0);
        }

        public GmObject(int value)
        {
            Type = VariableType.Real;
            Value = new GmValue<float>(value);
        }

        public GmObject(float value)
        {
            Type = VariableType.Real;
            Value = new GmValue<float>(value);
        }

        public GmObject(string value)
        {
            Type = VariableType.String;
            Value = new GmValue<string>(value);
        }

        public GmObject(GmObject[] array)
        {
            Type = VariableType.Array1;
            Value = new GmValueArray<GmObject[]>(array);
        }

        public GmObject(GmObject[][] array)
        {
            Type = VariableType.Array2;
            Value = new GmValueArray<GmObject[][]>(array);
        }

        public static GmObject Empty()
        {
            return new GmObject(VariableType.Null, new GmValue<object>(null));
        }

        #region Raw Values

        public float GetNum()
        {
            if (Type != VariableType.Real)
                throw new InvalidGmTypeException($"Variable is supposed to be of type Real, is {Type} instead.");
            return ((GmValue<float>)Value).StrongValue;
        }

        public float GetNumUnchecked()
        {
            return ((GmValue<float>)Value).StrongValue;
        }

        public string GetString()
        {
            if (Type != VariableType.String)
                throw new InvalidGmTypeException($"Variable is supposed to be of type Real, is {Type} instead.");
            return ((GmValue<string>)Value).StrongValue;
        }

        public string GetStringUnchecked()
        {
            return ((GmValue<string>)Value).StrongValue;
        }

        public bool GetBool()
        {
            return GetNum() >= .5f;
        }

        public GmInstance GetInstance()
        {
            if (!GmInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            return inst;
        }

        public GmObject[] GetArray1D()
        {
            if (Type != VariableType.Array1)
                throw new InvalidGmTypeException($"Variable is supposed to be of type Array1D, is {Type} instead.");
            return ((GmValueArray<GmObject[]>)Value).StrongValue;
        }

        public GmObject[][] GetArray2D()
        {
            if (Type != VariableType.Array2)
                throw new InvalidGmTypeException($"Variable is supposed to be of type Array2D, is {Type} instead.");
            return ((GmValueArray<GmObject[][]>)Value).StrongValue;
        }

        public object GetValue()
        {
            return Value.WeakValue;
        }

        #endregion

        #region Member Access

        public void MemberSet(string name, float value)
        {
            if (!GmInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = new GmObject(value);
        }

        public void MemberSet(string name, string value)
        {
            if (!GmInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = new GmObject(value);
        }

        public void MemberSet(string name, GmObject value)
        {
            if (!GmInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            inst[name] = value;
        }

        public GmObject MemberGet(string name)
        {
            if (!GmInstance.TryGet(GetNum(), out var inst))
                throw new InvalidInstanceException();
            return inst[name];
        }

        #endregion

        #region Array Access

        public void ArraySet(GmObject index, GmObject right)
            => ArraySet((int)index.GetNum(), right);

        public void ArraySet(int index, GmObject right)
        {
            if (Type != VariableType.Array1)
                throw new InvalidInstanceException();
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            var self = (GmValueArray<GmObject[]>)Value;
            var arr = self.StrongValue;
            if (index >= arr.Length)
            {
                var temp = new GmObject[index + 1];
                Buffer.BlockCopy(arr, 0, temp, 0, arr.Length);
                arr = temp;
                self.StrongValue = temp;
            }
            arr[index] = right;
        }

        public void ArraySet(GmObject index1, GmObject index2, GmObject right)
            => ArraySet((int)index1.GetNum(), (int)index2.GetNum(), right);

        public void ArraySet(int index1, GmObject index2, GmObject right)
            => ArraySet(index1, (int)index2.GetNum(), right);

        public void ArraySet(GmObject index1, int index2, GmObject right)
            => ArraySet((int)index1.GetNum(), index2, right);

        public void ArraySet(int index1, int index2, GmObject right)
        {
            if (Type != VariableType.Array2)
                throw new InvalidInstanceException();
            if (index1 < 0 || index2 < 0)
                throw new IndexOutOfRangeException();
            var self = (GmValueArray<GmObject[][]>)Value;
            if(index1 >= self.StrongValue.Length)
            {
                var temp = new GmObject[index1 + 1][];
                Buffer.BlockCopy(self.StrongValue, 0, temp, 0, self.StrongValue.Length);
                self.StrongValue = temp;
            }
            if (self.StrongValue[index1] == null)
                self.StrongValue[index1] = new GmObject[index2 + 1];
            else if(index2 >= self.StrongValue[index1].Length)
            {
                var temp = new GmObject[index2 + 1];
                Buffer.BlockCopy(self.StrongValue[index1], 0, temp, 0, self.StrongValue[index1].Length);
                self.StrongValue[index1] = temp;
            }
            self.StrongValue[index1][index2] = right;
        }

        public GmObject ArrayGet(GmObject index)
            => ArrayGet((int)index.GetNum());

        public GmObject ArrayGet(int index)
        {
            var arr = GetArray1D();
            if (index < 0 || index >= arr.Length)
                throw new IndexOutOfRangeException();
            return arr[index];
        }

        public GmObject ArrayGet(GmObject index1, GmObject index2)
            => ArrayGet((int)index1, (int)index2);

        public GmObject ArrayGet(GmObject index1, int index2)
            => ArrayGet((int)index1, index2);

        public GmObject ArrayGet(int index1, GmObject index2)
            => ArrayGet(index1, (int)index2);

        public GmObject ArrayGet(int index1, int index2)
        {
            var arr = GetArray2D();
            if (index1 < 0 || index1 >= arr.Length || index2 < 0 || index2 >= arr[index1].Length)
                throw new IndexOutOfRangeException();
            return arr[index1][index2];
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

        #region Operators

        public static explicit operator float(GmObject right)
        {
            return right.GetNum();
        }

        public static explicit operator int(GmObject right)
        {
            return (int)right.GetNum();
        }

        public static explicit operator string(GmObject right)
        {
            return right.GetString();
        }

        public static explicit operator bool(GmObject right)
        {
            return right.GetBool();
        }

        public static explicit operator GmObject(float right)
        {
            return new GmObject(right);
        }

        public static explicit operator GmObject(int right)
        {
            return new GmObject(right);
        }

        public static explicit operator GmObject(string right)
        {
            return new GmObject(right);
        }

        public static explicit operator GmObject(bool right)
        {
            return new GmObject(right);
        }

        public static GmObject operator +(GmObject obj)
        {
            return new GmObject(+obj.GetNum());
        }

        public static GmObject operator -(GmObject obj)
        {
            return new GmObject(-obj.GetNum());
        }

        public static GmObject operator !(GmObject obj)
        {
            return new GmObject(!obj.GetBool());
        }

        public static GmObject operator ~(GmObject obj)
        {
            return new GmObject(~(int)obj.GetNum());
        }

        public static GmObject operator ++(GmObject obj)
        {
            return new GmObject(obj.GetNum() + 1f);
        }

        public static GmObject operator --(GmObject obj)
        {
            return new GmObject(obj.GetNum() - 1f);
        }

        public static bool operator true(GmObject obj) => obj.GetBool();

        public static bool operator false(GmObject obj) => !obj.GetBool();

        public static GmObject operator +(GmObject left, GmObject right)
        {
            if (left.Type == right.Type)
            {
                if (left.Type == VariableType.Real)
                    return new GmObject(left.GetNum() + right.GetNum());
                else if (left.Type == VariableType.String)
                    return new GmObject(left.GetString() + right.GetString());
            }
            throw new InvalidOperationException($"Cannot add {left.Type} and {right.Type} together.");
        }

        public static GmObject operator +(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot add {left.Type} and Real together.");
            return new GmObject(left.GetNum() + right);
        }

        public static GmObject operator +(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot add Real and {right.Type} together.");
            return new GmObject(left + right.GetNum());
        }

        public static GmObject operator +(GmObject left, string right)
        {
            if (left.Type != VariableType.String)
                throw new InvalidOperationException($"Cannot add {left.Type} and String together.");
            return new GmObject(left.GetString() + right);
        }

        public static GmObject operator +(string left, GmObject right)
        {
            if (right.Type != VariableType.String)
                throw new InvalidOperationException($"Cannot add String and {right.Type} together.");
            return new GmObject(left + right.GetString());
        }

        public static GmObject operator -(GmObject left, GmObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new GmObject(left.GetNum() - right.GetNum());
            throw new InvalidOperationException($"Cannot subtract {left.Type} and {right.Type}.");
        }

        public static GmObject operator -(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot subtract {left.Type} and Real together.");
            return new GmObject(left.GetNum() - right);
        }

        public static GmObject operator -(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot subtract Real and {right.Type} together.");
            return new GmObject(left - right.GetNum());
        }

        public static GmObject operator *(GmObject left, GmObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new GmObject(left.GetNum() * right.GetNum());
            throw new InvalidOperationException($"Cannot multiply {left.Type} and {right.Type}.");
        }

        public static GmObject operator *(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot multiply {left.Type} and Real together.");
            return new GmObject(left.GetNum() * right);
        }

        public static GmObject operator *(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot multiply Real and {right.Type} together.");
            return new GmObject(left * right.GetNum());
        }

        public static GmObject operator /(GmObject left, GmObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new GmObject(left.GetNum() / right.GetNum());
            throw new InvalidOperationException($"Cannot divide {left.Type} and {right.Type}.");
        }

        public static GmObject operator /(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot divide {left.Type} and Real together.");
            return new GmObject(left.GetNum() / right);
        }

        public static GmObject operator /(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot divide Real and {right.Type} together.");
            return new GmObject(left / right.GetNum());
        }

        public static GmObject operator %(GmObject left, GmObject right)
        {
            if (left.Type == VariableType.Real && right.Type == VariableType.Real)
                return new GmObject(left.GetNum() % right.GetNum());
            throw new InvalidOperationException($"Cannot modulo {left.Type} and {right.Type}.");
        }

        public static GmObject operator %(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot modulo {left.Type} and Real together.");
            return new GmObject(left.GetNum() % right);
        }

        public static GmObject operator %(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot modulo Real and {right.Type} together.");
            return new GmObject(left % right.GetNum());
        }

        public static GmObject operator &(GmObject left, GmObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and {right.Type}.");
            return new GmObject(Convert.ToInt32(left.GetNum()) & Convert.ToInt32(right.GetNum()));
        }

        public static GmObject operator &(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new GmObject(Convert.ToInt32(left.GetNum()) & Convert.ToInt32(right));
        }

        public static GmObject operator &(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & Real and {right.Type} together.");
            return new GmObject(Convert.ToInt32(left) & Convert.ToInt32(right.GetNum()));
        }

        public static GmObject operator |(GmObject left, GmObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | {left.Type} and {right.Type}.");
            return new GmObject(Convert.ToInt32(left.GetNum()) | Convert.ToInt32(right.GetNum()));
        }

        public static GmObject operator |(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | {left.Type} and Real together.");
            return new GmObject(Convert.ToInt32(left.GetNum()) | Convert.ToInt32(right));
        }

        public static GmObject operator |(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot | Real and {right.Type} together.");
            return new GmObject(Convert.ToInt32(left) | Convert.ToInt32(right.GetNum()));
        }

        public static GmObject operator ^(GmObject left, GmObject right)
        {
            if (left.Type != VariableType.Real || right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ {left.Type} and {right.Type}.");
            return new GmObject(Convert.ToInt32(left.GetNum()) ^ Convert.ToInt32(right.GetNum()));
        }

        public static GmObject operator ^(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ {left.Type} and Real together.");
            return new GmObject(Convert.ToInt32(left.GetNum()) ^ Convert.ToInt32(right));
        }

        public static GmObject operator ^(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot ^ Real and {right.Type} together.");
            return new GmObject(Convert.ToInt32(left) ^ Convert.ToInt32(right.GetNum()));
        }

        public static GmObject operator <<(GmObject left, int right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new GmObject(Convert.ToInt32(left.GetNum()) << right);
        }

        public static GmObject operator >>(GmObject left, int right)
        {
            if (left.Type != VariableType.Real)
                throw new InvalidOperationException($"Cannot & {left.Type} and Real together.");
            return new GmObject(Convert.ToInt32(left.GetNum()) >> right);
        }

        public static bool operator ==(GmObject left, GmObject right)
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

        public static bool operator ==(GmObject left, float right)
        {
            if (left.Type != VariableType.Real)
                return false;

            return left.GetNumUnchecked() == right;
        }

        public static bool operator ==(float left, GmObject right)
        {
            if (right.Type != VariableType.Real)
                return false;

            return right.GetNumUnchecked() == left;
        }

        public static bool operator ==(GmObject left, string right)
        {
            if (left.Type != VariableType.String)
                return false;

            return left.GetStringUnchecked() == right;
        }

        public static bool operator ==(string left, GmObject right)
        {
            if (right.Type != VariableType.String)
                return false;

            return right.GetStringUnchecked() == left;
        }

        public static bool operator !=(GmObject left, GmObject right)
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

        public static bool operator !=(GmObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return false;

            return left.GetNumUnchecked() != right;
        }

        public static bool operator !=(float left, GmObject right)
        {
            if (right.Type == VariableType.Real)
                return false;

            return right.GetNumUnchecked() != left;
        }

        public static bool operator !=(GmObject left, string right)
        {
            if (left.Type == VariableType.String)
                return false;

            return left.GetStringUnchecked() != right;
        }

        public static bool operator !=(string left, GmObject right)
        {
            if (right.Type == VariableType.String)
                return false;

            return right.GetStringUnchecked() != left;
        }

        public static bool operator <(GmObject left, GmObject right)
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

        public static bool operator <(GmObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() < right;
            return left.GetHashCode() < right;
        }

        public static bool operator <(float left, GmObject right)
        {
            if (right.Type == VariableType.Real)
                return left < right.GetNumUnchecked();
            return left < right.GetHashCode();
        }

        public static bool operator <(GmObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() < right.GetHashCode();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <(string left, GmObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() < right.GetNumUnchecked();
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator >(GmObject left, GmObject right)
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

        public static bool operator >(GmObject left, float right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() > right;
            return left.GetHashCode() > right;
        }

        public static bool operator >(float left, GmObject right)
        {
            if (right.Type == VariableType.Real)
                return left > right.GetNumUnchecked();
            return left > right.GetHashCode();
        }

        public static bool operator >(GmObject left, string right)
        {
            if (left.Type == VariableType.Real)
                return left.GetNumUnchecked() > right.GetHashCode();
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >(string left, GmObject right)
        {
            if (right.Type == VariableType.Real)
                return left.GetHashCode() > right.GetNumUnchecked();
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator <=(GmObject left, GmObject right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(GmObject left, float right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(float left, GmObject right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(GmObject left, string right)
        {
            return left == right || left < right;
        }

        public static bool operator <=(string left, GmObject right)
        {
            return left == right || left < right;
        }

        public static bool operator >=(GmObject left, GmObject right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(GmObject left, float right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(float left, GmObject right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(GmObject left, string right)
        {
            return left == right || left > right;
        }

        public static bool operator >=(string left, GmObject right)
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
            if (ReferenceEquals(obj, null))
                return 0;
            return obj.GetHashCode();
        }

#endif

        #region Base Class Library

        public static string ToString(GmObject obj)
        {
            return obj.ToString();
        }

        public static void ShowDebugMessage(GmObject obj)
        {
            Console.WriteLine(obj);
        }

        public static void ForTest()
        {
            for(var i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }
        }

        #endregion

    }
}
