---
layout: default
title: file_create
---

# file_create

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_create]({{site.baseurl}}/docs/TaffyScript/IO/file_create/)

_Creates a file with the specified path and returns a FileStream for it._

```cs
file_create(file_name, [buffer_size], [options])
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
      <td>The path of the new file.</td>
    </tr>
    <tr>
      <td>[buffer_size]</td>
      <td>number</td>
      <td>The size of the FileStream buffer.</td>
    </tr>
    <tr>
      <td>[options]</td>
      <td>[FileOptions](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileoptions?view=netframework-4.7)</td>
      <td>Describes how to create or overwrite the file.</td>
    </tr>
  </tbody>
</table>

**Returns:** [FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)
