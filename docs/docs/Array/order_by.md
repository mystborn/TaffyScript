---
layout: default
title: Array.order_by
---

# Array.order_by

[\[global\]]({{site.baseurl}}/docs/).[Array]({{site.baseurl}}/docs/Array/).[order_by]({{site.baseurl}}/docs/Array/order_by/)

_Sorts the elements in the sequence based on the key returned by a script, optionally using an IComparer to determine order._

```cs
Array.order_by(key_selector, [comparer])
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
