using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static unsafe class UnsafeOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory(void* address, int count) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* source, void* destination, int count) { }
    }
}
