---
layout: default
title: string.last_index_of
---

# string.last_index_of

[\[global\]]({{site.baseurl}}/docs/).[string]({{site.baseurl}}/docs/string/).[last_index_of]({{site.baseurl}}/docs/string/last_index_of/)

Searches for the index of the last occurrence of a string starting at the specified index. Returns -1 if the substring wasn't found.

```cs
string.last_index_of(substring, [start_index])
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
      <td>substring</td>
      <td>string</td>
      <td>The string to search for.</td>
    </tr>
    <tr>
      <td>[start_index]</td>
      <td>number</td>
      <td>The index to start searching from. Defaults to the last character in this string.</td>
    </tr>
  </tbody>
</table>

**Returns:** number
