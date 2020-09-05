using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Houseplants.Model.Events;

namespace Houseplants.Model
{
    /// <summary>
    /// Конкретный экземпляр.
    /// </summary>
    public class Flower
    {
        private PlantSource _source;
        /// <summary>
        /// Новый комнатный цветок - растение в конкретном месте.
        /// </summary>
        /// <param name="creatingEvent">Событие посадки</param>
        //public Flower(Planting creatingEvent)
        //    : this()
        //{
        //    Contract.Ensures(creatingEvent.Pot.GetFlower(creatingEvent.Date) == this);
        //    Contract.Ensures(this.GetPot(creatingEvent.Date) == creatingEvent.Pot);

        //    _source = creatingEvent.Source;

        //    Events.Add(creatingEvent);
        //}

        /// <summary>
        /// Новый комнатный цветок в горшке.
        /// </summary>
        /// <param name="source">Из чего получен</param>
        /// <param name="soil"></param>
        /// <param name="pot"></param>
        //public Flower(PlantSource source, Soil soil, Pot pot)
        //    : this(new Planting(source, soil, pot))
        //{
        //}

        /// <summary>
        /// Новый комнатный цветок в горшке.
        /// </summary>
        /// <param name="source">Из чего получен</param>
        /// <param name="soil"></param>
        /// <param name="pot"></param>
        public Flower(PlantSource source, Soil soil, Pot pot)
            : this()
        {
            _source = source;
            new Planting(this, soil, pot);
        }

        /// <summary>
        /// Непосаженный цветок.
        /// </summary>
        /// <param name="source"></param>
        public Flower(PlantSource source)
            : this()
        {
            _source = source;
        }

        /// <summary>
        /// Новый цветок, полученный из другого.
        /// </summary>
        /// <param name="flower"></param>
        /// <param name="type"></param>
        /// <param name="soil"></param>
        /// <param name="pot"></param>
        public Flower(Flower flower, SeedType type, Soil soil, Pot pot)
            : this(flower.GetSource(type), soil, pot)
        {
        }

        [Obsolete("For XAML editor")]
        public Flower()
        {
            Events = new List<Event>();
            Photos = new List<Photo>();
        }

        public IList<Photo> Photos { get; private set; }
        public IList<Event> Events { get; private set; }
        public Plant Plant { get { return _source.Plant; } }

        /// <summary>
        /// Предок цветка.
        /// </summary>
        public Flower Parent { get { return _source.Flower; } }

        public Pot CurrentPot { get { return GetPot(DateTime.UtcNow); } }

        public PlantSource GetSource(SeedType type)
        {
            return new PlantSource(this, type);
        }

        /// <summary>
        /// Горшок, в котором был цветок в момент.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Pure]
        public Pot GetPot(DateTime date)
        {
            var lastPlanting = Events
                .OfType<Planting>()
                .Where(x => x.Date <= date)
                .OrderBy(x => x.Date)
                .LastOrDefault();
            if (lastPlanting == null)
                return null;

            var lastPulling = Events
               .OfType<Pulling>()
               .Where(x => x.Date <= date)
               .OrderBy(x => x.Date)
               .LastOrDefault();

            if (lastPulling == null || lastPulling.Date < lastPlanting.Date)
                return lastPlanting.Pot;

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0} in {1}", Plant, CurrentPot);
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(Plant != null);
        }
    }
}