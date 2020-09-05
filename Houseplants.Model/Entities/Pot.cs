using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Houseplants.Model.Events;

namespace Houseplants.Model
{
    /// <summary>
    /// Роль для компоновки.
    /// </summary>
    public enum CompositionRole
    {
        /// <summary>
        /// Обычный горшок.
        /// </summary>
        General,

        /// <summary>
        /// Кашпо. Содержит обычный горшок.
        /// </summary>
        Cashpo,

        /// <summary>
        /// Двойной горшок, без отдельного поддона. Можно считать как кашпо+обычный.
        /// </summary>
        Double
    }

    public class Pot
    {
        Dimension _dim;

        public Pot(Dimension dim)
            : this()
        {
            _dim = dim;
        }

        public Pot(string title)
            : this()
        {
            Title = title;
        }

        public Pot()
        {
            Events = new List<Event>();
            Photos = new List<Photo>();
        }

        public string Title
        {
            get;
            private set;
        }

        public PotMaterial Material { get; set; }

        public CompositionRole Role { get; set; }

        public Color Color { get; set; }

        public bool WithTray { get; set; }

        public double Volume { get { return _dim == null ? 0 : _dim.AutoVolume; } }

        public DateTime BuyDate { get; set; }

        public DateTime TrashDate { get; set; }

        public IList<Event> Events { get; private set; }

        public IList<Photo> Photos { get; private set; }

        public Flower CurrentFlower { get { return GetFlower(DateTime.UtcNow); } }

        public StorableAmount<Soil> CurrentSoil { get { return GetSoil(DateTime.UtcNow); } }

        /// <summary>
        /// Вытаскивает цветок и землю из горшка.
        /// Землю надо отправить на склад по частям, но нельзя разделить готовую смесь.
        /// Только отделить керамзит. Осататки так и хранить вместе. И пометить как использованную.
        /// </summary>
        public Tuple<StorableAmount<Soil>, Flower> PullEverything()
        {
            Contract.Ensures(CurrentFlower == null);
            Contract.Ensures(CurrentSoil.Amount == 0);

            var flower = CurrentFlower;
            var soil = CurrentSoil;
            new Pulling(this);

            return Tuple.Create(soil, flower);
        }

        /// <summary>
        /// Вытаскивает цветок из горшка.
        /// </summary>
        public Flower PullFlower()
        {
            Contract.Ensures(CurrentFlower == null);
            Contract.Ensures(CurrentSoil.Equals(Contract.OldValue<object>(CurrentSoil)));

            var flower = CurrentFlower;
            new Pulling(flower);

            return flower;
        }

        public void Plant(Flower flower)
        {
            //new Planting(flower, GetSoil(DateTime.UtcNow), this);
        }

        public void FillSoil(Soil soil)
        {
        }

        /// <summary>
        /// Цветок, который был в горшке в момент.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [Pure]
        public virtual Flower GetFlower(DateTime time)
        {
            var lastPlanting = Events
                 .OfType<Planting>()
                 .Where(x => x.Date <= time)
                 .OrderBy(x => x.Date)
                 .LastOrDefault();
            if (lastPlanting == null) return null;

            var lastPulling = Events
                 .OfType<Pulling>()
                 .Where(x => x.Date <= time)
                 .OrderBy(x => x.Date)
                 .LastOrDefault();

            Debug.WriteLine("lastPlanting: {0}, lastPulling: {1}", lastPlanting, lastPulling);

            if (lastPulling == null || lastPulling.Date < lastPlanting.Date)
                return lastPlanting.Flower;

            return null;
        }

        [Pure]
        public virtual StorableAmount<Soil> GetSoil(DateTime time)
        {
            var lastPlanting = Events
                 .OfType<Planting>()
                 .Where(x => x.Date <= time)
                 .OrderBy(x => x.Date)
                 .LastOrDefault();

            if (lastPlanting == null) return default(StorableAmount<Soil>);

            var lastPulling = Events
                 .OfType<Pulling>()
                 .Where(x => x.WithSoil && x.Date <= time)
                 .OrderBy(x => x.Date)
                 .LastOrDefault();

            if (lastPulling != null && lastPulling.Date > lastPlanting.Date)
                return default(StorableAmount<Soil>);

            var result = new StorableAmount<Soil>() { Amount = this.Volume, Item = lastPlanting.Soil };
            return result;
        }

        public virtual bool IsEmpty(DateTime time)
        {
            return GetFlower(time) == null && GetSoil(time).Amount == 0;
        }

        public override string ToString()
        {
            return Title ?? string.Empty;
        }
    }
}