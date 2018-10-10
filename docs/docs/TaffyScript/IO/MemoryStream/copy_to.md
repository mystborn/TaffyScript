---
layout: default
title: MemoryStream.copy_to
---

# MemoryStream.copy_to

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[MemoryStream]({{site.baseurl}}/docs/TaffyScript/IO/MemoryStream/).[copy_to]({{site.baseurl}}/docs/TaffyScript/IO/MemoryStream/copy_to/)

_Reads the bytes from this stream and writes them to another._

```cs
MemoryStream.copy_to(other, [buffer_size])
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
      <td>other</td>
      <td>[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)</td>
      <td>The stream to write to.</td>
    </tr>
    <tr>
      <td>[buffer_size]</td>
      <td>number</td>
      <td>The size of the buffer.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
