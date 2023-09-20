using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Projekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for PregledPredlogaView.xaml
    /// </summary>
    public partial class PregledPredlogaView : UserControl
    {
        public PregledPredlogaView()
        {
            InitializeComponent();
        }

        private (double, double) coordinates { get; set; }

        private void Address_Updated(object sender, RoutedEventArgs e)
        {
            coordinates = (DataContext as PregledPredlogaViewModel).GetLocationFromAddressAsync();
        }

        private void Load_On_Map(object sender, RoutedEventArgs e)
        {
            MapControl_Loaded(null, null);

            if (coordinates.Item1 == -1 && coordinates.Item2 == -1)
            {
                SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Adresa koju ste uneli ne postoji.";
                newDialog.DataContext = dialogModel;
                newDialog.ShowDialog();
                return;
            }
        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            coordinates = (DataContext as PregledPredlogaViewModel).GetLocationFromAddressAsync();

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
    }
}
