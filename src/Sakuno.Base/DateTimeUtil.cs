using System;

namespace Sakuno
{
    public static class DateTimeUtil
    {
        public const long UnixEpochSeconds = 62135596800L;
        public const long UnixEpochTicks = 621355968000000000L;

        public const long TicksPerSecond = 10000000L;
        public const long TicksPerDay = 864000000000L;

        public const int DaysPerYear = 365;
        public const int DaysPer4Years = DaysPerYear * 4 + 1;
        public const int DaysPer100Years = DaysPer4Years * 25 - 1;
        public const int DaysPer400Years = DaysPer100Years * 4 + 1;

        internal static readonly int[] DaysToMonth365 = new[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        internal static readonly int[] DaysToMonth366 = new[] { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

        public static DateTimeOffset UnixEpoch { get; } = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        internal static void ExtractDate(long ticks, out int year, out int month, out int day, out bool isLeapYear)
        {
            var totalDays = (int)(ticks / TicksPerDay);

            var period400Years = totalDays / DaysPer400Years;
            totalDays -= period400Years * DaysPer400Years;

            var period100Years = totalDays / DaysPer100Years;
            if (period100Years == 4)
                period100Years = 3;
            totalDays -= period100Years * DaysPer100Years;

            var period4Years = totalDays / DaysPer4Years;
            totalDays -= period4Years * DaysPer4Years;

            var period1Year = totalDays / DaysPerYear;
            if (period1Year == 4)
                period1Year = 3;

            year = period400Years * 400 + period100Years * 100 + period4Years * 4 + period1Year + 1;

            totalDays -= period1Year * DaysPerYear;

            isLeapYear = period1Year == 3 && (period4Years != 24 || period100Years == 3);

            var days = isLeapYear ? DaysToMonth366 : DaysToMonth365;

            month = totalDays >> 5 + 1;
            while (totalDays >= days[month])
                month++;

            day = totalDays - days[month - 1] + 1;
        }
    }
}
