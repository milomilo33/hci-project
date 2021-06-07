using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Projekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for AddSaradnik.xaml
    /// </summary>
    public partial class AddSaradnik : Window
    {
        private string coordinates { get; set; }
        public AddSaradnik()
        {
            InitializeComponent();
            DataContext = new AddSaradnikViewModel();
            
        }

        public void NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /*private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            coordinates = await (DataContext as AddSaradnikViewModel).GetLocationFromAddressAsync();
            Console.WriteLine(coordinates);
            // Specify a known location.
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 1, Longitude = 1 };
            var cityCenter = new Geopoint(cityPosition);

            // Set the map location.
            await (sender as MapControl).TrySetViewAsync(cityCenter, 20);
        }*/
    }
}
