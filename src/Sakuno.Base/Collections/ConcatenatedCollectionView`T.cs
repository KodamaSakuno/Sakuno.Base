using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Sakuno.Collections
{
    public sealed partial class ConcatenatedCollectionView<T> : DisposableObject, IReadOnlyList<T>, IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        readonly IReadOnlyList<IReadOnlyList<T>> _sources;
        readonly List<Node> _nodes;
        readonly IDictionary<IReadOnlyList<T>, Node> _nodeMap;

        readonly List<INotifyCollectionChanged> _notifyCollectionChanged = new List<INotifyCollectionChanged>();

        public int Count
        {
            get
            {
                var result = 0;

                foreach (var node in _nodes)
                    result += node.Snapshot.Count;

                return result;
            }
        }

        public T this[int index]
        {
            get
            {
                foreach (var node in _nodes)
                {
                    if (index < node.Snapshot.Count)
                        return node.Snapshot[index];

                    index -= node.Snapshot.Count;
                }

                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ConcatenatedCollectionView(IReadOnlyList<IReadOnlyList<T>> source)
        {
            _sources = source ?? throw new ArgumentNullException(nameof(source));
            _nodes = new List<Node>(source.Count + 4);
            _nodeMap = new Dictionary<IReadOnlyList<T>, Node>();

            ProjectFromSource();

            if (_sources is INotifyCollectionChanged sourceCollectionChanged)
                sourceCollectionChanged.CollectionChanged += OnSourceCollectionChanged;
            else
                GC.SuppressFinalize(this);
        }

        void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var newItems = new List<T>();

                        for (var i = 0; i < e.NewItems.Count; i++)
                        {
                            var newItem = (IReadOnlyList<T>)e.NewItems[i];
                            var node = AddItem(newItem, e.NewStartingIndex + i);

                            newItems.AddRange(node.Snapshot);

                            if (newItem is INotifyCollectionChanged notifyCollectionChanged && !_notifyCollectionChanged.Contains(notifyCollectionChanged))
                            {
                                _notifyCollectionChanged.Add(notifyCollectionChanged);

                                notifyCollectionChanged.CollectionChanged += OnSubCollectionChanged;
                            }
                        }

                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, GetOffset(e.NewStartingIndex)));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        var oldItems = new List<T>();
                        var offset = GetOffset(e.OldStartingIndex);

                        foreach (IReadOnlyList<T> oldItem in e.OldItems)
                        {
                            var node = RemoveItem(oldItem, e.OldStartingIndex);

                            oldItems.AddRange(node.Snapshot);

                            if (oldItem is INotifyCollectionChanged notifyCollectionChanged && _notifyCollectionChanged.Contains(notifyCollectionChanged))
                            {
                                _notifyCollectionChanged.Remove(notifyCollectionChanged);

                                notifyCollectionChanged.CollectionChanged -= OnSubCollectionChanged;
                            }
                        }

                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, offset));
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        var oldItem = (IReadOnlyList<T>)e.OldItems[0];
                        var newItem = (IReadOnlyList<T>)e.NewItems[0];
                        var node = RemoveItem(oldItem, e.NewStartingIndex);
                        var oldItems = node.Snapshot;

                        node = AddItem(newItem, e.NewStartingIndex);

                        if (oldItem is INotifyCollectionChanged notifyCollectionChanged && _notifyCollectionChanged.Contains(notifyCollectionChanged))
                        {
                            _notifyCollectionChanged.Remove(notifyCollectionChanged);

                            notifyCollectionChanged.CollectionChanged -= OnSubCollectionChanged;
                        }

                        if (newItem is INotifyCollectionChanged notifyCollectionChanged2 && !_notifyCollectionChanged.Contains(notifyCollectionChanged2))
                        {
                            _notifyCollectionChanged.Add(notifyCollectionChanged2);

                            notifyCollectionChanged2.CollectionChanged += OnSubCollectionChanged;
                        }

                        var offset = GetOffset(e.NewStartingIndex);
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, offset));
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node.Snapshot, offset));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _notifyCollectionChanged)
                        item.CollectionChanged -= OnSubCollectionChanged; ;
                    _notifyCollectionChanged.Clear();

                    _nodes.Clear();

                    if (_sources.Count > 0)
                        ProjectFromSource();

                    NotifyCollectionChanged(EventArgsCache.CollectionChanged.Reset);
                    break;
            }
        }

        void ProjectFromSource()
        {
            for (var i = 0; i < _sources.Count; i++)
            {
                var item = _sources[i];

                AddItem(item, i);

                if (item is INotifyCollectionChanged notifyCollectionChanged && !_notifyCollectionChanged.Contains(notifyCollectionChanged))
                {
                    _notifyCollectionChanged.Add(notifyCollectionChanged);

                    notifyCollectionChanged.CollectionChanged += OnSubCollectionChanged;
                }
            }
        }

        Node AddItem(IReadOnlyList<T> collection, int index)
        {
            if (!_nodeMap.TryGetValue(collection, out var result))
                _nodeMap[collection] = result = new Node(collection);

            result.Indexes.Add(index);
            _nodes.Insert(index, result);

            return result;
        }
        Node RemoveItem(IReadOnlyList<T> collection, int index)
        {
            var result = _nodeMap[collection];

            result.Indexes.Remove(index);
            _nodes.RemoveAt(index);

            if (result.Indexes.Count == 0)
                _nodeMap.Remove(collection);

            return result;
        }

        void OnSubCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var source = (IReadOnlyList<T>)sender;
            var node = _nodeMap[source];
            var snapshot = node.Snapshot;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (var i = 0; i < e.NewItems.Count; i++)
                        snapshot.Insert(e.NewStartingIndex + i, (T)e.NewItems[i]);

                    foreach (var index in node.Indexes)
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems, GetOffset(index) + e.NewStartingIndex));
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (var i = 0; i < e.OldItems.Count; i++)
                        snapshot.RemoveAt(e.OldStartingIndex);

                    foreach (var index in node.Indexes)
                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems, GetOffset(index) + e.OldStartingIndex));
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var oldItem = snapshot[e.NewStartingIndex];
                    var newItem = (T)e.NewItems[0];

                    snapshot[e.NewStartingIndex] = newItem;

                    foreach (var index in node.Indexes)
                        NotifyCollectionItemChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, GetOffset(index) + e.NewStartingIndex));
                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    {
                        var oldItems = snapshot.ToArray();

                        snapshot.Clear();
                        if (node.Source.Count > 0)
                            snapshot.AddRange(node.Source);

                        foreach (var index in node.Indexes)
                            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, GetOffset(index)));

                        if (snapshot.Count > 0)
                            foreach (var index in node.Indexes)
                                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, snapshot, GetOffset(index)));
                    }
                    break;
            }
        }

        int GetOffset(int index)
        {
            var result = 0;

            for (var i = 0; i < index; i++)
                result += _nodes[i].Snapshot.Count;

            return result;
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                var result = node.Snapshot.IndexOf(item);
                if (result != -1)
                    return result + GetOffset(i);
            }

            return -1;
        }
        public bool Contains(T item) => IndexOf(item) != -1;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var node in _nodes)
                foreach (var item in node.Snapshot)
                    yield return item;
        }

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
            foreach (var item in _notifyCollectionChanged)
                item.CollectionChanged -= OnSubCollectionChanged;
            _notifyCollectionChanged.Clear();

            if (_sources is INotifyCollectionChanged sourceCollectionChanged)
                sourceCollectionChanged.CollectionChanged -= OnSourceCollectionChanged;
        }

        class Node
        {
            public IReadOnlyList<T> Source { get; }
            public List<T> Snapshot { get; }

            public ISet<int> Indexes { get; }

            public Node(IReadOnlyList<T> source)
            {
                Source = source;
                Snapshot = new List<T>(source);

                Indexes = new HashSet<int>();
            }
        }
    }
}
