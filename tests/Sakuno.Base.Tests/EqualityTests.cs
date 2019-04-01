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

            var num = .1;
            num += .2;
            Assert.False(num == .3);
            Assert.True(DoubleUtil.AreClose(num, .3));
        }
    }
}
