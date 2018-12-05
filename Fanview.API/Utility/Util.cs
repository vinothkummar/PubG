using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace Fanview.API.Utility
{
    public static class Util
    {
        public static DateTime ToDateTimeFormat(this string lastEventTimeStamp)
        {
            DateTimeFormatInfo dateFormatProvider = new DateTimeFormatInfo();
            dateFormatProvider.LongDatePattern = "MM/dd/yy HH:mm:ss";
            return DateTime.Parse(lastEventTimeStamp, dateFormatProvider);
        }

        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }

        public static KeyValuePair<string, object> WithValue(this string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }

        public static ExpandoObject Init(
            this ExpandoObject expando, params KeyValuePair<string, object>[] values)
        {
            foreach (KeyValuePair<string, object> kvp in values)
            {
                ((IDictionary<string, Object>)expando)[kvp.Key] = kvp.Value;
            }
            return expando;
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            if (dateTime == null)
            {
                return 0 ;
            }

            //var dateTimeFormat = DateTime.ParseExact(dateTime, "dd/MM/yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture);

            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
     
    }
}
