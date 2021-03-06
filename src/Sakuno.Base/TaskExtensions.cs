using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TaskExtensions
    {
#pragma warning disable IDE0060
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Forget(this Task task) { }
#pragma warning restore IDE0060

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAndUnwarp(this Task task) => task.GetAwaiter().GetResult();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WaitAndUnwarp<T>(this Task<T> task) => task.GetAwaiter().GetResult();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAll(this Task[] tasks) => Task.WaitAll(tasks);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WaitAny(this Task[] tasks) => Task.WaitAny(tasks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks) => Task.WhenAll(tasks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<Task> WhenAny(this IEnumerable<Task> tasks) => Task.WhenAny(tasks);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<Task<T>> WhenAny<T>(this IEnumerable<Task<T>> tasks) => Task.WhenAny(tasks);

#if NETSTANDARD2_1
        public static async IAsyncEnumerable<T> AsAsyncEnumeable<T>(this IEnumerable<Task<T>> tasks)
        {
            foreach (var task in tasks.ToArray())
                yield return await task;
        }
#endif
    }
}
