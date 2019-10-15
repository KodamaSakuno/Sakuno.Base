using System;
using System.Collections;

namespace Sakuno.Collections
{
    partial class FilteredCollectionView<T>
    {
        bool IList.IsFixedSize => throw new NotSupportedException();
        bool IList.IsReadOnly => true;
        bool ICollection.IsSynchronized => throw new NotSupportedException();
        object ICollection.SyncRoot => throw new NotSupportedException();

        object? IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        int IList.Add(object value) => throw new NotSupportedException();
        void IList.Clear() => throw new NotSupportedException();
        bool IList.Contains(object value) => Contains((T)value);

        int IList.IndexOf(object value) => IndexOf((T)value);

        void IList.Insert(int index, object value) => throw new NotSupportedException();
        void IList.Remove(object value) => throw new NotSupportedException();
        void IList.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1 || !(array is T[] target))
                throw new ArgumentException(nameof(array));
            if (array.Length < _indexes.Count + index)
                throw new IndexOutOfRangeException();

            for (var i = 0; i < _indexes.Count; i++)
                target[index + i] = _sourceSnapshot[_indexes[i]];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
