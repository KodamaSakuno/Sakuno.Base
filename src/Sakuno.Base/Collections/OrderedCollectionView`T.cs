using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Sakuno.Collections
{
    public sealed partial class OrderedCollectionView<T> : DisposableObject, IReadOnlyList<T>, IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        readonly IReadOnlyList<T> _source;
        readonly Predicate<string> _shouldUpdate;

        readonly List<T> _sourceSnapshot;
        readonly List<T> _ordered;
        readonly HashSet<INotifyPropertyChanged> _notifyPropertyChanged;

        readonly IComparer<T> _comparer;

        public int Count => _ordered.Count;

        public T this[int index] => _ordered[index];

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public OrderedCollectionView(IReadOnlyList<T> source) : this(source, null) { }
        public OrderedCollectionView(IReadOnlyList<T> source, Predicate<string> shouldUpdate)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _shouldUpdate = shouldUpdate;

            _ordered = new List<T>(_source.Count + 4);
            _notifyPropertyChanged = new HashSet<INotifyPropertyChanged>();

            _sourceSnapshot = new List<T>(_source.Count + 4);
            _sourceSnapshot.AddRange(source);

            _comparer = Comparer<T>.Default;

            ProjectFromSource();

            if (_source is INotifyCollectionChanged sourceCollectionChanged)
                sourceCollectionChanged.CollectionChanged += OnSourceCollectionChanged;
            else if (shouldUpdate == null)
                GC.SuppressFinalize(this);
        }

        void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        var newItem = (T)e.NewItems[i];
                        _sourceSnapshot.Insert(e.NewStartingIndex + i, newItem);

                        var index = _ordered.BinarySearch(newItem, _comparer).EnsurePositiveIndex();

                        _ordered.Insert(index, newItem);
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, index));

                        if (_shouldUpdate != null && newItem is INotifyPropertyChanged notifyPropertyChanged && _notifyPropertyChanged.Add(notifyPropertyChanged))
                            notifyPropertyChanged.PropertyChanged += OnItemPropertyChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T oldItem in e.OldItems)
                    {
                        _sourceSnapshot.RemoveAt(e.OldStartingIndex);

                        if (_shouldUpdate != null && oldItem is INotifyPropertyChanged notifyPropertyChanged && _notifyPropertyChanged.Remove(notifyPropertyChanged))
                            notifyPropertyChanged.PropertyChanged -= OnItemPropertyChanged;

                        var index = _ordered.BinarySearch(oldItem, _comparer);

                        _ordered.RemoveAt(index);
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        var oldItem = (T)e.OldItems[0];

                        if (_shouldUpdate != null && oldItem is INotifyPropertyChanged oldNotifyPropertyChanged && _notifyPropertyChanged.Remove(oldNotifyPropertyChanged))
                            oldNotifyPropertyChanged.PropertyChanged -= OnItemPropertyChanged;

                        var oldIndex = _ordered.BinarySearch(oldItem, _comparer);
                        _ordered.RemoveAt(oldIndex);

                        var newItem = (T)e.NewItems[0];
                        var newIndex = _ordered.BinarySearch(newItem, _comparer).EnsurePositiveIndex();

                        _sourceSnapshot[e.NewStartingIndex] = newItem;
                        _ordered.Insert(newIndex, newItem);

                        if (_shouldUpdate != null && newItem is INotifyPropertyChanged newNotifyPropertyChanged && _notifyPropertyChanged.Add(newNotifyPropertyChanged))
                            newNotifyPropertyChanged.PropertyChanged += OnItemPropertyChanged;

                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, oldIndex));
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, newIndex));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _notifyPropertyChanged)
                        item.PropertyChanged -= OnItemPropertyChanged;

                    _notifyPropertyChanged.Clear();
                    _ordered.Clear();
                    _sourceSnapshot.Clear();

                    NotifyCollectionChanged(EventArgsCache.CollectionChanged.Reset);
                    break;
            }
        }

        void ProjectFromSource()
        {
            foreach (var item in _source)
            {
                if (_shouldUpdate != null && item is INotifyPropertyChanged notifyPropertyChanged && _notifyPropertyChanged.Add(notifyPropertyChanged))
                    notifyPropertyChanged.PropertyChanged += OnItemPropertyChanged;

                var index = _ordered.BinarySearch(item, _comparer).EnsurePositiveIndex();

                _ordered.Insert(index, item);
            }
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_shouldUpdate == null || !_shouldUpdate(e.PropertyName) || _ordered.Count == 1)
                return;

            var item = (T)sender;
            var oldIndex = _ordered.IndexOf(item);
            var newIndex = _ordered.BinarySearch(0, oldIndex, item, _comparer).EnsurePositiveIndex();

            if (newIndex == oldIndex)
            {
                newIndex = _ordered.BinarySearch(oldIndex + 1, _ordered.Count - oldIndex - 1, item, _comparer).EnsurePositiveIndex() - 1;

                if (newIndex == oldIndex)
                    return;
            }

            _ordered.RemoveAt(oldIndex);

            _ordered.Insert(newIndex, item);
            NotifyCollectionItemChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, new[] { item }, newIndex, oldIndex));
        }

        public int IndexOf(T item) => _ordered.IndexOf(item);
        public bool Contains(T item) => IndexOf(item) != -1;

        public List<T>.Enumerator GetEnumerator() => _ordered.GetEnumerator();

        void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, EventArgsCache.PropertyChanged.Count);
                propertyChanged(this, EventArgsCache.PropertyChanged.Indexer);
            }

            CollectionChanged?.Invoke(this, e);
        }
        void NotifyCollectionItemChanged(NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, EventArgsCache.PropertyChanged.Indexer);
            CollectionChanged?.Invoke(this, e);
        }

        protected override void DisposeNativeResources()
        {
            if (_source is INotifyCollectionChanged sourceCollectionChanged)
                sourceCollectionChanged.CollectionChanged -= OnSourceCollectionChanged;

            foreach (var item in _notifyPropertyChanged)
                item.PropertyChanged -= OnItemPropertyChanged;
        }
    }
}
