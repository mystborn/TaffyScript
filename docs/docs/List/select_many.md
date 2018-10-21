---
layout: default
title: List.select_many
---

# List.select_many

[\[global\]]({{site.baseurl}}/docs/).[List]({{site.baseurl}}/docs/List/).[select_many]({{site.baseurl}}/docs/List/select_many/)

_Projects each element in the sequence to an Enumerable and flattens the resulting sequences into one sequence._

```cs
List.select_many(collection_selector, [result_selector])
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
      <td>collection_selector</td>
      <td>script</td>
      <td>A script that transforms each element in the sequence into an Enumerable.</td>
    </tr>
    <tr>
      <td>[result_selector]</td>
      <td>script</td>
      <td>A script that transforms each element of the intermediate sequences.</td>
    </tr>
  </tbody>
</table>

**Returns:** Enumerable