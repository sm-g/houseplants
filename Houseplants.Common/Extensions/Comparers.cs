using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Houseplants.Common
{
    internal static class Compare
    {
        public static IEqualityComparer<TSource> By<TSource, TKey>(Func<TSource, TKey> identitySelector, IEqualityComparer<TKey> comparer = null)
        {
            return new KeyEqualityComparer<TSource, TKey>(identitySelector, comparer);
        }
    }

    public class KeyEqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        protected readonly Func<T, TKey> keyExtractor;
        protected readonly IEqualityComparer<TKey> comparer;

        public KeyEqualityComparer(Func<T, TKey> keyExtractor, IEqualityComparer<TKey> comparer = null)
        {
            this.keyExtractor = keyExtractor;
            this.comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public virtual bool Equals(T x, T y)
        {
            return comparer.Equals(this.keyExtractor(x), this.keyExtractor(y));
        }

        public int GetHashCode(T obj)
        {
            return comparer.GetHashCode(this.keyExtractor(obj));
        }
    }

    public class StrictKeyEqualityComparer<T, TKey> : KeyEqualityComparer<T, TKey>
        where TKey : IEquatable<TKey>
    {
        public StrictKeyEqualityComparer(Func<T, TKey> keyExtractor)
            : base(keyExtractor)
        { }

        public override bool Equals(T x, T y)
        {
            // This will use the overload that accepts a TKey parameter
            // instead of an object parameter.
            return this.keyExtractor(x).Equals(this.keyExtractor(y));
        }
    }
}
