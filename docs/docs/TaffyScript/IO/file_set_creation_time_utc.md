---
layout: default
title: file_set_creation_time_utc
---

# file_set_creation_time_utc

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[file_set_creation_time_utc]({{site.baseurl}}/docs/TaffyScript/IO/file_set_creation_time_utc/)

_Sets the creation time of the specified file in universal coordinated time._

```cs
file_set_creation_time_utc(file_name, time)
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
      <td>file_name</td>
      <td>string</td>
      <td>The path of the file to set the creation time.</td>
    </tr>
    <tr>
      <td>time</td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>The date and time of the files creation.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
