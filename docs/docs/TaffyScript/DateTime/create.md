---
layout: default
title: DateTime.create
---

# DateTime Constructor

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime/).[create]({{site.baseurl}}/docs/TaffyScript/DateTime/create/)

_Initializes a new DateTime with the specified date and time. Accepts 3, 6, 7, or 8 arguments._

```cs
new DateTime.create(year, month, day, hour, minute, second, millisecond, kind)
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
      <td>year</td>
      <td>number</td>
      <td>The year (1 - 9999).</td>
    </tr>
    <tr>
      <td>month</td>
      <td>number</td>
      <td>The month (1- 12).</td>
    </tr>
    <tr>
      <td>day</td>
      <td>number</td>
      <td>The day (1 - the number of days in month).</td>
    </tr>
    <tr>
      <td>hour</td>
      <td>number</td>
      <td>The hour (0 - 23).</td>
    </tr>
    <tr>
      <td>minute</td>
      <td>number</td>
      <td>The minute (0 - 59).</td>
    </tr>
    <tr>
      <td>second</td>
      <td>number</td>
      <td>The seconds (0 - 59).</td>
    </tr>
    <tr>
      <td>millisecond</td>
      <td>number</td>
      <td>The milliseconds (0 - 999).</td>
    </tr>
    <tr>
      <td>kind</td>
      <td>[DateTimeKind](https://docs.microsoft.com/en-us/dotnet/api/system.datetimekind?view=netframework-4.7)</td>
      <td>Determines if this is in local time or UTC.</td>
    </tr>
  </tbody>
</table>
