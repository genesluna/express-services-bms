using Caliburn.Micro;
using ExpressServices.Core.Helpers;
using ExpressServices.Core.Models;
using ExpressServices.Dialogs;
using ExpressServices.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace ExpressServices.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();

            Window.Current.SetTitleBar(AppTitleBar);

            Network.InternetConnectionChanged += async (s, e) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    var User = CurrentUser.Instance;

                    ViewModel.AppConnectivityText = Network.IsConnected ? "Online 📡" : "Offline ⛔";

                    if (e.IsConnected)
                    {
                        // Full Data Sync
                        SyncDialog syncDialog = new SyncDialog();
                        await syncDialog.ShowAsync();
                    }
                });
            };
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            var navigationService = container.RegisterNavigationService(shellFrame);
            //navigationViewHeaderBehavior.Initialize(navigationService);
            return navigationService;
        }

        public WinUI.NavigationView GetNavigationView()
        {
            return navigationView;
        }

        public Frame GetFrame()
        {
            return shellFrame;
        }
    }
}
