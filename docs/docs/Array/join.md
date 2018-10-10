---
layout: default
title: Array.join
---

# Array.join

[\[global\]]({{site.baseurl}}/docs/).[Array]({{site.baseurl}}/docs/Array/).[join]({{site.baseurl}}/docs/Array/join/)

_Correlates the elements of two sequences based on matching keys._

```cs
Array.join(other, outer_key_selector, inner_key_selector, result_selector, [comparer])
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
      <td>other</td>
      <td>IEnumerable</td>
      <td>The other sequence to join with.</td>
    </tr>
    <tr>
      <td>outer_key_selector</td>
      <td>script</td>
      <td>A script to extract the join key from this sequence.</td>
    </tr>
    <tr>
      <td>inner_key_selector</td>
      <td>script</td>
      <td>A script to extract the join key from the other sequence.</td>
    </tr>
    <tr>
      <td>result_selector</td>
      <td>script</td>
      <td>A script to create a result from two matching elements.</td>
    </tr>
    <tr>
      <td>[comparer]</td>
      <td>EqualityComparer</td>
      <td>A comparer used to hash and compare keys.</td>
    </tr>
  </tbody>
</table>

**Returns:** Enumerable
