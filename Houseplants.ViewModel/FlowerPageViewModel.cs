using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Houseplants.Data;
using Houseplants.Model;
using MetroLog;

namespace Houseplants.ViewModel
{
    public class FlowerPageViewModel : ViewModelBase
    {
        private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<FlowerPageViewModel>();

        readonly IDataService _dataService;
        private Flower _selectedFlower;

        public FlowerPageViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Initialize();
        }

        public Flower SelectedFlower
        {
            get { return _selectedFlower; }
            set { Set(ref _selectedFlower, value); }
        }

        private async Task Initialize()
        {
            try
            {
                var pots = await _dataService.GetPots();
                var flowers = await _dataService.GetFlowers();
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
            }
        }
    }
}