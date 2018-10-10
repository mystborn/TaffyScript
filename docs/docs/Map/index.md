---
layout: default
title: Map
---

# Map

[\[global\]]({{site.baseurl}}/docs/).[Map]({{site.baseurl}}/docs/Map/)

_Maps a collection of keys to values._

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
      <td><a href="{{site.baseurl}}/docs/Map/count/">count</a></td>
      <td>number</td>
      <td>Gets the number of items added to the map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/keys/">keys</a></td>
      <td>Enumerable</td>
      <td>Gets a collection of the keys added to the map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/values/">values</a></td>
      <td>Enumerable</td>
      <td>Gets a collection of the values added to the map.</td>
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
      <td><a href="{{site.baseurl}}/docs/Map/get">get(key)</a></td>
      <td>Gets the value associated with the specified key.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/get_enumerator">get_enumerator()</a></td>
      <td>Gets an Enumerator used to iterate over the items in the map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/set">set(key, value)</a></td>
      <td>Sets the value associated with the specified key.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/add">add(key, value)</a></td>
      <td>Attempts to add an item to the map. Returns true if the item was added, false otherwise.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/clear">clear()</a></td>
      <td>Clears all items from the map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/remove">remove(key)</a></td>
      <td>Attempts to remove the item with the specified key.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/contains_key">contains_key(key)</a></td>
      <td>Determines if an item with the specified key is in the map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/copy">copy()</a></td>
      <td>Creates a copy of the map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/any">any([condition])</a></td>
      <td>Determines if there are any elements in the sequence. If a script is provided, determines if any element satisfies a condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/average">average(selector)</a></td>
      <td>Computes the average of the elements in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/concat">concat(other)</a></td>
      <td>Combines this sequence with another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/contains">contains(value, [comparer])</a></td>
      <td>Determines if the collection contains a value, optionally using an EqualityComparer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/distinct">distinct([comparer])</a></td>
      <td>Returns the distinct elements from the sequence, optionally using an EqualityComparer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/element_at">element_at(index)</a></td>
      <td>Gets the element at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/element_at_or_default">element_at_or_default(index)</a></td>
      <td>Gets the element at the specified index, or null if index is out of range.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/except">except(other, [comparer])</a></td>
      <td>Gets the set difference between this and another sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/first">first([condition])</a></td>
      <td>Gets the first element in the sequence. If a script is provided, gets the first element in the sequence that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/first_or_default">first_or_default([condition])</a></td>
      <td>Gets the first element in the sequence. If a script is provided, gets the first element in the sequence that satisfies the condition. Returns null if no element is found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/intersect">intersect(other, [comparer])</a></td>
      <td>Gets the set intersection between this and another sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/iterate">iterate(function)</a></td>
      <td>Invokes a script over each element in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/join">join(other, outer_key_selector, inner_key_selector, result_selector, [comparer])</a></td>
      <td>Correlates the elements of two sequences based on matching keys.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/last">last([condition])</a></td>
      <td>Gets the last element in the sequence. If a script is provided, gets the last element in the sequence that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/last_or_default">last_or_default([condition])</a></td>
      <td>Gets the last element in the sequence. If a script is provided, gets the last element in the sequence that satisfies the condition. Returns null if no element is found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/max">max([transform])</a></td>
      <td>Gets the maximum value in the sequence, optionally using a script to get a numeric value from each element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/min">min([transform])</a></td>
      <td>Gets the minimum value in the sequence, optionally using a script to get a numeric value from each element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/order_by">order_by(key_selector, [comparer])</a></td>
      <td>Sorts the elements in the sequence based on the key returned by a script, optionally using an IComparer to determine order.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/reverse">reverse()</a></td>
      <td>Inverts the order of elements in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/select">select(selector)</a></td>
      <td>Projects each element in the sequence into a new form.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/select_many">select_many(collection_selector, [result_selector])</a></td>
      <td>Projects each element in the sequence to an Enumerable and flattens the resulting sequences into one sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/sequence_equal">sequence_equal(other, [comparer])</a></td>
      <td>Determines if this sequence is equal to another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/single">single([condition])</a></td>
      <td>Returns the only element in the sequence or throws an exception if there is not exactly one element. If a script is given, returns the only element that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/single_or_default">single_or_default([condition])</a></td>
      <td>Returns the only element in the sequence or null if there are nor elements. Throws an exception if there is more than one element. If a script is given, returns the only element that satisfies the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/skip">skip(count)</a></td>
      <td>Bypasses the specified number of elements and returns the rest.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/skip_while">skip_while(condition)</a></td>
      <td>Bypasses elements while the specified condition is true and returns the rest.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/sum">sum([selector])</a></td>
      <td>Computes the sum of the elements in the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/take">take(count)</a></td>
      <td>Returns the specified number of elements from the start of the sequence.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/take_while">take_while(condition)</a></td>
      <td>Returns elements from the start of the sequence while the condition holds true.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/to_array">to_array()</a></td>
      <td>Converts the sequence to an array.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/to_map">to_map(key_selector, [value_selector], [comparer])</a></td>
      <td>Converts the sequence to a map.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/to_list">to_list()</a></td>
      <td>Converts the sequence to a list.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/total">total(condition)</a></td>
      <td>Counts the number of elements in the sequence. If a script is given, counts the number of elements that satisfy the condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/union">union(other, comparer)</a></td>
      <td>Produces the set union between this sequence and another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/where">where(condition)</a></td>
      <td>Filters the elements in the sequence based on a condition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map/zip">zip(other, result_selector)</a></td>
      <td>Applies a script to the corresponding elements of two sequences, producing a sequence of the results.</td>
    </tr>
  </tbody>
</table>
