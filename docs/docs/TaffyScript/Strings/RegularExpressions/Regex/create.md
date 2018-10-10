---
layout: default
title: Regex.create
---

# Regex Constructor

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[Regex]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/).[create]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/create/)

_Initializes a new instance of a Regex._

```cs
new Regex.create(pattern, [options], [matchTimeout])
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
      <td>pattern</td>
      <td>string</td>
      <td>The regular expression pattern.</td>
    </tr>
    <tr>
      <td>[options]</td>
      <td>[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)</td>
      <td>A combination of options that modify the regex.</td>
    </tr>
    <tr>
      <td>[matchTimeout]</td>
      <td>[TimeSpan]({{site.baseurl}}/docs/TaffyScript.TimeSpan)</td>
      <td>A timeout interval.</td>
    </tr>
  </tbody>
</table>
