using System.Collections.Generic;

namespace Sakuno.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            dictionary.TryGetValue(key, out var result);

            return result;
        }
    }
}
