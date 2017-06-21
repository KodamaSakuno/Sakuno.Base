using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DisposableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDisposable CombineWith(this IDisposable x, IDisposable y) => DisposableUtil.Combine(x, y);
    }
}
