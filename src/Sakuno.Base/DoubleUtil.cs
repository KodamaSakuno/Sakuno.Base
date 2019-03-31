using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleUtil
    {
        public const double Epsilon = 2.2204460492503131E-16;

        public static object GetBoxed(double value)
        {
            if (IsCloseToZero(value))
                return BoxedConstants.Double.Zero;

            if (IsCloseToOne(value))
                return BoxedConstants.Double.One;

            if (value.IsNaN())
                return BoxedConstants.Double.NaN;

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCloseToZero(double value) => Math.Abs(value) < Epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCloseToOne(double value) => Math.Abs(value - 1.0) < Epsilon;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp(double from, double to, double frac) => from + (to - from) * frac;
    }
}
