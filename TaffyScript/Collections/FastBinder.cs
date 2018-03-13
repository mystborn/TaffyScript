using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Binds objects to an index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBinder<T>
    {
        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>Item</returns>
        T this[int index] { get; }

        /// <summary>
        /// Gets the number of bound items.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds an item to the binder, returning it's index.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item index.</returns>
        int Add(T item);

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        void Remove(int index);

        /// <summary>
        /// Attempts to get the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="value">The item, if found.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        bool TryGetValue(int index, out T value);


        /// <summary>
        /// Returns an enumerator used to enumerate through all of the bound items.
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> GetEnumerator();
    }

    /// <summary>
    /// Binds heap allocated objects to an index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassBinder<T> : IBinder<T> where T : class
    {
        private readonly List<T> _binder = new List<T>();
        private readonly Stack<int> _openSlots = new Stack<int>();

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>Item</returns>
        public T this[int index]
        {
            get => _binder[index];
        }

        /// <summary>
        /// Gets the number of bound items.
        /// </summary>
        public int Count => _binder.Count - _openSlots.Count;

        /// <summary>
        /// Adds an item to the binder, returning its index.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item index</returns>
        public int Add(T item)
        {
            int index;
            if (_openSlots.Count == 0)
            {
                index = _binder.Count;
                _binder.Add(item);
            }
            else
            {
                index = _openSlots.Pop();
                _binder[index] = item;
            }

            return index;
        }

        /// <summary>
        /// Returns an enumerator used to enumerate through all of the bound items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _binder)
                if (item != null)
                    yield return item;
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        public void Remove(int index)
        {
            // Originally did a bounds check, but currently
            // I've decided to let the List implementation control that.
            // if (index < 0 || index >= _binder.Count)
            //     throw new ArgumentOutOfRangeException(nameof(index));
            _binder[index] = null;

            //The application will throw before this operation 
            //so it's safe to push any index that's made it this far.
            _openSlots.Push(index);
        }

        /// <summary>
        /// Attempts to get the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="value">The item, if found.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        public bool TryGetValue(int index, out T value)
        {
            value = null;
            if (index < 0 || index >= _binder.Count)
                return false;
            value = _binder[index];
            if (value == null)
                return false;
            return true;
        }
    }

    /// <summary>
    /// Binds stack allocated objects to an index. Requires a default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructBinder<T> : IBinder<T> where T : struct
    {
        private readonly List<T> _binder = new List<T>();
        private readonly Stack<int> _openSlots = new Stack<int>();

        /// <summary>
        /// Creates a new StructBinder with the specified default value.
        /// </summary>
        /// <param name="defaultValue"></param>
        public StructBinder(T defaultValue)
        {
            Default = defaultValue;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>Item</returns>
        public T this[int index] => _binder[index];

        /// <summary>
        /// Gets the number of bound items.
        /// </summary>
        public int Count => _binder.Count - _openSlots.Count;

        /// <summary>
        /// Gets the default value of this binder.
        /// </summary>
        public T Default { get; }

        /// <summary>
        /// Adds an item to the binder, returning it's index.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item index.</returns>
        public int Add(T item)
        {
            int index;
            if (_openSlots.Count == 0)
            {
                index = _binder.Count;
                _binder.Add(item);
            }
            else
            {
                index = _openSlots.Pop();
                _binder[index] = item;
            }

            return index;
        }

        /// <summary>
        /// Returns an enumerator used to enumerate through all of the bound items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _binder)
                if (!item.Equals(Default))
                    yield return item;
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        public void Remove(int index)
        {
            _binder[index] = Default;
            _openSlots.Push(index);
        }

        /// <summary>
        /// Attempts to get the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="value">The item, if found.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        public bool TryGetValue(int index, out T value)
        {
            value = Default;
            if (index < 0 || index >= _binder.Count)
                return false;
            value = _binder[index];
            if (value.Equals(Default))
                return false;
            return true;
        }
    }
}
