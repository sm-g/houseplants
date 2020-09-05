using System;
using System.Linq;

namespace Houseplants.Model
{
    /// <summary>
    /// Удобрение
    /// </summary>
    public class Fertilizer : IStorable
    {
        public string Description { get; set; }
    }
}