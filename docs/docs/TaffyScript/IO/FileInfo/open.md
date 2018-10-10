---
layout: default
title: FileInfo.open
---

# FileInfo.open

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/).[open]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo/open/)

_Opens the file with the specified options._

```cs
FileInfo.open(mode, [access], [share])
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
      <td>mode</td>
      <td>[FileMode](https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode)</td>
      <td>Specifies the mode in which to open the file.</td>
    </tr>
    <tr>
      <td>[access]</td>
      <td>[FileAccess](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileaccess)</td>
      <td>Determines the access with which to open the file.</td>
    </tr>
    <tr>
      <td>[share]</td>
      <td>[FileShare](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileshare)</td>
      <td>Determines the type of access other FileStreams have to this file.</td>
    </tr>
  </tbody>
</table>

**Returns:** [FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)
