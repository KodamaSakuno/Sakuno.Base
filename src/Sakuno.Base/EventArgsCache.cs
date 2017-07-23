using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Sakuno
{
    public static class EventArgsCache
    {
        public static class PropertyChanged
        {
            static ConcurrentDictionary<string, PropertyChangedEventArgs> _cache =
                new ConcurrentDictionary<string, PropertyChangedEventArgs>(StringComparer.OrdinalIgnoreCase);

            public static readonly PropertyChangedEventArgs Count = Get("Count");
            public static readonly PropertyChangedEventArgs Indexer = Get("Item[]");

            public static PropertyChangedEventArgs Get(string propertyName) =>
                _cache.GetOrAdd(propertyName ?? string.Empty, r => new PropertyChangedEventArgs(r));
        }

        public static class CollectionChanged
        {
            public static readonly NotifyCollectionChangedEventArgs Reset =
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        }
    }
}
