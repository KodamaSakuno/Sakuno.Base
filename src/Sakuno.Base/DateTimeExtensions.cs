using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DateTimeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset AsOffset(this DateTime dateTime) =>
            new DateTimeOffset(dateTime);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset DateAsOffset(this DateTimeOffset dateTime) =>
            new DateTimeOffset(dateTime.Date, dateTime.Offset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime Tomorrow(this DateTime dateTime) =>
            new DateTime(TomorrowTicks(dateTime.Ticks), dateTime.Kind);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset Tomorrow(this DateTimeOffset dateTime) =>
            new DateTimeOffset(TomorrowTicks(dateTime.Ticks), dateTime.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long TomorrowTicks(long ticks) =>
            ticks - ticks % DateTimeUtil.TicksPerDay + DateTimeUtil.TicksPerDay;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime LastMonday(this DateTime dateTime) =>
            new DateTime(LastMondayTicks(dateTime.Ticks), dateTime.Kind);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset LastMonday(this DateTimeOffset dateTime) =>
            new DateTimeOffset(LastMondayTicks(dateTime.Ticks), dateTime.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long LastMondayTicks(long ticks)
        {
            var days = (int)(ticks / DateTimeUtil.TicksPerDay);

            return (days - days % 7) * DateTimeUtil.TicksPerDay;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime NextMonday(this DateTime dateTime) =>
            new DateTime(NextMondayTicks(dateTime.Ticks), dateTime.Kind);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTimeOffset NextMonday(this DateTimeOffset dateTime) =>
            new DateTimeOffset(NextMondayTicks(dateTime.Ticks), dateTime.Offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long NextMondayTicks(long ticks)
        {
            var days = (int)(ticks / DateTimeUtil.TicksPerDay);

            return (days - days % 7 + 7) * DateTimeUtil.TicksPerDay;
        }

        public static DateTime StartOfLastMonth(this DateTime dateTime) =>
            new DateTime(StartOfLastMonthTicks(dateTime.Ticks), dateTime.Kind);
        public static DateTimeOffset StartOfLastMonth(this DateTimeOffset dateTime) =>
            new DateTimeOffset(StartOfLastMonthTicks(dateTime.Ticks), dateTime.Offset);
        static long StartOfLastMonthTicks(long ticks)
        {
            DateTimeUtil.ExtractDate(ticks, out _, out _, out var day, out _);

            if (day > 1)
                ticks -= (day - 1) * DateTimeUtil.TicksPerDay;

            return ticks - ticks % DateTimeUtil.TicksPerDay;
        }

        public static DateTime StartOfNextMonth(this DateTime dateTime) =>
            new DateTime(StartOfNextMonthTicks(dateTime.Ticks), dateTime.Kind);
        public static DateTimeOffset StartOfNextMonth(this DateTimeOffset dateTime) =>
            new DateTimeOffset(StartOfNextMonthTicks(dateTime.Ticks), dateTime.Offset);
        static long StartOfNextMonthTicks(long ticks)
        {
            DateTimeUtil.ExtractDate(ticks, out _, out var month, out var day, out var isLeapYear);

            var days = isLeapYear ? DateTimeUtil.DaysToMonth366 : DateTimeUtil.DaysToMonth365;

            day = days[month] - days[month - 1] - day + 1;
            ticks += day * DateTimeUtil.TicksPerDay;

            return ticks - ticks % DateTimeUtil.TicksPerDay;
        }
    }
}
