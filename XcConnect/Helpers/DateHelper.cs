using System;

namespace XcConnect.Helpers
{
    /// <summary>
    /// DateTime Utils
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Get Colombian Time
        /// </summary>
        /// <returns></returns>
        public static DateTime GetColombiaDateTime()
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }
    }
}