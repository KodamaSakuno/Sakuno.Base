using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static unsafe class UnsafeOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory(void* address, int count) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(void* source, void* destination, int count) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2 As<T1, T2>(T1 value) where T1 : struct where T2 : struct => default(T2);
    }
}
