using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DoubleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(this double value) => double.IsInfinity(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(this double value) => double.IsNaN(value);
    }
}
