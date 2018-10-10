---
layout: default
title: random
---

# random

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Numbers]({{site.baseurl}}/docs/TaffyScript/Numbers/).[random]({{site.baseurl}}/docs/TaffyScript/Numbers/random/)

_Gets a random number. If there are no arguments, returns a number between 0 and 1. If there is one argument, returns a number between 0 and argument0. If there are two arguments, returns a number between argument0 and argument1._

```cs
random([max_or_min], [max])
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
      <td>[max_or_min]</td>
      <td>number</td>
      <td>If there are two arguments, this is the inclusive minimum possible result. Otherwise, this is the exclusive maximum possible result.</td>
    </tr>
    <tr>
      <td>[max]</td>
      <td>number</td>
      <td>The exclusive maximum result.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
