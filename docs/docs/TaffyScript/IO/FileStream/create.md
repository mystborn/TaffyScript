---
layout: default
title: FileStream.create
---

# FileStream Constructor

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/).[create]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/create/)

_Creates a new FileStream from a path._

```cs
new FileStream.create(path, mode, [access], [share], [buffer_size=4096], [options])
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
      <td>Absolute or relative path to a file.</td>
    </tr>
    <tr>
      <td>mode</td>
      <td>[FileMode](https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode)</td>
      <td>Determines how to open or create the file.</td>
    </tr>
    <tr>
      <td>[access]</td>
      <td>[FileAccess](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileaccess)</td>
      <td>Determines how the file can be accessed by the FileStream.</td>
    </tr>
    <tr>
      <td>[share]</td>
      <td>[FileShare](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileshare)</td>
      <td>Determines how the file will be shared by processes.</td>
    </tr>
    <tr>
      <td>[buffer_size=4096]</td>
      <td>number</td>
      <td>A value greater than 0 indicating the buffer size.</td>
    </tr>
    <tr>
      <td>[options]</td>
      <td>[FileOptions](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileoptions)</td>
      <td>Determines additional file options.</td>
    </tr>
  </tbody>
</table>
