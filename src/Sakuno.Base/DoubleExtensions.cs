using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DoubleExtensions
    {
        const long _encodingOfPositiveInfinity = 0x7FF0000000000000L;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(this double value) => double.IsInfinity(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(this double value) => double.IsNaN(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double value, double min, double max)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this double value) =>
#if NETSTANDARD2_1
            double.IsFinite(value);
#else
            (BitConverter.DoubleToInt64Bits(value) & long.MaxValue) < _encodingOfPositiveInfinity;
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInteger(this double value) => value.IsFinite() && Math.Floor(value) == Math.Ceiling(value);
    }
}
