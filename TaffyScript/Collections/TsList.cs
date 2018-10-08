using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Represents a list of objects that can be access by index.
    /// </summary>
    /// <property name="count" type="number" access="get">
    ///     <summary>Gets the number of items in the list.</summary>
    /// </property>
    public class TsList : TsEnumerable
    {
        private List<TsObject> _source;

        public override string ObjectType => "List";
        public List<TsObject> Source => _source;

        public TsList()
        {
            _source = new List<TsObject>();
        }

        /// <summary>
        /// Creates a new list composed of the arguments.
        /// </summary>
        /// <arg name="[..args]" type="objects">The objects to initialize the list with.</arg>
        public TsList(TsObject[] args)
        {
            if (args is null)
                _source = new List<TsObject>();
            else
                _source = new List<TsObject>(args);
        }

        public TsList(IEnumerable<TsObject> source)
        {
            _source = new List<TsObject>(source);
        }

        private TsList(List<TsObject> source)
        {
            _source = source;
        }

        public static TsList Wrap(List<TsObject> source)
        {
            return new TsList(source);
        }

        public override IEnumerator<TsObject> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        public override TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "add":
                    _source.AddRange(args);
                    break;
                case "clear":
                    _source.Clear();
                    break;
                case "copy":
                    return new TsList(_source);
                case "get":
                    return _source[(int)args[0]];
                case "insert":
                    _source.Insert((int)args[0], args[1]);
                    break;
                case "index_of":
                    return _source.IndexOf(args[0]);
                case "remove":
                    _source.RemoveAt((int)args[0]);
                    break;
                case "set":
                    var index = (int)args[0];
                    while (_source.Count <= index)
                        _source.Add(TsObject.Empty);
                    _source[index] = args[1];
                    break;
                case "shuffle":
                    _source.Shuffle();
                    break;
                case "sort":
                    return sort(args);
                default:
                    throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
            }
            return TsObject.Empty;
        }

        public override TsObject GetMember(string name)
        {
            switch(name)
            {
                case "count":
                    return _source.Count;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;

                    throw new MemberAccessException($"Couldn't find member with the name {name}");
            }
        }

        public override bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName) {
                case "add":
                    del = new TsDelegate(add, "add");
                    return true;
                case "clear":
                    del = new TsDelegate(clear, "clear");
                    return true;
                case "copy":
                    del = new TsDelegate(copy, "copy");
                    return true;
                case "get":
                    del = new TsDelegate(get, "get");
                    return true;
                case "insert":
                    del = new TsDelegate(insert, "insert");
                    return true;
                case "index_of":
                    del = new TsDelegate(index_of, "index_of");
                    return true;
                case "remove":
                    del = new TsDelegate(remove, "remove");
                    return true;
                case "set":
                    del = new TsDelegate(set, "set");
                    return true;
                case "shuffle":
                    del = new TsDelegate(shuffle, "shuffle");
                    return true;
                case "sort":
                    del = new TsDelegate(sort, "sort");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public static explicit operator TsList(TsObject obj)
        {
            return (TsList)obj.WeakValue;
        }

        public static implicit operator TsObject(TsList list)
        {
            return new TsInstanceWrapper(list);
        }

#pragma warning disable IDE1006 // Naming Styles

        /// <summary>
        /// Adds the arguments to the list.
        /// </summary>
        /// <arg name="..elements" type="objects">The elements to add to the list.</arg>
        /// <returns>null</returns>
        public TsObject add(TsObject[] args)
        {
            _source.AddRange(args);
            return TsObject.Empty;
        }

        /// <summary>
        /// Removes all elements from this list.
        /// </summary>
        /// <returns>null</returns>
        public TsObject clear(TsObject[] args)
        {
            _source.Clear();
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a copy of the list.
        /// </summary>
        /// <returns>[List]({{site.baseurl}}/docs/List)</returns>
        public TsObject copy(TsObject[] args)
        {
            return new TsList(_source);
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <arg name="index" type="number">The index of the element to get.</arg>
        /// <returns>object</returns>
        public TsObject get(TsObject[] args)
        {
            return _source[(int)args[0]];
        }

        /// <summary>
        /// Gets an enumerator used to iterate over the elements in the list.
        /// </summary>
        /// <returns>[TsEnumerator]({{site.baseurl}}/docs/TaffyScript/Collections/TsEnumerator)</returns>
        public override TsObject get_enumerator(TsObject[] args)
        {
            return new TsEnumerator(_source.GetEnumerator());
        }

        /// <summary>
        /// Inserts a value into the list at the specified index.
        /// </summary>
        /// <arg name="index" type="number">The index to insert the value.</arg>
        /// <arg name="item" type="object">The value to insert.</arg>
        /// <returns>null</returns>
        public TsObject insert(TsObject[] args)
        {
            _source.Insert((int)args[0], args[1]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Finds the index of the first occurrence of the value in the list. Returns -1 if the value isn't found.
        /// </summary>
        /// <arg name="item" type="object">The item to search for.</arg>
        /// <returns></returns>
        public TsObject index_of(TsObject[] args)
        {
            return _source.IndexOf(args[0]);
        }

        /// <summary>
        /// Removes the value at the specified index within the list.
        /// </summary>
        /// <arg name="index" type="number">The index of the value to remove.</arg>
        /// <returns>null</returns>
        public TsObject remove(TsObject[] args)
        {
            _source.RemoveAt((int)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the value at the specified index within the list. If `index` is greater than the size of the list, null elements will be added until the index can be set.
        /// </summary>
        /// <arg name="index" type="number">The index of the item to set.</arg>
        /// <arg name="value" type="object">The value to set the index.</arg>
        /// <returns>null</returns>
        public TsObject set(TsObject[] args)
        {
            var index = (int)args[0];
            while (_source.Count <= index)
                _source.Add(TsObject.Empty);
            _source[index] = args[1];
            return TsObject.Empty;
        }

        /// <summary>
        /// Shiffles the values in the list.
        /// </summary>
        /// <returns>null</returns>
        public TsObject shuffle(TsObject[] args)
        {
            _source.Shuffle();
            return TsObject.Empty;
        }

        /// <summary>
        /// Sorts the elements in the list.
        /// </summary>
        /// <arg name="[comparer]" type="script">A script used to compare elements against each other.</arg>
        /// <returns>null</returns>
        public TsObject sort(TsObject[] args)
        {
            if(args is null)
                _source.Sort();
            switch(args.Length)
            {
                case 0:
                    _source.Sort();
                    break;
                case 1:
                    var script = args[0].GetDelegate();
                    _source.Sort((x, y) => (int)script.Invoke(x, y));
                    break;
            }
            return TsObject.Empty;
        }
    }
}
