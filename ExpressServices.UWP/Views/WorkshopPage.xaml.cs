using System;

using ExpressServices.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ExpressServices.Views
{
    public sealed partial class WorkshopPage : Page
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on WorkshopPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public WorkshopPage()
        {
            InitializeComponent();
        }

        private WorkshopViewModel ViewModel
        {
            get { return DataContext as WorkshopViewModel; }
        }
    }
}
