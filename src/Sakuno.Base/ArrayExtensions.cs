using System.Collections.Generic;
using System.ComponentModel;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ArrayExtensions
    {
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
