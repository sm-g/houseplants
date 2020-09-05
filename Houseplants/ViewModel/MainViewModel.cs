using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Houseplants.Data;

namespace Houseplants.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        private RelayCommand _navigateCommand;
        private RelayCommand<string> _navToCommand;
        private string _originalTitle;
        private string _welcomeTitle = string.Empty;


        public MainViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Initialize();
        }

        public RelayCommand NavigateToPotsCommand
        {
            get
            {
                return _navigateCommand
                       ?? (_navigateCommand = new RelayCommand(
                           () => _navigationService.NavigateTo(ViewModelLocator.MyPotsPageKey)));
            }
        }

        public RelayCommand<string> NavigateToCommand
        {
            get
            {
                return _navToCommand
                    ?? (_navToCommand = new RelayCommand<string>(
                    page =>
                    {
                        _navigationService.NavigateTo(page);
                    }));
            }
        }

        public string WelcomeTitle
        {
            get { return _welcomeTitle; }
            set { Set(ref _welcomeTitle, value); }
        }

        public override void Cleanup()
        {
            // Clean up if needed

            base.Cleanup();
        }

        public void Load(DateTime lastVisit)
        {
            if (lastVisit > DateTime.MinValue)
            {
                WelcomeTitle = string.Format(
                    "{0} (last visit on the {1})",
                    _originalTitle,
                    lastVisit);
            }
        }

        private async Task Initialize()
        {
            try
            {
                _originalTitle = "123";
                WelcomeTitle = "123";
            }
            catch (Exception ex)
            {
                // Report error here
            }
        }
    }
}