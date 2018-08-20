---
layout: default
title: string
---

# string

[\[global\]]({{site.baseurl}}/docs/).[string]({{site.baseurl}}/docs/string/)

_The string literal built into TaffyScript. This type cannot be constructed using the `new` keyword._

## Fields

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
      <td><a href="{{page.url}}length/">length</a></td>
      <td>number</td>
      <td>Gets the number of characters in the string.</td>
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
      <td><a href="{{page.url}}contains">contains(substring)</a></td>
      <td>Determines if the string contains a substring.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}copy">copy([start_index], [count])</a></td>
      <td>Returns a copy of a portion of the string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}count">count(substring)</a></td>
      <td>Gets the number of occurrences of a substring in the string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}delete">delete(start_index, count)</a></td>
      <td>Removes a portion of the string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}digits">digits()</a></td>
      <td>Returns a copy of this string with all non-digit characters removed.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}duplicate">duplicate(count)</a></td>
      <td>Returns a string with this string repeated the specified number of times.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}ends_with">ends_with(value)</a></td>
      <td>Determines if this string ends with the specified string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}get">get(index)</a></td>
      <td>Gets the character at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}index_of">index_of(substring, [start_index = 0])</a></td>
      <td>Searches for the index of the first occurrence of a string starting at the specified index. Returns -1 if the substring wasn't found.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}insert">insert(index, substring)</a></td>
      <td>Inserts the specified substring into this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}last_index_of">last_index_of(substring, [start_index])</a></td>
      <td>Searches for the index of the last occurrence of a string starting at the specified index. Returns -1 if the substring wasn't found.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}letters">letters()</a></td>
      <td>Returns a copy of this string with all non-letter characters removed.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}letters_digits">letters_digits()</a></td>
      <td>Returns a copy of this string with all non-letter and non-digit characters removed.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}lower">lower()</a></td>
      <td>Returns a copy of this string with all letters converted to lowercase.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}ord">ord(index)</a></td>
      <td>Gets the ordinal value of the character at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}replace">replace(substring, new_string)</a></td>
      <td>Replaces the first occurrence of a substring with another string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}repalce_all">repalce_all(substring, new_string)</a></td>
      <td>Replaces all occurrences of a substring with another string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}starts_with">starts_with(value)</a></td>
      <td>Determines if this string starts with the specified string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}split">split(..seperators, [remove_empty_entries = false])</a></td>
      <td>Based on a set of substrings, splits this string into an array.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}trim">trim([..charcters])</a></td>
      <td>Removes all leading and trailing whitespace characters (or the specified characters, if any) from this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}trim_end">trim_end([..characters])</a></td>
      <td>Removes all trailing whitespace characters (or the specified characters, if any) from this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}trim_start">trim_start([..characters])</a></td>
      <td>Removes all leading whitespace characters (or the specified characters, if any) from this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}upper">upper()</a></td>
      <td>Returns a copy of this string with characters converted to uppercase.</td>
    </tr>
  </tbody>
</table>
