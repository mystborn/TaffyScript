---
layout: default
title: print
---

# print

[\[global\]]({{site.baseurl}}/docs/).[print]({{site.baseurl}}/docs/print/)

_Prints a value depending on the arguments to the Standard Output._

```cs
print([value], [..format_args])
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
      <td>[value]</td>
      <td>object</td>
      <td>A value to write to stdout. If this isn't provided, this script just writes a line terminator.</td>
    </tr>
    <tr>
      <td>[..format_args]</td>
      <td>objects</td>
      <td>These arguments will be used to format value.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
