using Sakuno.Collections;
using System;
using System.Collections.ObjectModel;
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

            Assert.Equal(source.Count, projection.Count);
            Assert.Equal(source.Select(r => r * 2), projection);

            source.Clear();

            Assert.Empty(projection);
        }
    }
}
