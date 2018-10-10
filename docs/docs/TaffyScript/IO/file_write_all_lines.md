---
layout: default
title: file_write_all_lines
---

# file_write_all_lines

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_write_all_lines]({{site.baseurl}}/docs/TaffyScript/IO/file_write_all_lines/)

_Creates a new file, writes one or more strings to the file, then closes the file._

```cs
file_write_all_lines(path, lines, [encoding=*utf-8*])
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
      <td>The path of the file to create.</td>
    </tr>
    <tr>
      <td>lines</td>
      <td>[Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable)</td>
      <td>The lines to write to the file.</td>
    </tr>
    <tr>
      <td>[encoding=*utf-8*]</td>
      <td>string</td>
      <td>The name of the encoding used to write to the file.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
