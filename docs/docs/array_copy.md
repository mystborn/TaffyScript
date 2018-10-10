---
layout: default
title: array_copy
---

# array_copy

[\[global\]]({{site.baseurl}}/docs/).[array_copy]({{site.baseurl}}/docs/array_copy/)

_Copies a range of elements from one array to another._

```cs
array_copy(source_array, source_index, dest_array, dest_index, count)
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
      <td>source_array</td>
      <td>array</td>
      <td>The array to copy from.</td>
    </tr>
    <tr>
      <td>source_index</td>
      <td>number</td>
      <td>The index of the source array to start copying from.</td>
    </tr>
    <tr>
      <td>dest_array</td>
      <td>array</td>
      <td>The array to write to.</td>
    </tr>
    <tr>
      <td>dest_index</td>
      <td>number</td>
      <td>The index of the destination array to start writing to.</td>
    </tr>
    <tr>
      <td>count</td>
      <td>number</td>
      <td>The number of elements to copy.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
