---
layout: default
title: TimeSpan.try_parse
---

# TimeSpan.try_parse

[\[global\]]({{site.baseurl}}/docs/).[TimeSpan]({{site.baseurl}}/docs/TimeSpan/).[try_parse]({{site.baseurl}}/docs/TimeSpan/try_parse/)

_Attempts to convert the string representation of a time interval to its TimeSpan equivalent._

```cs
TimeSpan.try_parse(str, [culture])
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
      <td>The string to convert.</td>
    </tr>
    <tr>
      <td>[culture]</td>
      <td>string</td>
      <td>The name of the culture used to parse the string.</td>
    </tr>
  </tbody>
</table>

**Returns:** [ParseResult]({{site.baseurl}}/docs/TaffyScript/ParseResult)
