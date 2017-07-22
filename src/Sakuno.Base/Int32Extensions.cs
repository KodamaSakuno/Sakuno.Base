﻿using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class Int32Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
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