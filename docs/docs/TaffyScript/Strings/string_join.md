---
layout: default
title: string_join
---

# string_join

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[string_join]({{site.baseurl}}/docs/TaffyScript/Strings/string_join/)

_Converts each argument into a string and concatenates them together with the specified seperator._

```cs
string_join(seperator, ..args)
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
      <td>seperator</td>
      <td>string</td>
      <td>The string to put in between each argument.</td>
    </tr>
    <tr>
      <td>..args</td>
      <td>objects</td>
      <td>Any number of arguments to convert to a string.</td>
    </tr>
  </tbody>
</table>

**Returns:** string
