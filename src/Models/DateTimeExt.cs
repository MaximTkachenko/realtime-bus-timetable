using System;

namespace Models
{
    public static class DateTimeExt
    {
        public static double ToUnixTimestamp(this DateTime dt)
        {
            return dt.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }
    }
}
