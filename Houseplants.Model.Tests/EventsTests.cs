using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Moq;
using System.Diagnostics.Contracts;
using Houseplants.Model.Events;

namespace Houseplants.Model.Tests
{
    [TestClass]
    public class EventsTests
    {
        [TestMethod]
        public void Planting_Flower_ItIsInPot()
        {
            var pot = new Pot("");
            var source = new PlantSource(Plants.Hedera, SeedType.Seedling);

            // act
            var flower = new Flower(source, new Soil(""), pot);
            //new Planting(flower, pot);

            Assert.AreEqual(pot, flower.CurrentPot);
            Assert.AreEqual(flower, pot.CurrentFlower);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotPlant_ToNonEmptyPot()
        {
            // arrange
            var source = new PlantSource(Plants.Hedera, SeedType.Seedling);
            var potMock = new Mock<Pot>();
            potMock.Setup(x => x.GetFlower(It.IsAny<DateTime>())).Returns(new Flower(source));
            var potWithFlower = potMock.Object;

            // act
            new Planting(new Flower(source), new Soil(""), potWithFlower);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Plant_FlowerInPot_Throws()
        {
            // arrange
            var source = new PlantSource(Plants.Hedera, SeedType.Seedling);
            var flowerMock = new Mock<Flower>(source);
            flowerMock.Setup(x => x.GetPot(It.IsAny<DateTime>())).Returns(new Pot());
            var flowerInPot = flowerMock.Object;

            // act
            new Planting(flowerInPot, new Soil(""), new Pot());
        }

    }
}