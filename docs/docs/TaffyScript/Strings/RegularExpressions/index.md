---
layout: default
title: TaffyScript.Strings.RegularExpressions
---

# TaffyScript.Strings.RegularExpressions

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/)

Contains objects and scripts that provide access to the .NET regular expression engine.

## Objects

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Capture">Capture</a></td>
      <td>Represents the result of a successfully captured subexpression from a Regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Group">Group</a></td>
      <td>Represents the result of a single capturing group from a Regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Match">Match</a></td>
      <td>Represents the results from a single Regex match.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex">Regex</a></td>
      <td>Represents a regular expression.</td>
    </tr>
  </tbody>
</table>

## Scripts

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Signature</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_is_match">regex_is_match(input, regex, [options], [matchTimeout])</a></td>
      <td>Determines if the regex finds a match in the specified input string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_match">regex_match(input, regex, [options], [matchTimeout])</a></td>
      <td>Searches the input string for the last occurrence of the specified regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_matches">regex_matches(input, regex, [options], [matchTimeout])</a></td>
      <td>Searches the input string for all occurrences of the specified regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_replace">regex_replace(input, regex, replacement, [options], [matchTimeout])</a></td>
      <td>Replaces all instances of a regex in an input string with the specified replacement string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/regex_split">regex_split(input, regex, [options], [matchTimeout])</a></td>
      <td>Splits a string into an array at the positions defined by a regex.</td>
    </tr>
  </tbody>
</table>
