---
layout: default
title: Enumerable.join
---

# Enumerable.join

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Collections]({{site.baseurl}}/docs/TaffyScript/Collections/).[Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable/).[join]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable/join/)

_Correlates the elements of two sequences based on matching keys._

```cs
Enumerable.join(other, outer_key_selector, inner_key_selector, result_selector, [comparer])
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
