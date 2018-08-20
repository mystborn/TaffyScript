---
layout: default
title: array_length
---

# array_length

[\[global\]]({{site.baseurl}}/docs/).[array_length]({{site.baseurl}}/docs/array_length/)

Gets the length of an array or one of its nested arrays.

```cs
array_length(array, [..indeces])
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
      <td>array</td>
      <td>array</td>
      <td>The array to get the length of.</td>
    </tr>
    <tr>
      <td>[..indeces]</td>
      <td>numbers</td>
      <td>If the target is a jagged array, these should be the indeces of the inner arrays until the desired array to get the length of.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
