using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class WeakReferenceExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetTargetOrDefault<T>(this WeakReference<T> weakReference) where T : class
        {
            weakReference.TryGetTarget(out var result);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive<T>(this WeakReference<T> weakReference) where T : class =>
            weakReference.TryGetTarget(out _);
    }
}
