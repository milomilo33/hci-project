using Projekat.Model;
using Projekat.ViewModels;
using System;
using System.Collections;
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
    /// Interaction logic for RasporedSedenjaView.xaml
    /// </summary>
    public partial class RasporedSedenjaView : UserControl
    {
        public RasporedSedenjaView()
        {
            InitializeComponent();
        }

        ListView dragSource = null;

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListView parent = (ListView)sender;
            dragSource = parent;
            object data = GetDataFromListView(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private static object GetDataFromListView(ListView source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            ListView parent = (ListView)sender;
            //object data = e.Data.GetData(typeof(string));
            object data = e.Data.GetData(typeof(Gost));
            ((IList)dragSource.ItemsSource).Remove(data);
            //parent.Items.Add(data);

            RasporedSedenjaViewModel viewModel = (RasporedSedenjaViewModel) this.DataContext;

            //var backup = parent.ItemsSource;
            //parent.ItemsSource = null;
            //((IList)backup).Add(data);
            //parent.ItemsSource = backup;

            ((IList)parent.ItemsSource).Add(data);
            var backup = viewModel.RasporedSedenja;
            viewModel.RasporedSedenja = null;
            viewModel.RasporedSedenja = backup;
        }
    }
}
