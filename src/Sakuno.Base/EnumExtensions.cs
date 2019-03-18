using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this T value, T flag) where T : Enum =>
#if NET462
            false;
#else
            value.HasFlag(flag);
#endif
    }
}
