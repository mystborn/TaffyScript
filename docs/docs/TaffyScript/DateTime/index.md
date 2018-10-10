---
layout: default
title: DateTime
---

# DateTime

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime/)

_Represents an instant in time._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/date/">date</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</td>
      <td>Gets the date component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/day/">day</a></td>
      <td>number</td>
      <td>Gets the day of month represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/day_of_week/">day_of_week</a></td>
      <td>[DayOfWeek](https://docs.microsoft.com/en-us/dotnet/api/system.dayofweek?view=netframework-4.7)</td>
      <td>Gets the day of the week represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/day_of_year/">day_of_year</a></td>
      <td>number</td>
      <td>Gets the day of the year represented by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/hour/">hour</a></td>
      <td>number</td>
      <td>Gets the hour component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/kind/">kind</a></td>
      <td>[DateTimeKind](https://docs.microsoft.com/en-us/dotnet/api/system.datetimekind?view=netframework-4.7)</td>
      <td>Determines if this instance is based on local time, UTC, or neither.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/millisecond/">millisecond</a></td>
      <td>number</td>
      <td>Gets the millisecond component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/minute/">minute</a></td>
      <td>number</td>
      <td>Gets the minute component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/month/">month</a></td>
      <td>number</td>
      <td>Gets the month component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/second/">second</a></td>
      <td>number</td>
      <td>Gets the seconds component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/ticks/">ticks</a></td>
      <td>number</td>
      <td>Gets the number of ticks that represent the date and time of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/time_of_day/">time_of_day</a></td>
      <td>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</td>
      <td>Gets the time of day for this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/year/">year</a></td>
      <td>number</td>
      <td>Gets the year component of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/now/">now</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</td>
      <td>Gets a DateTime that is set to the current date and time on this computer, expressed as the local time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/today/">today</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</td>
      <td>Gets the current date.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/utc_now/">utc_now</a></td>
      <td>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</td>
      <td>Gets a DateTime that is set to the current date and time on this computer, expressed as UTC.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/create/">create(year, month, day, hour, minute, second, millisecond, kind)</a></td>
      <td>Initializes a new DateTime with the specified date and time. Accepts 3, 6, 7, or 8 arguments.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add">add(value)</a></td>
      <td>Gets a new DateTime with the specified TimeSpan added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_days">add_days(value)</a></td>
      <td>Returns a new DateTime with the specified number of days added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_hours">add_hours(value)</a></td>
      <td>Returns a new DateTime with the specified number of hours added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_milliseconds">add_milliseconds(value)</a></td>
      <td>Returns a new DateTime with the specified number of milliseconds added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_minutes">add_minutes(value)</a></td>
      <td>Returns a new DateTime with the specified number of minutes added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_months">add_months(value)</a></td>
      <td>Returns a new DateTime with the specified number of months added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_seconds">add_seconds(value)</a></td>
      <td>Returns a new DateTime with the specified number of seconds added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_ticks">add_ticks(value)</a></td>
      <td>Returns a new DateTime with the specified number of ticks added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/add_years">add_years(value)</a></td>
      <td>Returns a new DateTime with the specified number of years added to this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/compare">compare(first, second)</a></td>
      <td>Compares two DateTime instances and returns a number representing the result. -1 if the first is less than the second, 0 if they are equal, 1 if the first is greater than the second.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/days_in_month">days_in_month(year, month)</a></td>
      <td>Gets the number of days in the specified year and month.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/is_daylight_saving_time">is_daylight_saving_time()</a></td>
      <td>Determines if this instance is within the daylight saving time for the current time zone.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/is_leap_year">is_leap_year(year)</a></td>
      <td>Determines if the given year is a leap year.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/parse">parse(str, [culture], [styles])</a></td>
      <td>Converts the string representation of a date and time to its DateTime equivalent.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/specify_kind">specify_kind(value, kind)</a></td>
      <td>Creates a new DateTime that has the same number of ticks as the specified DateTime but is designated as either local time, UTC, or neither.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/subtract">subtract(value)</a></td>
      <td>Subtracts either the specified DateTime or TimeSpan from this instance and returns the result. The result will be the opposite type of the argument type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_local_time">to_local_time()</a></td>
      <td>Converts the value of this DateTime to local time.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_long_date_string">to_long_date_string()</a></td>
      <td>Converts the value of this DateTime to its equivalent long date string representation.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_long_time_string">to_long_time_string()</a></td>
      <td>Converts the value of this DateTime to its equivalent long time string representation.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_short_date_string">to_short_date_string()</a></td>
      <td>Converts the value of this DateTime to its equivalent short date string representation.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_short_time_string">to_short_time_string()</a></td>
      <td>Converts the value of this DateTime to its equivalent short time string representation.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_string">to_string([format], [culture])</a></td>
      <td>Converts the value of this DateTime to its equivalent string representation.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/to_utc">to_utc()</a></td>
      <td>Converts the value of this DateTime to UTC.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/DateTime/try_parse">try_parse(str, [culture], [styles])</a></td>
      <td>Attempts to convert a string representation of a date and time into a DateTime instance. Accepts 1 and 3 arguments.</td>
    </tr>
  </tbody>
</table>
