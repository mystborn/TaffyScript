---
layout: default
title: Regex.match
---

# Regex.match

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[Regex]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/).[match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/match/)

_Searches the input string for the last occurrence of the regex._

```cs
Regex.match(input, [start_at=0], [length])
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
      <td>The string to search for a match.</td>
    </tr>
    <tr>
      <td>[start_at=0]</td>
      <td>number</td>
      <td>The position to start the search.</td>
    </tr>
    <tr>
      <td>[length]</td>
      <td>number</td>
      <td>The number of characters to include in the search. Includes all characters by default.</td>
    </tr>
  </tbody>
</table>

**Returns:** [Match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Match)
