using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value) => string.IsNullOrEmpty(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? value) => string.IsNullOrWhiteSpace(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInterned([NotNullWhen(true)] this string value) => string.IsInterned(value) == value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Join(this IEnumerable<string>? values, string? separator) => string.Join(separator, values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICCompare(this string? str, string? value) => string.Compare(str, value, StringComparison.OrdinalIgnoreCase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICEquals(this string? str, string? value) => string.Equals(str, value, StringComparison.OrdinalIgnoreCase);

#if NETSTANDARD2_1
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICIndexOf(this string str, char value) =>
            str.IndexOf(value, StringComparison.OrdinalIgnoreCase);
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICIndexOf(this string str, string value) =>
            str.IndexOf(value, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICIndexOf(this string str, string value, int startIndex) =>
            str.IndexOf(value, startIndex, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICIndexOf(this string str, string value, int startIndex, int count) =>
            str.IndexOf(value, startIndex, count, StringComparison.OrdinalIgnoreCase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICLastIndexOf(this string str, string value) =>
            str.LastIndexOf(value, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICLastIndexOf(this string str, string value, int startIndex) =>
            str.LastIndexOf(value, startIndex, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int OICLastIndexOf(this string str, string value, int startIndex, int count) =>
            str.LastIndexOf(value, startIndex, count, StringComparison.OrdinalIgnoreCase);

#if NETSTANDARD2_1
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICContains(this string str, char value) => str.Contains(value, StringComparison.OrdinalIgnoreCase);
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICContains(this string str, string value) => str.OICIndexOf(value) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICStartsWith(this string str, string value) => str.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OICEndsWith(this string str, string value) => str.EndsWith(value, StringComparison.OrdinalIgnoreCase);
    }
}
