using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Maps a collection of keys to values.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2</source>
    /// <property name="count" type="number" access="get">
    ///     <summary>Gets the number of items added to the map.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.count</source>
    /// </property>
    /// <property name="keys" type="[MapKeyCollection]({{site.baseurl}}/docs/TaffyScript/Collections/MapKeyCollection)" access="get">
    ///     <summary>Gets a collection of the keys added to the map.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.keys</source>
    /// </property>
    /// <property name="values" type="[MapValueCollection]({{site.baseurl}}/docs/TaffyScript/Collections/MapValueCollection)" access="get">
    ///     <summary>Gets a collection of the values added to the map.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.values</source>
    /// </property>
    [TaffyScriptObject("Map")]
    public class TsMap : TsEnumerable
    {
        public override string ObjectType => "Map";

        public Dictionary<TsObject, TsObject> Source { get; }

        public TsMap()
        {
            Source = new Dictionary<TsObject, TsObject>();
        }

        public TsMap(TsObject[] args)
        {
            Source = new Dictionary<TsObject, TsObject>();
        }

        public TsMap(IDictionary<TsObject, TsObject> source)
        {
            Source = new Dictionary<TsObject, TsObject>(source);
        }

        private TsMap(Dictionary<TsObject, TsObject> source)
        {
            Source = source;
        }

        public static TsMap Wrap(Dictionary<TsObject, TsObject> source)
        {
            return new TsMap(source);
        }

        public override IEnumerator<TsObject> GetEnumerator()
        {
            foreach (var kvp in Source)
                yield return new TsKeyValuePair(kvp);
        }

        public override TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "add":
                    if (Source.ContainsKey(args[0]))
                        return false;
                    Source.Add(args[0], args[1]);
                    return true;
                case "clear":
                    Source.Clear();
                    break;
                case "contains_key":
                    return Source.ContainsKey(args[0]);
                case "copy":
                    return new TsMap(Source);
                case "get":
                    if (Source.TryGetValue(args[0], out var result))
                        return result;
                    break;
                case "remove":
                    return Source.Remove(args[0]);
                case "set":
                    Source[args[0]] = args[1];
                    break;
                default:
                    return base.Call(scriptName, args);
            }
            return TsObject.Empty;
        }

        public override TsObject GetMember(string name)
        {
            switch(name)
            {
                case "count":
                    return Source.Count;
                case "keys":
                    return new MapKeyCollection(Source.Keys);
                case "values":
                    return new MapValueCollection(Source.Values);
                default:
                    return base.GetMember(name);
            }
        }

        public override bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "get":
                    del = new TsDelegate(get, "get");
                    return true;
                case "set":
                    del = new TsDelegate(set, "set");
                    return true;
                case "add":
                    del = new TsDelegate(add, "add");
                    return true;
                case "clear":
                    del = new TsDelegate(clear, "clear");
                    return true;
                case "remove":
                    del = new TsDelegate(remove, "remove");
                    return true;
                case "contains_key":
                    del = new TsDelegate(contains_key, "contains_key");
                    return true;
                default:
                    return base.TryGetDelegate(scriptName, out del);
            }
        }

        public static explicit operator TsMap(TsObject obj)
        {
            return (TsMap)obj.WeakValue;
        }

        public static implicit operator TsObject(TsMap map)
        {
            return new TsInstanceWrapper(map);
        }

