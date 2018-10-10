---
layout: default
title: FileInfo.copy_to
---

# FileInfo.copy_to

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/).[copy_to]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/copy_to/)

_Copies the file to a new file, optionally allowing the overwrite of an existing file._

```cs
FileInfo.copy_to(new_file_name, [overwrite=false])
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
      <td>new_file_name</td>
      <td>string</td>
      <td>The name of the new file to copy to.</td>
    </tr>
    <tr>
      <td>[overwrite=false]</td>
      <td>bool</td>
      <td>Determines whether this script can overwrite an existing file.</td>
    </tr>
  </tbody>
</table>

**Returns:** [FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo)
