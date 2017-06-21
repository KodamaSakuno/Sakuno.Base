using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICEquals(this string str, string value) => str.Equals(value, StringComparison.OrdinalIgnoreCase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICIndexOf(this string str, string value) => str.IndexOf(value, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICLastIndexOf(this string str, string value) => str.LastIndexOf(value, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICContains(this string str, string value) => str.OICIndexOf(value) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICStartsWith(this string str, string value) => str.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICEndsWith(this string str, string value) => str.EndsWith(value, StringComparison.OrdinalIgnoreCase);
    }
}
