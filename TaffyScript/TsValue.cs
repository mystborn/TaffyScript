using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public interface ITsValue
    {
        object WeakValue { get; }
    }

    public interface ITsValue<T> : ITsValue
    {
        T StrongValue { get; }
    }

    public struct TsValue<T> : ITsValue
    {
        public T StrongValue { get; }
        public object WeakValue => StrongValue;

        public TsValue(T value)
        {
            StrongValue = value;
        }

        public override int GetHashCode()
        {
            return StrongValue.GetHashCode();
        }
    }

    public class TsValueArray<T> : ITsValue<T>
    {
        public T StrongValue { get; set; }
        public object WeakValue => StrongValue;

        public TsValueArray(T value)
        {
            StrongValue = value;
        }

        public override int GetHashCode()
        {
            return StrongValue.GetHashCode();
        }
    }
}
