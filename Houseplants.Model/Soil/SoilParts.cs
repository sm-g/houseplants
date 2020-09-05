using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houseplants.Model
{
    public interface ISoilPart
    {
    }

    public struct Sand : ISoilPart
    {
        //public static KeyValuePair<ISoilPart, int> Part(int i)
        //{
        //    return new KeyValuePair<ISoilPart, int>(default(Sand), i);
        //}
    }

    /// <summary>
    /// Дерн
    /// </summary>
    public struct Turf : ISoilPart
    {
    }

    /// <summary>
    /// Торф
    /// </summary>
    public struct Peat : ISoilPart
    {
    }

    // TODO strict enum pattern
    public static class SoilParts
    {
        public static Sand Sand = new Sand();
        public static Turf Turf = new Turf();
        public static Peat Peat = new Peat();
    }
}
