---
layout: default
title: Array.set
---

# Array.set

[\[global\]]({{site.baseurl}}/docs/).[Array]({{site.baseurl}}/docs/Array/).[set]({{site.baseurl}}/docs/Array/set/)

_Sets an index in the array or a nested array to the specified value._

```cs
Array.set(..indeces, number)
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
      <td>..indeces</td>
      <td>numbers</td>
      <td>The index of the value to set. If there is more than one number, these should be the indeces of the inner arrays until the desired array to set.</td>
    </tr>
    <tr>
      <td>number</td>
      <td>object</td>
      <td>The value to set the index to.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
