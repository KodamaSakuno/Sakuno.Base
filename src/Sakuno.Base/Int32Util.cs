using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class Int32Util
    {
        public static readonly object Zero = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int HighestBit(int value) => UInt32Util.HighestBit((uint)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateLeft(int value, int count) => (int)UInt32Util.RotateLeft((uint)value, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RotateRight(int value, int count) => (int)UInt32Util.RotateRight((uint)value, count);
    }
}
