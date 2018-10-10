---
layout: default
title: Buffer
---

# Buffer

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[Buffer]({{site.baseurl}}/docs/TaffyScript/IO/Buffer/)

_Represents an array of bytes that can be used to efficiently encode data._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/length/">length</a></td>
      <td>number</td>
      <td>Gets the number of bytes in the buffer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/count/">count</a></td>
      <td>number</td>
      <td>Gets the number of bytes in the buffer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/position/">position</a></td>
      <td>number</td>
      <td>Gets or sets the read and write position in the buffer.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/create/">create(size)</a></td>
      <td>Initializes a new buffer with the specified number of bytes.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/clear">clear()</a></td>
      <td>Sets all bytes in the buffer to zero and sets the position to 0.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/clone">clone()</a></td>
      <td>Creates a copy of the buffer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/copy_to">copy_to(dest, length, [source_index=0], [destination_index=0])</a></td>
      <td>Copies the data from this buffer to another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/get">get(index)</a></td>
      <td>Gets the byte at the specified position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_bool">read_bool()</a></td>
      <td>Reads a bool from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_byte">read_byte()</a></td>
      <td>Reads a byte from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_sbyte">read_sbyte()</a></td>
      <td>Reads an sbyte from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_ushort">read_ushort()</a></td>
      <td>Reads a ushort from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_short">read_short()</a></td>
      <td>Reads a short from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_uint">read_uint()</a></td>
      <td>Reads a uint from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_int">read_int()</a></td>
      <td>Reads an int from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_ulong">read_ulong()</a></td>
      <td>Reads a ulong from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_long">read_long()</a></td>
      <td>Reads a long from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_float">read_float()</a></td>
      <td>Reads a float from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_double">read_double()</a></td>
      <td>Reads a double from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/read_string">read_string()</a></td>
      <td>Reads a null-terminated unicode string from the buffer and increments the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/resize">resize(new_size)</a></td>
      <td>Resizes the buffer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/set">set(index, byte)</a></td>
      <td>Sets the byte at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_bool">write_bool(value)</a></td>
      <td>Writes a bool to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_byte">write_byte(value)</a></td>
      <td>Writes a byte to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_sbyte">write_sbyte(value)</a></td>
      <td>Writes an sbyte to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_ushort">write_ushort(value)</a></td>
      <td>Writes a ushort to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_short">write_short(value)</a></td>
      <td>Writes a short to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_uint">write_uint(value)</a></td>
      <td>Writes a uint to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_int">write_int(value)</a></td>
      <td>Writes an int to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_ulong">write_ulong(value)</a></td>
      <td>Writes a ulong to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_long">write_long(value)</a></td>
      <td>Writes a long to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_float">write_float(value)</a></td>
      <td>Writes a float to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_double">write_double(value)</a></td>
      <td>Writes a double to the buffer then increments and returns the position.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Buffer/write_string">write_string(value)</a></td>
      <td>Writes a null-terminated unicode string to the buffer then increments and returns the position.</td>
    </tr>
  </tbody>
</table>
