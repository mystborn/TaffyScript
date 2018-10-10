---
layout: default
title: Regex
---

# Regex

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/).[RegularExpressions]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/).[Regex]({{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/)

_Represents a regular expression._

## Properties

<table>
  <col width="15%">
  <col width="15%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/match_timeout/">match_timeout</a></td>
      <td>numer</td>
      <td>Gets the timeout interval of the regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/options/">options</a></td>
      <td>[RegexOptions](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions)</td>
      <td>Gets the options passed into the Regex constructor.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/pattern/">pattern</a></td>
      <td>string</td>
      <td>Gets the pattern used to create the regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/right_to_left/">right_to_left</a></td>
      <td>bool</td>
      <td>Determines if the regex searches from right to left.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/cache_size/">cache_size</a></td>
      <td>number</td>
      <td>Gets or sets the maximum number of entries in the static cache of compiled regular expressions.</td>
    </tr>
  </tbody>
</table>

## Constructor

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/create/">create(pattern, [options], [matchTimeout])</a></td>
      <td>Initializes a new instance of a Regex.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/escape">escape(str)</a></td>
      <td>Returns a modified string with certain characters replaced with their escape codes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/get_group_names">get_group_names()</a></td>
      <td>Returns an array of capture group names for the regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/get_group_numbers">get_group_numbers()</a></td>
      <td>Returns an array of capture group numbers for the regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/group_name_from_number">group_name_from_number()</a></td>
      <td>Gets the group name that corresponds to the group number.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/group_number_from_name">group_number_from_name()</a></td>
      <td>Gets the group number that corresponds to the group name.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/is_match">is_match(input, [start_at=0])</a></td>
      <td>Determines if the regex finds a match in the specified input string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/match">match(input, [start_at=0], [length])</a></td>
      <td>Searches the input string for the last occurrence of the regex.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/matches">matches(input, [start_at=0])</a></td>
      <td>Searches an input string for all occurrences of the regex and returns a list of the matches.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/replace">replace(input, replacement, [count], [start_at=0])</a></td>
      <td>Replaces all occurrences of the regex in an input string with the specified replacement string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/split">split(input, [count], [start_at=0])</a></td>
      <td>Splits the input string into an array at the positions where the regex matches.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Strings/RegularExpressions/Regex/unescape">unescape(str)</a></td>
      <td>Returns a modified string with all escaped characters replaced with their normal counterparts.</td>
    </tr>
  </tbody>
</table>
