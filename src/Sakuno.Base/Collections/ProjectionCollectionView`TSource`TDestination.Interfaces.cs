using System;
using System.Collections;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    partial class ProjectionCollectionView<TSource, TDestination>
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
        bool IList.Contains(object value) => Contains((TDestination)value);
        int IList.IndexOf(object value) => IndexOf((TDestination)value);

        void IList.Insert(int index, object value) => throw new NotSupportedException();
        void IList.Remove(object value) => throw new NotSupportedException();
        void IList.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection.CopyTo(Array array, int index) => _destination.CopyTo((TDestination[])array, index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<TDestination> IEnumerable<TDestination>.GetEnumerator() => GetEnumerator();
    }
}
