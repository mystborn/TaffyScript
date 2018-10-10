---
layout: default
title: StreamReader
---

# StreamReader

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[StreamReader]({{site.baseurl}}/docs/TaffyScript/IO/StreamReader/)

_Reads characters from a byte stream using a particular encoding._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/base_stream/">base_stream</a></td>
      <td>[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)</td>
      <td>Gets the underlying stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/current_encoding/">current_encoding</a></td>
      <td>string</td>
      <td>Gets the name of the encoding the StreamReader is using.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/end_of_stream/">end_of_stream</a></td>
      <td>bool</td>
      <td>Determines if the stream position is at the end of the stream.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/create/">create(source, [encoding=*utf-8*], [detect_encoding_from_byte_order_marks], [buffer_size])</a></td>
      <td>Initializes a new StreamReader from a Stream or a path to a file.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/dispose">dispose()</a></td>
      <td>Releases all resource used by this TextReader.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/peek">peek()</a></td>
      <td>Gets the next character as a number without actually reading it.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/read">read()</a></td>
      <td>Reads the next character as a number.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/read_line">read_line()</a></td>
      <td>Reads a line of characters and returns the data as a string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/IO/StreamReader/read_to_end">read_to_end()</a></td>
      <td>Reads all characters from the current position to the end of the TextReader and returns them as one string.</td>
    </tr>
  </tbody>
</table>
