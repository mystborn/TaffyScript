---
layout: default
title: DirectoryInfo.get_files
---

# DirectoryInfo.get_files

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/).[get_files]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/get_files/)

_Gets an array of the files in the directory._

```cs
DirectoryInfo.get_files([search_pattern], [search_option=SearchOption.TopDirectoryOnly])
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
      <td>[search_pattern]</td>
      <td>string</td>
      <td>The search string to compare against the names of files. Can contain literal path characters and the wildcards * and ?</td>
    </tr>
    <tr>
      <td>[search_option=SearchOption.TopDirectoryOnly]</td>
      <td>[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption)</td>
      <td>Specifies whether the search operation should only include the current directory or all subdirectories.</td>
    </tr>
  </tbody>
</table>

**Returns:** array
