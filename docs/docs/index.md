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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/">TaffyScript</a></td>
      <td>Container namespace for basic TaffyScript functionality.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Collections/">TaffyScript.Collections</a></td>
      <td>Contains objects and interfaces that define various collections of objects.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/">TaffyScript.IO</a></td>
      <td>Provides access to scripts and objects related to processing input and output.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Numbers/">TaffyScript.Numbers</a></td>
      <td>Provides commonly used, general math scripts.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Reflection/">TaffyScript.Reflection</a></td>
      <td>Provides scripts to dynamically retrieve information about loaded assemblies.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/">TaffyScript.Strings</a></td>
      <td>Provides mechanisms for procesing and manipulating strings.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/">TaffyScript.Strings.RegularExpressions</a></td>
      <td>Contains objects and scripts that provide access to the .NET regular expression engine.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/">TaffyScript.Xml</a></td>
      <td>Provides standards-based support for processing XML.</td>
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
      <td><a href="{{site.baseurl}}/docs/List">List</a></td>
      <td>Represents a list of objects that can be access by index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Map">Map</a></td>
      <td>Maps a collection of keys to values.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/Array">Array</a></td>
      <td>THe array literal type built into TaffyScript.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string">string</a></td>
      <td>The string literal built into TaffyScript.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan">TimeSpan</a></td>
      <td>Represents a time interval.</td>
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
      <td><a href="{{site.baseurl}}/docs/array_copy">array_copy(source_array, source_index, dest_array, dest_index, count)</a></td>
      <td>Copies a range of elements from one array to another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/array_create">array_create(size, [default_element=null])</a></td>
      <td>Creates an array of the specified size, and optionally initialized with the specified value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/array_equals">array_equals(array1, array2)</a></td>
      <td>Determines if the elements in two arrays are equivalent.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/print">print([value], [..format_args])</a></td>
      <td>Prints a value depending on the arguments to the Standard Output.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/parse_number">parse_number(str, [styles], [culture])</a></td>
      <td>Converts a string into a number.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/try_parse_number">try_parse_number(str, [styles], [culture])</a></td>
      <td>Attempts to convert a string into a number.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/show_error">show_error(message, throws)</a></td>
      <td>Creates an error with the specified message and either throws it as an exception or writes it to the debug output.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string">string(value)</a></td>
      <td>Converts a value to a string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/typeof">typeof(value)</a></td>
      <td>Gets the type of a value.</td>
    </tr>
  </tbody>
</table>
