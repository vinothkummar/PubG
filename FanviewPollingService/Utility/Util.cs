using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FanviewPollingService.Utility
{
    public static class Util
    {
        public static DateTime ToDateTimeFormat(this string lastEventTimeStamp)
        {
            DateTimeFormatInfo dateFormatProvider = new DateTimeFormatInfo();
            dateFormatProvider.LongDatePattern = "MM/dd/yy HH:mm:ss";
            return DateTime.Parse(lastEventTimeStamp, dateFormatProvider);
        }
    }
}
