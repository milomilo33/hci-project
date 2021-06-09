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
            RasporedSedenjaViewModel viewModel = (RasporedSedenjaViewModel)this.DataContext;
            if (e.OriginalSource is Path || e.OriginalSource is Grid || viewModel.Organizovan)
            {
                return;
            }

            ListView parent = (ListView)sender;
            dragSource = parent;
            object data = GetDataFromListView(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }

            //CommandManager.InvalidateRequerySuggested();
        }

        //private void ItemsList_DragOver(object sender, System.Windows.DragEventArgs e)
        //{
        //    ListBox li = sender as ListBox;
        //    ScrollViewer sv = FindVisualChild<ScrollViewer>(ListView);

        //    double tolerance = 10;
        //    double verticalPos = e.GetPosition(li).Y;
        //    double offset = 3;

        //    if (verticalPos < tolerance) // Top of visible list?
        //    {
        //        sv.ScrollToVerticalOffset(sv.VerticalOffset - offset); //Scroll up.
        //    }
        //    else if (verticalPos > li.ActualHeight - tolerance) //Bottom of visible list?
        //    {
        //        sv.ScrollToVerticalOffset(sv.VerticalOffset + offset); //Scroll down.    
        //    }
        //}

        //public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        //{
        //    // Search immediate children first (breadth-first)
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);

        //        if (child != null && child is childItem)
        //            return (childItem)child;

        //        else
        //        {
        //            childItem childOfChild = FindVisualChild<childItem>(child);

        //            if (childOfChild != null)
        //                return childOfChild;
        //        }
        //    }

        //    return null;
        //}

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
            RasporedSedenjaViewModel viewModel = (RasporedSedenjaViewModel)this.DataContext;

            ListView parent = (ListView)sender;

            // provera kapaciteta
            foreach (KapacitetStola ks in viewModel.RasporedSedenja)
            {
                if (ks.GostiZaStolom == parent.ItemsSource)
                {
                    if (((List<Gost>)parent.ItemsSource).Count() >= ks.Kapacitet)
                    {
                        SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                        SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                        dialogModel.Message = $"Sto '{ks.Naziv}' je već popunjen!";
                        dialog.DataContext = dialogModel;
                        dialogModel.IsError = true;
                        dialog.Owner = viewModel._window;
                        dialog.ShowDialog();

                        return;
                    }
                }
            }
            
            //object data = e.Data.GetData(typeof(string));
            object data = e.Data.GetData(typeof(Gost));
            ((IList)dragSource.ItemsSource).Remove(data);
            //parent.Items.Add(data);

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
