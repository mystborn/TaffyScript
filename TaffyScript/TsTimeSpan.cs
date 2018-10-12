using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents a time interval.
    /// </summary>
    /// <property name="days" type="number" access="get">
    ///     <summary>Gets the days component of the time interval represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.days?view=netframework-4.7</source>
    /// </property>
    /// <property name="hours" type="number" access="get">
    ///     <summary>Gets the hours component of the time interval represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.hours?view=netframework-4.7</source>
    /// </property>
    /// <property name="milliseconds" type="number" access="get">
    ///     <summary>Gets the milliseconds component of the time interval represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.milliseconds?view=netframework-4.7</source>
    /// </property>
    /// <property name="minutes" type="number" access="get">
    ///     <summary>Gets the minutes component of the time interval represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.minutes?view=netframework-4.7</source>
    /// </property>
    /// <property name="seconds" type="number" access="get">
    ///     <summary>Gets the seconds component of the time interval represented by this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.seconds?view=netframework-4.7</source>
    /// </property>
    /// <property name="ticks" type="number" access="get">
    ///     <summary>Gets the number of ticks that represent the value of this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.ticks?view=netframework-4.7</source>
    /// </property>
    /// <property name="total_days" type="number" access="get">
    ///     <summary>Gets the value of this TimeSpan expressed as whole and fractional days.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.totaldays?view=netframework-4.7</source>
    /// </property>
    /// <property name="total_hours" type="number" access="get">
    ///     <summary>Gets the value of this TimeSpan expressed as whole and fractional hours.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.totalhours?view=netframework-4.7</source>
    /// </property>
    /// <property name="total_milliseconds" type="number" access="get">
    ///     <summary>Gets the value of this TimeSpan expressed as whole and fractional milliseconds.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.totalmilliseconds?view=netframework-4.7</source>
    /// </property>
    /// <property name="total_minutes" type="number" access="get">
    ///     <summary>Gets the value of this TimeSpan expressed as whole and fractional minutes.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.totalminutes?view=netframework-4.7</source>
    /// </property>
    /// <property name="total_seconds" type="number" access="get">
    ///     <summary>Gets the value of this TimeSpan expressed as whole and fractional seconds.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.totalseconds?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject("TaffyScript.TimeSpan")]
    public sealed class TsTimeSpan : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.TimeSpan";
        public TimeSpan Source { get; }

        /// <summary>
        /// Gets the maximum TimeSpan value.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.maxvalue?view=netframework-4.7</source>
        /// <type>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</type>
        public static TsObject max_value { get; }

        /// <summary>
        /// Gets the minimum Timespan value.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.minvalue?view=netframework-4.7</source>
        /// <type>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</type>
        public static TsObject min_value { get; }

        /// <summary>
        /// Gets the number of ticks in one day.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.ticksperday?view=netframework-4.7</source>
        /// <type>number</type>
        public static TsObject ticks_per_day { get; }

        /// <summary>
        /// Gets the number of ticks in one hour.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.ticksperhour?view=netframework-4.7</source>
        /// <type>number</type>
        public static TsObject ticks_per_hour { get; }

        /// <summary>
        /// Gets the number of ticks in one millisecond.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tickspermillisecond?view=netframework-4.7</source>
        /// <type>number</type>
        public static TsObject ticks_per_millisecond { get; }

        /// <summary>
        /// Gets the number of ticks in one minute.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.ticksperminute?view=netframework-4.7</source>
        /// <type>number</type>
        public static TsObject ticks_per_minute { get; }

        /// <summary>
        /// Gets the number of ticks in one second.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tickspersecond?view=netframework-4.7</source>
        /// <type>number</type>
        public static TsObject ticks_per_second { get; }

        /// <summary>
        /// Gets a TimeSpan that represents zero.
        /// </summary>
        /// <source></source>
        /// <type>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</type>
        public static TsObject zero { get; }

        static TsTimeSpan()
        {
            max_value = new TsTimeSpan(TimeSpan.MaxValue);
            min_value = new TsTimeSpan(TimeSpan.MinValue);
            ticks_per_day = TimeSpan.TicksPerDay;
            ticks_per_hour = TimeSpan.TicksPerHour;
            ticks_per_millisecond = TimeSpan.TicksPerMillisecond;
            ticks_per_minute = TimeSpan.TicksPerMinute;
            ticks_per_second = TimeSpan.TicksPerSecond;
            zero = new TsTimeSpan(TimeSpan.Zero);
        }

        public TsTimeSpan(TimeSpan timeSpan)
        {
            Source = timeSpan;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "add":
                    return add(args);
                case "duration":
                    return duration(args);
                case "negate":
                    return negate(args);
                case "subtract":
                    return subtract(args);
                case "to_string":
                    return to_string(args);
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
                case "days":
                    return Source.Days;
                case "hours":
                    return Source.Hours;
                case "milliseconds":
                    return Source.Milliseconds;
                case "minutes":
                    return Source.Minutes;
                case "seconds":
                    return Source.Seconds;
                case "ticks":
                    return Source.Ticks;
                case "total_days":
                    return Source.TotalDays;
                case "total_hours":
                    return Source.TotalHours;
                case "total_milliseconds":
                    return Source.TotalMilliseconds;
                case "total_minutes":
                    return Source.TotalMinutes;
                case "total_seconds":
                    return Source.TotalSeconds;
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
                case "duration":
                    del = new TsDelegate(duration, scriptName);
                    break;
                case "negate":
                    del = new TsDelegate(negate, scriptName);
                    break;
                case "subtract":
                    del = new TsDelegate(subtract, scriptName);
                    break;
                case "to_string":
                    del = new TsDelegate(to_string, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a new TimeSpan by adding this instance and another TimeSpan together.
        /// </summary>
        /// <arg name="value" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">The other TimeSpan to add.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.add?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public TsObject add(TsObject[] args)
        {
            return new TsTimeSpan(Source.Add(((TsTimeSpan)args[0]).Source));
        }

        /// <summary>
        /// Compares two TimeSpan instances and returns a number representing the result. -1 if the first is less than the second, 0 if they are equal, 1 if the first is greater than the second.
        /// </summary>
        /// <arg name="first" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">The first TimeSpan to compare.</arg>
        /// <arg name="second" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">The second TimeSpan to compare.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.compare?view=netframework-4.7</source>
        /// <returns>number</returns>
        public static TsObject compare(TsObject[] args)
        {
            return TimeSpan.Compare(((TsTimeSpan)args[0]).Source, ((TsTimeSpan)args[1]).Source);
        }

        /// <summary>
        /// Gets a new TimeSpan who is value is the absolute value of this instance.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.duration?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public TsObject duration(TsObject[] args)
        {
            return new TsTimeSpan(Source.Duration());
        }

        /// <summary>
        /// Returns a TimeSpan that represents a specified number of days.
        /// </summary>
        /// <arg name="value">A number of days.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.fromdays?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject from_days(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromDays((double)args[0]));
        }

        /// <summary>
        /// Returns a TimeSpan that represents a specified number of hours.
        /// </summary>
        /// <arg name="value">A number of days.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.fromhours?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject from_hours(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromHours((double)args[0]));
        }

        /// <summary>
        /// Returns a TimeSpan that represents a specified number of milliseconds.
        /// </summary>
        /// <arg name="value">A number of days.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.frommilliseconds?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject from_milliseconds(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromMilliseconds((double)args[0]));
        }

        /// <summary>
        /// Returns a TimeSpan that represents a specified number of minutes.
        /// </summary>
        /// <arg name="value">A number of days.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.fromminutes?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject from_minutes(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromMinutes((double)args[0]));
        }

        /// <summary>
        /// Returns a TimeSpan that represents a specified number of seconds.
        /// </summary>
        /// <arg name="value">A number of days.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.fromseconds?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject from_seconds(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromSeconds((double)args[0]));
        }

        /// <summary>
        /// Returns a TimeSpan that represents a specified number of ticks.
        /// </summary>
        /// <arg name="value">A number of days.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.fromticks?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject from_ticks(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromTicks((long)args[0]));
        }

        /// <summary>
        /// Returns a new TimeSpan whose value is the negated value of this instance.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.negate?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public TsObject negate(TsObject[] args)
        {
            return new TsTimeSpan(Source.Negate());
        }

        /// <summary>
        /// Converts the string representation of a time interval to its TimeSpan equivalent.
        /// </summary>
        /// <arg name="str" type="string">The string to convert.</arg>
        /// <arg name="[culture]" type="string">The name of the culture used to parse the string.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.parse?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public static TsObject parse(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new TsTimeSpan(TimeSpan.Parse((string)args[0]));
                case 2:
                    return new TsTimeSpan(TimeSpan.Parse((string)args[0], CultureInfo.GetCultureInfo((string)args[1])));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TimeSpan.{nameof(parse)}");
            }
        }

        /// <summary>
        /// Creates a new TimeSpan by subtracting this instance and another TimeSpan.
        /// </summary>
        /// <arg name="value" type="[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)">The other TimeSpan to subtract.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.subtract?view=netframework-4.7</source>
        /// <returns>[TimeSpan]({{site.baseurl}}/docs/TaffyScript/TimeSpan)</returns>
        public TsObject subtract(TsObject[] args)
        {
            return new TsTimeSpan(Source.Subtract(((TsTimeSpan)args[0]).Source));
        }

        /// <summary>
        /// Converts this instance to its equivalent string representation.
        /// </summary>
        /// <arg name="[format]" type="string">A standard or custom format string.</arg>
        /// <arg name="[culture]" type="string">The name of the culture used to convert this instance.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tostring?view=netframework-4.7</source>
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
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TimeSpan.{nameof(to_string)}");
            }
        }

        /// <summary>
        /// Attempts to convert the string representation of a time interval to its TimeSpan equivalent.
        /// </summary>
        /// <arg name="str" type="string">The string to convert.</arg>
        /// <arg name="[culture]" type="string">The name of the culture used to parse the string.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tryparse?view=netframework-4.7</source>
        /// <returns>[ParseResult]({{site.baseurl}}/docs/TaffyScript/ParseResult)</returns>
        public static TsObject try_parse(TsObject[] args)
        {
            bool success;
            TimeSpan result;
            switch (args.Length)
            {
                case 1:
                    success = TimeSpan.TryParse((string)args[0], out result);
                    break;
                case 2:
                    success = TimeSpan.TryParse((string)args[0], CultureInfo.CreateSpecificCulture((string)args[1]), out result);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TimeSpan.{nameof(try_parse)}");
            }
            return new ParseResult(success, new TsTimeSpan(result));
        }

        public static implicit operator TsObject(TsTimeSpan timeSpan) => new TsInstanceWrapper(timeSpan);
        public static explicit operator TsTimeSpan(TsObject obj) => (TsTimeSpan)obj.WeakValue;
    }
}
