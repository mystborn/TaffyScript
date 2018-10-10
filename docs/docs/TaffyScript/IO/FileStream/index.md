---
layout: default
title: FileStream
---

# FileStream

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/)

_Provides a Stream for a file._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/can_read/">can_read</a></td>
      <td>bool</td>
      <td>Determines if the stream can be read from.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/can_seek/">can_seek</a></td>
      <td>bool</td>
      <td>Determines if the stream supports seeking.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/can_write/">can_write</a></td>
      <td>bool</td>
      <td>Determines if the stream can be written to.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/is_async/">is_async</a></td>
      <td>bool</td>
      <td>Determines if the file stream was opened asynchronously or synchronously.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/length/">length</a></td>
      <td>number</td>
      <td>Gets the length in bytes of the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/name/">name</a></td>
      <td>string</td>
      <td>Gets the absolute path of the file opened in the FileStream</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/position/">position</a></td>
      <td>number</td>
      <td>Gets or sets the position within the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/can_timeout/">can_timeout</a></td>
      <td>bool</td>
      <td>Determines if the stream can time out.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/read_timeout/">read_timeout</a></td>
      <td>number</td>
      <td>Gets or sets the length of time in milliseconds the stream will attempt to read before timing out.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/write_timeout/">write_timeout</a></td>
      <td>number</td>
      <td>Gets or set the length of tume in milliseconds the stream will attempt to write before timing out.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/create/">create(path, mode, [access], [share], [buffer_size=4096], [options])</a></td>
      <td>Creates a new FileStream from a path.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/copy_to">copy_to(other, [buffer_size])</a></td>
      <td>Reads the bytes from this stream and writes them to another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/dispose">dispose()</a></td>
      <td>Releases all resources used by this stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/flush">flush()</a></td>
      <td>Clears all buffers for this stream and writes any buffered data to the underlying device.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/read">read(buffer, offset, count)</a></td>
      <td>Reads a sequence of bytes from the stream into a buffer and returns the number of bytes read.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/read_byte">read_byte()</a></td>
      <td>Reads the next byte from the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/seek">seek(offset, origin)</a></td>
      <td>Sets the position within the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/set_length">set_length(length)</a></td>
      <td>Sets the length of the current stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/write">write(buffer, offset, count)</a></td>
      <td>Writes a sequence of bytes from a buffer to the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/FileStream/write_byte">write_byte(byte)</a></td>
      <td>Writes a byte to the stream.</td>
    </tr>
  </tbody>
</table>
