using System;
using System.ComponentModel;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class WeakReferenceExtensions
    {
        public static T GetTargetOrDefault<T>(this WeakReference<T> weakReference) where T : class
        {
            weakReference.TryGetTarget(out var result);

            return result;
        }
    }
}
