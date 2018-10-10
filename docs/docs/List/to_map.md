---
layout: default
title: List.to_map
---

# List.to_map

[\[global\]]({{site.baseurl}}/docs/).[List]({{site.baseurl}}/docs/List/).[to_map]({{site.baseurl}}/docs/List/to_map/)

_Converts the sequence to a map._

```cs
List.to_map(key_selector, [value_selector], [comparer])
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
      <td>key_selector</td>
      <td>script</td>
      <td>A script to extract a key from each element.</td>
    </tr>
    <tr>
      <td>[value_selector]</td>
      <td>script</td>
      <td>A script to extract a value from each element. If absent, the value will be the full element.</td>
    </tr>
    <tr>
      <td>[comparer]</td>
      <td>EqualityComparer</td>
      <td>A comparer used to compare key values.</td>
    </tr>
  </tbody>
</table>

**Returns:** [Map]({{site.baseurl}}/docs/Map)
