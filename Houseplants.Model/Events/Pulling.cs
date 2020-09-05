using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Houseplants.Model.Events
{
    /// <summary>
    /// Вытаскивание из горшка.
    /// Может быть за один раз - и цветок и земля - или сначала цветок, потом земля.
    /// </summary>
    public class Pulling : Event, IPotEvent, IFlowerEvent
    {
        public readonly bool WithSoil;
        readonly Flower flower;
        readonly Pot pot;

        /// <summary>
        /// Вытаскивание цветка из горшка.
        /// </summary>
        public Pulling(Flower flower)
        {
            Contract.Ensures(flower == null || flower.CurrentPot == null);
            Contract.Ensures(flower == null || Contract.OldValue<Pot>(flower.CurrentPot) == null || Contract.OldValue<Pot>(flower.CurrentPot).CurrentFlower == null);

            if (flower == null) return;

            var pot = flower.CurrentPot;
            if (pot == null) return;

            this.flower = flower;
            this.pot = pot;
            flower.Events.Add(this);
            pot.Events.Add(this);

            Debug.WriteLine("{0} pulled from {1}", flower, pot);
        }

        /// <summary>
        /// Вытаскивание земли и цветка из горшка.
        /// </summary>
        public Pulling(Pot pot)
        {
            Contract.Ensures(pot == null || pot.CurrentFlower == null && pot.CurrentSoil.Amount == 0);
            Contract.Ensures(pot == null || Contract.OldValue<Flower>(pot.CurrentFlower) == null || Contract.OldValue<Flower>(pot.CurrentFlower).CurrentPot == null);

            if (pot == null) return;

            this.pot = pot;
            WithSoil = true;

            var flower = pot.CurrentFlower;
            if (flower != null)
            {
                this.flower = flower;
                flower.Events.Add(this);
            }
            pot.Events.Add(this);
            Debug.WriteLine("{0} pulled from {1}", flower, pot);
        }

        public Pot Pot
        {
            get { return pot; }
        }

        public Flower Flower
        {
            get { return flower; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Pot, Flower, base.ToString());
        }
    }
}