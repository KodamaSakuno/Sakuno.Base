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
        public static bool OICEquals(this string rpString, string rpValue) => rpString.Equals(rpValue, StringComparison.OrdinalIgnoreCase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICIndexOf(this string rpString, string rpValue) => rpString.IndexOf(rpValue, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICLastIndexOf(this string rpString, string rpValue) => rpString.LastIndexOf(rpValue, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICContains(this string rpString, string rpValue) => rpString.OICIndexOf(rpValue) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICStartsWith(this string rpString, string rpValue) => rpString.StartsWith(rpValue, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICEndsWith(this string rpString, string rpValue) => rpString.EndsWith(rpValue, StringComparison.OrdinalIgnoreCase);
    }
}
