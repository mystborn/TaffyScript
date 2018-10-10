---
layout: default
title: path_change_extension
---

# path_change_extension

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[path_change_extension]({{site.baseurl}}/docs/TaffyScript/IO/path_change_extension/)

_Changes the extension of a path._

```cs
path_change_extension(path, extension)
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
      <td>The path to modify.</td>
    </tr>
    <tr>
      <td>extension</td>
      <td>string</td>
      <td>The new extension (with or without a leading period). Use null to remove an existing extension.</td>
    </tr>
  </tbody>
</table>

**Returns:** string
