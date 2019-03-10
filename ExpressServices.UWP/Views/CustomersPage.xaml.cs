using System;

using ExpressServices.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ExpressServices.Views
{
    public sealed partial class CustomersPage : Page
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on CustomersPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public CustomersPage()
        {
            InitializeComponent();
        }

        private CustomersViewModel ViewModel
        {
            get { return DataContext as CustomersViewModel; }
        }
    }
}
