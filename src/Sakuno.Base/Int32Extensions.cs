using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Int32Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(this int value, int min, int max)
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
