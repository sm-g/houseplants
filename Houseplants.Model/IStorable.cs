using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houseplants.Model
{
    public interface IStorable
    {
    }

    public struct StorableAmount
    {
        public IStorable Item { get; set; }
        public double Amount { get; set; }
    }

    public struct StorableAmount<T> where T : IStorable
    {
        public T Item { get; set; }
        public double Amount { get; set; }
    }
}
