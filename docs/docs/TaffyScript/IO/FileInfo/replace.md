---
layout: default
title: FileInfo.replace
---

# FileInfo.replace

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/).[replace]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/replace/)

_Replaces the contents of the specified file with the contents of this file._

```cs
FileInfo.replace(destination_file_name, destination_backup_file_name, [ignore_metadata_errors])
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
      <td>destination_file_name</td>
      <td>string</td>
      <td>The name of a file to replace.</td>
    </tr>
    <tr>
      <td>destination_backup_file_name</td>
      <td>string</td>
      <td>The name of a file with which to create a backup of the replaced file. Will not be created if this argument is null.</td>
    </tr>
    <tr>
      <td>[ignore_metadata_errors]</td>
      <td>bool</td>
      <td>Determines whether to ignore merge errors from the replaced file to the replacement file.</td>
    </tr>
  </tbody>
</table>

**Returns:** [FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo)
