using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Houseplants.Model.Events;

namespace Houseplants.Model
{
    public class Place
    {
        public Place()
        {
            Events = new List<Event>();
        }

        public string Description { get; set; }
        public IList<Pot> Pots { get; private set; }
        public IList<Event> Events { get; private set; }

        /// <summary>
        /// Горшки в месте в момент.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Pure]
        public IEnumerable<Pot> GetPots(DateTime date)
        {
            throw new NotImplementedException();
            var lastTransposing = Events
                 .OfType<Transposing>()
                 .Where(x => x.Date <= date)
                 .OrderBy(x => x.Date)
                 .LastOrDefault();
            if (lastTransposing == null) return null;
        }
    }
}