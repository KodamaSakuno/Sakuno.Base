using System;
using System.Diagnostics;

namespace Sakuno
{
    public static class StopwatchExtensions
    {
        public static TimeSpan GetTimeSpan(this Stopwatch stopwatch) =>
            TimeSpan.FromTicks(stopwatch.ElapsedTicks);
    }
}
