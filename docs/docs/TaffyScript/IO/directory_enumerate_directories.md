---
layout: default
title: directory_enumerate_directories
---

# directory_enumerate_directories

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[directory_enumerate_directories]({{site.baseurl}}/docs/TaffyScript/IO/directory_enumerate_directories/)

_Gets a collection of directory names that meet the specified criteria._

```cs
directory_enumerate_directories(path, [search_pattern], [search_option])
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
      <td>The path of the directory to search.</td>
    </tr>
    <tr>
      <td>[search_pattern]</td>
      <td>string</td>
      <td>The string to match against names of the directories in the specified path. Can include path literals and the wildcards * and ?</td>
    </tr>
    <tr>
      <td>[search_option]</td>
      <td>[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)</td>
      <td>Specifies whether the search operation should only include the top directory or all subdirectories.</td>
    </tr>
  </tbody>
</table>

**Returns:** Enumerable
