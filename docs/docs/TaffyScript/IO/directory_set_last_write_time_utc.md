---
layout: default
title: directory_set_last_write_time_utc
---

# directory_set_last_write_time_utc

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[IO]({{site.baseurl}}/docs/TaffyScript/IO/).[directory_set_last_write_time_utc]({{site.baseurl}}/docs/TaffyScript/IO/directory_set_last_write_time_utc/)

_Sets the last write time of a directory in universal coordinated time._

```cs
directory_set_last_write_time_utc(path, time)
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
      <td>The path of the directory.</td>
    </tr>
    <tr>
      <td>time</td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>The date and time the directory was last written to.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
