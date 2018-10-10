---
layout: default
title: file_write_all_bytes
---

# file_write_all_bytes

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_write_all_bytes]({{site.baseurl}}/docs/TaffyScript/IO/file_write_all_bytes/)

_Creates a new file, writes the specified Buffer to the file, then closes the file. If the files exists, it is overwritten._

```cs
file_write_all_bytes(path, buffer)
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
      <td>The path of the file to write.</td>
    </tr>
    <tr>
      <td>buffer</td>
      <td>[Buffer]({{site.baseurl}}/docs/TaffyScript/IO/Buffer)</td>
      <td>The buffer to write to the file.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
