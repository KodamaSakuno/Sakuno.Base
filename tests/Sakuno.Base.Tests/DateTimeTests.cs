using System;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class DateTimeTests
    {
        static DateTimeOffset Base = new DateTimeOffset(2017, 1, 15, 12, 34, 56, TimeSpan.FromHours(8.0));

        [Fact]
        public static void Tomorrow()
        {
            Assert.Equal(new DateTimeOffset(2017, 1, 16, 0, 0, 0, TimeSpan.FromHours(8.0)), Base.Tomorrow());
        }

        [Fact]
        public static void LastMonday()
        {
            Assert.Equal(new DateTimeOffset(2017, 1, 9, 0, 0, 0, TimeSpan.FromHours(8.0)), Base.LastMonday());
        }
        [Fact]
        public static void NextMonday()
        {
            Assert.Equal(new DateTimeOffset(2017, 1, 16, 0, 0, 0, TimeSpan.FromHours(8.0)), Base.NextMonday());
        }

        [Fact]
        public static void StartOfLastMonth()
        {
            Assert.Equal(new DateTimeOffset(2017, 1, 1, 0, 0, 0, TimeSpan.FromHours(8.0)), Base.StartOfLastMonth());
        }
        [Fact]
        public static void StartOfNextMonth()
        {
            Assert.Equal(new DateTimeOffset(2017, 2, 1, 0, 0, 0, TimeSpan.FromHours(8.0)), Base.StartOfNextMonth());
        }
    }
}
