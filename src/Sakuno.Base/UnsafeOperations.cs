using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static unsafe class UnsafeOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory(void* address, int count) =>
            Unsafe.InitBlock(address, 0, (uint)count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* source, void* destination, int count) =>
            Unsafe.CopyBlock(destination, source, (uint)count);
    }
}
