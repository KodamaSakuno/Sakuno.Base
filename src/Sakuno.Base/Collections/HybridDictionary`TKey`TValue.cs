using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sakuno.Collections
{
    public class HybridDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        ListDictionary<TKey, TValue>? _listDictionary;
        Dictionary<TKey, TValue>? _dictionary;

        IEqualityComparer<TKey>? _comparer;

        public int Count
        {
            get
            {
                if (_dictionary != null)
                    return _dictionary.Count;

                if (_listDictionary != null)
                    return _listDictionary.Count;

                return 0;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_dictionary != null)
                    return _dictionary[key];

                if (_listDictionary != null)
                    return _listDictionary[key];

                throw new KeyNotFoundException();
            }
            set
            {
                if (_dictionary != null)
                {
                    _dictionary[key] = value;
                    return;
                }

                if (_listDictionary == null)
                {
                    _listDictionary = new ListDictionary<TKey, TValue>(_comparer) { [key] = value };
                    return;
                }
                if (_listDictionary.Count < 8)
                {
                    _listDictionary[key] = value;
                    return;
                }

                SwitchToDictionary();
                _dictionary![key] = value;
            }
        }

        ListDictionary<TKey, TValue> ListDictionary =>
            _listDictionary ??= new ListDictionary<TKey, TValue>(_comparer);

        public ICollection<TKey> Keys
        {
            get
            {
                if (_dictionary != null)
                    return _dictionary.Keys;

                return ListDictionary.Keys;
            }
        }
        public ICollection<TValue> Values
        {
            get
            {
                if (_dictionary != null)
                    return _dictionary.Values;

                return ListDictionary.Values;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public HybridDictionary() { }
        public HybridDictionary(IEqualityComparer<TKey> comparer)
        {
            _comparer = comparer;
        }
        public HybridDictionary(int capacity) : this(capacity, null) { }
        public HybridDictionary(int capacity, IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer;

            if (capacity > 5)
                _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        void SwitchToDictionary()
        {
            var dictionary = new Dictionary<TKey, TValue>(13, _comparer);

            foreach (var item in _listDictionary!)
                dictionary.Add(item.Key, item.Value);

            _dictionary = dictionary;
            _listDictionary = null;
        }

        public void Add(TKey key, TValue value)
        {
            if (_dictionary != null)
            {
                _dictionary.Add(key, value);
                return;
            }

            if (_listDictionary == null)
            {
                _listDictionary = new ListDictionary<TKey, TValue>(_comparer) { [key] = value };
                return;
            }
            if (_listDictionary.Count < 8)
            {
                _listDictionary.Add(key, value);
                return;
            }

            SwitchToDictionary();
            _dictionary!.Add(key, value);
        }
        public bool Remove(TKey key)
        {
            if (_dictionary != null)
                return _dictionary.Remove(key);

            if (_listDictionary != null)
                return _listDictionary.Remove(key);

            return false;
        }
        public void Clear()
        {
            if (_dictionary != null)
            {
                _dictionary.Clear();
                _dictionary = null;
                return;
            }

            if (_listDictionary != null)
            {
                _listDictionary.Clear();
                _listDictionary = null;
            }
        }
        public bool ContainsKey(TKey key)
        {
            if (_dictionary != null)
                return _dictionary.ContainsKey(key);

            if (_listDictionary != null)
                return _listDictionary.ContainsKey(key);

            return false;
        }
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_dictionary != null)
                return _dictionary.TryGetValue(key, out value);

            if (_listDictionary == null)
            {
                _listDictionary = new ListDictionary<TKey, TValue>(_comparer);

                value = default!;
                return false;
            }

            return _listDictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (_dictionary != null)
                return _dictionary.GetEnumerator();

            return ListDictionary.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value))
            {
                Remove(item.Key);
                return true;
            }

            return false;
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (TryGetValue(item.Key, out var value))
                return EqualityComparer<TValue>.Default.Equals(value, item.Value);

            return false;
        }
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (_dictionary != null)
                ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, index);

            if (_listDictionary != null)
                ((ICollection<KeyValuePair<TKey, TValue>>)_listDictionary).CopyTo(array, index);
        }
    }
}
