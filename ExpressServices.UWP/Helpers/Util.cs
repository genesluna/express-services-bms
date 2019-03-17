using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ExpressServices.Helpers
{
    public class Util
    {
        public async void DoMajorAppReconfiguration()
        {
            // await App.CloseLocalServicesServer(); <==============

            // Attempt restart, with arguments.
            AppRestartFailureReason result = await CoreApplication.RequestRestartAsync("-fastInit");

            // Restart request denied, send a toast to tell the user to restart manually
            if (result == AppRestartFailureReason.NotInForeground || result == AppRestartFailureReason.Other)
            {
                //SendToast("Please manually restart.");
            }
        }

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }

        public async Task<ContentDialogResult> ShowDialog<TViewModel>()
        {
            var viewModel = IoC.Get<TViewModel>();
            var view = ViewLocator.LocateForModel(viewModel, null, null);

            ViewModelBinder.Bind(viewModel, view, null);

            var dialog = new ContentDialog
            {
                Content = view
            };

            ScreenExtensions.TryActivate(viewModel);

            var result = await dialog.ShowAsync();

            ScreenExtensions.TryDeactivate(viewModel, true);

            return result;
        }
    }
}
