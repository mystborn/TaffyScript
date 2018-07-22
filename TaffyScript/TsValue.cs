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
    /// Wraps an immutable TaffyScript value.
    /// <para>
    /// I.E. numbers, strings, and delegates.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TsImmutableValue<T> : ITsValue
    {
        public T StrongValue { get; }
        public object WeakValue => StrongValue;

        public TsImmutableValue(T value)
        {
            StrongValue = value;
        }

        public override int GetHashCode()
        {
            return StrongValue.GetHashCode();
        }
    }

    /// <summary>
    /// Wraps a mutable TaffyScript value.
    /// <para>
    /// I.E. arrays.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TsMutableValue<T> : ITsValue<T>
    {
        public T StrongValue { get; set; }
        public object WeakValue => StrongValue;

        public TsMutableValue(T value)
        {
            StrongValue = value;
        }

        public override int GetHashCode()
        {
            return StrongValue.GetHashCode();
        }
    }
}
