---
layout: default
title: Stream
---

# Stream

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream/)

_Base class for TaffyScript streams._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/can_read/">can_read</a></td>
      <td>bool</td>
      <td>Determines if the stream can be read from.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/can_seek/">can_seek</a></td>
      <td>bool</td>
      <td>Determines if the stream supports seeking.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/can_timeout/">can_timeout</a></td>
      <td>bool</td>
      <td>Determines if the stream can time out.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/can_write/">can_write</a></td>
      <td>bool</td>
      <td>Determines if the stream can be written to.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/length/">length</a></td>
      <td>number</td>
      <td>Gets the length in bytes of the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/position/">position</a></td>
      <td>number</td>
      <td>Gets or sets the position within the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/read_timeout/">read_timeout</a></td>
      <td>number</td>
      <td>Gets or sets the length of time in milliseconds the stream will attempt to read before timing out.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/write_timeout/">write_timeout</a></td>
      <td>number</td>
      <td>Gets or set the length of tume in milliseconds the stream will attempt to write before timing out.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/copy_to">copy_to(other, [buffer_size])</a></td>
      <td>Reads the bytes from this stream and writes them to another.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/dispose">dispose()</a></td>
      <td>Releases all resources used by this stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/flush">flush()</a></td>
      <td>Clears all buffers for this stream and writes any buffered data to the underlying device.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/read">read(buffer, offset, count)</a></td>
      <td>Reads a sequence of bytes from the stream into a buffer and returns the number of bytes read.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/read_byte">read_byte()</a></td>
      <td>Reads the next byte from the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/seek">seek(offset, origin)</a></td>
      <td>Sets the position within the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/set_length">set_length(length)</a></td>
      <td>Sets the length of the current stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/write">write(buffer, offset, count)</a></td>
      <td>Writes a sequence of bytes from a buffer to the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/Stream/write_byte">write_byte(byte)</a></td>
      <td>Writes a byte to the stream.</td>
    </tr>
  </tbody>
</table>
