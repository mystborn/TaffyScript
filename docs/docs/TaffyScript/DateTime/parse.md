---
layout: default
title: DateTime.parse
---

# DateTime.parse

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime/).[parse]({{site.baseurl}}/docs/TaffyScript/DateTime/parse/)

_Converts the string representation of a date and time to its DateTime equivalent._

```cs
DateTime.parse(str, [culture], [styles])
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
      <td>str</td>
      <td>string</td>
      <td>The string to try and parse.</td>
    </tr>
    <tr>
      <td>[culture]</td>
      <td>string</td>
      <td>The name of the culture to use when parsing the string. Defaults to the system culture.</td>
    </tr>
    <tr>
      <td>[styles]</td>
      <td>[DateTimeStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimestyles?view=netframework-4.7)</td>
      <td>Indicates the style elements that can be in str and still have the parse succeed.</td>
    </tr>
  </tbody>
</table>

**Returns:** [DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)
