using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Houseplants.Data;
using Houseplants.Model;
using Splat;

namespace Houseplants.ViewModel
{
    public class FlowersPageViewModel : ViewModelBase, IEnableLogger
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        public FlowersPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Initialize();
        }

        public ObservableCollection<Flower> Flowers { get; set; }

        private async Task Initialize()
        {
            try
            {
                var flowers = await _dataService.GetFlowers();
                Flowers = new ObservableCollection<Flower>(flowers);
            }
            catch (Exception ex)
            {
                this.Log().ErrorException("", ex);
            }
        }
    }
}