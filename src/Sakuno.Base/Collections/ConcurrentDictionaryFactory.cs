using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    public static class ConcurrentDictionaryFactory
    {
        public static ConcurrentDictionary<TKey, TValue> Create<TKey, TValue>(ConcurrencyLevel level) =>
            new ConcurrentDictionary<TKey, TValue>(level.ToValue(), 0);
        public static ConcurrentDictionary<TKey, TValue> Create<TKey, TValue>(ConcurrencyLevel level, IEqualityComparer<TKey> comparer) =>
            new ConcurrentDictionary<TKey, TValue>(level.ToValue(), 0, comparer);
    }
}
