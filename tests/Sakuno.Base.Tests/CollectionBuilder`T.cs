using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Sakuno.Base.Tests
{
    sealed class CollectionBuilder<T> : IDisposable, IEnumerable<T>
    {
        IReadOnlyList<T> _source;
        INotifyCollectionChanged _notifyCollectionChanged;

        List<T> _list = new List<T>();

        public CollectionBuilder(IReadOnlyList<T> source)
        {
            _source = source;
            _notifyCollectionChanged = _source as INotifyCollectionChanged ?? throw new ArgumentException(nameof(source));

            _notifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;

            _list.AddRange(_source);
        }

        private void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        var newItem = (T)e.NewItems[i];

                        _list.Insert(e.NewStartingIndex + i, newItem);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (var i = 0; i < e.OldItems.Count; i++)
                        _list.RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Move:
                    _list.RemoveAt(e.OldStartingIndex);
                    _list.Insert(e.NewStartingIndex, (T)e.NewItems[0]);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    _list[e.NewStartingIndex] = (T)e.NewItems[0];
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _list.Clear();

                    if (_source.Count > 0)
                        _list.AddRange(_source);
                    break;
            }
        }
        public void Dispose() => _notifyCollectionChanged.CollectionChanged -= NotifyCollectionChanged_CollectionChanged;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
