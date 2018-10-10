---
layout: default
title: string.copy
---

# string.copy

[\[global\]]({{site.baseurl}}/docs/).[string]({{site.baseurl}}/docs/string/).[copy]({{site.baseurl}}/docs/string/copy/)

_Returns a copy of a portion of the string._

```cs
string.copy([start_index=0], [count])
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
      <td>[start_index=0]</td>
      <td>number</td>
      <td>The index to start copying characters from.</td>
    </tr>
    <tr>
      <td>[count]</td>
      <td>number</td>
      <td>The number of characters to copy. If absent, copies characters from the start index to the end of the string.</td>
    </tr>
  </tbody>
</table>

**Returns:** string
