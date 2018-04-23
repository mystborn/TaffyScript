//Inspired by the FastList class found in Nez.
//Source: https://github.com/prime31/Nez/blob/master/Nez.Portable/Utils/Collections/FastList.cs
//License: https://github.com/prime31/Nez/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// An unordered List with O(1) removal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastList<T> : IEnumerable<T>
    {
        //Use fields for performance in case property calls can't be jitted away.

        /// <summary>
        /// The backing array. Use to access elements.
        /// </summary>
        public T[] Buffer;

        /// <summary>
        /// The number of elements in the list.
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// Creates a new list with the default initial capacity.
        /// </summary>
        public FastList() : this(5) { }

        /// <summary>
        /// Creates a new list with the specified initial capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public FastList(int capacity)
        {
            Buffer = new T[capacity];
        }

        /// <summary>
        /// Creates a list from a collection of items.
        /// </summary>
        /// <param name="items"></param>
        public FastList(IEnumerable<T> items)
        {
            Buffer = items.ToArray();
            Count = Buffer.Length;
        }

        /// <summary>
        /// Clears all items from the list.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Buffer, 0, Count);
            Count = 0;
        }

        /// <summary>
        /// Resets the list size to 0, without removing any items.
        /// </summary>
        public void Reset()
        {
            Count = 0;
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (Count == Buffer.Length)
                Array.Resize(ref Buffer, Count << 1);
            Buffer[Count++] = item;
        }

        /// <summary>
        /// Removes an item from the list. O(n) time.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            var comp = EqualityComparer<T>.Default;
            for (var i = 0; i < Count; ++i)
            {
                if (comp.Equals(Buffer[i], item))
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes the specified index from the list. O(1) time.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            Buffer[index] = Buffer[Count - 1];
            Buffer[--Count] = default(T);
        }

        /// <summary>
        /// Determines if the list contains the specified item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            var comp = EqualityComparer<T>.Default;
            for (var i = 0; i < Count; --i)
            {
                if (comp.Equals(Buffer[i], item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets an enumerator used to iterate through the list.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return Buffer[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
