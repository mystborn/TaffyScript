using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class TsDateTime : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.DateTime";
        public DateTime Source { get; }

        public static TsObject now => new TsDateTime(DateTime.Now);
        public static TsObject today => new TsDateTime(DateTime.Today);
        public static TsObject utc_now => new TsDateTime(DateTime.UtcNow);

        public TsDateTime(DateTime dateTime)
        {
            Source = dateTime;
        }

        public TsDateTime(TsObject[] args)
        {
            switch(args.Length)
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
                    // Todo: implement TsDateTime.time_of_day
                    throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public TsObject add_days(TsObject[] args)
        {
            return new TsDateTime(Source.AddDays((double)args[0]));
        }

        public TsObject add_hours(TsObject[] args)
        {
            return new TsDateTime(Source.AddHours((double)args[0]));
        }

        public TsObject add_milliseconds(TsObject[] args)
        {
            return new TsDateTime(Source.AddMilliseconds((double)args[0]));
        }

        public TsObject add_minutes(TsObject[] args)
        {
            return new TsDateTime(Source.AddMinutes((double)args[0]));
        }

        public TsObject add_months(TsObject[] args)
        {
            return new TsDateTime(Source.AddMonths((int)args[0]));
        }

        public TsObject add_seconds(TsObject[] args)
        {
            return new TsDateTime(Source.AddSeconds((double)args[0]));
        }

        public TsObject add_ticks(TsObject[] args)
        {
            return new TsDateTime(Source.AddTicks((long)args[0]));
        }

        public TsObject add_years(TsObject[] args)
        {
            return new TsDateTime(Source.AddYears((int)args[0]));
        }

        public static TsObject days_in_month(TsObject[] args)
        {
            return DateTime.DaysInMonth((int)args[0], (int)args[1]);
        }

        public TsObject is_daylight_saving_time(TsObject[] args)
        {
            return Source.IsDaylightSavingTime();
        }

        public static TsObject is_leap_year(TsObject[] args)
        {
            return DateTime.IsLeapYear((int)args[0]);
        }

        public static TsObject parse(TsObject[] args)
        {
            switch(args.Length)
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

        public static TsObject specify_kind(TsObject[] args)
        {
            return new TsDateTime(DateTime.SpecifyKind(((TsDateTime)args[0]).Source, (DateTimeKind)(int)args[1]));
        }

        public TsObject to_local_time(TsObject[] args)
        {
            return new TsDateTime(Source.ToLocalTime());
        }

        public TsObject to_long_date_string(TsObject[] args)
        {
            return Source.ToLongDateString();
        }

        public TsObject to_long_time_string(TsObject[] args)
        {
            return Source.ToLongTimeString();
        }

        public TsObject to_short_date_string(TsObject[] args)
        {
            return Source.ToShortDateString();
        }

        public TsObject to_short_time_string(TsObject[] args)
        {
            return Source.ToShortTimeString();
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
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(to_string)}");
            }
        }

        public TsObject to_utc(TsObject[] args)
        {
            return new TsDateTime(Source.ToUniversalTime());
        }

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
