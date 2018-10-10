---
layout: default
title: Buffer.copy_to
---

# Buffer.copy_to

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[Buffer]({{site.baseurl}}/docs/TaffyScript/IO/Buffer/).[copy_to]({{site.baseurl}}/docs/TaffyScript/IO/Buffer/copy_to/)

_Copies the data from this buffer to another._

```cs
Buffer.copy_to(dest, length, [source_index=0], [destination_index=0])
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
      <td>dest</td>
      <td>[Buffer]({{site.baseurl}}/docs/TaffyScript/IO/Buffer)</td>
      <td>The buffer to copy to.</td>
    </tr>
    <tr>
      <td>length</td>
      <td>number</td>
      <td>The number of bytes to copy.</td>
    </tr>
    <tr>
      <td>[source_index=0]</td>
      <td>number</td>
      <td>The position to start copying from.</td>
    </tr>
    <tr>
      <td>[destination_index=0]</td>
      <td>number</td>
      <td>The position to start copying to.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
