---
layout: default
title: StreamWriter
---

# StreamWriter

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/)

_Implements a TextWriter for writing characters to a stream in a particular encoding._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/auto_flush/">auto_flush</a></td>
      <td>bool</td>
      <td>Determines if the StreamWriter will flush its buffer to the underlying stream after every call to write.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/culture/">culture</a></td>
      <td>string</td>
      <td>Gets the name of the culture that controls the formatting.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/encoding/">encoding</a></td>
      <td>string</td>
      <td>Gets the name of the character encoding in which the output is written.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/new_line/">new_line</a></td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/create/">create(source, [append], [encoding=*utf-8*], [buffer_size], [leave_open])</a></td>
      <td>Initializes a new StreamWriter from a Stream or a path to a file.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/dispose">dispose()</a></td>
      <td>Releases all resources used by the TextWriter.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/flush">flush()</a></td>
      <td>Clears all buffers and writes any buffered data to be written to the underlying device.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write">write(value)</a></td>
      <td>Writes the text representation of the given string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_bool">write_bool(value)</a></td>
      <td>Writes the text representation of the given bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_byte">write_byte(value)</a></td>
      <td>Writes the text representation of the given byte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_sbyte">write_sbyte(value)</a></td>
      <td>Writes the text representation of the given sbyte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_ushort">write_ushort(value)</a></td>
      <td>Writes the text representation of the given ushort.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_short">write_short(value)</a></td>
      <td>Writes the text representation of the given short.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_uint">write_uint(value)</a></td>
      <td>Writes the text representation of the given uint.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_int">write_int(value)</a></td>
      <td>Writes the text representation of the given int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_ulong">write_ulong(value)</a></td>
      <td>Writes the text representation of the given ulong.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_long">write_long(value)</a></td>
      <td>Writes the text representation of the given long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_float">write_float(value)</a></td>
      <td>Writes the text representation of the given float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_double">write_double(value)</a></td>
      <td>Writes the text representation of the given double.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line">write_line([value])</a></td>
      <td>Writes a line terminator.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_bool">write_line_bool(value)</a></td>
      <td>Writes the text representation of the given bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_byte">write_line_byte(value)</a></td>
      <td>Writes the text representation of the given byte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_sbyte">write_line_sbyte(value)</a></td>
      <td>Writes the text representation of the given sbyte.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_ushort">write_line_ushort(value)</a></td>
      <td>Writes the text representation of the given ushort.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_short">write_line_short(value)</a></td>
      <td>Writes the text representation of the given short.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_uint">write_line_uint(value)</a></td>
      <td>Writes the text representation of the given uint.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_int">write_line_int(value)</a></td>
      <td>Writes the text representation of the given int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_ulong">write_line_ulong(value)</a></td>
      <td>Writes the text representation of the given ulong.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_long">write_line_long(value)</a></td>
      <td>Writes the text representation of the given long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_float">write_line_float(value)</a></td>
      <td>Writes the text representation of the given float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/write_line_double">write_line_double(value)</a></td>
      <td>Writes the text representation of the given double</td>
    </tr>
  </tbody>
</table>
