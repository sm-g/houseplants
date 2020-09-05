using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Houseplants.Model.Tests
{
    [TestClass]
    public class SoilTests
    {
        [TestMethod]
        public void Ctor_ReduceProportions()
        {
            // arrange
            var parts = new KeyValuePair<ISoilPart, int>[] {
                Soil.Part<Sand>(4),
                Soil.Part<Peat>(2),
                Soil.Part<Turf>(2)
            };
            var soil = new Soil(parts);

            // assert
            Assert.AreEqual(2, soil[SoilParts.Sand]);
            Assert.AreEqual(1, soil[SoilParts.Peat]);
            Assert.AreEqual(1, soil[SoilParts.Turf]);
        }

        [TestMethod]
        public void AddPart_OneByOne_SaveProportions()
        {
            // arrange
            var soil = new Soil();

            // act
            soil.Add<Sand>(2);
            soil.Add<Turf>(4);
            soil.Add<Peat>(2);

            // assert
            Assert.AreEqual(2, soil[SoilParts.Sand]);
            Assert.AreEqual(4, soil[SoilParts.Turf]);
            Assert.AreEqual(2, soil[SoilParts.Peat]);
        }

        [TestMethod]
        public void AddPart_AfterEndEditing_ReduceProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.EndEdit();
            soil.Add<Sand>(1);

            // assert
            Assert.AreEqual(1, soil.PartsOf<Sand>());
        }

        [TestMethod]
        public void AddPart_AfterBeginEditing_ChangesProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1), Soil.Part<Turf>(3) };

            // act
            soil.EndEdit();
            soil.BeginEdit();
            soil.Add<Sand>(2);

            // assert
            Assert.AreEqual(3, soil[SoilParts.Sand]);
            Assert.AreEqual(3, soil[SoilParts.Turf]);
        }

        [TestMethod]
        public void AddPart_WithZeroCount_ChangesNothing()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };
            var copy = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Add<Turf>(0);

            // assert
            Assert.AreEqual(copy, soil);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPart_WithNegativeCount_Throws()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Add<Sand>(-1);
        }

        [TestMethod]
        public void RemovePart_NotInSoil_IsOk()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Remove<Turf>(1);

            // assert
            Assert.AreEqual(1, soil.PartsOf<Sand>());
        }

        [TestMethod]
        public void RemovePart_CountEq_RemovesPartAtAll()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Remove<Sand>(1);

            // assert
            Assert.AreEqual(0, soil.Parts.Keys.Count());
        }

        [TestMethod]
        public void RemovePart_CountGt_RemovesPartAtAll()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1), Soil.Part<Turf>(2) };

            // act
            soil.Remove<Sand>(2);

            // assert
            Assert.AreEqual(new Turf(), soil.Parts.Keys.Single());
        }

        [TestMethod]
        public void RemovePart_AfterEndEditing_ReduceProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(5), Soil.Part<Turf>(1) };

            // act
            soil.EndEdit();
            soil.Remove<Sand>(1);

            // assert
            Assert.AreEqual(4, soil.PartsOf<Sand>());
        }

        [TestMethod]
        public void RemovePart_AfterBeginEditing_ChangesProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(5), Soil.Part<Turf>(3) };

            // act
            soil.EndEdit();
            soil.BeginEdit();
            soil.Remove<Sand>(2);

            // assert
            Assert.AreEqual(3, soil[SoilParts.Sand]);
            Assert.AreEqual(3, soil[SoilParts.Turf]);
        }

        [TestMethod]
        public void RemovePart_WithZeroCount_ChangesNothing()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };
            var copy = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Remove<Sand>(0);

            // assert
            Assert.AreEqual(copy, soil);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemovePart_WithNegativeCount_Throws()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Remove<Sand>(-1);
        }


        [TestMethod]
        public void SetPart_NotInSoil_AddsIt()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Set<Turf>(1);

            // assert
            Assert.AreEqual(1, soil.PartsOf<Sand>());
            Assert.AreEqual(1, soil.PartsOf<Turf>());
        }

        [TestMethod]
        public void SetPart_AfterEndEditing_ReduceProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(5), Soil.Part<Turf>(1) };

            // act
            soil.EndEdit();
            soil.Set<Turf>(5);

            // assert
            Assert.AreEqual(1, soil.PartsOf<Sand>());
            Assert.AreEqual(1, soil.PartsOf<Turf>());
        }

        [TestMethod]
        public void SetPart_AfterBeginEditing_ChangesProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(5), Soil.Part<Turf>(3) };

            // act
            soil.EndEdit();
            soil.BeginEdit();
            soil.Set<Sand>(3);

            // assert
            Assert.AreEqual(3, soil[SoilParts.Sand]);
            Assert.AreEqual(3, soil[SoilParts.Turf]);
        }

        [TestMethod]
        public void SetPart_WithPositiveCount_SetParts()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1), Soil.Part<Turf>(1) };

            // act
            soil.Set<Sand>(2);
            soil.Set<Turf>(1);

            // assert
            Assert.AreEqual(2, soil.PartsOf<Sand>());
            Assert.AreEqual(1, soil.PartsOf<Turf>());
        }

        [TestMethod]
        public void SetPart_WithZeroCount_RemovesPartAtAll()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Set<Sand>(0);

            // assert
            Assert.AreEqual(0, soil.Parts.Keys.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetPart_WithNegativeCount_Throws()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            soil.Set<Sand>(-1);
        }

        [TestMethod]
        public void EndEditing_MultiParted_ReduceProportions()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(4), Soil.Part<Peat>(2), Soil.Part<Turf>(2) };

            // act
            soil.EndEdit();

            // assert
            Assert.AreEqual(2, soil[SoilParts.Sand]);
            Assert.AreEqual(1, soil[SoilParts.Peat]);
            Assert.AreEqual(1, soil[SoilParts.Turf]);
        }

        [TestMethod]
        public void EndEditing_OnEmpty_IsOk()
        {
            // arrange
            var soil = new Soil();

            // act
            soil.EndEdit();
        }

        [TestMethod]
        public void EndEditing_OnMonoParted_IsOk()
        {
            // arrange
            var soil = new Soil() { { SoilParts.Peat, 1 } };

            // act
            soil.EndEdit();
        }

        [TestMethod]
        public void Equals_CompareReducedProportionsButDontChangeThem()
        {
            // arrange
            var soil1 = new Soil() { Soil.Part<Sand>(4), Soil.Part<Peat>(2), Soil.Part<Turf>(2) };
            var soil2 = new Soil() { Soil.Part<Sand>(2), Soil.Part<Peat>(1), Soil.Part<Turf>(1) };

            // act
            var result = soil1.Equals(soil2);

            // assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, soil1[SoilParts.Sand]);
        }

        [TestMethod]
        public void IndexerGet_PartNotInSoil_ReturnsZero()
        {
            // arrange
            var soil = new Soil() { Soil.Part<Sand>(1) };

            // act
            var result = soil[SoilParts.Peat];

            // assert
            Assert.AreEqual(0, result); ;
        }

        [TestMethod]
        [Ignore]
        public void Mix_Soils_CreateSoilWithNewProportions()
        {
            // arrange
            var soil1 = new Soil("1 sand, 2 turf") { Soil.Part<Sand>(1), Soil.Part<Turf>(2) };
            var soil2 = new Soil("1 sand, 2 peat") { Soil.Part<Sand>(1), Soil.Part<Peat>(2) };

            // act
            var result = soil1.MixWith(soil2);

            // assert
            Assert.AreEqual(1, result[SoilParts.Sand]);
            Assert.AreEqual(1, result[SoilParts.Turf]);
            Assert.AreEqual(1, result[SoilParts.Peat]);
        }
    }
}