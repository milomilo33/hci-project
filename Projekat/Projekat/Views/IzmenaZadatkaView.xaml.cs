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
using System.Windows.Shapes;
using static Projekat.ViewModels.KreiranjeZadatkaViewModel;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for IzmenaZadatkaView.xaml
    /// </summary>
    public partial class IzmenaZadatkaView : Window
    {
        public TaskViewModel Next;
        public IzmenaZadatkaView(TaskViewModel next)
        {
            Next = next;
            InitializeComponent();
            Loaded += Izmena_Loaded;
        }

        private void Izmena_Loaded(object sender, RoutedEventArgs e)
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
    }
}
