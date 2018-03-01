using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Implements a generic Row-Column lookup table.
    /// </summary>
    /// <remarks>
    /// This is just a helper class used to make working with nested dictionaries less tedius.
    /// </remarks>
    /// <typeparam name="TRow"></typeparam>
    /// <typeparam name="TCol"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LookupTable<TRow, TCol, TValue> : IEnumerable<TableItem<TRow, TCol, TValue>>
    {
        private int _count = 0;
        private int _columns = 0;
        private Dictionary<TRow, Dictionary<TCol, TValue>> _source;
        private IEqualityComparer<TCol> _colComparer;

        public IEqualityComparer<TRow> RowComparer => _source.Comparer;
        public IEqualityComparer<TCol> ColumnComparer => _colComparer;

        public int Count => _count;

        /// <summary>
        /// Gets or sets the value at the associated row and column.
        /// </summary>
        /// <param name="row">The row of the value to get or set.</param>
        /// <param name="col">The column of the value to get or set.</param>
        public TValue this[TRow row, TCol col]
        {
            get
            {
                if(TryGetInternalSource(row, out var inner))
                {
                    if (inner.TryGetValue(col, out var value))
                        return value;
                    else
                        throw new ArgumentException($"No value at index: {row}, {col}", "col");
                }
                else
                    throw new ArgumentException($"No row with the key: {row}", "row");
            }
            set
            {
                var inner = GetInternalSource(row);
                var count = inner.Count;
                inner[col] = value;
                if (inner.Count > count)
                    ++_count;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="LookupTable{TRow, TCol, TValue}"/> with default values.
        /// </summary>
        public LookupTable() : this(EqualityComparer<TRow>.Default, EqualityComparer<TCol>.Default) { }

        /// <summary>
        /// Constructs a new <see cref="LookupTable{TRow, TCol, TValue}"/> with the values from the given <paramref name="source"/> inserted.
        /// </summary>
        /// <param name="source">A collection of values to be inserted into the table.</param>
        public LookupTable(ICollection<TableItem<TRow, TCol, TValue>> source) : this(source, EqualityComparer<TRow>.Default, EqualityComparer<TCol>.Default) { }

        /// <summary>
        /// Initializes a new <see cref="LookupTable{TRow, TCol, TValue}"/> with the values from the given <paramref name="source"/> inserted
        /// and using the specified <see cref="IEqualityComparer{T}"/>'s for the rows and columns.
        /// </summary>
        /// <param name="source">A collection of values to be inserted into the table.</param>
        /// <param name="rowComparer">The <see cref="IEqualityComparer{T}"/> used when comparing row values.</param>
        /// <param name="columnComparer">The <see cref="IEqualityComparer{T}"/> used when comparing column values.</param>
        public LookupTable(ICollection<TableItem<TRow, TCol, TValue>> source, IEqualityComparer<TRow> rowComparer, IEqualityComparer<TCol> columnComparer)
        {
            _source = new Dictionary<TRow, Dictionary<TCol, TValue>>(rowComparer);
            _colComparer = columnComparer;
            foreach(var item in source)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="LookupTable{TRow, TCol, TValue}"/> using the specified <see cref="IEqualityComparer{T}"/>'s for the rows and columns.
        /// </summary>
        /// <param name="rowComparer">The <see cref="IEqualityComparer{T}"/> used when comparing row values.</param>
        /// <param name="columnComparer">The <see cref="IEqualityComparer{T}"/> used when comparing column values.</param>
        public LookupTable(IEqualityComparer<TRow> rowComparer, IEqualityComparer<TCol> columnComparer) : this(0, 0, rowComparer, columnComparer) { }

        /// <summary>
        /// Initializes a new <see cref="LookupTable{TRow, TCol, TValue}"/> with a default capacity for the rows and columns.
        /// </summary>
        /// <param name="defaultRows">The default capacity for the rows.</param>
        /// <param name="defaultColumns">The default capacity for the columns.</param>
        public LookupTable(int defaultRows, int defaultColumns) : this(defaultRows, defaultColumns, EqualityComparer<TRow>.Default, EqualityComparer<TCol>.Default) { }

        /// <summary>
        /// Initializes a new <see cref="LookupTable{TRow, TCol, TValue}"/> with a default capacity for the rows and columns 
        /// and using the specified <see cref="IEqualityComparer{T}"/>'s for the rows and columns.
        /// </summary>
        /// <param name="defaultRows">The default capacity for the rows.</param>
        /// <param name="defaultColumns">The default capacity for the columns.</param>
        /// <param name="rowComparer">The <see cref="IEqualityComparer{T}"/> used when comparing row values.</param>
        /// <param name="columnComparer">The <see cref="IEqualityComparer{T}"/> used when comparing column values.</param>
        public LookupTable(int defaultRows, int defaultColumns, IEqualityComparer<TRow> rowComparer, IEqualityComparer<TCol> columnComparer)
        {
            _colComparer = columnComparer;
            _columns = defaultColumns;
            _source = new Dictionary<TRow, Dictionary<TCol, TValue>>(defaultRows, rowComparer);
        }

        /// <summary>
        /// Adds a value at the specified row and column.
        /// </summary>
        /// <param name="row">The row to add the value.</param>
        /// <param name="col">The column to add the value.</param>
        /// <param name="value">The value to add.</param>
        public void Add(TRow row, TCol col, TValue value)
        {
            var dict = GetInternalSource(row);
            dict.Add(col, value);
            _count++;
        }

        public void Add(TableItem<TRow, TCol, TValue> item)
        {
            Add(item.Row, item.Col, item.Value);
        }

        /// <summary>
        /// Removes all values from the <see cref="LookupTable{TRow, TCol, TValue}"/>.
        /// </summary>
        public void Clear()
        {
            _source.Clear();
            _count = 0;
        }

        /// <summary>
        /// Determines if any item has been inserted at the specified row and column.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns></returns>
        public bool ContainsIndex(TRow row, TCol col)
        {
            if(TryGetInternalSource(row, out var inner))
                return inner.ContainsKey(col);

            return false;
        }

        /// <summary>
        /// Determines if the specified value has been inserted.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        public bool ContainsValue(TValue value)
        {
            foreach(var inner in _source.Values)
            {
                foreach(var element in inner.Values)
                {
                    if (element.Equals(value))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the specified value exists using the <see cref="IEqualityComparer{T}"/> to determine equality.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> used to determine the equality of the values.</param>
        public bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer)
        {
            foreach (var inner in _source.Values)
            {
                foreach (var element in inner.Values)
                {
                    if (comparer.Equals(value, element))
                        return true;
                }
            }
            return false;
        }
        
        public IEnumerator<TableItem<TRow, TCol, TValue>> GetEnumerator()
        {
            foreach(var row in _source)
            {
                foreach(var col in row.Value)
                {
                    yield return new TableItem<TRow, TCol, TValue>(row.Key, col.Key, col.Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes a value at the given row and column.
        /// </summary>
        /// <param name="row">The row of the value.</param>
        /// <param name="col">The column of the value.</param>
        public bool Remove(TRow row, TCol col)
        {
            if (TryGetInternalSource(row, out var inner))
            {
                _count--;
                return inner.Remove(col);
            }
            return false;
        }

        /// <summary>
        /// Attempts to find the value at the given row and column. Returns true if the value was found, false otherwise.
        /// </summary>
        /// <param name="row">The row of the value.</param>
        /// <param name="col">The column of the value.</param>
        /// <param name="value">When the method returns, this contains the value of the index if successful, otherwise it contains a default value.</param>
        /// <returns></returns>
        public bool TryGetValue(TRow row, TCol col, out TValue value)
        {
            if (TryGetInternalSource(row, out var inner))
                return inner.TryGetValue(col, out value);

            value = default(TValue);
            return false;
        }

        private Dictionary<TCol, TValue> GetInternalSource(TRow key)
        {
            if(!_source.TryGetValue(key, out var inner))
            {
                inner = new Dictionary<TCol, TValue>(_columns, _colComparer);
                _source.Add(key, inner);
            }

            return inner;
        }

        private bool TryGetInternalSource(TRow key, out Dictionary<TCol, TValue> inner)
        {
            return _source.TryGetValue(key, out inner);
        }
    }

    public struct TableItem<TRow, TCol, TValue> : IEquatable<TableItem<TRow, TCol, TValue>>
    {
        public TRow Row { get; }
        public TCol Col { get; }
        public TValue Value { get; }

        public TableItem(TRow row, TCol col, TValue value)
        {
            Row = row;
            Col = col;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is TableItem<TRow, TCol, TValue> item)
                return Equals(item);

            return false;
        }

        public bool Equals(TableItem<TRow, TCol, TValue> other)
        {
            return Row.Equals(other.Row) &&
                Col.Equals(other.Col) &&
                Value.Equals(other.Value);
        }

        public static bool operator ==(TableItem<TRow, TCol, TValue> left, TableItem<TRow, TCol, TValue> right)
        {
            //Don't check for nullity, this is a struct.
            return left.Equals(right);
        }

        public static bool operator !=(TableItem<TRow, TCol, TValue> left, TableItem<TRow, TCol, TValue> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Row.GetHashCode();
                hash = hash * 23 + Col.GetHashCode();
                hash = hash * 23 + Value.GetHashCode();
                return hash;
            }
        }
    }
}
