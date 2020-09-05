using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Houseplants.Model.Tests
{
    [TestClass]
    public class StockTest
    {
        [TestMethod]
        public void CanPlaceToStock()
        {
            var f = new Fertilizer();
            var stock = new Stock();

            stock.Items.Add(f);

            Assert.IsTrue(stock.Items.Contains(f));
        }
    }
}