using Sakuno.Collections;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class CollectionViewTests
    {
        [Fact]
        public static void SimpleProjection()
        {
            var source = new ObservableCollection<int>();
            using var projection = new ProjectionCollectionView<int, int>(source, r => r * 2);

            DoTest(source);

            Assert.Equal(source.Count, projection.Count);
            Assert.Equal(source.Select(r => r * 2), projection);

            source.Clear();

            Assert.Empty(projection);

            DoTest(source);

            Assert.Equal(source.Count, projection.Count);
            Assert.Equal(source.Select(r => r * 2), projection);
        }

        [Fact]
        public static void SimpleFiltering()
        {
            var source = new ObservableCollection<int>();
            using var filtered = new FilteredCollectionView<int>(source, r => r % 2 == 0);

            DoTest(source);

            Assert.Equal(source.Where(r => r % 2 == 0), filtered);

            source.Clear();

            Assert.Empty(filtered);

            DoTest(source);

            Assert.Equal(source.Where(r => r % 2 == 0), filtered);
        }

        [Fact]
        public static void FilterByProperty()
        {
            var source = new ObservableCollection<Item>(Enumerable.Range(0, 100).Select(r => new Item(r)));
            using var filtered = new FilteredCollectionView<Item>(source, r => r.Value % 2 == 0, propertyName => propertyName == nameof(Item.Value));

            DoTest(source);

            Assert.Equal(source.Where(r => r.Value % 2 == 0), filtered);
        }

        [Fact]
        public static void SimpleOrdering()
        {
            var source = new ObservableCollection<int>();
            using var ordered = new OrderedCollectionView<int>(source, null);

            DoTest(source);

            Assert.Equal(source.OrderBySelf(), ordered);

            source.Clear();

            Assert.Empty(ordered);

            DoTest(source);

            Assert.Equal(source.OrderBySelf(), ordered);
        }

        [Fact]
        public static void OrderByProperty()
        {
            var source = new ObservableCollection<Item>(Enumerable.Range(0, 100).Select(r => new Item(r)));
            using var ordered = new OrderedCollectionView<Item>(source, propertyName => propertyName == nameof(Item.Value));

            DoTest(source);

            Assert.Equal(source.OrderBy(r => r.Value), ordered);
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
        static void DoTest(ObservableCollection<Item> source)
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
