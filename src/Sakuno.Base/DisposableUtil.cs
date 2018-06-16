using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sakuno
{
    public static class DisposableUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free<T>(ref T value) where T : class, IDisposable =>
            Interlocked.Exchange(ref value, null)?.Dispose();

        public static IDisposable Combine(IDisposable x, IDisposable y)
        {
            if (y == null)
                throw new ArgumentNullException(nameof(y));

            if (x == null)
                return y;

            return Disposable.Create(() =>
            {
                x.Dispose();
                y.Dispose();
            });
        }
    }
}
