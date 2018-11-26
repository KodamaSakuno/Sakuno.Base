using System;
using System.Collections.Concurrent;

namespace Sakuno
{
    public static class EnumUtil
    {
        public static object GetBoxed<T>(T value) where T : Enum => BoxedEnum<T>.Get(value);

        public static object[] GetBoxedValues<T>() where T : Enum => BoxedEnum<T>._boxedValues.Value;

        static class BoxedEnum<T> where T : Enum
        {
            static readonly ConcurrentDictionary<T, object> _boxes = new ConcurrentDictionary<T, object>();

            internal static Lazy<object[]> _boxedValues = new Lazy<object[]>(() =>
            {
                var values = (T[])Enum.GetValues(typeof(T));
                var result = new object[values.Length];

                for (var i = 0; i < values.Length; i++)
                    result[i] = Get(values[i]);

                return result;
            });

            public static object Get(T value) => _boxes.GetOrAdd(value, IdentityFunction<T>.BoxedInstance);
        }
    }
}
