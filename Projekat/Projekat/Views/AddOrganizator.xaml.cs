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
    /// Interaction logic for AddOrganizator.xaml
    /// </summary>
    public partial class AddOrganizator : Window
    {
        public AddOrganizator()
        {
            InitializeComponent();
        }

        public AddOrganizator(AddOrganizatorViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            
        }

        public void NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
