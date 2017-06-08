using Xunit;

namespace Sakuno.Base.Tests
{
    public static class NumericalCalculationTests
    {
        [Fact]
        public static void MostSignificantBit()
        {
            Assert.Equal(0, Int32Util.HighestBit(0));
            Assert.Equal(7, Int32Util.HighestBit(149));
            Assert.Equal(30, Int32Util.HighestBit(int.MaxValue));
            Assert.Equal(31, UInt32Util.HighestBit(uint.MaxValue));
        }
    }
}
