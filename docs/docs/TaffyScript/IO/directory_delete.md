---
layout: default
title: directory_delete
---

# directory_delete

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[directory_delete]({{site.baseurl}}/docs/TaffyScript/IO/directory_delete/)

_Deletes the directory, and, if specified, any subdirectories and files._

```cs
directory_delete(path, [recursive=false])
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
      <td>The path of the directory to delete.</td>
    </tr>
    <tr>
      <td>[recursive=false]</td>
      <td>bool</td>
      <td>Determines whether to delete files and subdirectories. Throws an exception if this is false but the specified directory isn't empty.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
