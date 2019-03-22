using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Int64Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(this long value, long min, long max)
        {
#if NETSTANDARD2_1
            return Math.Clamp(value, min, max);
#else
            if (min > max)
                throw new ArgumentOutOfRangeException("Min must be less than or equal to max.");

            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
#endif
        }
    }
}
