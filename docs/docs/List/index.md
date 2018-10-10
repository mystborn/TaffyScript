---
layout: default
title: List
---

# List

[\[global\]]({{site.baseurl}}/docs/).[List]({{site.baseurl}}/docs/List/)

_Represents a list of objects that can be access by index._

## Properties

<table>
  <col width="15%">
  <col width="15%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/count/">count</a></td>
      <td>number</td>
      <td>Gets the number of items in the list.</td>
    </tr>
  </tbody>
</table>

## Constructor

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/create/">create([..args])</a></td>
      <td>Creates a new list composed of the arguments.</td>
    </tr>
  </tbody>
</table>

## Scripts

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Signature</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/add">add(..elements)</a></td>
      <td>Adds the arguments to the list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/clear">clear()</a></td>
      <td>Removes all elements from this list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/copy">copy()</a></td>
      <td>Creates a copy of the list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/get">get(index)</a></td>
      <td>Gets the element at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/get_enumerator">get_enumerator()</a></td>
      <td>Gets an enumerator used to iterate over the elements in the list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/insert">insert(index, item)</a></td>
      <td>Inserts a value into the list at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/index_of">index_of(item)</a></td>
      <td>Finds the index of the first occurrence of the value in the list. Returns -1 if the value isn't found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/remove">remove(index)</a></td>
      <td>Removes the value at the specified index within the list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/set">set(index, value)</a></td>
      <td>Sets the value at the specified index within the list. If `index` is greater than the size of the list, null elements will be added until the index can be set.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/shuffle">shuffle()</a></td>
      <td>Shuffles the values in the list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/sort">sort([comparer])</a></td>
      <td>Sorts the elements in the list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/any">any([condition])</a></td>
      <td>Determines if there are any elements in the sequence. If a script is provided, determines if any element satisfies a condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/average">average(selector)</a></td>
      <td>Computes the average of the elements in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/concat">concat(other)</a></td>
      <td>Combines this sequence with another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/contains">contains(value, [comparer])</a></td>
      <td>Determines if the collection contains a value, optionally using an EqualityComparer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/distinct">distinct([comparer])</a></td>
      <td>Returns the distinct elements from the sequence, optionally using an EqualityComparer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/element_at">element_at(index)</a></td>
      <td>Gets the element at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/element_at_or_default">element_at_or_default(index)</a></td>
      <td>Gets the element at the specified index, or null if index is out of range.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/except">except(other, [comparer])</a></td>
      <td>Gets the set difference between this and another sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/first">first([condition])</a></td>
      <td>Gets the first element in the sequence. If a script is provided, gets the first element in the sequence that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/first_or_default">first_or_default([condition])</a></td>
      <td>Gets the first element in the sequence. If a script is provided, gets the first element in the sequence that satisfies the condition. Returns null if no element is found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/intersect">intersect(other, [comparer])</a></td>
      <td>Gets the set intersection between this and another sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/iterate">iterate(function)</a></td>
      <td>Invokes a script over each element in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/join">join(other, outer_key_selector, inner_key_selector, result_selector, [comparer])</a></td>
      <td>Correlates the elements of two sequences based on matching keys.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/last">last([condition])</a></td>
      <td>Gets the last element in the sequence. If a script is provided, gets the last element in the sequence that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/last_or_default">last_or_default([condition])</a></td>
      <td>Gets the last element in the sequence. If a script is provided, gets the last element in the sequence that satisfies the condition. Returns null if no element is found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/max">max([transform])</a></td>
      <td>Gets the maximum value in the sequence, optionally using a script to get a numeric value from each element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/min">min([transform])</a></td>
      <td>Gets the minimum value in the sequence, optionally using a script to get a numeric value from each element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/order_by">order_by(key_selector, [comparer])</a></td>
      <td>Sorts the elements in the sequence based on the key returned by a script, optionally using an IComparer to determine order.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/reverse">reverse()</a></td>
      <td>Inverts the order of elements in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/select">select(selector)</a></td>
      <td>Projects each element in the sequence into a new form.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/select_many">select_many(collection_selector, [result_selector])</a></td>
      <td>Projects each element in the sequence to an Enumerable and flattens the resulting sequences into one sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/sequence_equal">sequence_equal(other, [comparer])</a></td>
      <td>Determines if this sequence is equal to another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/single">single([condition])</a></td>
      <td>Returns the only element in the sequence or throws an exception if there is not exactly one element. If a script is given, returns the only element that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/single_or_default">single_or_default([condition])</a></td>
      <td>Returns the only element in the sequence or null if there are nor elements. Throws an exception if there is more than one element. If a script is given, returns the only element that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/skip">skip(count)</a></td>
      <td>Bypasses the specified number of elements and returns the rest.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/skip_while">skip_while(condition)</a></td>
      <td>Bypasses elements while the specified condition is true and returns the rest.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/sum">sum([selector])</a></td>
      <td>Computes the sum of the elements in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/take">take(count)</a></td>
      <td>Returns the specified number of elements from the start of the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/take_while">take_while(condition)</a></td>
      <td>Returns elements from the start of the sequence while the condition holds true.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/to_array">to_array()</a></td>
      <td>Converts the sequence to an array.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/to_map">to_map(key_selector, [value_selector], [comparer])</a></td>
      <td>Converts the sequence to a map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/to_list">to_list()</a></td>
      <td>Converts the sequence to a list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/total">total(condition)</a></td>
      <td>Counts the number of elements in the sequence. If a script is given, counts the number of elements that satisfy the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/union">union(other, comparer)</a></td>
      <td>Produces the set union between this sequence and another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/where">where(condition)</a></td>
      <td>Filters the elements in the sequence based on a condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/List/zip">zip(other, result_selector)</a></td>
      <td>Applies a script to the corresponding elements of two sequences, producing a sequence of the results.</td>
    </tr>
  </tbody>
</table>
