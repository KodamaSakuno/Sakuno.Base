using Xunit;

namespace Sakuno.Base.Tests
{
    public static class EqualityTests
    {
        [Fact]
        public static void FloatingNumber()
        {
            Assert.True(DoubleUtil.IsCloseToZero(.0));
            Assert.True(DoubleUtil.IsCloseToOne(1.0));
        }
    }
}
