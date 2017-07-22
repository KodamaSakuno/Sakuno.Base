using System.Collections.Generic;
using System.ComponentModel;

namespace Sakuno.Collections
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            dictionary.TryGetValue(key, out var result);

            return result;
        }
    }
}
