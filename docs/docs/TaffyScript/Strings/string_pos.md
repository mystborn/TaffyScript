---
layout: default
title: string_pos
---

# string_pos

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[string_pos]({{site.baseurl}}/docs/TaffyScript/Strings/string_pos/)

Searches a string for another string and returns the index of the result, or -1 if it wasn't found.

```cs
string_pos(source, sub_string, [start = 0])
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
      <td>source</td>
      <td>string</td>
      <td>The source string.</td>
    </tr>
    <tr>
      <td>sub_string</td>
      <td>string</td>
      <td>The sub string to find within the source.</td>
    </tr>
    <tr>
      <td>[start = 0]</td>
      <td>number</td>
      <td>The position of the string to start searching.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
