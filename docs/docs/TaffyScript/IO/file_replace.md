---
layout: default
title: file_replace
---

# file_replace

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_replace]({{site.baseurl}}/docs/TaffyScript/IO/file_replace/)

_Replaces the contents of a file with the contents of another file, deleting the original file and creating a backup of the replaced file._

```cs
file_replace(source_file, dest_file, dest_backup_file, [ignore_metadata_errors=false])
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
      <td>The path of the file to be replaced.</td>
    </tr>
    <tr>
      <td>dest_backup_file</td>
      <td>string</td>
      <td>The path of the backup file.</td>
    </tr>
    <tr>
      <td>[ignore_metadata_errors=false]</td>
      <td>bool</td>
      <td>Determines whether to ignore merge errors from the replaced file to the replacement file.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
