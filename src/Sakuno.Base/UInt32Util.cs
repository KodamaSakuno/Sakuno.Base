using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class UInt32Util
    {
        static readonly int[] _multiplyDeBruijnBitPosition = new[]
        {
            0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
            8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HighestBit(uint value)
        {
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;

            return _multiplyDeBruijnBitPosition[value * 0x07C4ACDD >> 27];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int count)
        {
            if (count < 0)
                return RotateRight(value, -count);

            var shift = count & 0x1F;

            return value << shift | value >> 32 - shift;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(uint value, int count)
        {
            if (count < 0)
                return RotateLeft(value, -count);

            var shift = count & 0x1F;

            return value >> shift | value << 32 - shift;
        }
    }
}
