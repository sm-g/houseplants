/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="using:Houseplants.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Houseplants.Data;
using Houseplants.Design;
using Houseplants.Pages;
using Microsoft.Practices.ServiceLocation;

namespace Houseplants.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public static string MainPageKey { get { return "MainPage"; } }
        public static string MyFlowersPageKey { get { return "FlowersPage"; } }
        public static string MyPotsPageKey { get { return "PotsPage"; } }
        public static string FlowerPageKey { get { return "FlowerPage"; } }
        public static string EventPageKey { get { return "EventPage"; } }
        public static string PlantsPageKey { get { return "PlantsPage"; } }

        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PotsPageViewModel Pots
        {
            get { return ServiceLocator.Current.GetInstance<PotsPageViewModel>(); }
        }

        [SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public FlowersPageViewModel Flowers
        {
            get { return ServiceLocator.Current.GetInstance<FlowersPageViewModel>(); }
        }

        [SuppressMessage("Microsoft.Performance",
           "CA1822:MarkMembersAsStatic",
           Justification = "This non-static member is needed for data binding purposes.")]
        public EditEventViewModel Event
        {
            get { return ServiceLocator.Current.GetInstance<EditEventViewModel>(); }
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            var nav = new NavigationService();
            nav.Configure(ViewModelLocator.MainPageKey, typeof(MainPage));
            nav.Configure(ViewModelLocator.MyPotsPageKey, typeof(MyPotsPage));
            nav.Configure(ViewModelLocator.MyFlowersPageKey, typeof(MyFlowersPage));
            nav.Configure(ViewModelLocator.EventPageKey, typeof(EventPage));
            nav.Configure(ViewModelLocator.FlowerPageKey, typeof(FlowerPage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PotsPageViewModel>();
            SimpleIoc.Default.Register<FlowerPageViewModel>();
            SimpleIoc.Default.Register<FlowersPageViewModel>();
            SimpleIoc.Default.Register<EditEventViewModel>();
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}