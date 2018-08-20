---
layout: default
title: Array
---

# Array

[\[global\]]({{site.baseurl}}/docs/).[Array]({{site.baseurl}}/docs/Array/)

_The array literal type built in to TaffyScript_

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
      <td><a href="{{page.url}}length/">length</a></td>
      <td>number</td>
      <td>Gets the number of elements in the array.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}count/">count</a></td>
      <td>number</td>
      <td>Gets the number of elements in the array. It works exactly the same as length. The typical syntax when using arrays is length, but the count field exists to unify the Array and List API.</td>
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
      <td><a href="{{page.url}}create/">create(element_number)</a></td>
      <td>Creates a new array with the specified number of elements.</td>
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
      <td><a href="{{page.url}}get">get(index)</a></td>
      <td>Gets the value at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}get_length">get_length([..nested_indeces])</a></td>
      <td>Gets the number of elements in this array or a nested array.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}set">set(index, value)</a></td>
      <td>Sets an index in the array to the specified value.</td>
    </tr>
  </tbody>
</table>
