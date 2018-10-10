---
layout: default
title: DateTime.specify_kind
---

# DateTime.specify_kind

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime/).[specify_kind]({{site.baseurl}}/docs/TaffyScript/DateTime/specify_kind/)

_Creates a new DateTime that has the same number of ticks as the specified DateTime but is designated as either local time, UTC, or neither._

```cs
DateTime.specify_kind(value, kind)
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
      <td>value</td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</td>
      <td>A date and time.</td>
    </tr>
    <tr>
      <td>kind</td>
      <td>[DateTimeKind](https://docs.microsoft.com/en-us/dotnet/api/system.datetimekind?view=netframework-4.7)</td>
      <td>Indicates the new kind of the result.</td>
    </tr>
  </tbody>
</table>

**Returns:** [DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)
