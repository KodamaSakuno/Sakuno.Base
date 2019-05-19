using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Sakuno.Collections
{
    public sealed partial class ProjectionCollectionView<TSource, TDestination> : DisposableObject, IReadOnlyList<TDestination>, IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        readonly IReadOnlyList<TSource> _source;
        readonly IProjector<TSource, TDestination> _projector;

        readonly List<TSource> _sourceSnapshot;
        readonly List<TDestination> _destination;

        public int Count => _destination.Count;

        public TDestination this[int index] => _destination[index];

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ProjectionCollectionView(IReadOnlyList<TSource> source, Func<TSource, TDestination> projector)
            : this(source, projector != null ? new DelegatedProjector<TSource, TDestination>(projector) : null) { }
        public ProjectionCollectionView(IReadOnlyList<TSource> source, IProjector<TSource, TDestination> projector)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _projector = projector ?? throw new ArgumentNullException(nameof(projector));

            _sourceSnapshot = new List<TSource>(_source.Count + 4);
            _sourceSnapshot.AddRange(_source);

            _destination = new List<TDestination>(_source.Count + 4);
            ProjectFromSource();

            if (_source is INotifyCollectionChanged sourceCollectionChanged)
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
                        var newItems = new TDestination[e.NewItems.Count];

                        for (var i = 0; i < newItems.Length; i++)
                        {
                            var newSourceItem = (TSource)e.NewItems[i];
                            var newItem = _projector.Project(newSourceItem);

                            _sourceSnapshot.Insert(e.NewStartingIndex + i, newSourceItem);
                            _destination.Insert(e.NewStartingIndex + i, newItem);

                            newItems[i] = newItem;
                        }

                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, e.NewStartingIndex));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        var oldItems = new TDestination[e.OldItems.Count];

                        for (var i = 0; i < oldItems.Length; i++)
                        {
                            var oldSourceItem = (TSource)e.OldItems[i];

                            oldItems[i] = _destination[e.OldStartingIndex];

                            _sourceSnapshot.RemoveAt(e.OldStartingIndex);
                            _destination.RemoveAt(e.OldStartingIndex);
                        }

                        NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, e.OldStartingIndex));
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        var newSourceItem = (TSource)e.NewItems[0];

                        var oldItem = _destination[e.OldStartingIndex];
                        var newItem = _projector.Project(newSourceItem);

                        _sourceSnapshot[e.OldStartingIndex] = newSourceItem;
                        _destination[e.OldStartingIndex] = newItem;

                        NotifyCollectionItemChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, e.NewStartingIndex));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    var movedItem = _destination[e.OldStartingIndex];
                    var movedItemOfSource = _sourceSnapshot[e.OldStartingIndex];

                    _destination.RemoveAt(e.OldStartingIndex);
                    _sourceSnapshot.RemoveAt(e.OldStartingIndex);

                    _destination.Insert(e.NewStartingIndex, movedItem);
                    _sourceSnapshot.Insert(e.NewStartingIndex, movedItemOfSource);

                    NotifyCollectionItemChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItem, e.NewStartingIndex, e.OldStartingIndex));
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
            }
        }

        public void Reset()
        {
            _destination.Clear();
            _sourceSnapshot.Clear();

            if (_source.Count > 0)
            {
                _sourceSnapshot.AddRange(_source);
                ProjectFromSource();
            }

            NotifyCollectionChanged(EventArgsCache.CollectionChanged.Reset);
        }

        void ProjectFromSource()
        {
            for (var i = 0; i < _source.Count; i++)
                _destination.Insert(i, _projector.Project(_source[i]));
        }

        public int IndexOf(TDestination item) => _destination.IndexOf(item);
        public bool Contains(TDestination item) => IndexOf(item) != -1;

        public List<TDestination>.Enumerator GetEnumerator() => _destination.GetEnumerator();

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
        }
    }
}
