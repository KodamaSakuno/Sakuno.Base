﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sakuno
{
    public static class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Forget(this Task task) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAll(this Task[] tasks) => Task.WaitAll(tasks);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAny(this Task[] tasks) => Task.WaitAny(tasks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks) => Task.WhenAll(tasks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<Task> WhenAny(this IEnumerable<Task> tasks) => Task.WhenAny(tasks);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<Task<T>> WhenAny<T>(this IEnumerable<Task<T>> tasks) => Task.WhenAny(tasks);
    }

}