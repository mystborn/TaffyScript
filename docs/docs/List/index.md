---
layout: default
title: List
---

# List

[\[global\]]({{site.baseurl}}/docs/).[List]({{site.baseurl}}/docs/List/)

_Represents a list of objects that can be accessed by index._

## Fields

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
      <td><a href="{{page.url}}count/">count</a></td>
      <td>number</td>
      <td>Gets the number of elements in the list.</td>
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
      <td><a href="{{page.url}}create/">create()</a></td>
      <td>Initializes a new list.</td>
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
      <td><a href="{{page.url}}add">add(..values)</a></td>
      <td>Adds any number of values to the end of a list.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}clear">clear()</a></td>
      <td>Removes all values from the list.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}copy">copy()</a></td>
      <td>Creates a shallow copy of the list.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}get">get(index)</a></td>
      <td>Gets the value at the specified index in the list.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}insert">insert(index, value)</a></td>
      <td>Inserts a value into the list at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}index_of">index_of(value)</a></td>
      <td>Finds the index of the first occurrence of the value in the list. Returns -1 if the value isn't found.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}remove">remove(index)</a></td>
      <td>Removes the value at the specified index within the list.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}set">set(index, value)</a></td>
      <td>Sets the value at the specified index within the list. If `index` is greater than the size of the list, null elements will be added until the index can be set.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}shuffle">shuffle()</a></td>
      <td>Shuffles the values in the list.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}sort">sort()</a></td>
      <td>Sorts the values in the list.</td>
    </tr>
  </tbody>
</table>
