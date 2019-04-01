using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this T value, T flag) where T : Enum => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<T>(this T value, T flag) where T : Enum => false;
    }
}
