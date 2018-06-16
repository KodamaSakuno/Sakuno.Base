using Sakuno.Collections;
using System.Collections.ObjectModel;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class ProjectionCollectionTests
    {
        [Fact]
        public static void SimpleProjection()
        {
            var source = new ObservableCollection<int>();
            var projection = new ProjectionCollection<int, int>(source, r => r * 2);

            source.Add(1);
            source.Add(2);
            source.Add(3);
            source.Add(4);
            source.Add(5);

            source.Remove(3);
            source.Insert(1, 6);
            source.Insert(2, 10);

            Assert.Equal(projection.Count, source.Count);

            using (var projectionEnumerator = projection.GetEnumerator())
            using (var sourceEnumerator = source.GetEnumerator())
            {
                while (projectionEnumerator.MoveNext() && sourceEnumerator.MoveNext())
                    Assert.Equal(sourceEnumerator.Current * 2, projectionEnumerator.Current);
            }

            source.Clear();

            Assert.Empty(projection);

            projection.Dispose();
        }
    }
}
