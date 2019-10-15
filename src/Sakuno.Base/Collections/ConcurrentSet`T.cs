using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    public sealed class ConcurrentSet<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        ConcurrentDictionary<T, byte> _dictionary;

        public int Count => _dictionary.Count;

        public bool IsEmpty => _dictionary.IsEmpty;

        bool ICollection<T>.IsReadOnly => false;

        public ConcurrentSet() : this(ConcurrencyLevel.Default) { }
        public ConcurrentSet(ConcurrencyLevel level)
        {
            _dictionary = ConcurrentDictionaryFactory.Create<T, byte>(level);
        }
        public ConcurrentSet(IEqualityComparer<T> comparer) : this(comparer, ConcurrencyLevel.Default) { }
        public ConcurrentSet(IEqualityComparer<T> comparer, ConcurrencyLevel level)
        {
            _dictionary = ConcurrentDictionaryFactory.Create<T, byte>(level, comparer);
        }

        public bool TryAdd(T item) => _dictionary.TryAdd(item, 0);
        public bool TryRemove(T item) => _dictionary.TryRemove(item, out _);

        public void Clear() => _dictionary.Clear();

        public bool Contains(T item) => _dictionary.ContainsKey(item);

        public Enumerator GetEnumerator() => new Enumerator(_dictionary.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        void ICollection<T>.Add(T item) => TryAdd(item);
        bool ICollection<T>.Remove(T item) => TryRemove(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public struct Enumerator : IEnumerator<T>
        {
            IEnumerator<KeyValuePair<T, byte>> _source;

            public T Current => _source.Current.Key;

            object? IEnumerator.Current => Current;

            public Enumerator(IEnumerator<KeyValuePair<T, byte>> source)
            {
                _source = source;
            }

            public bool MoveNext() => _source.MoveNext();

            public void Reset() => _source.Reset();

            public void Dispose() => _source.Dispose();
        }
    }
}
