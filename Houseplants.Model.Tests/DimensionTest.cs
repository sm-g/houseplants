using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Houseplants.Model.Tests
{
    [TestClass]
    public class DimensionTest
    {
        [TestMethod]
        public void GetVolumeForBox()
        {
            var dim = Dimension.Box(1, 2, 1);

            Assert.AreEqual(2, dim.AutoVolume);
            Assert.AreEqual(2, dim.Volume);
        }

        [TestMethod]
        public void GetVolumeForConus()
        {
            var dim = Dimension.Conus(topDiam: 2, bottomDiam: 4, height: 1);

            Assert.AreEqual(7.3, dim.AutoVolume, 0.1);
            Assert.AreEqual(7.3, dim.Volume.Value, 0.1);
        }
        [TestMethod]
        public void GetVolumeForConusByTopDAndHeight()
        {
            var dim = Dimension.Conus(topDiam: 2, height: 1);

            Assert.AreEqual(2.2, dim.AutoVolume, 0.1);
            Assert.AreEqual(2.2, dim.Volume.Value, 0.1);
        }

        [TestMethod]
        public void GetVolumeForCylinder()
        {
            var dim = Dimension.Cylinder(diam: 2, height: 1);

            Assert.AreEqual(3.1, dim.AutoVolume, 0.1);
            Assert.AreEqual(3.1, dim.Volume.Value, 0.1);
        }

        [TestMethod]
        public void SetVolume()
        {
            var dim = Dimension.Box(1, 1, 1);
            dim.Volume = 4;

            Assert.AreEqual(4, dim.Volume);
            Assert.AreEqual(1, dim.AutoVolume);
        }

    }
}