using System;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static class EnumTests
    {
        [Flags]
        public enum Flags
        {
            A = 1,
            B = 1 << 1,
            C = 1 << 2,
            D = 1 << 3,
            E = 1 << 4,
        }

        [Fact]
        public static void Has()
        {
            var f = Flags.A | Flags.B | Flags.D;

            Assert.True(f.Has(Flags.A));
            Assert.True(f.Has(Flags.B));
            Assert.True(f.Has(Flags.D));

            Assert.False(f.Has(Flags.C));
        }
    }
}
