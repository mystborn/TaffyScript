---
layout: default
title: StringBuilder.append
---

# StringBuilder.append

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/).[append]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder/append/)

_Appends a string or substring to this instance._

```cs
StringBuilder.append(value, [start_index=0], [count])
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
      <td>value</td>
      <td>string</td>
      <td>The string to append.</td>
    </tr>
    <tr>
      <td>[start_index=0]</td>
      <td>number</td>
      <td>The index to start copying characters from the string to this instance. If given, count must also be supplied.</td>
    </tr>
    <tr>
      <td>[count]</td>
      <td>number</td>
      <td>The number of characters to copy from the string to this instance. If absent, copies the full string.</td>
    </tr>
  </tbody>
</table>

**Returns:** [StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder)
