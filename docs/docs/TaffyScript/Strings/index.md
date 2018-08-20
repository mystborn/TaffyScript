---
layout: default
title: Strings
---

# TaffyScript.Strings

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Strings]({{site.baseurl}}/docs/TaffyScript/Strings/)

Contains scripts for processing and manipulating strings. Many of the scripts found here are imperative versions of the scripts found on the string object.

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
      <td><a href="{{page.url}}base64_decode">base64_decode(str)</a></td>
      <td>Decodes a 64 bit encoded string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}base64_encode">base64_encode(str)</a></td>
      <td>Converts a string into a base64 format encoded string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_char_at">string_char_at(str, index)</a></td>
      <td>Gets the character at a specific location within a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_copy">string_copy(str, start, count)</a></td>
      <td>Returns a copy of a portion of a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_count">string_count(source, sub_string)</a></td>
      <td>Counts the number of a string within another string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_delete">string_delete(str, start, count)</a></td>
      <td>Removes a portion of characters from a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_digits">string_digits(str)</a></td>
      <td>Removes all non-digit characters from a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_insert">string_insert(source, sub_string, index)</a></td>
      <td>Inserts a string into another string at the specified location.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_join">string_join(seperator, ..args)</a></td>
      <td>Converts each argument into a string and concatenates them together with the specified seperator.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_length">string_length(str)</a></td>
      <td>Gets the length of a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_letters">string_letters(str)</a></td>
      <td>Removes all non-letter characters from a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_letters_digits">string_letters_digits(str)</a></td>
      <td>Removes all non-letter and non-digit characters from a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_lower">string_lower(str)</a></td>
      <td>Converts all letters in a string to lower case.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_ord_at">string_ord_at(str, index)</a></td>
      <td>Gets the ordinal value of a character within a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_pos">string_pos(source, sub_string, [start = 0])</a></td>
      <td>Searches a string for another string and returns the index of the result, or -1 if it wasn't found.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_repeat">string_repeat(str, count)</a></td>
      <td>Repeats a string the specified number of times.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_replace">string_replace(source, replace, replacement)</a></td>
      <td>Replaces the first occurrence of one string with another within a source string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_replace_all">string_replace_all(source, replace, replacement)</a></td>
      <td>Replaces all occurrences of one string with another within a source string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_split">string_split(str, ..seperators, [remove_empty_entries = false])</a></td>
      <td>Based on a set of sub-strings, splits a string into an array.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}string_upper">string_upper(str)</a></td>
      <td>Converts all letters in a string to upper case.</td>
    </tr>
  </tbody>
</table>
