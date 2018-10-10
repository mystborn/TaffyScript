---
layout: default
title: file_append_all_lines
---

# file_append_all_lines

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_append_all_lines]({{site.baseurl}}/docs/TaffyScript/IO/file_append_all_lines/)

_Appends the lines to a file and then closes the file. If the file does not exist, it is created first._

```cs
file_append_all_lines(file_name, lines, [encoding])
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
    <tr>
      <td>lines</td>
      <td>array or [Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable)</td>
      <td>The lines to write to the file.</td>
    </tr>
    <tr>
      <td>[encoding]</td>
      <td>string</td>
      <td>The name of the encoding to use to write the lines. Defaults to utf-8.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
