---
layout: default
title: directory_enumerate_file_system_entries
---

# directory_enumerate_file_system_entries

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[directory_enumerate_file_system_entries]({{site.baseurl}}/docs/TaffyScript/IO/directory_enumerate_file_system_entries/)

_Gets a collection of file and directory names that meet the specified criteria._

```cs
directory_enumerate_file_system_entries(path, [search_pattern], [search_option])
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
      <td>The string to match against names in the specified path. Can include path literals and the wildcards * and ?</td>
    </tr>
    <tr>
      <td>[search_option]</td>
      <td>[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)</td>
      <td>Specifies whether the search operation should only include files and directories in the top directory or all subdirectories.</td>
    </tr>
  </tbody>
</table>

**Returns:** Enumerable
