using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ServiceProviderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? GetService<T>(this IServiceProvider serviceProvider) where T : class =>
            serviceProvider.GetService(typeof(T)) as T;
    }
}
