using System;
using System.Collections.Generic;
using System.Linq;

namespace Houseplants.Common
{
    public static class EnumerableExtensions
    {
        public static void ForAll<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection.ToList())
            {
                action(item);
            }
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            var pos = 0;
            while (source.Skip(pos).Any())
            {
                yield return source.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }

        public static IEnumerable<T> Evens<T>(this IEnumerable<T> collection)
        {
            return collection.Where((r, i) => i % 2 == 0);
        }

        public static IEnumerable<T> Odds<T>(this IEnumerable<T> collection)
        {
            return collection.Where((r, i) => i % 2 != 0);
        }

        public static bool AllUnique<T>(this IEnumerable<T> collection)
        {
            return collection.Distinct().Count() == collection.Count();
        }

        public static bool AllUnique<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> selector)
        {
            return collection.Select(x => selector(x)).Distinct().Count() == collection.Count();
        }

        public static bool IsSingle<T>(this IEnumerable<T> collection)
        {
            return !collection.Any() || collection.Distinct().Count() == 1;
        }

        public static bool IsSingle<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> selector)
        {
            return !collection.Any() || collection.Select(x => selector(x)).Distinct().Count() == 1;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keyExtractor, IEqualityComparer<TKey> comparer = null)
        {
            return source.Distinct(Compare.By(keyExtractor, comparer));
        }

        /// <summary>
        /// True if two lists contain same items.
        /// from http://stackoverflow.com/questions/3669970/compare-two-listt-objects-for-equality-ignoring-order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        /// <summary>
        /// True if items ordered by not descending key (next >= prev).
        /// </summary>
        public static bool IsOrdered<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keyExtractor)
            where TKey : IComparable<TKey>
        {
            return items.Zip(items.Skip(1), (a, b) => new { a, b })
                .All(x => !(keyExtractor(x.a).CompareTo(keyExtractor(x.b)) > 0));
        }

        /// <summary>
        /// True if items ordered by ascending key without equal keys (next > prev).
        /// </summary>
        public static bool IsStrongOrdered<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keyExtractor)
            where TKey : IComparable<TKey>
        {
            return items.Zip(items.Skip(1), (a, b) => new { a, b })
                .All(x => keyExtractor(x.a).CompareTo(keyExtractor(x.b)) < 0);
        }
    }
}