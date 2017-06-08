﻿using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleUtil
    {
        const double _epsilon = 2.2204460492503131E-15;

        public static readonly object Zero = 0.0;
        public static readonly object One = 1.0;
        public static readonly object NaN = double.NaN;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCloseToZero(double value) => Math.Abs(value) < _epsilon;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCloseToOne(double value) => Math.Abs(value - 1.0) < _epsilon;
    }
}