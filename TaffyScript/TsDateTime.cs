using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents an instant in time.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=netframework-4.7</source>
    /// <property name="date" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime" access="get">
    ///     <summary>Gets the date component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.date?view=netframework-4.7</source>
    /// </property>
    /// <property name="day" type="number" access="get">
    ///     <summary>Gets the day of month represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.day?view=netframework-4.7</source>
    /// </property>
    /// <property name="day_of_week" type="[DayOfWeek](https://docs.microsoft.com/en-us/dotnet/api/system.dayofweek?view=netframework-4.7)" access="get">
    ///     <summary>Gets the day of the week represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.dayofweek?view=netframework-4.7</source>
    /// </property>
    /// <property name="day_of_year" type="number" access="get">
    ///     <summary>Gets the day of the year represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.dayofyear?view=netframework-4.7</source>
    /// </property>
    /// <property name="hour" type="number" access="get">
    ///     <summary>Gets the hour component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.hour?view=netframework-4.7</source>
    /// </property>
    /// <property name="kind" type="[DateTimeKind](https://docs.microsoft.com/en-us/dotnet/api/system.datetimekind?view=netframework-4.7)" access="get">
    ///     <summary>Determines if this instance is based on local time, UTC, or neither.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.kind?view=netframework-4.7</source>
    /// </property>
    /// <property name="millisecond" type="number" access="get">
    ///     <summary>Gets the millisecond component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.millisecond?view=netframework-4.7</source>
    /// </property>
    /// <property name="minute" type="number" access="get">
    ///     <summary>Gets the minute component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.minute?view=netframework-4.7</source>
    /// </property>
    /// <property name="month" type="number" access="get">
    ///     <summary>Gets the month component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.month?view=netframework-4.7</source>
    /// </property>
    /// <property name="second" type="number" access="get">
    ///     <summary>Gets the seconds component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.second?view=netframework-4.7</source>
    /// </property>
    /// <property name="ticks" type="number" access="get">
    ///     <summary>Gets the number of ticks that represent the date and time of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=netframework-4.7</source>
    /// </property>
    /// <property name="time_of_day" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)" access="get">
    ///     <summary>Gets the time of day for this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.timeofday?view=netframework-4.7</source>
    /// </property>
    /// <property name="year" type="number" access="get">
    ///     <summary>Gets the year component of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.year?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject("TaffyScript.DateTime")]
    public class TsDateTime : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.DateTime";
        public DateTime Source { get; }

        /// <summary>
        /// Gets a DateTime that is set to the current date and time on this computer, expressed as the local time.
        /// </summary>
        /// <type>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</type>
        public static TsObject now => new TsDateTime(DateTime.Now);

        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <type>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</type>
        public static TsObject today => new TsDateTime(DateTime.Today);

        /// <summary>
        /// Gets a DateTime that is set to the current date and time on this computer, expressed as UTC.
        /// </summary>
        /// <type>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime</type>
        public static TsObject utc_now => new TsDateTime(DateTime.UtcNow);

        public TsDateTime(DateTime dateTime)
        {
            Source = dateTime;
        }

        /// <summary>
        /// Initializes a new DateTime with the specified date and time. Accepts 3, 6, 7, or 8 arguments.
        /// </summary>
        /// <arg name="year" type="number">The year (1 - 9999).</arg>
        /// <arg name="month" type="number">The month (1- 12).</arg>
        /// <arg name="day" type="number">The day (1 - the number of days in month).</arg>
        /// <arg name="hour" type="number">The hour (0 - 23).</arg>
        /// <arg name="minute" type="number">The minute (0 - 59).</arg>
        /// <arg name="second" type="number">The seconds (0 - 59).</arg>
        /// <arg name="millisecond" type="number">The milliseconds (0 - 999).</arg>
        /// <arg name="kind" type="[DateTimeKind](https://docs.microsoft.com/en-us/dotnet/api/system.datetimekind?view=netframework-4.7)">Determines if this is in local time or UTC.</arg>
        public TsDateTime(TsObject[] args)
        {
            switch (args.Length)
            {
                case 3:
                    Source = new DateTime((int)args[0], (int)args[1], (int)args[2]);
                    break;
                case 6:
                    Source = new DateTime((int)args[0], (int)args[1], (int)args[2], (int)args[3], (int)args[4], (int)args[5]);
                    break;
                case 7:
                    Source = new DateTime((int)args[0], (int)args[1], (int)args[2], (int)args[3], (int)args[4], (int)args[5], (int)args[6]);
                    break;
                case 8:
                    Source = new DateTime((int)args[0], (int)args[1], (int)args[2], (int)args[3], (int)args[4], (int)args[5], (int)args[6], (DateTimeKind)(int)args[7]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to constructor of {ObjectType}");
            }
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "add":
                    return add(args);
                case "add_days":
                    return add_days(args);
                case "add_hours":
                    return add_hours(args);
                case "add_milliseconds":
                    return add_milliseconds(args);
                case "add_minutes":
                    return add_minutes(args);
                case "add_months":
                    return add_months(args);
                case "add_seconds":
                    return add_seconds(args);
                case "add_ticks":
                    return add_ticks(args);
                case "add_years":
                    return add_years(args);
                case "is_daylight_saving_time":
                    return is_daylight_saving_time(args);
                case "subtract":
                    return subtract(args);
                case "to_local_time":
                    return to_local_time(args);
                case "to_long_date_string":
                    return to_long_date_string(args);
                case "to_long_time_string":
                    return to_long_time_string(args);
                case "to_short_date_string":
                    return to_short_date_string(args);
                case "to_short_time_string":
                    return to_short_time_string(args);
                case "to_string":
                    return to_string(args);
                case "to_utc":
                    return to_utc(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "date":
                    return new TsDateTime(Source.Date);
                case "day":
                    return Source.Day;
                case "day_of_week":
                    return (float)Source.DayOfWeek;
                case "day_of_year":
                    return Source.DayOfYear;
                case "hour":
                    return Source.Hour;
                case "kind":
                    return (float)Source.Kind;
                case "millisecond":
                    return Source.Millisecond;
                case "minute":
                    return Source.Minute;
                case "month":
                    return Source.Month;
                case "second":
                    return Source.Second;
                case "ticks":
                    return Source.Ticks;
                case "time_of_day":
                    return new TsTimeSpan(Source.TimeOfDay);
                case "year":
                    return Source.Year;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "add":
                    del = new TsDelegate(add, scriptName);
                    break;
                case "add_days":
                    del = new TsDelegate(add_days, scriptName);
                    break;
                case "add_hours":
                    del = new TsDelegate(add_hours, scriptName);
                    break;
                case "add_milliseconds":
                    del = new TsDelegate(add_milliseconds, scriptName);
                    break;
                case "add_minutes":
                    del = new TsDelegate(add_minutes, scriptName);
                    break;
                case "add_months":
                    del = new TsDelegate(add_months, scriptName);
                    break;
                case "add_seconds":
                    del = new TsDelegate(add_seconds, scriptName);
                    break;
                case "add_ticks":
                    del = new TsDelegate(add_ticks, scriptName);
                    break;
                case "add_years":
                    del = new TsDelegate(add_years, scriptName);
                    break;
                case "is_daylight_saving_time":
                    del = new TsDelegate(is_daylight_saving_time, scriptName);
                    break;
                case "subtract":
                    del = new TsDelegate(subtract, scriptName);
                    break;
                case "to_local_time":
                    del = new TsDelegate(to_local_time, scriptName);
                    break;
                case "to_long_date_string":
                    del = new TsDelegate(to_long_date_string, scriptName);
                    break;
                case "to_long_time_string":
                    del = new TsDelegate(to_long_time_string, scriptName);
                    break;
                case "to_short_date_string":
                    del = new TsDelegate(to_short_date_string, scriptName);
                    break;
                case "to_short_time_string":
                    del = new TsDelegate(to_short_time_string, scriptName);
                    break;
                case "to_string":
                    del = new TsDelegate(to_string, scriptName);
                    break;
                case "to_utc":
                    del = new TsDelegate(to_utc, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a new DateTime with the specified TimeSpan added to this instance.
        /// </summary>
        /// <arg name="value" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">The time interval.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.add?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add(TsObject[] args)
        {
            return new TsDateTime(Source.Add(((TsTimeSpan)args[0]).Source));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of days added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of days. Can be fractional and positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.adddays?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_days(TsObject[] args)
        {
            return new TsDateTime(Source.AddDays((double)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of hours added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of hours. Can be fractional, and positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addhours?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_hours(TsObject[] args)
        {
            return new TsDateTime(Source.AddHours((double)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of milliseconds added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of milliseconds. Can be positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addmilliseconds?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_milliseconds(TsObject[] args)
        {
            return new TsDateTime(Source.AddMilliseconds((double)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of minutes added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of minutes. Can be fractional, and positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addminutes?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_minutes(TsObject[] args)
        {
            return new TsDateTime(Source.AddMinutes((double)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of months added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of months. Can be fractional, and positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addmonths?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_months(TsObject[] args)
        {
            return new TsDateTime(Source.AddMonths((int)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of seconds added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of seconds. Can be fractional, and positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addseconds?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_seconds(TsObject[] args)
        {
            return new TsDateTime(Source.AddSeconds((double)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of ticks added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of hours. Can be positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addticks?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_ticks(TsObject[] args)
        {
            return new TsDateTime(Source.AddTicks((long)args[0]));
        }

        /// <summary>
        /// Returns a new DateTime with the specified number of years added to this instance.
        /// </summary>
        /// <arg name="value" type="number">The number of years. Can be positive or negative.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.addyears?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject add_years(TsObject[] args)
        {
            return new TsDateTime(Source.AddYears((int)args[0]));
        }

        /// <summary>
        /// Compares two DateTime instances and returns a number representing the result. -1 if the first is less than the second, 0 if they are equal, 1 if the first is greater than the second.
        /// </summary>
        /// <arg name="first" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime">The first DateTime to compare.</arg>
        /// <arg name="second" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime">The second DateTime to compare.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.compare?view=netframework-4.7</source>
        /// <returns>number</returns>
        public static TsObject compare(TsObject[] args)
        {
            return TimeSpan.Compare(((TsTimeSpan)args[0]).Source, ((TsTimeSpan)args[1]).Source);
        }

        /// <summary>
        /// Gets the number of days in the specified year and month.
        /// </summary>
        /// <arg name="year" type="number">The year.</arg>
        /// <arg name="month" type="number">The month to get the number of days of.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.daysinmonth?view=netframework-4.7</source>
        /// <returns>number</returns>
        public static TsObject days_in_month(TsObject[] args)
        {
            return DateTime.DaysInMonth((int)args[0], (int)args[1]);
        }

        /// <summary>
        /// Determines if this instance is within the daylight saving time for the current time zone. 
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.isdaylightsavingtime?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject is_daylight_saving_time(TsObject[] args)
        {
            return Source.IsDaylightSavingTime();
        }

        /// <summary>
        /// Determines if the given year is a leap year.
        /// </summary>
        /// <arg name="year" type="number">The year to check.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.isleapyear?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject is_leap_year(TsObject[] args)
        {
            return DateTime.IsLeapYear((int)args[0]);
        }

        /// <summary>
        /// Converts the string representation of a date and time to its DateTime equivalent.
        /// </summary>
        /// <arg name="str" type="string">The string to try and parse.</arg>
        /// <arg name="[culture]" type="string">The name of the culture to use when parsing the string. Defaults to the system culture.</arg>
        /// <arg name="[styles]" type="[DateTimeStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimestyles?view=netframework-4.7)">Indicates the style elements that can be in str and still have the parse succeed.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.parse?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject parse(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new TsDateTime(DateTime.Parse((string)args[0]));
                case 2:
                    return new TsDateTime(DateTime.Parse((string)args[0], CultureInfo.CreateSpecificCulture((string)args[1])));
                case 3:
                    return new TsDateTime(DateTime.Parse((string)args[0], CultureInfo.CreateSpecificCulture((string)args[1]), (DateTimeStyles)(int)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TsDateTime.{nameof(parse)}");
            }
        }

        /// <summary>
        /// Creates a new DateTime that has the same number of ticks as the specified DateTime but is designated as either local time, UTC, or neither.
        /// </summary>
        /// <arg name="value" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">A date and time.</arg>
        /// <arg name="kind" type="[DateTimeKind](https://docs.microsoft.com/en-us/dotnet/api/system.datetimekind?view=netframework-4.7)">Indicates the new kind of the result.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.specifykind?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject specify_kind(TsObject[] args)
        {
            return new TsDateTime(DateTime.SpecifyKind(((TsDateTime)args[0]).Source, (DateTimeKind)(int)args[1]));
        }

        /// <summary>
        /// Subtracts either the specified DateTime or TimeSpan from this instance and returns the result. The result will be the opposite type of the argument type.
        /// </summary>
        /// <arg name="value" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime) or [TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">The value to subtract from this instance.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.subtract?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime) or [TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public TsObject subtract(TsObject[] args)
        {
            switch(args[0].WeakValue)
            {
                case TsDateTime dt:
                    return new TsTimeSpan(Source.Subtract(dt.Source));
                case TsTimeSpan ts:
                    return new TsDateTime(Source.Subtract(ts.Source));
                default:
                    throw new ArgumentException($"Expected the argument passed to {ObjectType}.{nameof(subtract)} to be either a DateTime or TimeSpan. Instead it was a {args[0].WeakValue.GetType().FullName}");
            }
        }

        /// <summary>
        /// Converts the value of this DateTime to local time.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tolocaltime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject to_local_time(TsObject[] args)
        {
            return new TsDateTime(Source.ToLocalTime());
        }

        /// <summary>
        /// Converts the value of this DateTime to its equivalent long date string representation.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tolongdatestring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject to_long_date_string(TsObject[] args)
        {
            return Source.ToLongDateString();
        }

        /// <summary>
        /// Converts the value of this DateTime to its equivalent long time string representation.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tolongtimestring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject to_long_time_string(TsObject[] args)
        {
            return Source.ToLongTimeString();
        }

        /// <summary>
        /// Converts the value of this DateTime to its equivalent short date string representation.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.toshortdatestring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject to_short_date_string(TsObject[] args)
        {
            return Source.ToShortDateString();
        }

        /// <summary>
        /// Converts the value of this DateTime to its equivalent short time string representation.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.toshorttimestring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject to_short_time_string(TsObject[] args)
        {
            return Source.ToShortTimeString();
        }

        /// <summary>
        /// Converts the value of this DateTime to its equivalent string representation.
        /// </summary>
        /// <arg name="[format]" type="string">A standard or custom date and time format string.</arg>
        /// <arg name="[culture]" type="string">The name of the culture used to convert this DateTime.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject to_string(TsObject[] args)
        {
            if (args is null)
                return Source.ToString();
            switch (args.Length)
            {
                case 0:
                    return Source.ToString();
                case 1:
                    return Source.ToString((string)args[0]);
                case 2:
                    return Source.ToString((string)args[0], CultureInfo.GetCultureInfo((string)args[1]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(to_string)}");
            }
        }

        /// <summary>
        /// Converts the value of this DateTime to UTC.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.touniversaltime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject to_utc(TsObject[] args)
        {
            return new TsDateTime(Source.ToUniversalTime());
        }

        /// <summary>
        /// Attempts to convert a string representation of a date and time into a DateTime instance. Accepts 1 and 3 arguments.
        /// </summary>
        /// <arg name="str" type="string">The string to try and convert.</arg>
        /// <arg name="[culture]" type="string">The name of the culture used to convert the string.</arg>
        /// <arg name="[styles]" type="[DateTimeStyles](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimestyles?view=netframework-4.7)">Indicates the style elements that can be in str and still have the parse succeed.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tryparse?view=netframework-4.7</source>
        /// <returns>[ParseResult]({{site.baseurl}}/docs/TaffyScript/ParseResult)</returns>
        public static TsObject try_parse(TsObject[] args)
        {
            bool success;
            DateTime result;
            switch (args.Length)
            {
                case 1:
                    success = DateTime.TryParse((string)args[0], out result);
                    break;
                case 3:
                    success = DateTime.TryParse((string)args[0], CultureInfo.CreateSpecificCulture((string)args[1]), (DateTimeStyles)(int)args[2], out result);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TsDateTime.{nameof(try_parse)}");
            }
            return new ParseResult(success, new TsDateTime(result));
        }

        public static implicit operator TsObject(TsDateTime dt) => new TsInstanceWrapper(dt);
        public static explicit operator TsDateTime(TsObject obj) => (TsDateTime)obj.WeakValue;
    }
}
