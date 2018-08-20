---
layout: default
title: string_split
---

# string_split

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[string_split]({{site.baseurl}}/docs/TaffyScript/Strings/string_split/)

Based on a set of sub-strings, splits a string into an array.

```cs
string_split(str, ..seperators, [remove_empty_entries = false])
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
      <td>str</td>
      <td>string</td>
      <td>The string to split.</td>
    </tr>
    <tr>
      <td>..seperators</td>
      <td>strings</td>
      <td>Any number of strings that will be used to determine split borders.</td>
    </tr>
    <tr>
      <td>[remove_empty_entries = false]</td>
      <td>bool</td>
      <td>Optional value to determine if any empty strings should be removed from the resultant array.</td>
    </tr>
  </tbody>
</table>

**Returns:** array
