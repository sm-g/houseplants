using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Houseplants.Data;
using Houseplants.Model;
using Houseplants.Model.Events;
using MetroLog;
using Mutzl.MvvmLight;

namespace Houseplants.ViewModel
{
    public class EditEventViewModel : ViewModelBase
    {
        private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<EditEventViewModel>();

        public const string SelectedFlowerPropertyName = "SelectedFlower";
        public const string SelectedPotPropertyName = "SelectedPot";
        readonly IDataService _dataService;
        DateTime _date;
        string _descr;
        private ICommand _saveCommand;

        private Flower _selectedFlower = null;

        private Pot _selectedPot = null;

        public EditEventViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Initialize();
        }

        public DateTime Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        public string Description
        {
            get { return _descr; }
            set { Set(ref _descr, value); }
        }

        public ObservableCollection<Pot> Pots { get; private set; }

        public ObservableCollection<Flower> Flowers { get; private set; }

        public Flower SelectedFlower
        {
            get { return _selectedFlower; }
            set { Set(ref _selectedFlower, value); }
        }

        public Pot SelectedPot
        {
            get { return _selectedPot; }
            set { Set(ref _selectedPot, value); }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new DependentRelayCommand(
                    () =>
                    {
                        if (!SaveCommand.CanExecute(null))
                            return;

                        new Planting(SelectedFlower, null, SelectedPot);
                    },
                    () => CanPlantWithSelected(),
                    this, () => SelectedFlower, () => SelectedPot));
            }
        }

        private bool CanPlantWithSelected()
        {
            return SelectedFlower != null && SelectedPot != null
                && SelectedFlower.CurrentPot == null && SelectedPot.CurrentFlower == null;
        }

        private async Task Initialize()
        {
            try
            {
                var pots = await _dataService.GetPots();
                var flowers = await _dataService.GetFlowers();

                Pots = new ObservableCollection<Pot>(pots);
                Flowers = new ObservableCollection<Flower>(flowers);
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
            }
        }
    }
}