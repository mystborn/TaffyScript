---
layout: default
title: StringBuilder
---

# StringBuilder

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/)

_Represents a mutable string of characters._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/capacity/">capacity</a></td>
      <td>number</td>
      <td>Gets or sets the maximum number of characters that can be contained in the allocated memory of this StringBuilder.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/length/">length</a></td>
      <td>number</td>
      <td>Gets or sets the length of this StringBuilder.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/max_capacity/">max_capacity</a></td>
      <td>number</td>
      <td>Gets the maximum capacity of this instance.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append">append(value, [start_index=0], [count])</a></td>
      <td>Appends a string or substring to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_bool">append_bool(value)</a></td>
      <td>Appends the text representation of a bool to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_sbyte">append_sbyte(value)</a></td>
      <td>Appends the text representation of an sbyte to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_byte">append_byte(value)</a></td>
      <td>Appends the text representation of a byte to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_ushort">append_ushort(value)</a></td>
      <td>Appends the text representation of a ushort to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_short">append_short(value)</a></td>
      <td>Appends the text representation of a short to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_uint">append_uint(value)</a></td>
      <td>Appends the text representation of a uint to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_int">append_int(value)</a></td>
      <td>Appends the text representation of an int to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_ulong">append_ulong(value)</a></td>
      <td>Appends the text representation of a ulong to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_long">append_long(value)</a></td>
      <td>Appends the text representation of a long to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_float">append_float(value)</a></td>
      <td>Appends the text representation of a float to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_double">append_double(value)</a></td>
      <td>Appends the text representation of a double to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append_line">append_line([str])</a></td>
      <td>Appends a line terminator to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/clear">clear()</a></td>
      <td>Clears all characters from the current StringBuilder.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/get">get(index)</a></td>
      <td>Gets the character at the specified position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert">insert(index, value, [count=1])</a></td>
      <td>Inserts a string into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_bool">insert_bool(index, value)</a></td>
      <td>Inserts a bool into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_byte">insert_byte(index, value)</a></td>
      <td>Inserts a byte into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_sbyte">insert_sbyte(index, value)</a></td>
      <td>Inserts an sbyte into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_ushort">insert_ushort(index, value)</a></td>
      <td>Inserts a ushort into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_short">insert_short(index, value)</a></td>
      <td>Inserts a short into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_uint">insert_uint(index, value)</a></td>
      <td>Inserts a uint into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_int">insert_int(index, value)</a></td>
      <td>Inserts an int into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_ulong">insert_ulong(index, value)</a></td>
      <td>Inserts a ulong into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_long">insert_long(index, value)</a></td>
      <td>Inserts a long into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_float">insert_float(index, value)</a></td>
      <td>Inserts a float into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/insert_double">insert_double(index, value)</a></td>
      <td>Inserts a double into this instance at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/remove">remove(start_index, count)</a></td>
      <td>Removes the specified range of characters from this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/replace">replace(substring, replacement)</a></td>
      <td>Replaces all occurrences of the specified string with a replacement string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/set">set(index, char)</a></td>
      <td>Sets the character at the specified position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/to_string">to_string([start_index], [count])</a></td>
      <td>Converts this instance or a portion of it to a string.</td>
    </tr>
  </tbody>
</table>
