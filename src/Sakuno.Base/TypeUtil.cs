using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class TypeUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>() => 0;
    }
}
