---
layout: default
title: regex_is_match
---

# regex_is_match

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[regex_is_match]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_is_match/)

_Determines if the regex finds a match in the specified input string._

```cs
regex_is_match(input, regex, [options], [matchTimeout])
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

**Returns:** bool
