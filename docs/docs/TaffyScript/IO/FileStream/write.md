---
layout: default
title: FileStream.write
---

# FileStream.write

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/).[write]({{site.baseurl}}/docs/TaffyScript/IO/FileStream/write/)

_Writes a sequence of bytes from a buffer to the stream._

```cs
FileStream.write(buffer, offset, count)
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
      <td>The buffer to read the data from.</td>
    </tr>
    <tr>
      <td>offset</td>
      <td>number</td>
      <td>The position in the buffer to start reading from.</td>
    </tr>
    <tr>
      <td>count</td>
      <td>number</td>
      <td>The number of bytes to read from the buffer.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
