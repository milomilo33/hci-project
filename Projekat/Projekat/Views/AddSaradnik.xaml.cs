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
        private (double, double) coordinates { get; set; }
        public AddSaradnik()
        {
            InitializeComponent();
            DataContext = new AddSaradnikViewModel();
            
        }

        public AddSaradnik(AddSaradnikViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        public void NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Address_Updated(object sender, RoutedEventArgs e)
        {
            coordinates = (DataContext as AddSaradnikViewModel).GetLocationFromAddressAsync();
        }

        private void Load_On_Map(object sender, RoutedEventArgs e)
        {
            MapControl_Loaded(null, null);

            if (coordinates.Item1 == -1 && coordinates.Item2 == -1)
            {
                
                MessageBox.Show("Uneta adresa ne postoji.", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            coordinates = (DataContext as AddSaradnikViewModel).GetLocationFromAddressAsync();

            if (coordinates.Item1 == -1 && coordinates.Item2 == -1)
            {
                
                return;
            }
            // Specify a known location.
           

            var position = new BasicGeoposition() { Latitude = coordinates.Item1, Longitude = coordinates.Item2 };
            var center = new Geopoint(position);

            if (center == null) return;
            // Set the map location.
            
            await (this.FindName("mapControl") as MapControl).TrySetViewAsync(center, 15);
           
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext == null) return;
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                (DataContext as AddSaradnikViewModel).IsLokal = true;
                (DataContext as AddSaradnikViewModel).IsOther = false;
            }
            else if ((sender as ComboBox).SelectedIndex == 1)
            {
                (DataContext as AddSaradnikViewModel).IsOther = true;
                (DataContext as AddSaradnikViewModel).IsLokal = false;
            }
        }
    }
}
