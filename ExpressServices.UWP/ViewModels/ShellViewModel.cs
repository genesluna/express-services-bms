using Caliburn.Micro;
using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Helpers;
using ExpressServices.Core.Models;
using ExpressServices.Core.Services;
using ExpressServices.Dialogs;
using ExpressServices.Helpers;
using ExpressServices.Services;
using ExpressServices.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using User = ExpressServices.Core.Models.User;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace ExpressServices.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private string _paneTitle;
        private string _appTitle;
        private string _appConnectivityText;
        private readonly WinRTContainer _container;
        private static INavigationService _navigationService;
        private WinUI.NavigationView _navigationView;
        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;
        private readonly ApplicationDataContainer _localSettings;
        private readonly ICloudService _cloudService;
        private readonly SyncDialog _syncDialog;

        public ShellViewModel(WinRTContainer container)
        {
            _container = container;
            _paneTitle = "Manutenção Express";
            _appTitle = "Carregando...";
            _localSettings = ApplicationData.Current.LocalSettings;
            _cloudService = AzureCloudService.Instance;
            _syncDialog = new SyncDialog();
        }

        public bool IsBackEnabled
        {
            get => _isBackEnabled;
            set => Set(ref _isBackEnabled, value);
        }

        public WinUI.NavigationViewItem Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public string PaneTitle
        {
            get => _paneTitle;
            set => Set(ref _paneTitle, value);
        }

        public string AppTitle
        {
            get => _appTitle;
            set => Set(ref _appTitle, value);
        }

        public string AppConnectivityText
        {
            get => _appConnectivityText;
            set => Set(ref _appConnectivityText, value);
        }

        private async Task LoginAsync()
        {
            await _cloudService.InitializeAsync();

            var user = new User();

            if (Network.IsConnected) // OnLine Auth
            {
                var currentClient = await _cloudService.AuthenticateAsync();

                if (currentClient != null)
                {
                    await _cloudService.GetAuthenticatedUserAsync();

                    //User Local Password
                    //if (User.LocalPassword == null)
                    //{
                    //    var dialog = new LocalPasswordDialog(User);

                    //    await dialog.ShowAsync();
                    //}
                }
                else //The user had trouble loging in
                {
                    Application.Current.Exit();
                    return;
                }
            }
            else //OffLine Auth
            {
                //await _cloudService.LogoutAsync();

                //OffLineAuthService OffLineAuth = new OffLineAuthService();

                //var user = await OffLineAuth.AuthenticateLocallyAsync();

                //if (user != null)
                //{
                //    User = user;
                //}
                //else
                //{
                //    Application.Current.Exit();
                //    return;
                //}
            }
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            var view = GetView() as IShellView;

            _navigationService = view?.CreateNavigationService(_container);
            _navigationView = view?.GetNavigationView();

            if (_navigationService != null)
            {
                _navigationService.NavigationFailed += (sender, e) => throw e.Exception;
                _navigationService.Navigated += NavigationService_Navigated;
                if (_navigationView != null) _navigationView.BackRequested += OnBackRequested;
            }

            await LoginAsync();

            // Full Data Sync
            await _syncDialog.ShowAsync();

            TitleBarCustomization();

            PupulateMenuItems();

           // _navigationService.NavigateToViewModel(typeof(WorkshopViewModel));
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            if (!(GetView() is UIElement page)) return;
            page.KeyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            page.KeyboardAccelerators.Add(_backKeyboardAccelerator);
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate(typeof(SettingsPage));
                return;
            }

            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);

            var pageType = GetType().Assembly.GetType(item.Tag.ToString());
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            _navigationService.NavigateToViewModel(viewModelType);
        }

        private async void OnFooterItemTapped(dynamic sender, TappedRoutedEventArgs args)
        {
            switch (sender.Tag.ToString())
            {
                case "Logout":
                    await OnLogout();
                    break;

                default:
                    break;
            }
        }

        private async Task OnLogout()
        {
            // IsBusy = true;

            var deleteFileDialog = new ContentDialog
            {
                Title = "Deseja mesmo fazer logout?",
                Content = "Caso prossiga, o aplicativo será reinicializado e novas credenciais serão solicitadas.",
                PrimaryButtonText = "Logout",
                CloseButtonText = "Cancelar"
            };

            var result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await _cloudService.LogoutAsync();

                // await App.CloseLocalServicesServer();

                Application.Current.Exit();
            }

            // IsBusy = false;
        }

        private static void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            if (_navigationService.BackStack[0].SourcePageType.Name == typeof(BlankPage).Name)
            {
                return;
            }
            _navigationService.GoBack();
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                var entryPage = frame.BackStack.FirstOrDefault(page => page.SourcePageType == typeof(BlankPage));

                if ( entryPage != null)
                {
                    frame.BackStack.Remove(entryPage);
                }
            }

            IsBackEnabled = _navigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourceViewModelType = ViewModelLocator.LocateTypeForViewType(sourcePageType, false);
            var pageType = GetType().Assembly.GetType(menuItem.Tag.ToString());
            var viewModelType = ViewModelLocator.LocateTypeForViewType(pageType, false);
            return viewModelType == sourceViewModelType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (!_navigationService.CanGoBack) return;

            _navigationService.GoBack();
            args.Handled = true;
        }

        private void PupulateMenuItems()
        {
            foreach (var menuItem in MenuService.GetMenus())
            {
                _navigationView.MenuItems.Add(menuItem);
            }
        }

        private void TitleBarCustomization()
        {
            var util = new Util();

            var appView = ApplicationView.GetForCurrentView();
            appView.TitleBar.BackgroundColor = util.GetSolidColorBrush("#FF333333").Color;
            appView.TitleBar.ForegroundColor = Colors.White;
            appView.TitleBar.ButtonBackgroundColor = util.GetSolidColorBrush("#FF555555").Color;
            appView.TitleBar.ButtonForegroundColor = Colors.White;

            var package = Package.Current;
            var appName = package.DisplayName;
            var appVersion = package.Id.Version;
            var appVersionString = $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}";
            var appNameVersion = $"{appName} {appVersionString}";

            switch (_localSettings.Values["BackendEnvironment"].ToString())
            {
                case "Local":
                    {
                        AppTitle = $"{appNameVersion} - {CurrentUser.Instance.Name} - Working Locally (DEBUG)";
                        break;
                    }

                case "Stage":
                    {
                        AppTitle = $"{appNameVersion} - {CurrentUser.Instance.Name} - Working at Stage (DEBUG)";
                        break;
                    }

                default:
                    {
                        AppTitle = $"{appNameVersion} - {CurrentUser.Instance.Name}";
                        break;
                    }
            }

            AppConnectivityText = Network.IsConnected ? "Online 📡" : "Offline ⛔";
        }
    }
}
