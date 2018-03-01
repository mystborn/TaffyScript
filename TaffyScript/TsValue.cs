using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents a weakly typed TaffyScript value.
    /// </summary>
    public interface ITsValue
    {
        object WeakValue { get; }
    }

    /// <summary>
    /// Represents a stringly type TaffyScript value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITsValue<T> : ITsValue
    {
        T StrongValue { get; }
    }

    /// <summary>
    /// Represents a strongly typed TaffyScript value as a struct.
    /// <para>
    /// Used for reals and strings.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

    /// <summary>
    /// Represents a strongly typed TaffyScript value.
    /// <para>
    /// Used for arrays
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
