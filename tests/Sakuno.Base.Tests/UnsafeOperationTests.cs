using System;
using System.Linq;
using Xunit;

namespace Sakuno.Base.Tests
{
    public static unsafe class UnsafeOperationTests
    {
        const int BufferSize = 1024;

        [Fact]
        public static void ZeroMemory()
        {
            var buffer = new byte[BufferSize];

            new Random().NextBytes(buffer);

            fixed (byte* ptr = buffer)
                UnsafeOperations.ZeroMemory(ptr, BufferSize);

            Assert.True(buffer.All(r => r == 0));
        }

        [Fact]
        public static void CopyMemory()
        {
            var source = new byte[BufferSize];
            var destination = new byte[BufferSize];

            new Random().NextBytes(source);

            fixed (byte* sourcePtr = source)
            fixed (byte* destinationPtr = destination)
                UnsafeOperations.CopyMemory(sourcePtr, destinationPtr, BufferSize);

            Assert.True(source.SequenceEqual(destination));
        }
    }
}
