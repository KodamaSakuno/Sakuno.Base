using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleUtil
    {
        const double _epsilon = 2.2204460492503131E-15;

        public static readonly object Zero = 0.0;
        public static readonly object One = 1.0;
        public static readonly object NaN = double.NaN;

        public static object GetBoxed(double value)
        {
            if (IsCloseToZero(value))
                return Zero;

            if (IsCloseToOne(value))
                return One;

            if (value.IsNaN())
                return NaN;

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCloseToZero(double value) => Math.Abs(value) < _epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCloseToOne(double value) => Math.Abs(value - 1.0) < _epsilon;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double from, double to, double frac) => from + (to - from) * frac;
    }
}
