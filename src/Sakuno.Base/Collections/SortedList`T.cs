using System;
using System.Collections;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    public sealed class SortedList<T> : IList<T>, IReadOnlyList<T>
    {
        List<T> _list;
        IComparer<T> _comparer;

        public T this[int index]
        {
            get => _list[index];
            set => throw new NotSupportedException();
        }

        public int Count => _list.Count;

        bool ICollection<T>.IsReadOnly => false;

        public SortedList() : this(4, null) { }
        public SortedList(int capacity) : this(capacity, null) { }
        public SortedList(IComparer<T>? comparer) : this(4, comparer) { }
        public SortedList(int capacity, IComparer<T>? comparer)
        {
            _list = new List<T>(capacity);
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public void Add(T item)
        {
            var index = _list.BinarySearch(item, _comparer);

            if (index < 0)
                index = ~index;

            _list.Insert(index, item);
        }

        public bool Remove(T item) => _list.Remove(item);
        public void RemoveAt(int index) => _list.RemoveAt(index);

        public void Clear() => _list.Clear();

        public int IndexOf(T item)
        {
            var result = _list.BinarySearch(item, _comparer);

            return result >= 0 ? result : -1;
        }
        public bool Contains(T item) => _list.BinarySearch(item, _comparer) >= 0;

        public T[] ToArray()
        {
            if (_list.Count == 0)
                return Array.Empty<T>();

            return _list.ToArray();
        }

        public void Insert(int index, T item) => throw new NotSupportedException();

        public List<T>.Enumerator GetEnumerator() => _list.GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
