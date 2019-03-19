using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ArrayExtensions
    {
        public static unsafe string ToHexString(this byte[] bytes)
        {
            var bufferSize = bytes.Length * 2;
#if NETSTANDARD2_1
            Span<char> buffer = bufferSize <= 64 ? stackalloc char[bufferSize] : new char[bufferSize];
#else
            var buffer = new char[bufferSize];
#endif
            var position = 0;

            foreach (var b in bytes)
            {
                buffer[position++] = GetHexValue(b / 16);
                buffer[position++] = GetHexValue(b % 16);
            }

            return new string(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char GetHexValue(int value) => (char)(value < 10 ? value + '0' : value - 10 + 'a');

        public static bool SequenceEqual<T>(this T[] array, T[] value) => array.SequenceEqual(value, null);
        public static bool SequenceEqual<T>(this T[] array, T[] value, IEqualityComparer<T> comparer)
        {
            if (array == value)
                return true;

            if (array == null || value == null)
                return false;

            if (array.Length != value.Length)
                return false;

            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            for (var i = 0; i < array.Length; i++)
                if (!comparer.Equals(array[i], value[i]))
                    return false;

            return true;
        }
    }
}
