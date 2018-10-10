---
layout: default
title: Regex.split
---

# Regex.split

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[Regex]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/).[split]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/split/)

_Splits the input string into an array at the positions where the regex matches._

```cs
Regex.split(input, [count], [start_at=0])
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
      <td>input</td>
      <td>string</td>
      <td>The string to split.</td>
    </tr>
    <tr>
      <td>[count]</td>
      <td>number</td>
      <td>The maximum number of splits. Defaults to no limit.</td>
    </tr>
    <tr>
      <td>[start_at=0]</td>
      <td>number</td>
      <td>The position to start the search.</td>
    </tr>
  </tbody>
</table>

**Returns:** array
