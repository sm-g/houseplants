using System;
using System.Collections.Generic;
using System.Linq;

namespace Houseplants.Model
{
    /// <summary>
    /// Класс растений.
    /// </summary>
    public class Plant
    {
        IEnumerable<SeedType> _seedTypes;
        public Plant(string name, params SeedType[] types)
        {
            Name = name;
            _seedTypes = new List<SeedType>(types);
            Conditions = new List<Condition>();
        }

        public string Name { get; set; }

        public IList<Condition> Conditions { get; private set; }

        /// <summary>
        /// Пример растения.
        /// </summary>
        public Photo Photo { get; set; }

        public IEnumerable<SeedType> SeedTypes { get { return _seedTypes; } }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }

    public class Plants
    {
        static Plant _hedera = new Plant("Hedera", SeedType.Seedling);
        public static Plant Hedera { get { return _hedera; } }
    }
}