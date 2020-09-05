using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using Houseplants.Model.Events;

namespace Houseplants.Model.Tests
{
    [TestClass]
    public class PotTests
    {
        [TestMethod]
        public void IsEmptyGet_NewPot_IsTrue()
        {
            // arrange
            var pot = new Pot();

            // assert
            Assert.AreEqual(true, pot.IsEmpty(DateTime.UtcNow));
        }

        [TestMethod]
        public void CurrentFlowerGet_OfNewPot_ReturnsNull()
        {
            // arrange
            var pot = new Pot();

            // assert
            Assert.AreEqual(null, pot.CurrentFlower);
        }

        [TestMethod]
        public void GetFlower_FromPotWithFlower_ReturnsIt()
        {
            // arrange
            var pot = new Pot();
            var flower = new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling));
            new Planting(flower, new Soil(""), pot);

            // act
            var result = pot.GetFlower(DateTime.UtcNow);

            // assert
            Assert.AreEqual(flower, result);
        }

        [TestMethod]
        public void PullEverything_PotWithFlower_ReturnsCurrentContent()
        {
            // arrange
            var pot = new Pot();
            var flower = new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling));
            var soil = new Soil("");
            new Planting(flower, soil, pot);

            // act
            var result = pot.PullEverything();

            // assert
            Assert.AreEqual(flower, result.Item2);
            Assert.AreEqual(soil, result.Item1.Item);
        }

        [TestMethod]
        public void CurrentFlowerGet_PotAfterPullFlower_ReturnsNull()
        {
            // arrange
            var pot = new Pot();
            var flower = new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling));
            new Planting(flower, new Soil(""), pot);

            // act
            pot.PullFlower();
            var result = pot.CurrentFlower;

            // assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetFlower_BeforeDateOfPlanting_ReturnsNull()
        {
            // arrange
            var pot = new Pot();
            var flower = new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling));
            new Planting(flower, new Soil(""), pot);

            // act
            var result = pot.GetFlower(DateTime.UtcNow.AddDays(-1));

            // assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetFlower_AtMomentBetweenTwoPlantings_ReturnsFirstFlower()
        {
            // arrange
            var pot = new Pot("small pot");
            var flower1 = new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling));
            var flower2 = new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling));

            new Planting(flower1, new Soil(""), pot);
            var time = DateTime.UtcNow;

            // dirty fix
            Thread.Sleep(1000);
            pot.PullEverything();
            Thread.Sleep(1000);
            new Planting(flower2, new Soil(""), pot);

            // act
            var result = pot.GetFlower(time);

            // assert
            Assert.AreEqual(flower1, result);
        }
    }
}