using System;
using System.Collections;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    partial class OrderedCollectionView<T>
    {
        bool IList.IsFixedSize => throw new NotSupportedException();
        bool IList.IsReadOnly => true;
        bool ICollection.IsSynchronized => throw new NotSupportedException();
        object ICollection.SyncRoot => throw new NotSupportedException();

        object IList.this[int index]
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
        void ICollection.CopyTo(Array array, int index) => _ordered.CopyTo((T[])array, index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
    }
}
