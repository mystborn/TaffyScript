---
layout: default
title: Grid.value_exists_in_region
---

# Grid.value_exists_in_region

[\[global\]]({{site.baseurl}}/docs/).[Grid]({{site.baseurl}}/docs/Grid/).[value_exists_in_region]({{site.baseurl}}/docs/Grid/value_exists_in_region/)

Determines if a value exists within a region on the grid.

```cs
Grid.value_exists_in_region(x1, y1, x2, y2, value)
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
      <td>x1</td>
      <td>number</td>
      <td>The left side of the region.</td>
    </tr>
    <tr>
      <td>y1</td>
      <td>number</td>
      <td>The top of the region.</td>
    </tr>
    <tr>
      <td>x2</td>
      <td>number</td>
      <td>The right side of the region.</td>
    </tr>
    <tr>
      <td>y2</td>
      <td>number</td>
      <td>The bottom of the region.</td>
    </tr>
    <tr>
      <td>value</td>
      <td>object</td>
      <td>The value to find.</td>
    </tr>
  </tbody>
</table>

**Returns:** bool
