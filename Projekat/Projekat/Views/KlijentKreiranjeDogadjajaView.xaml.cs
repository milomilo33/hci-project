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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for KlijentKreiranjeDogadjajaView.xaml
    /// </summary>
    public partial class KlijentKreiranjeDogadjajaView : UserControl
    {
        public KlijentKreiranjeDogadjajaView()
        {
            InitializeComponent();

            FutureDatePicker.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.GetCultureInfo(2074).IetfLanguageTag);

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
