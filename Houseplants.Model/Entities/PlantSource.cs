using System;
using System.Linq;

namespace Houseplants.Model
{
    public enum SeedType
    {
        /// <summary>
        /// Семя
        /// </summary>
        Seed,

        /// <summary>
        /// Лист
        /// </summary>
        Leaf,

        /// <summary>
        /// Черенок
        /// </summary>
        Stalk,

        /// <summary>
        /// Саженец
        /// </summary>
        Seedling,

        /// <summary>
        /// Отводок
        /// </summary>
        Ratoon,

        /// <summary>
        /// Деление - отпрыски, детки, клубни
        /// </summary>
        Division
    }

    /// <summary>
    /// Материал для посадки.
    /// </summary>
    public class PlantSource
    {
        readonly Plant _plant;
        readonly SeedType _type;
        readonly Flower _parent;

        public PlantSource(Plant plant, SeedType type)
        {
            if (!plant.SeedTypes.Contains(type))
                throw new ArgumentOutOfRangeException("type", "Растение не размножется таким способом.");

            _parent = null;
            _plant = plant;
            _type = type;
        }

        public PlantSource(Flower parent, SeedType type)
        {
            if (!parent.Plant.SeedTypes.Contains(type))
                throw new ArgumentOutOfRangeException("type", "Растение не размножется таким способом.");

            _parent = parent;
            _plant = parent.Plant;
            _type = type;
        }

        public Plant Plant { get { return _plant; } }

        public SeedType SeedType { get { return _type; } }

        public Flower Flower { get { return _parent; } }
    }
}