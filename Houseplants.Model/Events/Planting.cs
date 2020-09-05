using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Houseplants.Model.Events
{
    /// <summary>
    /// Посадка растения
    /// </summary>
    public class Planting : Event, IPotEvent, IFlowerEvent
    {
        readonly Pot pot;
        readonly Flower flower;
        readonly Soil soil;

        public Planting(PlantSource source, Soil soil, Pot pot)
            : this(new Flower(source), soil, pot)
        {
        }

        public Planting(Flower flower, Soil soil, Pot pot)
        {
            // садить только цветок без горшка в пустой горшок
            Contract.Requires<ArgumentException>(flower.GetPot(DateTime.UtcNow) == null);
            Contract.Requires<ArgumentException>(pot.GetFlower(DateTime.UtcNow) == null);

            Contract.Ensures(pot.GetFlower(this.Date) == flower, "must be planted flower in pot on date of planting");
            Contract.Ensures(flower.GetPot(this.Date) == pot, "flower must be in this pot on date of planting");

            this.pot = pot;
            this.soil = soil;
            this.flower = flower;

            pot.Events.Add(this);
            flower.Events.Add(this);
            Debug.WriteLine("{0} planted to {1} with {2}", flower, pot, soil);
        }

        public Pot Pot
        {
            get { return pot; }
        }

        public Flower Flower
        {
            get { return flower; }
        }

        public Soil Soil
        {
            get { return soil; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Pot, Flower, Soil, base.ToString());
        }
    }
}