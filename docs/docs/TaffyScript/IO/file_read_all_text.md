---
layout: default
title: file_read_all_text
---

# file_read_all_text

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_read_all_text]({{site.baseurl}}/docs/TaffyScript/IO/file_read_all_text/)

_Opens a text file, reads the contents into a string, then closes the file._

```cs
file_read_all_text(path, [encoding=*utf-8*])
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

**Returns:** string
