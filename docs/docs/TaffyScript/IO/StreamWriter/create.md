---
layout: default
title: StreamWriter.create
---

# StreamWriter Constructor

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/).[create]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter/create/)

_Initializes a new StreamWriter from a Stream or a path to a file._

```cs
new StreamWriter.create(source, [append], [encoding=*utf-8*], [buffer_size], [leave_open])
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
      <td>[append]</td>
      <td>bool</td>
      <td>Determines whether to append data to a file or overwrite it. Ignored if initialized from a stream.</td>
    </tr>
    <tr>
      <td>[encoding=*utf-8*]</td>
      <td>string</td>
      <td>The name of the character encoding to use.</td>
    </tr>
    <tr>
      <td>[buffer_size]</td>
      <td>number</td>
      <td>The buffer size, in bytes.</td>
    </tr>
    <tr>
      <td>[leave_open]</td>
      <td>bool</td>
      <td>Determines whether to leave the underlying stream open after the StreamWriter is disposed. Ignored it initialized from a path.</td>
    </tr>
  </tbody>
</table>
