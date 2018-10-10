---
layout: default
title: StringWriter
---

# StringWriter

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[StringWriter]({{site.baseurl}}/docs/TaffyScript/IO/StringWriter/)

_Implements a TextWriter for writing information to a string. The information is stored in a [StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder)._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/culture/">culture</a></td>
      <td>string</td>
      <td>Gets the name of the culture that controls the formatting.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/encoding/">encoding</a></td>
      <td>string</td>
      <td>Gets the name of the character encoding in which the output is written.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/new_line/">new_line</a></td>
      <td>string</td>
      <td>Gets or sets the line terminator string.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/create/">create([string_builder])</a></td>
      <td>Initializes a new StringWriter.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/to_string">to_string()</a></td>
      <td>Returns a string containing the characters written to the current StringWriter so far.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/dispose">dispose()</a></td>
      <td>Releases all resources used by the TextWriter.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/flush">flush()</a></td>
      <td>Clears all buffers and writes any buffered data to be written to the underlying device.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write">write(value)</a></td>
      <td>Writes the text representation of the given string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_bool">write_bool(value)</a></td>
      <td>Writes the text representation of the given bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_byte">write_byte(value)</a></td>
      <td>Writes the text representation of the given byte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_sbyte">write_sbyte(value)</a></td>
      <td>Writes the text representation of the given sbyte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_ushort">write_ushort(value)</a></td>
      <td>Writes the text representation of the given ushort.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_short">write_short(value)</a></td>
      <td>Writes the text representation of the given short.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_uint">write_uint(value)</a></td>
      <td>Writes the text representation of the given uint.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_int">write_int(value)</a></td>
      <td>Writes the text representation of the given int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_ulong">write_ulong(value)</a></td>
      <td>Writes the text representation of the given ulong.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_long">write_long(value)</a></td>
      <td>Writes the text representation of the given long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_float">write_float(value)</a></td>
      <td>Writes the text representation of the given float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_double">write_double(value)</a></td>
      <td>Writes the text representation of the given double.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line">write_line([value])</a></td>
      <td>Writes a line terminator.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_bool">write_line_bool(value)</a></td>
      <td>Writes the text representation of the given bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_byte">write_line_byte(value)</a></td>
      <td>Writes the text representation of the given byte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_sbyte">write_line_sbyte(value)</a></td>
      <td>Writes the text representation of the given sbyte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_ushort">write_line_ushort(value)</a></td>
      <td>Writes the text representation of the given ushort.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_short">write_line_short(value)</a></td>
      <td>Writes the text representation of the given short.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_uint">write_line_uint(value)</a></td>
      <td>Writes the text representation of the given uint.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_int">write_line_int(value)</a></td>
      <td>Writes the text representation of the given int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_ulong">write_line_ulong(value)</a></td>
      <td>Writes the text representation of the given ulong.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_long">write_line_long(value)</a></td>
      <td>Writes the text representation of the given long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_float">write_line_float(value)</a></td>
      <td>Writes the text representation of the given float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StringWriter/write_line_double">write_line_double(value)</a></td>
      <td>Writes the text representation of the given double</td>
    </tr>
  </tbody>
</table>
