using System;

using Caliburn.Micro;

using ExpressServices.Helpers;

namespace ExpressServices.ViewModels
{
    public class BlankViewModel : Screen
    {
        private INavigationService _navigationService;

        private string _test;

        public string Test
        {
            get { return _test; }
            set { Set(ref _test, value); }
        }
        
        public BlankViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _test = "/ErrorMessages/ExceptionSettingsStorageExtensionsFileNameIsNullOrEmpty".GetLocalized();
        }

        public void GoNow()
        {
            _navigationService.NavigateToViewModel<WorkshopViewModel>();
        }
    }
}
