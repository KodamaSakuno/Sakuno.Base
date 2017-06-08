using ExtraConstraints;
using System.Collections.Concurrent;

namespace Sakuno
{
    public static class EnumUtil
    {
        public static object GetBoxed<[EnumConstraint] T>(T value) =>
            BoxedEnum<T>.Get(value);

        static class BoxedEnum<[EnumConstraint] T>
        {
            static readonly ConcurrentDictionary<T, object> _boxes = new ConcurrentDictionary<T, object>();

            public static object Get(T value) => _boxes.GetOrAdd(value, IdentityFunction<T>.BoxedInstance);
        }
    }
}
