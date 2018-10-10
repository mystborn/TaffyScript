---
layout: default
title: Regex.replace
---

# Regex.replace

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[Regex]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/).[replace]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/replace/)

_Replaces all occurrences of the regex in an input string with the specified replacement string._

```cs
Regex.replace(input, replacement, [count], [start_at=0])
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
      <td>The string to search for matches.</td>
    </tr>
    <tr>
      <td>replacement</td>
      <td>string</td>
      <td>The replacement string.</td>
    </tr>
    <tr>
      <td>[count]</td>
      <td>number</td>
      <td>The maximum number of replacements. Defaults to no limit.</td>
    </tr>
    <tr>
      <td>[start_at=0]</td>
      <td>number</td>
      <td>The position to start the search.</td>
    </tr>
  </tbody>
</table>

**Returns:** string
