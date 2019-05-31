using Sakuno.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class CollectionViewTests
    {
        [Fact]
        public static void SimpleProjectionCollectionView()
        {
            var source = Enumerable.Range(0, 100).ToArray();
            var projection = new ProjectionCollectionView<int, int>(source, r => r * 2);

            Assert.Equal(source.Select(r => r * 2), projection);
        }
        [Fact]
        public static void ProjectionCollectionView()
        {
            var source = new ObservableCollection<int>();
            using var projection = new ProjectionCollectionView<int, int>(source, r => r * 2);
            using var builder = new CollectionBuilder<int>(projection);

            DoTest(source);

            Assert.Equal(source.Count, projection.Count);
            Assert.Equal(source.Select(r => r * 2), projection);

            source.Clear();

            Assert.Empty(projection);

            DoTest(source);

            Assert.Equal(source.Count, projection.Count);
            Assert.Equal(source.Select(r => r * 2), projection);
            Assert.Equal(source.Select(r => r * 2), builder);
        }

        [Fact]
        public static void SimpleFilteredCollectionView()
        {
            var source = Enumerable.Range(0, 100).ToArray();
            var filtered = new FilteredCollectionView<int>(source, r => r % 2 == 0);

            Assert.Equal(source.Where(r => r % 2 == 0), filtered);
        }
        [Fact]
        public static void FilteredCollectionView()
        {
            var source = new ObservableCollection<int>();
            using var filtered = new FilteredCollectionView<int>(source, r => r % 2 == 0);
            using var builder = new CollectionBuilder<int>(filtered);

            DoTest(source);

            Assert.Equal(source.Where(r => r % 2 == 0), filtered);

            source.Clear();

            Assert.Empty(filtered);

            DoTest(source);

            Assert.Equal(source.Where(r => r % 2 == 0), filtered);
            Assert.Equal(source.Where(r => r % 2 == 0), builder);
        }

        [Fact]
        public static void CollectionViewFilteredByProperty()
        {
            var source = Enumerable.Range(0, 100).Select(r => new Item(r)).ToArray();
            var filtered = new FilteredCollectionView<Item>(source, r => r.Value % 2 == 0, propertyName => propertyName == nameof(Item.Value));
            using var builder = new CollectionBuilder<Item>(filtered);

            DoTest(source);

            Assert.Equal(source.Where(r => r.Value % 2 == 0), filtered);
            Assert.Equal(source.Where(r => r.Value % 2 == 0), builder);
        }

        [Fact]
        public static void SimpleOrderedCollectionView()
        {
            var random = new Random();
            var source = Enumerable.Range(0, 100).Select(_ => random.Next(0, 100)).ToArray();
            var ordered = new OrderedCollectionView<int>(source, null);

            Assert.Equal(source.OrderBySelf(), ordered);
        }
        [Fact]
        public static void OrderedCollectionView()
        {
            var source = new ObservableCollection<int>();
            using var ordered = new OrderedCollectionView<int>(source, null);
            using var builder = new CollectionBuilder<int>(ordered);

            DoTest(source);

            Assert.Equal(source.OrderBySelf(), ordered);

            source.Clear();

            Assert.Empty(ordered);

            DoTest(source);

            Assert.Equal(source.OrderBySelf(), ordered);
            Assert.Equal(source.OrderBySelf(), builder);
        }

        [Fact]
        public static void CollectionViewOrderedByProperty()
        {
            var source = Enumerable.Range(0, 100).Select(r => new Item(r)).ToArray();
            var ordered = new OrderedCollectionView<Item>(source, propertyName => propertyName == nameof(Item.Value));
            using var builder = new CollectionBuilder<Item>(ordered);

            DoTest(source);

            Assert.Equal(source.OrderBy(r => r.Value), ordered);
            Assert.Equal(source.OrderBy(r => r.Value), builder);
        }

        static void DoTest(ObservableCollection<int> source)
        {
            for (var i = 0; i < 100; i++)
                source.Add(i);

            var random = new Random();

            for (var i = 0; i < 20; i++)
            {
                var index = random.Next(0, source.Count);

                source.RemoveAt(index);
            }

            for (var i = 0; i < 20; i++)
            {
                var index = random.Next(0, source.Count);

                source.Insert(index, random.Next(0, 1000));
            }

            for (var i = 0; i < 20; i++)
            {
                var index = random.Next(0, source.Count);

                source[index] = random.Next(0, 1000);
            }
        }
        static void DoTest(IReadOnlyList<Item> source)
        {
            var random = new Random();

            for (var i = 0; i < 80; i++)
            {
                var index = random.Next(0, source.Count);

                source[index].Value = random.Next(0, 1000);
            }
        }

        sealed class Item : INotifyPropertyChanged, IComparable<Item>
        {
            static readonly PropertyChangedEventArgs EventArgs = new PropertyChangedEventArgs(nameof(Value));

            int _value;
            public int Value
            {
                get => _value;
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        PropertyChanged?.Invoke(this, EventArgs);
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public Item(int value)
            {
                _value = value;
            }

            public int CompareTo(Item other) => _value - other._value;
        }
    }
}
