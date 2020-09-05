using System;
using System.Linq;

namespace Houseplants.Model
{
    public class Condition
    {
        public Condition(string descr)
        {
            Description = descr;
        }

        public string Description { get; set; }
    }
}