---
layout: default
title: DirectoryInfo.delete
---

# DirectoryInfo.delete

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/).[delete]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo/delete/)

_Deletes this directory._

```cs
DirectoryInfo.delete([delete_subdirectories=false])
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
      <td>[delete_subdirectories=false]</td>
      <td>bool</td>
      <td>Determines whether to delete files and subdirectories. If this is false and the directory is not empty, this throws an exception.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
