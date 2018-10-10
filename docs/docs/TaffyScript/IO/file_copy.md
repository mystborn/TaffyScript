---
layout: default
title: file_copy
---

# file_copy

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_copy]({{site.baseurl}}/docs/TaffyScript/IO/file_copy/)

_Copies an existing file to a new file, optionally allowing the overwrite of an existing file._

```cs
file_copy(source_file, dest_file, [overwrite=false])
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
      <td>source_file</td>
      <td>string</td>
      <td>The path of the source file.</td>
    </tr>
    <tr>
      <td>dest_file</td>
      <td>string</td>
      <td>The path of the destination file.</td>
    </tr>
    <tr>
      <td>[overwrite=false]</td>
      <td></td>
      <td>Determines whether this script is allowed to overwrite an existing file.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
