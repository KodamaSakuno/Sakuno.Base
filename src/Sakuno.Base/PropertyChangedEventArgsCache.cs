using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Sakuno
{
    public static class PropertyChangedEventArgsCache
    {
        static ConcurrentDictionary<string, PropertyChangedEventArgs> _cache = new ConcurrentDictionary<string, PropertyChangedEventArgs>(StringComparer.OrdinalIgnoreCase);

        public static readonly PropertyChangedEventArgs CountPropertyChanged = Get("Count");
        public static readonly PropertyChangedEventArgs IndexerPropertyChanged = Get("Item[]");

        public static PropertyChangedEventArgs Get(string propertyName) =>
            _cache.GetOrAdd(propertyName ?? string.Empty, r => new PropertyChangedEventArgs(r));
    }
}
