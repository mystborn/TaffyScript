---
layout: default
title: regex_match
---

# regex_match

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[regex_match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_match/)

_Searches the input string for the last occurrence of the specified regex._

```cs
regex_match(input, regex, [options], [matchTimeout])
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
      <td>input</td>
      <td>string</td>
      <td>The string to search for a match.</td>
    </tr>
    <tr>
      <td>regex</td>
      <td>string</td>
      <td>The regular expression pattern.</td>
    </tr>
    <tr>
      <td>[options]</td>
      <td>[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)</td>
      <td>The options to use while matching.</td>
    </tr>
    <tr>
      <td>[matchTimeout]</td>
      <td>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</td>
      <td>A timeout interval.</td>
    </tr>
  </tbody>
</table>

**Returns:** [Match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Match)
