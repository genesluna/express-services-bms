using System;

using ExpressServices.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ExpressServices.Views
{
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            InitializeComponent();
        }

        private BlankViewModel ViewModel
        {
            get { return DataContext as BlankViewModel; }
        }
    }
}
