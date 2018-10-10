---
layout: default
title: TimeSpan
---

# TimeSpan

[\[global\]]({{site.baseurl}}/docs/).[TimeSpan]({{site.baseurl}}/docs/TimeSpan/)

_Represents a time interval._

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
      <td><a href="{{site.baseurl}}/docs/TimeSpan/days/">days</a></td>
      <td>number</td>
      <td>Gets the days component of the time interval represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/hours/">hours</a></td>
      <td>number</td>
      <td>Gets the hours component of the time interval represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/milliseconds/">milliseconds</a></td>
      <td>number</td>
      <td>Gets the milliseconds component of the time interval represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/minutes/">minutes</a></td>
      <td>number</td>
      <td>Gets the minutes component of the time interval represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/seconds/">seconds</a></td>
      <td>number</td>
      <td>Gets the seconds component of the time interval represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/ticks/">ticks</a></td>
      <td>number</td>
      <td>Gets the number of ticks that represent the value of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/total_days/">total_days</a></td>
      <td>number</td>
      <td>Gets the value of this TimeSpan expressed as whole and fractional days.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/total_hours/">total_hours</a></td>
      <td>number</td>
      <td>Gets the value of this TimeSpan expressed as whole and fractional hours.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/total_milliseconds/">total_milliseconds</a></td>
      <td>number</td>
      <td>Gets the value of this TimeSpan expressed as whole and fractional milliseconds.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/total_minutes/">total_minutes</a></td>
      <td>number</td>
      <td>Gets the value of this TimeSpan expressed as whole and fractional minutes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/total_seconds/">total_seconds</a></td>
      <td>number</td>
      <td>Gets the value of this TimeSpan expressed as whole and fractional seconds.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/max_value/">max_value</a></td>
      <td>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</td>
      <td>Gets the maximum TimeSpan value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/min_value/">min_value</a></td>
      <td>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</td>
      <td>Gets the minimum Timespan value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/ticks_per_day/">ticks_per_day</a></td>
      <td>number</td>
      <td>Gets the number of ticks in one day.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/ticks_per_hour/">ticks_per_hour</a></td>
      <td>number</td>
      <td>Gets the number of ticks in one hour.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/ticks_per_millisecond/">ticks_per_millisecond</a></td>
      <td>number</td>
      <td>Gets the number of ticks in one millisecond.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/ticks_per_minute/">ticks_per_minute</a></td>
      <td>number</td>
      <td>Gets the number of ticks in one minute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/ticks_per_second/">ticks_per_second</a></td>
      <td>number</td>
      <td>Gets the number of ticks in one second.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/zero/">zero</a></td>
      <td>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</td>
      <td>Gets a TimeSpan that represents zero.</td>
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
      <td><a href="{{site.baseurl}}/docs/TimeSpan/add">add(value)</a></td>
      <td>Creates a new TimeSpan by adding this instance and another TimeSpan together.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/compare">compare(first, second)</a></td>
      <td>Compares two TimeSpan instances and returns a number representing the result. -1 if the first is less than the second, 0 if they are equal, 1 if the first is greater than the second.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/duration">duration()</a></td>
      <td>Gets a new TimeSpan who is value is the absolute value of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/from_days">from_days(value)</a></td>
      <td>Returns a TimeSpan that represents a specified number of days.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/from_hours">from_hours(value)</a></td>
      <td>Returns a TimeSpan that represents a specified number of hours.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/from_milliseconds">from_milliseconds(value)</a></td>
      <td>Returns a TimeSpan that represents a specified number of milliseconds.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/from_minutes">from_minutes(value)</a></td>
      <td>Returns a TimeSpan that represents a specified number of minutes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/from_seconds">from_seconds(value)</a></td>
      <td>Returns a TimeSpan that represents a specified number of seconds.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/from_ticks">from_ticks(value)</a></td>
      <td>Returns a TimeSpan that represents a specified number of ticks.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/negate">negate()</a></td>
      <td>Returns a new TimeSpan whose value is the negated value of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/parse">parse(str, [culture])</a></td>
      <td>Converts the string representation of a time interval to its TimeSpan equivalent.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/subtract">subtract(value)</a></td>
      <td>Creates a new TimeSpan by subtracting this instance and another TimeSpan.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/to_string">to_string([format], [culture])</a></td>
      <td>Converts this instance to its equivalent string representation.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TimeSpan/try_parse">try_parse(str, [culture])</a></td>
      <td>Attempts to convert the string representation of a time interval to its TimeSpan equivalent.</td>
    </tr>
  </tbody>
</table>