#pragma warning disable IDE1006 // Naming Styles

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <arg name="key" type="object">The key to get the value of.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.item?view=netframework-4.7</source>
        /// <returns>object</returns>
        public TsObject get(params TsObject[] args)
        {
            return Source[args[0]];
        }

        /// <summary>
        /// Gets an Enumerator used to iterate over the items in the map.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.getenumerator?view=netframework-4.7</source>
        /// <returns>Enumerator</returns>
        public override TsObject get_enumerator(TsObject[] args)
        {
            return new WrappedEnumerator(GetEnumerator());
        }

        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <arg name="key" type="object">The key of the item to set.</arg>
        /// <arg name="value" type="object">The value of the item to set.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.item?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject set(params TsObject[] args)
        {
            Source[args[0]] = args[1];
            return TsObject.Empty;
        }

        /// <summary>
        /// Attempts to add an item to the map. Returns true if the item was added, false otherwise.
        /// </summary>
        /// <arg name="key" type="object">The key of the item to add.</arg>
        /// <arg name="value" type="object">The value of the item to add.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.add?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject add(params TsObject[] args)
        {
            if (Source.ContainsKey(args[0]))
                return false;

            Source.Add(args[0], args[1]);
            return true;
        }

        /// <summary>
        /// Clears all items from the map.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.clear?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject clear(params TsObject[] args)
        {
            Source.Clear();
            return TsObject.Empty;
        }

        /// <summary>
        /// Attempts to remove the item with the specified key.
        /// </summary>
        /// <arg name="key" type="object">The key of the item to remove.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.remove?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject remove(params TsObject[] args)
        {
            return Source.Remove(args[0]);
        }

        /// <summary>
        /// Determines if an item with the specified key is in the map.
        /// </summary>
        /// <arg name="key" type="object">The key to check.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.containskey?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject contains_key(params TsObject[] args)
        {
            return Source.ContainsKey(args[0]);
        }

        /// <summary>
        /// Creates a copy of the map.
        /// </summary>
        /// <returns>[Map]({{site.baseurl}}/docs/Map)</returns>
        public TsObject copy(params TsObject[] args)
        {
            return new TsMap((IDictionary<TsObject, TsObject>)Source);
        }
    }

    /// <summary>
    /// Represents the collection of keys in a Map.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.keycollection?view=netframework-4.7</source>
    /// <property name="count" type="number" access="get">
    ///     <summary>Gets the number of elements contained in this collection.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.keycollection.count?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public sealed class MapKeyCollection : TsEnumerable
    {
        public override string ObjectType => "TaffyScript.Collections.MapKeyCollection";

        public Dictionary<TsObject, TsObject>.KeyCollection Keys { get; }

        public MapKeyCollection(Dictionary<TsObject, TsObject>.KeyCollection keys)
        {
            Keys = keys;
        }

        public override IEnumerator<TsObject> GetEnumerator()
        {
            return Keys.GetEnumerator();
        }

        public override TsObject GetMember(string name)
        {
            switch(name)
            {
                case "count":
                    return Keys.Count;
                default:
                    return base.GetMember(name);
            }
        }

        /// <summary>
        /// Gets an Enumerator used to iterate over the items in the key collection.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.keycollection.getenumerator?view=netframework-4.7</source>
        /// <returns>Enumerator</returns>
        public override TsObject get_enumerator(TsObject[] args)
        {
            return TsEnumerator.Wrap(Keys.GetEnumerator());
        }
    }

    /// <summary>
    /// Represents the collection of values in a Map.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.valuecollection?view=netframework-4.7</source>
    /// <property name="count" type="number" access="get">
    ///     <summary>Gets the number of elements contained in this collection.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.valuecollection.count?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class MapValueCollection : TsEnumerable
    {
        public override string ObjectType => "TaffyScript.Collections.MapValueCollection";

        public Dictionary<TsObject, TsObject>.ValueCollection Values { get; }

        public MapValueCollection(Dictionary<TsObject, TsObject>.ValueCollection values)
        {
            Values = values;
        }

        public override IEnumerator<TsObject> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public override TsObject GetMember(string name)
        {
            switch (name)
            {
                case "count":
                    return Values.Count;
                default:
                    return base.GetMember(name);
            }
        }

        /// <summary>
        /// Gets an Enumerator used to iterate over the items in the key collection.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.valuecollection.getenumerator?view=netframework-4.7</source>
        /// <returns>Enumerator</returns>
        public override TsObject get_enumerator(TsObject[] args)
        {
            return TsEnumerator.Wrap(Values.GetEnumerator());
        }
    }
}
