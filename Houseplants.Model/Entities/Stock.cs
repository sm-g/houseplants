using System;
using System.Collections.Generic;
using System.Linq;

namespace Houseplants.Model
{
    public class Stock
    {
        public Stock()
        {
            Items = new List<IStorable>();
        }

        public IList<IStorable> Items { get; set; }

        public T Get<T>(double amout)
        {
            return default(T);
        }

        public void Add(IStorable item, double amount)
        {
        }
    }
}