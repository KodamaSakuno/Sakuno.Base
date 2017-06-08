using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class TypeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssignableFrom<T>(this Type type) => type.IsAssignableFrom(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubclassOf<T>(this Type type) => type.IsSubclassOf(typeof(T));
    }
}
