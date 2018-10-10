---
layout: default
title: DateTime.try_parse
---

# DateTime.try_parse

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime/).[try_parse]({{site.baseurl}}/docs/TaffyScript/DateTime/try_parse/)

_Attempts to convert a string representation of a date and time into a DateTime instance. Accepts 1 and 3 arguments._

```cs
DateTime.try_parse(str, [culture], [styles])
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
      <td>The string to try and convert.</td>
    </tr>
    <tr>
      <td>[culture]</td>
      <td>string</td>
      <td>The name of the culture used to convert the string.</td>
    </tr>
    <tr>
      <td>[styles]</td>
      <td>[DateTimeStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimestyles?view=netframework-4.7)</td>
      <td>Indicates the style elements that can be in str and still have the parse succeed.</td>
    </tr>
  </tbody>
</table>

**Returns:** [ParseResult]({{site.baseurl}}/docs/TaffyScript/ParseResult)
