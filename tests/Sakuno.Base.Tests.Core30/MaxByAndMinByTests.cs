using Sakuno.Collections;
using System.Collections.Generic;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class MaxByAndMinByTests
    {
        static KeyValuePair<int, string>[] _kvps = new[]
        {
            KeyValuePair.Create(3, "d"),
            KeyValuePair.Create(1, "a"),
            KeyValuePair.Create(3, "c"),
            KeyValuePair.Create(3, "e"),
            KeyValuePair.Create(2, "b"),
        };

        [Fact]
        public static void MaxBy()
        {
            var maxItem = _kvps.MaxBy(r => r.Key);

            Assert.Equal(KeyValuePair.Create(3, "d"), maxItem);
        }

        [Fact]
        public static void MinBy()
        {
            var minItem = _kvps.MinBy(r => r.Key);

            Assert.Equal(KeyValuePair.Create(1, "a"), minItem);
        }

#if NET462
        static class KeyValuePair
        {
            public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
                new KeyValuePair<TKey, TValue>(key, value);
        }
#endif
    }
}
