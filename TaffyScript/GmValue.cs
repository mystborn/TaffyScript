using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public interface IGmValue
    {
        object WeakValue { get; }
    }

    public interface IGmValue<T> : IGmValue
    {
        T StrongValue { get; }
    }

    public struct GmValue<T> : IGmValue
    {
        public T StrongValue { get; }
        public object WeakValue => StrongValue;

        public GmValue(T value)
        {
            StrongValue = value;
        }

        public override int GetHashCode()
        {
            return StrongValue.GetHashCode();
        }
    }

    public class GmValueArray<T> : IGmValue<T>
    {
        public T StrongValue { get; set; }
        public object WeakValue => StrongValue;

        public GmValueArray(T value)
        {
            StrongValue = value;
        }

        public override int GetHashCode()
        {
            return StrongValue.GetHashCode();
        }
    }
}
