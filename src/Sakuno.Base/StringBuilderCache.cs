using System;
using System.Text;

namespace Sakuno
{
    public static class StringBuilderCache
    {
        const int MaxLength = 360;

        [ThreadStatic]
        static StringBuilder _cachedInstance;

        public static StringBuilder Acquire(int capacity = 16)
        {
            if (capacity <= MaxLength)
            {
                var rCachedInstance = _cachedInstance;

                if (rCachedInstance != null && capacity <= rCachedInstance.Capacity)
                {
                    _cachedInstance = null;
                    rCachedInstance.Clear();

                    return rCachedInstance;
                }
            }

            return new StringBuilder(capacity);
        }

        public static void Release(StringBuilder stringBuilder)
        {
            if (stringBuilder.Capacity > MaxLength)
                return;

            _cachedInstance = stringBuilder;
        }

        public static string GetStringAndRelease(this StringBuilder stringBuilder)
        {
            var result = stringBuilder.ToString();

            Release(stringBuilder);

            return result;
        }
    }
}
