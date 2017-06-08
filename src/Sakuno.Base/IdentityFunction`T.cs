using System;

namespace Sakuno
{
    public static class IdentityFunction<T>
    {
        public static Func<T, T> Instance { get; } = Core;
        public static Func<T, object> BoxedInstance { get; } = BoxedCore;

        static T Core(T value) => value;
        static object BoxedCore(T value) => value;
    }
}
