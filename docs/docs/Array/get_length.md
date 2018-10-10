---
layout: default
title: Array.get_length
---

# Array.get_length

[\[global\]]({{site.baseurl}}/docs/).[Array]({{site.baseurl}}/docs/Array/).[get_length]({{site.baseurl}}/docs/Array/get_length/)

_Gets the number of elements in this array or a nested array._

```cs
Array.get_length([..nested_indeces])
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
      <td>[..nested_indeces]</td>
      <td>numbers</td>
      <td>If the target is a jagged array, these should be the indeces of the inner arrays until the desired array to get the length of.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
