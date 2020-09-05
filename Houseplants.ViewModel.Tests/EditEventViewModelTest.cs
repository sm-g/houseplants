using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GalaSoft.MvvmLight.Views;
using Houseplants.ViewModel;
using Houseplants.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Houseplants.Model;

namespace Houseplants.ViewModel.Tests
{
    [TestClass]
    public class EditEventViewModelTest
    {
        [TestMethod]
        public void SaveCommand_SelectedPotAndFlower_CanExecute()
        {
            // arrange
            var vm = new EditEventViewModel(new DataServiceStub());
            vm.SelectedFlower = vm.Flowers.First();
            vm.SelectedPot = vm.Pots.First();

            // act
            var result = vm.SaveCommand.CanExecute(null);

            // assert
            Assert.IsTrue(result);
        }
    }

    public class DataServiceStub : IDataService
    {
        public async Task<IEnumerable<Pot>> GetPots()
        {

            var pots = new Pot[] {
                new Pot("pot1"),
                new Pot("pot1") {WithTray = true},
                new Pot("pot1")

            };
            return pots;
        }


        public async Task<IEnumerable<Flower>> GetFlowers()
        {
            var result = new Flower[] {
                new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling)),
                new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling)),

            };
            return result;
        }
    }
}
