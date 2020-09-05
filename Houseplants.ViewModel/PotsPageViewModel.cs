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
    public class PotsPageViewModel : ViewModelBase, IEnableLogger
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        public PotsPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Initialize();
        }

        public ObservableCollection<Pot> Pots { get; set; }

        private async Task Initialize()
        {
            try
            {
                var pots = await _dataService.GetPots();
                Pots = new ObservableCollection<Pot>(pots);
            }
            catch (Exception ex)
            {
                this.Log().ErrorException("", ex);
            }
        }
    }
}