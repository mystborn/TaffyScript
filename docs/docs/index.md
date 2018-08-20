---
layout: default
title: Documentation
---

# Global

[\[global\]]({{site.baseurl}}/docs/)

These scripts and objects are available in the global scope. Reserved for the most commonly used functionality.

## Namespaces

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
      <td><a href="{{page.url}}TaffyScript/">TaffyScript</a></td>
      <td>Container namespace for basic TaffyScript functionality.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}TaffyScript/Numbers/">TaffyScript.Numbers</a></td>
      <td>General math scripts.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}TaffyScript/Reflection/">TaffyScript.Reflection</a></td>
      <td>Provides scripts to dynamically retrieve information about loaded assemblies.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}TaffyScript/Strings/">TaffyScript.Strings</a></td>
      <td>Contains scripts for processing and manipulating strings. Many of the scripts found here are imperative versions of the scripts found on the string object.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}TaffyScript/Xml/">TaffyScript.Xml</a></td>
      <td>Provides mechanisms to interact with xml files.</td>
    </tr>
  </tbody>
</table>

## Objects

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
      <td><a href="{{page.url}}Array/">Array</a></td>
      <td>The array literal type built in to TaffyScript</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}Grid/">Grid</a></td>
      <td>Represents a grid of objects that can be accessed by an x and y coordinate.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}List/">List</a></td>
      <td>Represents a list of objects that can be accessed by index.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}Map/">Map</a></td>
      <td>Maps a collection of keys to values.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string/">string</a></td>
      <td>The string literal built into TaffyScript. This type cannot be constructed using the `new` keyword.</td>
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
      <td><a href="{{page.url}}array_copy">array_copy(source, source_index, destination, destination_index, length)</a></td>
      <td>Copies a number of elements from one array to another.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}array_create">array_create(size, [default = null])</a></td>
      <td>Creates a new array of the specified size and value.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}array_equals">array_equals(array1, array2)</a></td>
      <td>Compares the elements in two arrays to determine equality.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}array_length">array_length(array, [..indeces])</a></td>
      <td>Gets the length of an array or one of its nested arrays.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}print">print(output)</a></td>
      <td>Prints a value to the console.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}real">real(str)</a></td>
      <td>Converts a string to a number</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}show_error">show_error(message, throws)</a></td>
      <td>Shows or throws an error message.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string">string(value)</a></td>
      <td>Converts a value to a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}typeof">typeof(value)</a></td>
      <td>Gets the type of an object.</td>
    </tr>
  </tbody>
</table>
