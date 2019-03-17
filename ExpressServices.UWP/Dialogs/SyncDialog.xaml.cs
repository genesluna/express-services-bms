using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using ExpressServices.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace ExpressServices.Dialogs
{
    public sealed partial class SyncDialog : ContentDialog
    {
        ICloudService _cloudService;

        public SyncDialog()
        {
            this.InitializeComponent();

            Title = "Sincronizando";

            _cloudService = AzureCloudService.Instance;
            Loaded += SyncDialog_Loaded;
        }

        private async void SyncDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Full Data Sync
            var progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += Progress_ProgressChanged;
            await _cloudService.SyncOfflineCacheAsync(progress);
        }

        private void Progress_ProgressChanged(object sender, ProgressReportModel e)
        {
            this.SyscProgressBar.Value = e.PercentageComplete;
            this.TaskTextBlock.Text = e.CurrentTask;

            if (e.PercentageComplete == 100)
            {
                this.Hide();
            }
        }
    }
}
