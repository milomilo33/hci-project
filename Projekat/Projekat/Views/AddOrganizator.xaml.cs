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
using static Projekat.ViewModels.IzmjenaPonudeViewModel;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for AddOrganizator.xaml
    /// </summary>
    public partial class AddOrganizator : Window
    {
        public AdminPregledOrganizatoraViewModel Next;
        public AddOrganizator(AdminPregledOrganizatoraViewModel next)
        {
            InitializeComponent();
            Loaded += CreateAddOrganizator_Loaded;
            Next = next;
        }

        private void CreateAddOrganizator_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindow vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                    Next.refresh();
                };
            }
        }

        public void NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
