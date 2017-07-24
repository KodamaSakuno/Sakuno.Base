using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StopwatchExtensions
    {
        public static TimeSpan GetTimeSpan(this Stopwatch stopwatch) => TimeSpan.FromTicks(stopwatch.ElapsedTicks);
    }
}
