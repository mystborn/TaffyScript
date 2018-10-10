---
layout: default
title: Enumerable.order_by
---

# Enumerable.order_by

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Collections]({{site.baseurl}}/docs/TaffyScript/Collections/).[Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable/).[order_by]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable/order_by/)

_Sorts the elements in the sequence based on the key returned by a script, optionally using an IComparer to determine order._

```cs
Enumerable.order_by(key_selector, [comparer])
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
      <td>The script used to extract a key from each element in the sequence.</td>
    </tr>
    <tr>
      <td>[comparer]</td>
      <td>IComparer</td>
      <td>The comparer used to compare keys.</td>
    </tr>
  </tbody>
</table>

**Returns:** Enumerable
