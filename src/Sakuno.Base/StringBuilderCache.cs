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
                var cachedInstance = _cachedInstance;

                if (cachedInstance != null && capacity <= cachedInstance.Capacity)
                {
                    _cachedInstance = null;
                    cachedInstance.Clear();

                    return cachedInstance;
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
