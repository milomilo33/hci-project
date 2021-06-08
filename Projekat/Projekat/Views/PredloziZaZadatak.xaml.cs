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
using System.Windows.Shapes;
using static Projekat.ViewModels.IzmjenaPonudeViewModel;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for PredloziZaZadatak.xaml
    /// </summary>
    public partial class PredloziZaZadatak : Window
    {
        public PredloziZaZadatak()
        {
            InitializeComponent();
            Loaded += Predlozi_Loaded;
        }

        private void Predlozi_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindow vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                   
                };
            }

        }
    }
}
