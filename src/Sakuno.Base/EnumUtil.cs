using System;
using System.Collections.Concurrent;

namespace Sakuno
{
    public static class EnumUtil
    {
        public static object GetBoxed<T>(T value) where T : Enum => BoxedEnum<T>.Get(value);

        static class BoxedEnum<T> where T : Enum
        {
            static readonly ConcurrentDictionary<T, object> _boxes = new ConcurrentDictionary<T, object>();

            public static object Get(T value) => _boxes.GetOrAdd(value, IdentityFunction<T>.BoxedInstance);
        }
    }
}
