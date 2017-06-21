using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class DisposableUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free<T>(ref T value) where T : class, IDisposable
        {
            if (value == null)
                return;

            value.Dispose();
            value = null;
        }

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
