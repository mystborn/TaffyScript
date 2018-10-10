---
layout: default
title: Stream.seek
---

# Stream.seek

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream/).[seek]({{site.baseurl}}/docs/TaffyScript/IO/Stream/seek/)

_Sets the position within the stream._

```cs
Stream.seek(offset, origin)
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
      <td>offset</td>
      <td>number</td>
      <td>A byte offset relative to the origin parameter.</td>
    </tr>
    <tr>
      <td>origin</td>
      <td>[SeekOrigin](https://docs.microsoft.com/en-us/dotnet/api/system.io.seekorigin)</td>
      <td>Indicates the starting point to obtain the new position.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
