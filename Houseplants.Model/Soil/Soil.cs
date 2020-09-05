using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Houseplants.Common;
using Houseplants.Model.Helpers;

namespace Houseplants.Model
{
    /// <summary>
    /// Почва для посадки. Описание компонентов и пропорций.
    /// </summary>
    public class Soil : IStorable, IEnumerable
    {
        Dictionary<ISoilPart, int> dict = new Dictionary<ISoilPart, int>();
        bool inEditing;

        public Soil(string descr = "")
            : this()
        {
            Description = descr;

            BeginEdit();
        }

        public Soil(IEnumerable<KeyValuePair<ISoilPart, int>> parts)
            : this()
        {
            Description = "";

            BeginEdit();
            foreach (var item in parts)
            {
                Add(item.Key, item.Value);
            }
            EndEdit();
        }

        private Soil()
        {
            Ph = 7;
        }

        public IReadOnlyDictionary<ISoilPart, int> Parts { get { return dict; } }

        public string Description { get; set; }

        public float Ph { get; set; }

        public int this[ISoilPart part] { get { return PartsOf(part); } }

        public static KeyValuePair<ISoilPart, int> Part<T>(int proportion) where T : ISoilPart
        {
            return new KeyValuePair<ISoilPart, int>(default(T), proportion);
        }

        public Soil MixWith(Soil other)
        {
            // нужно знать объем чтобы установить пропорции
            throw new NotImplementedException();

            var result = new Soil("");
            foreach (var key in dict.Keys)
                result.Add(key, this.dict[key]);
            foreach (var key in other.dict.Keys)
                result.Add(key, other.dict[key]);

            result.EndEdit();
            return result;
        }

        public int PartsOf(ISoilPart part)
        {
            return dict.GetValueOrDefault(part);
        }

        public int PartsOf<T>() where T : ISoilPart
        {
            return PartsOf(default(T));
        }

        public void Add(ISoilPart part, int proportion)
        {
            if (proportion < 0) throw new ArgumentOutOfRangeException("proportion", proportion, "Can not add negative proportion of soil part.");
            if (proportion == 0) return;

            if (dict.ContainsKey(part))
                dict[part] += proportion;
            else
                dict.Add(part, proportion);

            if (!inEditing)
                ReduceProportions();
        }

        public void Add(KeyValuePair<ISoilPart, int> partProportion)
        {
            Add(partProportion.Key, partProportion.Value);
        }

        public void Add<T>(int proportion) where T : ISoilPart
        {
            var part = default(T);
            Add(part, proportion);
        }

        public void Remove(ISoilPart part, int proportion)
        {
            if (proportion < 0) throw new ArgumentOutOfRangeException("proportion", proportion, "Can not remove negative proportion of soil part.");
            if (proportion == 0) return;

            if (dict.ContainsKey(part))
            {
                dict[part] -= proportion;
                if (dict[part] <= 0)
                    dict.Remove(part);
            }

            if (!inEditing)
                ReduceProportions();
        }

        public void Remove<T>(int proportion) where T : ISoilPart
        {
            var part = default(T);
            Remove(part, proportion);
        }

        public void Set(ISoilPart part, int proportion)
        {
            if (proportion < 0) throw new ArgumentOutOfRangeException("proportion", proportion, "Can not set negative proportion of soil part.");
            if (proportion == 0)
                dict.Remove(part);
            else
                dict[part] = proportion;

            if (!inEditing)
                ReduceProportions();
        }

        public void Set<T>(int proportion) where T : ISoilPart
        {
            var part = default(T);
            Set(part, proportion);
        }

        /// <summary>
        /// Пока редактируется, пропорции не сокращаются.
        /// </summary>
        public void BeginEdit()
        {
            inEditing = true;
        }

        public void EndEdit()
        {
            inEditing = false;
            ReduceProportions();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Soil;
            if (other == null) return false;

            var that = this;
            if (this.inEditing)
            {
                that = this.GetClone();
                that.ReduceProportions();
            }
            if (other.inEditing)
            {
                other = other.GetClone();
                other.ReduceProportions();
            }

            return that.dict.Count == other.dict.Count && !that.dict.Except(other.dict).Any();
        }

        public override int GetHashCode()
        {
            return dict.Count;
        }

        public IEnumerator GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        public override string ToString()
        {
            return Description ?? string.Empty;
        }

        private void ReduceProportions()
        {
            if (dict.Count == 0) return;

            var gcd = MathHelper.GCD(dict.Values);
            foreach (var key in dict.Keys.ToArray())
            {
                dict[key] /= gcd;
            }
        }

        private Soil GetClone()
        {
            var result = new Soil(Description) { Ph = this.Ph };
            foreach (var key in dict.Keys)
            {
                result.Add(key, dict[key]);
            }
            result.EndEdit();
            return result;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(dict.Values.All(x => x > 0));
            Contract.Invariant(0 < Ph && Ph < 14);
        }
    }
}