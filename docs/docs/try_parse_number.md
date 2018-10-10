---
layout: default
title: try_parse_number
---

# try_parse_number

[\[global\]]({{site.baseurl}}/docs/).[try_parse_number]({{site.baseurl}}/docs/try_parse_number/)

_Attempts to convert a string into a number._

```cs
try_parse_number(str, [styles], [culture])
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
      <td>[styles]</td>
      <td>[NumberStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles?view=netframework-4.7)</td>
      <td>Indicates the permitted format of str.</td>
    </tr>
    <tr>
      <td>[culture]</td>
      <td>string</td>
      <td>The culture used to convert the string.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
