---
layout: default
title: show_error
---

# show_error

[\[global\]]({{site.baseurl}}/docs/).[show_error]({{site.baseurl}}/docs/show_error/)

_Creates an error with the specified message and either throws it as an exception or writes it to the debug output._

```cs
show_error(message, throws)
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
      <td>message</td>
      <td>string</td>
      <td>The error message.</td>
    </tr>
    <tr>
      <td>throws</td>
      <td>bool</td>
      <td>Determines whether to throw an exception or just print the error.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
