using System;
using System.Collections.Generic;
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
    }
}
