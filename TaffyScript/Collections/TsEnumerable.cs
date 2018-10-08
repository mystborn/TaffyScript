﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Exposes an enumerator, which supports iteration over a collection.
    /// </summary>
    public abstract class TsEnumerable : IEnumerable<TsObject>, ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public abstract string ObjectType { get; }

        public virtual IEnumerator<TsObject> GetEnumerator()
        {
            return (IEnumerator<TsObject>)get_enumerator(null);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Enumerator</returns>
        public abstract TsObject get_enumerator(TsObject[] args);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "get_enumerator":
                    return new TsEnumerator(GetEnumerator());
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public virtual TsObject GetMember(string name)
        {
            if (TryGetDelegate(name, out var del))
                return del;
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "get_enumerator":
                    del = new TsDelegate(get_enumerator, scriptName);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        /// <summary>
        /// Determines if there are any elements in the sequence. If a script is provided, determines if any element satisfies a condition.
        /// </summary>
        /// <arg name="[condition]" type="script">Tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any</source>
        /// <returns>bool</returns>
        public TsObject any(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return this.Any();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Any(o => (bool)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(any)}");
            }
        }

        /// <summary>
        /// Computes the average of the elements in the sequence.
        /// </summary>
        /// <arg name="selector" type="script">Gets a numeric value from each element.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.average</source>
        /// <returns>number</returns>
        public TsObject average(TsObject[] args)
        {
            var script = args[0].GetDelegate();
            return this.Average((o) => (double)script.Invoke(o));
        }

        /// <summary>
        /// Combines this sequence with another.
        /// </summary>
        /// <arg name="other" type="Enumerable">The other sequence to combine with.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.concat</source>
        /// <returns>Enumerable</returns>
        public TsObject concat(TsObject[] args)
        {
            return new WrappedEnumerable(this.Concat((IEnumerable<TsObject>)args[0].WeakValue));
        }

        /// <summary>
        /// Determines if the collection contains a value, optionally using an EqualityComparer.
        /// </summary>
        /// <arg name="value" type="object">The object to search for.</arg>
        /// <arg name="[comparer]" type="EqualityComparer">A comparer used to determine if an item in the sequence equals the value.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.contains</source>
        /// <returns>bool</returns>
        public TsObject contains(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return this.Contains(args[0]);
                case 2:
                    return this.Contains(args[0], (IEqualityComparer<TsObject>)args[1].WeakValue);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(contains)}");
            }
        }

        /// <summary>
        /// Returns the distinct elements from the sequence, optionally using an EqualityComparer.
        /// </summary>
        /// <arg name="[comparer]" type="EqualityComparer">A comparer used to check if an item has been added to the set.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.distinct</source>
        /// <returns>Enumerable</returns>
        public TsObject distinct(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return new WrappedEnumerable(this.Distinct());
                case 1:
                    return new WrappedEnumerable(this.Distinct((IEqualityComparer<TsObject>)args[0].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(distinct)}");
            }
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <arg name="index" type="number">The index of the element to get.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.elementat</source>
        /// <returns>object</returns>
        public TsObject element_at(TsObject[] args)
        {
            return this.ElementAt((int)args[0]);
        }

        /// <summary>
        /// Gets the element at the specified index, or null if index is out of range.
        /// </summary>
        /// <arg name="index" type="number">The index of the element to get.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.elementat</source>
        /// <returns>object</returns>
        public TsObject element_at_or_default(TsObject[] args)
        {
            return this.ElementAtOrDefault((int)args[0]) ?? TsObject.Empty;
        }

        /// <summary>
        /// Gets the set difference between this and another sequence.
        /// </summary>
        /// <arg name="other" type="Enumerable">The other sequence used to get the difference.</arg>
        /// <arg name="[comparer]" type="EqualityComparer">A comparer used to compare the elements in the sequences.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.except</source>
        /// <returns>Enumerable</returns>
        public TsObject except(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(this.Except((IEnumerable<TsObject>)args[0].WeakValue));
                case 2:
                    return new WrappedEnumerable(this.Except((IEnumerable<TsObject>)args[0].WeakValue, (IEqualityComparer<TsObject>)args[1].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(except)}");
            }
        }

        /// <summary>
        /// Gets the first element in the sequence. If a script is provided, gets the first element in the sequence that satisfies the condition.
        /// </summary>
        /// <arg name="[condition]" type="script">A script used to test each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.firstordefault</source>
        /// <returns>object</returns>
        public TsObject first(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return this.First();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.First(o => (bool)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(first)}");
            }
        }

        /// <summary>
        /// Gets the first element in the sequence. If a script is provided, gets the first element in the sequence that satisfies the condition. Returns null if no element is found.
        /// </summary>
        /// <arg name="[condition]" type="script">A script used to test each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.firstordefault</source>
        /// <returns>object</returns>
        public TsObject first_or_default(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return this.FirstOrDefault() ?? TsObject.Empty;
                case 1:
                    var script = args[0].GetDelegate();
                    return this.FirstOrDefault(o => (bool)script.Invoke(o)) ?? TsObject.Empty;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(first_or_default)}");
            }
        }

        /// <summary>
        /// Gets the set intersection between this and another sequence.
        /// </summary>
        /// <arg name="other" type="Enumerable">The other sequence used to get the intersection.</arg>
        /// <arg name="[comparer]" type="EqualityComparer">A comparer used to compare the values in the two sequences.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.intersect</source>
        /// <returns>Enumerable</returns>
        public TsObject intersect(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(this.Intersect((IEnumerable<TsObject>)args[0]));
                case 2:
                    return new WrappedEnumerable(this.Intersect((IEnumerable<TsObject>)args[0], (IEqualityComparer<TsObject>)args[1].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(intersect)}");
            }
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys.
        /// </summary>
        /// <arg name="other" type="IEnumerable">The other sequence to join with.</arg>
        /// <arg name="outer_key_selector" type="script">A script to extract the join key from this sequence.</arg>
        /// <arg name="inner_key_selector" type="script">A script to extract the join key from the other sequence.</arg>
        /// <arg name="result_selector" type="script">A script to create a result from two matching elements.</arg>
        /// <arg name="[comparer]" type="EqualityComparer">A comparer used to hash and compare keys.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.join</source>
        /// <returns>Enumerable</returns>
        public TsObject join(TsObject[] args)
        {
            var other = (IEnumerable<TsObject>)args[0];
            var del1 = args[1].GetDelegate();
            Func<TsObject, TsObject> outerKeySelector = o => del1.Invoke(o);
            var del2 = args[2].GetDelegate();
            Func<TsObject, TsObject> innerKeySelector = o => del2.Invoke(o);
            var del3 = args[3].GetDelegate();
            Func<TsObject, TsObject, TsObject> resultSelector = (o1, o2) => del3.Invoke(o1, o2);

            switch(args.Length)
            {
                case 4:
                    return new WrappedEnumerable(this.Join(other, outerKeySelector, innerKeySelector, resultSelector));
                case 5:
                    return new WrappedEnumerable(this.Join(other, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TsObject>)args[4].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(join)}");
            }
        }

        /// <summary>
        /// Gets the last element in the sequence. If a script is provided, gets the last element in the sequence that satisfies the condition.
        /// </summary>
        /// <arg name="[condition]" type="script">A script used to test each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.last</source>
        /// <returns>object</returns>
        public TsObject last(TsObject[] args)
        {
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.Last();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Last(o => (bool)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(last)}");
            }
        }

        /// <summary>
        /// Gets the last element in the sequence. If a script is provided, gets the last element in the sequence that satisfies the condition. Returns null if no element is found.
        /// </summary>
        /// <arg name="[condition]" type="script">A script used to test each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.lastordefault</source>
        /// <returns>object</returns>
        public TsObject last_or_default(TsObject[] args)
        {
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.LastOrDefault() ?? TsObject.Empty;
                case 1:
                    var script = args[0].GetDelegate();
                    return this.LastOrDefault(o => (bool)script.Invoke(o)) ?? TsObject.Empty;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(last_or_default)}");
            }
        }

        /// <summary>
        /// Gets the maximum value in the sequence, optionally using a script to get a numeric value from each element.
        /// </summary>
        /// <arg name="[transform]" type="script">Used to get a numeric value from each element in the sequence.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.max</source>
        /// <returns>object</returns>
        public TsObject max(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return this.Max();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Max(o => (double)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(max)}");
            }
        }

        /// <summary>
        /// Gets the minimum value in the sequence, optionally using a script to get a numeric value from each element.
        /// </summary>
        /// <arg name="[transform]" type="script">Used to get a numeric value from each element in the sequence.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.min</source>
        /// <returns>object</returns>
        public TsObject min(TsObject[] args)
        {
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.Min();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Min(o => (double)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(min)}");
            }
        }

        /// <summary>
        /// Sorts the elements in the sequence based on the key returned by a script, optionally using an IComparer to determine order.
        /// </summary>
        /// <arg name="key_selector" type="script">The script used to extract a key from each element in the sequence.</arg>
        /// <arg name="[comparer]" type="IComparer">The comparer used to compare keys.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderby</source>
        /// <returns>Enumerable</returns>
        public TsObject order_by(TsObject[] args)
        {
            var script = args[0].GetDelegate();
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(this.OrderBy(o => script.Invoke(o)));
                case 2:
                    return new WrappedEnumerable(this.OrderBy(o => script.Invoke(o), (IComparer<TsObject>)args[1].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(order_by)}");
            }
        }

        /// <summary>
        /// Inverts the order of elements in the sequence.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.reverse</source>
        /// <returns>Enumerable</returns>
        public TsObject reverse(TsObject[] args)
        {
            return new WrappedEnumerable(this.Reverse());
        }

        /// <summary>
        /// Projects each element in the sequence into a new form.
        /// </summary>
        /// <arg name="selector" type="script">A script that transforms each element in the sequence to a new form.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select</source>
        /// <returns>Enumerable</returns>
        public TsObject select(TsObject[] args)
        {
            var script = args[0].GetDelegate();
            return new WrappedEnumerable(this.Select(o => script.Invoke(o)));
        }

        /// <summary>
        /// Projects each element in the sequence to an Enumerable and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <arg name="collection_selector" type="script">A script that transforms each element in the sequence into an Enumerable.</arg>
        /// <arg name="[result_selector]" type="script">A script that transforms each element of the intermediate sequences.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany</source>
        /// <returns>Enumerable</returns>
        public TsObject select_many(TsObject[] args)
        {
            var script = args[0].GetDelegate();
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(this.SelectMany(o => (IEnumerable<TsObject>)script.Invoke(o).WeakValue));
                case 2:
                    //Todo: Make sure TsEnumerable works as intended.
                    var resultSelector = (TsDelegate)args[1];
                    return new WrappedEnumerable(this.SelectMany(o => (IEnumerable<TsObject>)script.Invoke(o).WeakValue, (o, c) => resultSelector.Invoke(o, c)));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(select_many)}");
            }
        }

        /// <summary>
        /// Determines if this sequence is equal to another.
        /// </summary>
        /// <arg name="other" type="Enumerable">The sequence to compare to.</arg>
        /// <arg name="[comparer]" type="EqualityComparer">The comparer used to compare elements.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sequenceequal</source>
        /// <returns>bool</returns>
        public TsObject sequence_equal(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return this.SequenceEqual((IEnumerable<TsObject>)args[0].WeakValue);
                case 2:
                    return this.SequenceEqual((IEnumerable<TsObject>)args[0].WeakValue, (IEqualityComparer<TsObject>)args[1].WeakValue);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(sequence_equal)}");
            }
        }

        /// <summary>
        /// Returns the only element in the sequence or throws an exception if there is not exactly one element. If a script is given, returns the only element that satisfies the condition.
        /// </summary>
        /// <arg name="[condition]" type="script">A script that tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.single</source>
        /// <returns>object</returns>
        public TsObject single(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return this.Single();
                case 1:
                    var script = (TsDelegate)args[0];
                    return this.Single(o => (bool)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(single)}");
            }
        }

        /// <summary>
        /// Returns the only element in the sequence or null if there are nor elements. Throws an exception if there is more than one element. If a script is given, returns the only element that satisfies the condition.
        /// </summary>
        /// <arg name="[condition]" type="script">A script that tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.singleordefault</source>
        /// <returns>object</returns>
        public TsObject single_or_default(TsObject[] args)
        {
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.SingleOrDefault() ?? TsObject.Empty;
                case 1:
                    var script = (TsDelegate)args[0];
                    return this.SingleOrDefault(o => (bool)script.Invoke(o)) ?? TsObject.Empty;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(single)}");
            }
        }

        /// <summary>
        /// Bypasses the specified number of elements and returns the rest.
        /// </summary>
        /// <arg name="count" type="number">The number of elements to skip.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skip</source>
        /// <returns>Enumerable</returns>
        public TsObject skip(TsObject[] args)
        {
            return new WrappedEnumerable(this.Skip((int)args[0]));
        }

        /// <summary>
        /// Bypasses elements while the specified condition is true and returns the rest.
        /// </summary>
        /// <arg name="condition" type="script">A script that tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skipwhile</source>
        /// <returns>Enumerable</returns>
        public TsObject skip_while(TsObject[] args)
        {
            var script = (TsDelegate)args[0];
            return new WrappedEnumerable(this.SkipWhile(o => (bool)script.Invoke(o)));
        }

        /// <summary>
        /// Computes the sum of the elements in the sequence.
        /// </summary>
        /// <arg name="[selector]" type="script">Extracts a numeric value from each element in the sequence.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum</source>
        /// <returns>number</returns>
        public TsObject sum(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return this.Sum(o => (double)o);
                case 1:
                    var script = (TsDelegate)args[0];
                    return this.Sum(o => (double)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(sum)}");
            }
        }

        /// <summary>
        /// Returns the specified number of elements from the start of the sequence.
        /// </summary>
        /// <arg name="count" type="number">The number of elements to take.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.take</source>
        /// <returns>Enumerable</returns>
        public TsObject take(TsObject[] args)
        {
            return new WrappedEnumerable(this.Take((int)args[0]));
        }

        /// <summary>
        /// Returns elements from the start of the sequence while the condition holds true.
        /// </summary>
        /// <arg name="condition" type="script">A script that tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.takewhile</source>
        /// <returns>Enumerable</returns>
        public TsObject take_while(TsObject[] args)
        {
            var script = (TsDelegate)args[0];
            return new WrappedEnumerable(this.TakeWhile(o => (bool)script.Invoke(o)));
        }

        /// <summary>
        /// Converts the sequence to an array.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.toarray</source>
        /// <returns>array</returns>
        public TsObject to_array(TsObject[] args)
        {
            return this.ToArray();
        }

        /// <summary>
        /// Converts the sequence to a map.
        /// </summary>
        /// <arg name="key_selector" type="script">A script to extract a key from each element.</arg>
        /// <arg name="[value_selector]" type="script">A script to extract a value from each element. If absent, the value will be the full element.</arg>
        /// <arg name="[comparer]" type="EqualityComparer">A comparer used to compare key values.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.todictionary</source>
        /// <returns>[Map]({{site.baseurl}}/docs/Map)</returns>
        public TsObject to_map(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    var keySelector = (TsDelegate)args[0];
                    return TsMap.Wrap(this.ToDictionary(o => keySelector.Invoke(o)));
                case 2:
                    keySelector = (TsDelegate)args[0];
                    var valueSelector = (TsDelegate)args[1];
                    return TsMap.Wrap(this.ToDictionary(o => keySelector.Invoke(o), o => valueSelector.Invoke(o)));
                case 3:
                    keySelector = (TsDelegate)args[0];
                    valueSelector = (TsDelegate)args[1];
                    return TsMap.Wrap(this.ToDictionary(o => keySelector.Invoke(o), o => valueSelector.Invoke(o), (IEqualityComparer<TsObject>)args[2].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(to_map)}");
            }
        }

        /// <summary>
        /// Converts the sequence to a list.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tolist</source>
        /// <returns>[List]({{site.baseurl}}/docs/List)</returns>
        public TsObject to_list(TsObject[] args)
        {
            return TsList.Wrap(this.ToList());
        }

        /// <summary>
        /// Counts the number of elements in the sequence. If a script is given, counts the number of elements that satisfy the condition.
        /// </summary>
        /// <arg name="condition" type="script">A script that tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count</source>
        /// <returns>number</returns>
        public TsObject total(TsObject[] args)
        {
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.Count();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Count(o => (bool)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(total)}");
            }
        }

        /// <summary>
        /// Produces the set union between this sequence and another.
        /// </summary>
        /// <arg name="other" type="Enumerable">The other sequence.</arg>
        /// <arg name="comparer" type="EqualityComparer">The comparer used to compare the values.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.union</source>
        /// <returns>Enumerable</returns>
        public TsObject union(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(this.Union((IEnumerable<TsObject>)args[0].WeakValue));
                case 2:
                    return new WrappedEnumerable(this.Union((IEnumerable<TsObject>)args[0].WeakValue, (IEqualityComparer<TsObject>)args[1].WeakValue));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(union)}");
            }
        }

        /// <summary>
        /// Filters the elements in the sequence based on a condition.
        /// </summary>
        /// <arg name="condition" type="script">A script that tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where</source>
        /// <returns>Enumerable</returns>
        public TsObject where(TsObject[] args)
        {
            var script = (TsDelegate)args[0];
            return new WrappedEnumerable(this.Where(o => (bool)script.Invoke(o)));
        }

        /// <summary>
        /// Applies a script to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <arg name="other" type="Enumerable">The other sequence to merge.</arg>
        /// <arg name="result_selector" type="script">A script that specifies how to merge the elements from the two sequences.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.zip</source>
        /// <returns>Enumerable</returns>
        public TsObject zip(TsObject[] args)
        {
            var script = (TsDelegate)args[1];
            return new WrappedEnumerable(this.Zip((IEnumerable<TsObject>)args[0].WeakValue, (o1, o2) => script.Invoke(o1, o2)));
        }

        public static implicit operator TsObject(TsEnumerable objects) => new TsInstanceWrapper(objects);
        public static explicit operator TsEnumerable(TsObject obj) => (TsEnumerable)obj.WeakValue;
    }

    public class WrappedEnumerable : TsEnumerable
    {
        private IEnumerable<TsObject> _enumerable;

        public override string ObjectType => "TaffyScript.Collections.WrappedEnumerable";

        public WrappedEnumerable(IEnumerable<TsObject> enumerable)
        {
            _enumerable = enumerable;
        }

        public override IEnumerator<TsObject> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        public override TsObject get_enumerator(TsObject[] args)
        {
            return new TsEnumerator(this);
        }
    }
}
