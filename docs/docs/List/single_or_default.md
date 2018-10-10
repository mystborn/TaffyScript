---
layout: default
title: List.single_or_default
---

# List.single_or_default

[\[global\]]({{site.baseurl}}/docs/).[List]({{site.baseurl}}/docs/List/).[single_or_default]({{site.baseurl}}/docs/List/single_or_default/)

_Returns the only element in the sequence or null if there are nor elements. Throws an exception if there is more than one element. If a script is given, returns the only element that satisfies the condition._

```cs
List.single_or_default([condition])
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
      <td>[condition]</td>
      <td>script</td>
      <td>A script that tests each element for a condition.</td>
    </tr>
  </tbody>
</table>

**Returns:** object
