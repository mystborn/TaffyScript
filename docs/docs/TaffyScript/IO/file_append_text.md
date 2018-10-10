---
layout: default
title: file_append_text
---

# file_append_text

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_append_text]({{site.baseurl}}/docs/TaffyScript/IO/file_append_text/)

_Creates a [StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter) that appends utf-8 encoded text to the file. If the file does not exist, it is created first._

```cs
file_append_text(file_name)
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
      <td>file_name</td>
      <td>string</td>
      <td>The path of the file to write to.</td>
    </tr>
  </tbody>
</table>

**Returns:** [StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter)
