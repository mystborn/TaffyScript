---
layout: default
title: StreamReader.create
---

# StreamReader Constructor

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[StreamReader]({{site.baseurl}}/docs/TaffyScript/IO/StreamReader/).[create]({{site.baseurl}}/docs/TaffyScript/IO/StreamReader/create/)

_Initializes a new StreamReader from a Stream or a path to a file._

```cs
new StreamReader.create(source, [encoding=*utf-8*], [detect_encoding_from_byte_order_marks], [buffer_size])
```

## Arguments

<table>
  <col width="15%">
  <col width="15%">
  <thead>
    <tr>
      <th>Argument</th>
      <th>Type</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>source</td>
      <td>string or [Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)</td>
      <td>A path to a file or a stream to write to.</td>
    </tr>
    <tr>
      <td>[encoding=*utf-8*]</td>
      <td>string</td>
      <td>The name of the character encoding to use.</td>
    </tr>
    <tr>
      <td>[detect_encoding_from_byte_order_marks]</td>
      <td>bool</td>
      <td>Determines whether to look for byte order marks at the beginning of the file.</td>
    </tr>
    <tr>
      <td>[buffer_size]</td>
      <td>number</td>
      <td>The minimum size of the buffer in bytes.</td>
    </tr>
  </tbody>
</table>
