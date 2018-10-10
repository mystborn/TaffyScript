---
layout: default
title: file_open
---

# file_open

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_open]({{site.baseurl}}/docs/TaffyScript/IO/file_open/)

_Opens a FileStream on the specified path._

```cs
file_open(path, mode, [access], [share])
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
      <td>mode</td>
      <td>[FileMode](https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode?view=netframework-4.7)</td>
      <td>Determines whether a new file is created if one doesn't exist and whether to keep or overwrite the contents of existing files.</td>
    </tr>
    <tr>
      <td>[access]</td>
      <td>[FileAccess](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileaccess?view=netframework-4.7)</td>
      <td>Determines the types of operations that can be performed on the file.</td>
    </tr>
    <tr>
      <td>[share]</td>
      <td>[FileShare](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileshare?view=netframework-4.7)</td>
      <td>Specifies the type of access other threads have to the file.</td>
    </tr>
  </tbody>
</table>

**Returns:** [FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)
