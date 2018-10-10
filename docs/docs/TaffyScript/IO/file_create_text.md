---
layout: default
title: file_create_text
---

# file_create_text

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_create_text]({{site.baseurl}}/docs/TaffyScript/IO/file_create_text/)

_Creates or opens a file for writing utf-8 encoded text. If the file exists, its contents are overwritten._

```cs
file_create_text(file_name)
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
      <td>The path of the file to open.</td>
    </tr>
  </tbody>
</table>

**Returns:** [StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter)
