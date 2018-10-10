---
layout: default
title: TextWriter
---

# TextWriter

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[TextWriter]({{site.baseurl}}/docs/TaffyScript/IO/TextWriter/)

_Represents a writer that can write a sequential series of characters._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/culture/">culture</a></td>
      <td>string</td>
      <td>Gets the name of the culture that controls the formatting.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/encoding/">encoding</a></td>
      <td>string</td>
      <td>Gets the name of the character encoding in which the output is written.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/new_line/">new_line</a></td>
      <td>string</td>
      <td>Gets or sets the line terminator string.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/dispose">dispose()</a></td>
      <td>Releases all resources used by the TextWriter.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/flush">flush()</a></td>
      <td>Clears all buffers and writes any buffered data to be written to the underlying device.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write">write(value)</a></td>
      <td>Writes the text representation of the given string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_bool">write_bool(value)</a></td>
      <td>Writes the text representation of the given bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_byte">write_byte(value)</a></td>
      <td>Writes the text representation of the given byte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_sbyte">write_sbyte(value)</a></td>
      <td>Writes the text representation of the given sbyte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_ushort">write_ushort(value)</a></td>
      <td>Writes the text representation of the given ushort.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_short">write_short(value)</a></td>
      <td>Writes the text representation of the given short.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_uint">write_uint(value)</a></td>
      <td>Writes the text representation of the given uint.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_int">write_int(value)</a></td>
      <td>Writes the text representation of the given int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_ulong">write_ulong(value)</a></td>
      <td>Writes the text representation of the given ulong.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_long">write_long(value)</a></td>
      <td>Writes the text representation of the given long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_float">write_float(value)</a></td>
      <td>Writes the text representation of the given float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_double">write_double(value)</a></td>
      <td>Writes the text representation of the given double.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line">write_line([value])</a></td>
      <td>Writes a line terminator.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_bool">write_line_bool(value)</a></td>
      <td>Writes the text representation of the given bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_byte">write_line_byte(value)</a></td>
      <td>Writes the text representation of the given byte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_sbyte">write_line_sbyte(value)</a></td>
      <td>Writes the text representation of the given sbyte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_ushort">write_line_ushort(value)</a></td>
      <td>Writes the text representation of the given ushort.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_short">write_line_short(value)</a></td>
      <td>Writes the text representation of the given short.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_uint">write_line_uint(value)</a></td>
      <td>Writes the text representation of the given uint.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_int">write_line_int(value)</a></td>
      <td>Writes the text representation of the given int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_ulong">write_line_ulong(value)</a></td>
      <td>Writes the text representation of the given ulong.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_long">write_line_long(value)</a></td>
      <td>Writes the text representation of the given long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_float">write_line_float(value)</a></td>
      <td>Writes the text representation of the given float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/TextWriter/write_line_double">write_line_double(value)</a></td>
      <td>Writes the text representation of the given double</td>
    </tr>
  </tbody>
</table>
