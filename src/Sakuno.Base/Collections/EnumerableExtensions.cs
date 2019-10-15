using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sakuno.Collections
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumerableExtensions
    {
        public static bool AnyNull<T>(this IEnumerable<T> source) where T : class
        {
            foreach (var item in source)
                if (item == null)
                    return true;

            return false;
        }

        public static IEnumerable<TSource> NotOfType<TSource, TExclusion>(this IEnumerable<TSource> source)
        {
            foreach (var item in source)
            {
                if (item is TExclusion)
                    continue;

                yield return item;
            }
        }

        public static IEnumerable<(TSource Item, int Index)> WithIndex<TSource>(this IEnumerable<TSource> source)
        {
            var index = -1;

            foreach (var item in source)
            {
                index = checked(index + 1);

                yield return (item, index);
            }
        }

        public static IEnumerable<T> OrderBySelf<T>(this IEnumerable<T> source) => source.OrderBy(IdentityFunction<T>.Instance);
        public static IEnumerable<T> OrderBySelf<T>(this IEnumerable<T> items, IComparer<T> comparer) => items.OrderBy(IdentityFunction<T>.Instance, comparer);

        public static IEnumerable<(T Item, bool IsLast)> EnumerateItemAndIfIsLast<T>(this IEnumerable<T> items)
        {
            using var enumerator = items.GetEnumerator();

            var last = default(T)!;

            if (!enumerator.MoveNext())
                yield break;

            var shouldYield = false;

            do
            {
                if (shouldYield)
                    yield return (last, false);

                shouldYield = true;
                last = enumerator.Current;
            } while (enumerator.MoveNext());

            yield return (last, true);
        }

        public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> resultSelector) where TResult : class
        {
            foreach (var element in source)
            {
                var result = resultSelector(element);
                if (result == null)
                    continue;

                yield return result;
            }
        }

        public static IEnumerable<(TSource, TResult)> SelectManyWithSelf<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> collectionSelector)
        {
            foreach (var element in source)
                foreach (var item in collectionSelector(element))
                    yield return (element, item);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            MaxBy(source, keySelector, Comparer<TKey>.Default);
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new InvalidOperationException("There's no any element in the collection.");

            var result = enumerator.Current;
            var resultKey = keySelector(result);

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var currentKey = keySelector(current);

                if (comparer.Compare(currentKey, resultKey) <= 0)
                    continue;

                result = current;
                resultKey = currentKey;
            }

            return result;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            MinBy(source, keySelector, Comparer<TKey>.Default);
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new InvalidOperationException("There's no any element in the collection.");

            var result = enumerator.Current;
            var resultKey = keySelector(result);

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var currentKey = keySelector(current);

                if (comparer.Compare(currentKey, resultKey) >= 0)
                    continue;

                result = current;
                resultKey = currentKey;
            }

            return result;
        }
    }
}
