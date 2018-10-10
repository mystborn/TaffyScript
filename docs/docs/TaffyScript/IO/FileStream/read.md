---
layout: default
title: FileStream.read
---

# FileStream.read

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/).[read]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/read/)

_Reads a sequence of bytes from the stream into a buffer and returns the number of bytes read._

```cs
FileStream.read(buffer, offset, count)
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
      <td>buffer</td>
      <td>[Buffer]({{site.baseurl}}/docs/TaffyScript.IO.Buffer)</td>
      <td>The buffer to write the data to.</td>
    </tr>
    <tr>
      <td>offset</td>
      <td>number</td>
      <td>The position in the buffer to write the data.</td>
    </tr>
    <tr>
      <td>count</td>
      <td>number</td>
      <td>The maximum number of bytes to be read.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
