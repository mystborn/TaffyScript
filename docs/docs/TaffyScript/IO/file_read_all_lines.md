---
layout: default
title: file_read_all_lines
---

# file_read_all_lines

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_read_all_lines]({{site.baseurl}}/docs/TaffyScript/IO/file_read_all_lines/)

_Opens a text file, reads all lines into an array, then closes the file._

```cs
file_read_all_lines(path, [encoding=*utf-8*])
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
      <td>path</td>
      <td>string</td>
      <td>The file to open.</td>
    </tr>
    <tr>
      <td>[encoding=*utf-8*]</td>
      <td>string</td>
      <td>The name of the encoding used to read the file.</td>
    </tr>
  </tbody>
</table>

**Returns:** array
