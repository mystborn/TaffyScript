---
layout: default
title: string
---

# string

[\[global\]]({{site.baseurl}}/docs/).[string]({{site.baseurl}}/docs/string/)

_The string literal built into TaffyScript._

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
      <td><a href="{{site.baseurl}}/docs/string/length/">length</a></td>
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
      <td><a href="{{site.baseurl}}/docs/string/contains">contains(substring)</a></td>
      <td>Determines if the string contains the specified substring.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/copy">copy([start_index=0], [count])</a></td>
      <td>Returns a copy of a portion of the string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/count">count(substring)</a></td>
      <td>Counts the number of time a substring appears in the string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/delete">delete(start_index, [count])</a></td>
      <td>Removes a portion of the string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/digits">digits()</a></td>
      <td>Returns a copy of the string with all non-digit characters removed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/duplicate">duplicate(count)</a></td>
      <td>Returns a new string with this string repeated the specified number of times.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/ends_with">ends_with(substring)</a></td>
      <td>Determines if this string ends with the specified substring.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/format">format(..args)</a></td>
      <td>Replaces format items with the corresponding arguments. A format item takes the following form: {<argument_number>}</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/get">get(index)</a></td>
      <td>Gets the character at the specified position within the string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/index_of">index_of(substring, [start_index=0])</a></td>
      <td>Gets the index of the first occurrence of the specified substring, or -1 if it wasn't found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/insert">insert(substring, index)</a></td>
      <td>Inserts the specified substring into this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/last_index_of">last_index_of(substring, [start_index=0])</a></td>
      <td>Gets the index of the last occurrence of the specified substring, or -1 if it wasn't found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/letters">letters()</a></td>
      <td>Returns a copy of this string with all non-letter characters removed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/letters_digits">letters_digits()</a></td>
      <td>Returns a copy of this string with all non-letter and non-digit characters removed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/lower">lower()</a></td>
      <td>Returns a copy of this string with all letters converted to lowercase.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/ord">ord(index)</a></td>
      <td>Gets the ordinal value of the character at the specified index.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/replace">replace(substring, replacement)</a></td>
      <td>Replaces the first occurrence of a substring with another string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/replace_all">replace_all(substring, replacement)</a></td>
      <td>Replaces all occurrences of a substring with another string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/starts_with">starts_with(substring)</a></td>
      <td>Determines if this string starts with the specified substring.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/split">split(..seperators, [remove_empty_entries=false])</a></td>
      <td>Based on a set of substrings, splits this string into an array.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/trim">trim([..characters])</a></td>
      <td>Removes all leading and trailing whitespace characters (or the specified characters, if any) from this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/trim_end">trim_end([..characters])</a></td>
      <td>Removes all trailing whitespace characters (or the specified characters, if any) from this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/trim_start">trim_start([..characters])</a></td>
      <td>Removes all leading whitespace characters (or the specified characters, if any) from this string and returns the result.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/string/upper">upper()</a></td>
      <td>Returns a copy of this string with characters converted to uppercase.</td>
    </tr>
  </tbody>
</table>
