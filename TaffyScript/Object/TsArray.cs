using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;
using Number = System.Single;

namespace TaffyScript
{
    /// <summary>
    /// THe array literal type built into TaffyScript.
    /// </summary>
    /// <property name="length" type="number" access="get">
    ///     <summary>Gets the number of elements in the array.</summary>
    /// </property>
    /// <property name="count" type="number" access="get">
    ///     <summary>Gets the number of elements in the array. It works exactly the same as length. The typical syntax when using arrays is length, but the count field exists to unify the Array and List API.</summary>
    /// </property>
    [TaffyScriptObject("Array")]
    public class TsArray : TsObject, ITsInstance, IEnumerable<TsObject>
    {
        public TsObject[] Value { get; set; }
        public override VariableType Type => VariableType.Array;
        public override object WeakValue => Value;
        public string ObjectType => "Array";

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsArray(TsObject[] value)
        {
            Value = value;
        }

        public override TsObject[] GetArray()
        {
            return Value;
        }

        public override ITsInstance GetInstance() => this;

        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override Number GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsArray array)
                return Value == array.Value;
            else if (obj is TsObject[] val)
                return Value == val;

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("[");
            if (Value.Length != 0)
            {
                sb.Append(Value[0].ToString());
                for (var i = 1; i < Value.Length; i++)
                    sb.Append(", ").Append(Value[i].ToString());
            }
            return sb.Append("]").ToString();
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "any":
                    return any(args);
                case "average":
                    return average(args);
                case "concat":
                    return concat(args);
                case "contains":
                    return contains(args);
                case "distinct":
                    return distinct(args);
                case "element_at":
                    return element_at(args);
                case "element_at_or_default":
                    return element_at_or_default(args);
                case "except":
                    return except(args);
                case "first":
                    return first(args);
                case "first_or_default":
                    return first_or_default(args);
                case "get":
                    return Value[(int)args[0]];
                case "get_enumerator":
                    return get_enumerator(args);
                case "get_length":
                    return get_length(args);
                case "intersect":
                    return intersect(args);
                case "iterate":
                    return iterate(args);
                case "join":
                    return join(args);
                case "last":
                    return last(args);
                case "last_or_default":
                    return last_or_default(args);
                case "max":
                    return max(args);
                case "min":
                    return min(args);
                case "order_by":
                    return order_by(args);
                case "reverse":
                    return reverse(args);
                case "select":
                    return select(args);
                case "select_many":
                    return select_many(args);
                case "sequence_equal":
                    return sequence_equal(args);
                case "set":
                    Value[(int)args[0]] = args[1];
                    return Empty;
                case "single":
                    return single(args);
                case "single_or_default":
                    return single_or_default(args);
                case "skip":
                    return skip(args);
                case "skip_while":
                    return skip_while(args);
                case "sum":
                    return sum(args);
                case "take":
                    return take(args);
                case "take_while":
                    return take_while(args);
                case "to_array":
                    return to_array(args);
                case "to_list":
                    return to_list(args);
                case "to_map":
                    return to_map(args);
                case "total":
                    return total(args);
                case "union":
                    return union(args);
                case "where":
                    return where(args);
                case "zip":
                    return zip(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "any":
                    del = new TsDelegate(any, scriptName);
                    break;
                case "average":
                    del = new TsDelegate(average, scriptName);
                    break;
                case "concat":
                    del = new TsDelegate(concat, scriptName);
                    break;
                case "contains":
                    del = new TsDelegate(contains, scriptName);
                    break;
                case "distinct":
                    del = new TsDelegate(distinct, scriptName);
                    break;
                case "element_at":
                    del = new TsDelegate(element_at, scriptName);
                    break;
                case "element_at_or_default":
                    del = new TsDelegate(element_at_or_default, scriptName);
                    break;
                case "except":
                    del = new TsDelegate(except, scriptName);
                    break;
                case "first":
                    del = new TsDelegate(first, scriptName);
                    break;
                case "first_or_default":
                    del = new TsDelegate(first_or_default, scriptName);
                    break;
                case "get":
                    del = new TsDelegate(get, scriptName);
                    break;
                case "get_enumerator":
                    del = new TsDelegate(get_enumerator, scriptName);
                    break;
                case "get_length":
                    del = new TsDelegate(get, scriptName);
                    break;
                case "intersect":
                    del = new TsDelegate(intersect, scriptName);
                    break;
                case "iterate":
                    del = new TsDelegate(iterate, scriptName);
                    break;
                case "join":
                    del = new TsDelegate(join, scriptName);
                    break;
                case "last":
                    del = new TsDelegate(last, scriptName);
                    break;
                case "last_or_default":
                    del = new TsDelegate(last_or_default, scriptName);
                    break;
                case "max":
                    del = new TsDelegate(max, scriptName);
                    break;
                case "min":
                    del = new TsDelegate(min, scriptName);
                    break;
                case "order_by":
                    del = new TsDelegate(order_by, scriptName);
                    break;
                case "reverse":
                    del = new TsDelegate(reverse, scriptName);
                    break;
                case "select":
                    del = new TsDelegate(select, scriptName);
                    break;
                case "select_many":
                    del = new TsDelegate(select_many, scriptName);
                    break;
                case "sequence_equal":
                    del = new TsDelegate(sequence_equal, scriptName);
                    break;
                case "set":
                    del = new TsDelegate(set, scriptName);
                    break;
                case "single":
                    del = new TsDelegate(single, scriptName);
                    break;
                case "single_or_default":
                    del = new TsDelegate(single_or_default, scriptName);
                    break;
                case "skip":
                    del = new TsDelegate(skip, scriptName);
                    break;
                case "skip_while":
                    del = new TsDelegate(skip_while, scriptName);
                    break;
                case "sum":
                    del = new TsDelegate(sum, scriptName);
                    break;
                case "take":
                    del = new TsDelegate(take, scriptName);
                    break;
                case "take_while":
                    del = new TsDelegate(take_while, scriptName);
                    break;
                case "to_array":
                    del = new TsDelegate(to_array, scriptName);
                    break;
                case "to_list":
                    del = new TsDelegate(to_list, scriptName);
                    break;
                case "to_map":
                    del = new TsDelegate(to_map, scriptName);
                    break;
                case "total":
                    del = new TsDelegate(total, scriptName);
                    break;
                case "union":
                    del = new TsDelegate(union, scriptName);
                    break;
                case "where":
                    del = new TsDelegate(where, scriptName);
                    break;
                case "zip":
                    del = new TsDelegate(zip, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, delegateName);
        }

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "length":
                    return Value.Length;
                case "count":
                    return Value.Length;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <arg name="..indeces" type="number">The index of the value to get. If there is more than one number, these should be the indeces of the inner arrays until the desired array to get.</arg>
        /// <returns>object</returns>
        public TsObject get(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return Value[(int)args[0]];
                case 2:
                    return Value[(int)args[0]].GetArray()[(int)args[1]];
                case 3:
                    return Value[(int)args[0]].GetArray()[(int)args[1]].GetArray()[(int)args[2]];
                default:
                    var array = Value;
                    var count = args.Length - 1;
                    for (var i = 0; i < count; i++)
                        array = array[(int)args[i]].GetArray();

                    return array[count];
            }
        }

        /// <summary>
        /// Gets the number of elements in this array or a nested array.
        /// </summary>
        /// <arg name="[..nested_indeces]" type="numbers">If the target is a jagged array, these should be the indeces of the inner arrays until the desired array to get the length of.</arg>
        /// <returns>number</returns>
        public TsObject get_length(TsObject[] args)
        {
            if(args is null)
                return Value.Length;

            switch(args.Length)
            {
                case 0:
                    return Value.Length;
                case 1:
                    return Value[(int)args[0]].GetArray().Length;
                case 2:
                    return Value[(int)args[0]].GetArray()[(int)args[1]].GetArray().Length;
                default:
                    var array = Value;
                    for (var i = 0; i < args.Length; i++)
                        array = array[(int)args[i]].GetArray();
                    return array.Length;
            }
        }

        /// <summary>
        /// Sets an index in the array or a nested array to the specified value.
        /// </summary>
        /// <arg name="..indeces" type="numbers">The index of the value to set. If there is more than one number, these should be the indeces of the inner arrays until the desired array to set.</arg>
        /// <arg name="number" type="object">The value to set the index to.</arg>
        /// <returns>null</returns>
        public TsObject set(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    Value[(int)args[0]] = args[1];
                    break;
                case 3:
                    Value[(int)args[0]].GetArray()[(int)args[1]] = args[2];
                    break;
                case 4:
                    Value[(int)args[0]].GetArray()[(int)args[1]].GetArray()[(int)args[2]] = args[3];
                    break;
                default:
                    var array = Value;
                    var last = args.Length - 2;
                    for (var i = 0; i < last; i++)
                        array = array[(int)args[i]].GetArray();

                    array[(int)args[last]] = args[last + 1];
                    break;
            }
            return Empty;
        }

        #region IEnumerable Implementation

        // Because TsArray must inherit from TsObject, it has to reimplement
        // the entire TsEnumerable class. Hopefully when interfaces get default implementations
        // (c# 8?) this will be made a whole lot less painless, but this is the best option for now.


        /// <summary>
        /// Determines if there are any elements in the sequence. If a script is provided, determines if any element satisfies a condition.
        /// </summary>
        /// <arg name="[condition]" type="script">Tests each element for a condition.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any</source>
        /// <returns>bool</returns>
        public TsObject any(TsObject[] args)
        {
            switch (args?.Length)
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
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.Average(o => (Number)o);
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Average((o) => (Number)script.Invoke(o));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(average)}");
            }
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
            switch (args.Length)
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
            switch (args?.Length)
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
            switch (args.Length)
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
            switch (args?.Length)
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
            switch (args?.Length)
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
            switch (args.Length)
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
        /// Invokes a script over each element in the sequence.
        /// </summary>
        /// <arg name="function" type="script">The script to run over each element.</arg>
        /// <returns>null</returns>
        public TsObject iterate(TsObject[] args)
        {
            var script = (TsDelegate)args[0];
            foreach (var element in this)
                script.Invoke(element);

            return TsObject.Empty;
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

            switch (args.Length)
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
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.Max();
                case 1:
                    var script = args[0].GetDelegate();
                    return this.Max(o => (Number)script.Invoke(o));
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
                    return this.Min(o => (Number)script.Invoke(o));
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
            switch (args.Length)
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
            switch (args.Length)
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
            switch (args.Length)
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
            switch (args?.Length)
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
            switch (args?.Length)
            {
                case null:
                case 0:
                    return this.Sum(o => (Number)o);
                case 1:
                    var script = (TsDelegate)args[0];
                    return this.Sum(o => (Number)script.Invoke(o));
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
            switch (args.Length)
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
            switch (args.Length)
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

        public TsObject get_enumerator(TsObject[] args)
        {
            return new WrappedEnumerator(GetEnumerator());
        }

        public IEnumerator<TsObject> GetEnumerator()
        {
            return (IEnumerator<TsObject>)Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        #endregion
    }
}
