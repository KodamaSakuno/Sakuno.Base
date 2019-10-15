using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sakuno.Collections
{
    public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        IEqualityComparer<TKey>? _comparer;

        Node? _head;

        int _count;
        public int Count => _count;

        int _version;

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                var node = _head;

                if (_comparer == null)
                    while (node != null)
                    {
                        var nodeKey = node.Key;
                        if (nodeKey != null && nodeKey.Equals(key))
                            return node.Value;

                        node = node.Next;
                    }
                else
                    while (node != null)
                    {
                        var nodeKey = node.Key;
                        if (nodeKey != null && _comparer.Equals(key, nodeKey))
                            return node.Value;

                        node = node.Next;
                    }

                throw new KeyNotFoundException();
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                AddOrSet(key, value, false);
            }
        }

        KeyCollection? _keys;
        public ICollection<TKey> Keys
        {
            get
            {
                if (_keys == null)
                    _keys = new KeyCollection(this);

                return _keys;
            }
        }

        ValueCollection? _values;
        public ICollection<TValue> Values
        {
            get
            {
                if (_values == null)
                    _values = new ValueCollection(this);

                return _values;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public ListDictionary() { }
        public ListDictionary(IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer;
        }

        public void Add(TKey key, TValue value) => AddOrSet(key, value, true);
        void AddOrSet(TKey key, TValue value, bool throwExceptionOnDuplicatedKey)
        {
            _version++;

            Node? node = null;
            Node? current;

            for (current = _head; current != null; current = current.Next)
            {
                var nodeKey = current.Key;

                if (_comparer == null ? EqualityComparer<TKey>.Default.Equals(nodeKey, key) : _comparer.Equals(key, nodeKey))
                    if (throwExceptionOnDuplicatedKey)
                        throw new ArgumentException();
                    else
                        break;

                node = current;
            }

            if (current != null)
            {
                current.Value = value;
                return;
            }

            var newNode = new Node(key, value);

            if (node != null)
                node.Next = newNode;
            else
                _head = newNode;

            _count++;
        }
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _version++;

            Node? node = null;
            Node? current;

            for (current = _head; current != null; current = current.Next)
            {
                var nodeKey = current.Key;

                if (_comparer == null ? EqualityComparer<TKey>.Default.Equals(nodeKey, key) : _comparer.Equals(key, nodeKey))
                    break;

                node = current;
            }

            if (current == null)
                return false;

            if (current == _head)
                _head = current.Next;
            else
                node!.Next = current.Next;

            _count--;

            return true;
        }
        public void Clear()
        {
            _count = 0;
            _head = null;
            _version++;
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            for (var current = _head; current != null; current = current.Next)
            {
                var nodeKey = current.Key;

                if (_comparer == null ? EqualityComparer<TKey>.Default.Equals(nodeKey, key) : _comparer.Equals(key, nodeKey))
                    return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            Node? current;

            for (current = _head; current != null; current = current.Next)
            {
                var nodeKey = current.Key;

                if (_comparer == null ? EqualityComparer<TKey>.Default.Equals(nodeKey, key) : _comparer.Equals(key, nodeKey))
                    break;
            }

            if (current == null)
            {
                value = default!;
                return false;
            }

            value = current.Value;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var current = _head; current != null; current = current.Next)
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
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
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0 || array.Length - index < _count)
                throw new ArgumentOutOfRangeException(nameof(index));

            for (var current = _head; current != null; current = current.Next)
                array[index++] = new KeyValuePair<TKey, TValue>(current.Key, current.Value);
        }

        class Node
        {
            public TKey Key { get; }
            public TValue Value { get; set; }

            public Node? Next;

            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public class KeyCollection : ICollection<TKey>
        {
            ListDictionary<TKey, TValue> _owner;

            public int Count => _owner.Count;

            bool ICollection<TKey>.IsReadOnly => true;

            internal KeyCollection(ListDictionary<TKey, TValue> owner)
            {
                _owner = owner;
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                for (var current = _owner._head; current != null; current = current.Next)
                    yield return current.Key;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void CopyTo(TKey[] array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (index < 0 || index > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (array.Length - index < Count)
                    throw new ArgumentException(nameof(index));

                for (var current = _owner._head; current != null; current = current.Next)
                    array[index++] = current.Key;
            }

            void ICollection<TKey>.Add(TKey item) =>throw new NotSupportedException();
            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException();
            void ICollection<TKey>.Clear() => throw new NotSupportedException();
            bool ICollection<TKey>.Contains(TKey item) => _owner.ContainsKey(item);
        }
        public class ValueCollection : ICollection<TValue>
        {
            ListDictionary<TKey, TValue> _owner;

            public int Count => _owner.Count;

            bool ICollection<TValue>.IsReadOnly => true;

            internal ValueCollection(ListDictionary<TKey, TValue> owner)
            {
                _owner = owner;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                for (var current = _owner._head; current != null; current = current.Next)
                    yield return current.Value;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void CopyTo(TValue[] array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (index < 0 || index > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (array.Length - index < Count)
                    throw new ArgumentException(nameof(index));

                for (var current = _owner._head; current != null; current = current.Next)
                    array[index++] = current.Value;
            }

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();
            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();
            void ICollection<TValue>.Clear() => throw new NotSupportedException();
            bool ICollection<TValue>.Contains(TValue item) => throw new NotSupportedException();
        }
    }
}
