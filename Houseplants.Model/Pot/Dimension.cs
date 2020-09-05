using System;

using System.Linq;

namespace Houseplants.Model
{
    /// <summary>
    /// Размеры горшка.
    /// </summary>
    public class Dimension
    {
        readonly PotForm _form;
        int x;
        int y;
        int z;
        double topD;
        double bottomD;
        double? volumeCalculated;
        double? volume;

        private Dimension(PotForm form)
        {
            _form = form;
        }

        /// <summary>
        /// Объем указывается непосредственно.
        /// Если не указан, возвращает вычисленный по размерам.
        /// </summary>
        public double? Volume
        {
            get
            {
                return volume ?? AutoVolume;
            }
            set
            {
                volume = value;
            }
        }

        /// <summary>
        /// Объем вычисляется по размерам.
        /// </summary>
        public double AutoVolume
        {
            get
            {
                if (volumeCalculated == null)
                {
                    volumeCalculated = Math.Round(CalcVolume(), 1);
                }
                return volumeCalculated.Value;
            }
        }

        public PotForm Form { get { return _form; } }

        /// <summary>
        /// Размеры усеченного конуса.
        /// </summary>
        /// <param name="topDiam">Диаметр открытой части.</param>
        /// <param name="bottomDiam">Диаметр основания.</param>
        /// <param name="height"></param>
        public static Dimension Conus(double topDiam, double bottomDiam, int height)
        {
            var dim = new Dimension(PotForm.Conus);
            dim.y = height;
            dim.topD = topDiam;
            dim.bottomD = bottomDiam;
            return dim;
        }

        /// <summary>
        /// Размеры усеченного конуса. Диаметр основания 0,66.
        /// </summary>
        /// <param name="topDiam">Диаметр открытой части.</param>
        /// <param name="height"></param>
        public static Dimension Conus(double topDiam, int height)
        {
            var dim = new Dimension(PotForm.Conus);
            dim.y = height;
            dim.topD = topDiam;
            dim.bottomD = topDiam * 2 / 3;
            return dim;
        }

        /// <summary>
        /// Размеры циллиндра.
        /// </summary>
        /// <param name="diam">Диаметр открытой части.</param>
        /// <param name="height"></param>
        public static Dimension Cylinder(double diam, int height)
        {
            var dim = new Dimension(PotForm.Cylinder);
            dim.y = height;
            dim.topD = diam;
            dim.bottomD = diam;
            return dim;
        }

        /// <summary>
        /// Размеры бокса.
        /// </summary>
        /// <param name="width">Ширина - длина "лицевой" стороны.</param>
        /// <param name="height">Высота</param>
        /// <param name="depth">Глубина - длина "торца".</param>
        public static Dimension Box(int width, int height, int depth)
        {
            var dim = new Dimension(PotForm.Box);
            dim.x = width;
            dim.y = height;
            dim.z = depth;
            return dim;
        }

        private double CalcVolume()
        {
            switch (_form)
            {
                case PotForm.Conus:
                    // frustum volume
                    var R = Math.Max(topD, bottomD) / 2;
                    var r = Math.Min(topD, bottomD) / 2;

                    return Math.PI / 3 * y * (R * R + r * R + r * r);

                case PotForm.Cylinder:
                    r = topD / 2;
                    return Math.PI * y * r * r;

                case PotForm.Box:
                    return x * y * z;

                case PotForm.Figured:
                    return x * y * z;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}