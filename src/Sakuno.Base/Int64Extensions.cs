using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class Int64Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException("Min must be less than or equal to max.");

            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}
