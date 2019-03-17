using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace ExpressServices.Services
{
    public static class MenuService
    {
        private static IEnumerable<WinUI.NavigationViewItem> Menus()
        {
            const string prefix = "ExpressServices.Views.";
            var FontAwesome = new FontFamily("ms-appx:///Assets/Fonts/FontAwesome.otf#FontAwesome");
            var MaterialIcons = new FontFamily("ms-appx:///Assets/Fonts/MaterialIcons-Regular.ttf#Material Icons");

            var menus = new ObservableCollection<WinUI.NavigationViewItem>
            {
                 // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
                 // Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
                 // Edit String/ en - US / Resources.resw: Add a menu item title for each page

                new WinUI.NavigationViewItem
                {
                    Content = "Oficina",
                    Icon = new SymbolIcon(Symbol.Repair),
                    Tag = prefix + "WorkshopPage",
                },

                new WinUI.NavigationViewItem
                {
                    Content = "Clientes",
                    Icon = new SymbolIcon(Symbol.People),
                    Tag = prefix + "CustomersPage"
                },

                new WinUI.NavigationViewItem
                {
                    Content = "Teste",
                    Icon = new SymbolIcon(Symbol.Document),
                    Tag = prefix + "BlankPage"
                },
            };

            return menus;
        }

        public static ObservableCollection<WinUI.NavigationViewItem> GetMenus()
        {
            return new ObservableCollection<WinUI.NavigationViewItem>(Menus());
        }
    }
}
