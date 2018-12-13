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
            using (var enumerator = items.GetEnumerator())
            {
                var last = default(T);

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
        }
    }
}
