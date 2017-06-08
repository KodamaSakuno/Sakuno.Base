using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class MathUtil
    {
        public const double Sqrt2 = 1.4142135623730951;
        public const double PIOver2 = 1.5707963267948966;

        const double _PiOver180 = 0.017453292519943295;
        const double _180OverPi = 57.295779513082323;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DegreesToRadians(double angle) => angle * _PiOver180;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RadiansToDegrees(double radians) => radians * _180OverPi;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2, int val3) =>
            Math.Min(val1, Math.Min(val2, val3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2, int val3, int val4) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, val4)));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2, int val3, int val4, int val5) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, val5))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2, int val3, int val4, int val5, int val6) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, val6)))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2, int val3, int val4, int val5, int val6, int val7) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, Math.Min(val6, val7))))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int val1, int val2, int val3, int val4, int val5, int val6, int val7, int val8) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, Math.Min(val6, Math.Min(val7, val8)))))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2, float val3) =>
            Math.Min(val1, Math.Min(val2, val3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2, float val3, float val4) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, val4)));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2, float val3, float val4, float val5) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, val5))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2, float val3, float val4, float val5, float val6) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, val6)))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2, float val3, float val4, float val5, float val6, float val7) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, Math.Min(val6, val7))))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float val1, float val2, float val3, float val4, float val5, float val6, float val7, float val8) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, Math.Min(val6, Math.Min(val7, val8)))))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2, double val3) =>
            Math.Min(val1, Math.Min(val2, val3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2, double val3, double val4) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, val4)));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2, double val3, double val4, double val5) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, val5))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2, double val3, double val4, double val5, double val6) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, val6)))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2, double val3, double val4, double val5, double val6, double val7) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, Math.Min(val6, val7))))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double val1, double val2, double val3, double val4, double val5, double val6, double val7, double val8) =>
            Math.Min(val1, Math.Min(val2, Math.Min(val3, Math.Min(val4, Math.Min(val5, Math.Min(val6, Math.Min(val7, val8)))))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2, int val3) =>
            Math.Max(val1, Math.Max(val2, val3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2, int val3, int val4) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, val4)));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2, int val3, int val4, int val5) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, val5))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2, int val3, int val4, int val5, int val6) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, val6)))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2, int val3, int val4, int val5, int val6, int val7) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, Math.Max(val6, val7))))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int val1, int val2, int val3, int val4, int val5, int val6, int val7, int val8) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, Math.Max(val6, Math.Max(val7, val8)))))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2, float val3) =>
            Math.Max(val1, Math.Max(val2, val3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2, float val3, float val4) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, val4)));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2, float val3, float val4, float val5) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, val5))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2, float val3, float val4, float val5, float val6) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, val6)))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2, float val3, float val4, float val5, float val6, float val7) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, Math.Max(val6, val7))))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float val1, float val2, float val3, float val4, float val5, float val6, float val7, float val8) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, Math.Max(val6, Math.Max(val7, val8)))))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2, double val3) =>
            Math.Max(val1, Math.Max(val2, val3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2, double val3, double val4) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, val4)));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2, double val3, double val4, double val5) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, val5))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2, double val3, double val4, double val5, double val6) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, val6)))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2, double val3, double val4, double val5, double val6, double val7) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, Math.Max(val6, val7))))));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double val1, double val2, double val3, double val4, double val5, double val6, double val7, double val8) =>
            Math.Max(val1, Math.Max(val2, Math.Max(val3, Math.Max(val4, Math.Max(val5, Math.Max(val6, Math.Max(val7, val8)))))));
    }
}
