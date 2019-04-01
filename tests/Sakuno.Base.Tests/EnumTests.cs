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
            Assert.False(f.Has(Flags.E));
            Assert.False(f.Has(Flags.C | Flags.E));
        }

        [Fact]
        public static void HasAny()
        {
            var f = Flags.A | Flags.B | Flags.D;

            Assert.True(f.HasAny(Flags.A));
            Assert.True(f.HasAny(Flags.B));
            Assert.True(f.HasAny(Flags.D));

            Assert.True(f.HasAny(Flags.A | Flags.B));
            Assert.True(f.HasAny(Flags.A | Flags.B | Flags.D));
            Assert.True(f.HasAny(Flags.A | Flags.D));
            Assert.True(f.HasAny(Flags.B | Flags.D));

            Assert.False(f.HasAny(Flags.C));
            Assert.False(f.HasAny(Flags.E));
            Assert.False(f.HasAny(Flags.C | Flags.E));
        }
    }
}
