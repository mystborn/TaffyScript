using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public sealed class TsTimeSpan : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.TimeSpan";
        public TimeSpan Source { get; }

        public static TsObject max_value { get; }
        public static TsObject min_value { get; }
        public static TsObject ticks_per_day { get; }
        public static TsObject ticks_per_hour { get; }
        public static TsObject ticks_per_minute { get; }
        public static TsObject ticks_per_second { get; }
        public static TsObject zero { get; }

        static TsTimeSpan()
        {
            max_value = new TsTimeSpan(TimeSpan.MaxValue);
            min_value = new TsTimeSpan(TimeSpan.MinValue);
            ticks_per_day = TimeSpan.TicksPerDay;
            ticks_per_hour = TimeSpan.TicksPerHour;
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
            throw new NotImplementedException();
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            throw new NotImplementedException();
        }

        public TsObject GetMember(string name)
        {
            switch(name)
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
            throw new NotImplementedException();
        }

        public TsObject add(TsObject[] args)
        {
            return new TsTimeSpan(Source.Add(((TsTimeSpan)args[0]).Source));
        }

        public static TsObject compare(TsObject[] args)
        {
            return TimeSpan.Compare(((TsTimeSpan)args[0]).Source, ((TsTimeSpan)args[1]).Source);
        }

        public TsObject duration(TsObject[] args)
        {
            return new TsTimeSpan(Source.Duration());
        }

        public static TsObject from_days(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromDays((double)args[0]));
        }

        public static TsObject from_hours(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromHours((double)args[0]));
        }

        public static TsObject from_milliseconds(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromMilliseconds((double)args[0]));
        }

        public static TsObject from_minutes(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromMinutes((double)args[0]));
        }

        public static TsObject from_seconds(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromSeconds((double)args[0]));
        }

        public static TsObject from_ticks(TsObject[] args)
        {
            return new TsTimeSpan(TimeSpan.FromTicks((long)args[0]));
        }

        public TsObject negate(TsObject[] args)
        {
            return new TsTimeSpan(Source.Negate());
        }

        public static TsObject parse(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new TsTimeSpan(TimeSpan.Parse((string)args[0]));
                case 2:
                    return new TsTimeSpan(TimeSpan.Parse((string)args[0], CultureInfo.CreateSpecificCulture((string)args[1])));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TimeSpan.{nameof(parse)}");
            }
        }

        public TsObject subtract(TsObject[] args)
        {
            return new TsTimeSpan(Source.Subtract(((TsTimeSpan)args[0]).Source));
        }

        public TsObject to_string(TsObject[] args)
        {
            if (args is null)
                return Source.ToString();

            switch(args.Length)
            {
                case 0:
                    return Source.ToString();
                case 1:
                    return Source.ToString((string)args[0]);
                case 2:
                    return Source.ToString((string)args[0], CultureInfo.CreateSpecificCulture((string)args[1]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to TaffyScript.TimeSpan.{nameof(to_string)}");
            }
        }

        public static TsObject try_parse(TsObject[] args)
        {
            bool success;
            TimeSpan result;
            switch(args.Length)
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
