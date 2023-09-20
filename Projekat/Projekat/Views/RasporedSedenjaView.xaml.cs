using Projekat.Model;
using Projekat.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        public IEnumerable CurrentNerasporedjeni { get; set; }
        public IEnumerable CurrentStolovi { get; set; }
        public ObservableCollection<Gost> NerasporedjeniTemp { get; set; }
        public ObservableCollection<KapacitetStola> StoloviTemp { get; set; }
        public Thread TutorialThread { get; set; }
        public void ExecuteTutorial(object sender, RoutedEventArgs e)
        {
            NerasporedjeniTemp = new ObservableCollection<Gost>();
            NerasporedjeniTemp.Add(new Gost { ImeIPrezime = "Marko Marković" });

            CurrentStolovi = ListBoxRasporedSedenja.ItemsSource;
            CurrentNerasporedjeni = ListViewNerasporedjeniGosti.ItemsSource;

            NerasporedjeniTemp.Add(new Gost
            {
                ImeIPrezime = "Petar Petrović"
            });
            
            NerasporedjeniTemp.Add(new Gost
            {
                ImeIPrezime = "Pera Perić"
            });
            
            NerasporedjeniTemp.Add(new Gost
            {
                ImeIPrezime = "Nina Ninić"
            });

            List<Gost> sto1 = new List<Gost>();
            sto1.Add(new Gost { ImeIPrezime = "Vera Milev" });
            sto1.Add(new Gost { ImeIPrezime = "Goca Božinovska" });

            List<Gost> sto2 = new List<Gost>();
            sto2.Add(new Gost { ImeIPrezime = "Gost Gostkovski" });
            sto2.Add(new Gost { ImeIPrezime = "Gost Gostović" });

            StoloviTemp = new ObservableCollection<KapacitetStola>();
            StoloviTemp.Add(new KapacitetStola { GostiZaStolom = new List<Gost>(), Kapacitet = 4, Naziv = "Sto 1" });
            StoloviTemp.Add(new KapacitetStola { GostiZaStolom = sto2, Kapacitet = 5, Naziv = "Sto 2" });
            StoloviTemp.Add(new KapacitetStola { GostiZaStolom = sto1, Kapacitet = 5, Naziv = "Sto 3" });

            TutorialThread = new Thread(CallTutorialMehotds);
            TutorialThread.Start();
        }

        private void CallTutorialMehotds()
        {
            while (true)
            {
                ToggleStopVisibility();
                DisableComponents();
                clearComponents();

                toggleGridEnable();
                Thread.Sleep(500);
                /*this.Dispatcher.Invoke((Action)(() =>
                {
                    DragDrop.Margin = new Thickness(DragDrop.Margin.Left + 100, DragDrop.Margin.Top + 100,
                        DragDrop.Margin.Right, DragDrop.Margin.Bottom);
                    Thread.Sleep(500);
                }));*/
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Thickness Margin = DragDropLabel.Margin;
                    Margin.Left = 170;
                    Margin.Top = 125;
                    DragDropLabel.Margin = Margin;
                }));

                //TutorialGrid.Visibility = Visibility.Visible;
                startAnimation();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Thickness Margin = DragDropLabel.Margin;
                    Margin.Left = 160;
                    Margin.Top = 120;
                    DragDropLabel.Margin = Margin;
                    NerasporedjeniTemp.Remove(NerasporedjeniTemp.First(s => s.ImeIPrezime == "Marko Marković"));
                    var temp = StoloviTemp;
                    temp.First(s => s.Naziv == "Sto 1").GostiZaStolom.Add(new Gost { ImeIPrezime = "Marko Marković" });
                    StoloviTemp = new ObservableCollection<KapacitetStola>(temp);
                    ListBoxRasporedSedenja.Items.Refresh();
                    //.First(s => s.Naziv == "Sto 1").GostiZaStolom.Add(new Gost { ImeIPrezime = "Marko Marković" });
                }));
                
                toggleGrid();
                Thread.Sleep(500);
                buttonDemo(PotvrdiBtn);
                rollbackComponents();
            }
        }

        private void toggleGrid()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    TutorialGrid.Visibility = System.Windows.Visibility.Collapsed; 
                }));
            }
            catch (Exception ex) { }
        }

        private void rollbackComponents()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                var temp = StoloviTemp.First(s => s.Naziv == "Sto 1").GostiZaStolom;
                temp.RemoveAt(temp.Count - 1);
                NerasporedjeniTemp.Insert(0, new Gost
                {
                    ImeIPrezime = "Marko Marković"
                });

                    
            }));
        }

        private void startAnimation()
        {
            try
            {
                
                    for (int i = 0; i < 20; i++)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Thickness Margin = DragDropLabel.Margin;
                            Margin.Left += 10;
                            Margin.Top += 15;
                            DragDropLabel.Margin = Margin;
                        }));
                        Thread.Sleep(100);
                    }
            }
            catch { }
        }

       private void clearComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ListViewNerasporedjeniGosti.ItemsSource = NerasporedjeniTemp;
                    ListBoxRasporedSedenja.ItemsSource = StoloviTemp;

                }));
            }
            catch (Exception e) { }
        }

        private void resetComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ListViewNerasporedjeniGosti.ItemsSource = CurrentNerasporedjeni;
                    ListBoxRasporedSedenja.ItemsSource = CurrentStolovi;
                    ListBoxRasporedSedenja.Items.Refresh();
                }));
            }
            catch (Exception e) { }
        }

        private void buttonDemo(Button button)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    button.IsEnabled = true;
                    button.Background = Brushes.GreenYellow;
                }));
                Thread.Sleep(300);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    BrushConverter bc = new BrushConverter();
                    button.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#4267B2");
                    button.IsEnabled = false;
                }));
            }
            catch (Exception ex) { }

        }

        private void DisableComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ListBoxRasporedSedenja.IsEnabled = false;
                    ListViewNerasporedjeniGosti.IsEnabled = false;
                    DodajBtn.IsEnabled = false;
                    PotvrdiBtn.IsEnabled = false;
                    PonistiBtn.IsEnabled = false;
                    
                }));
            }
            catch (Exception e) { }
        }

        private void ToggleStopVisibility()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    StopTutorialBtn.Visibility = System.Windows.Visibility.Visible;
                }));
            }
            catch (Exception ex) { }

        }


        private void enableComponents()
        {
            ListBoxRasporedSedenja.IsEnabled = true;
            ListViewNerasporedjeniGosti.IsEnabled = true;
            DodajBtn.IsEnabled = true;
            PonistiBtn.IsEnabled = true;
            PotvrdiBtn.IsEnabled = true;
        }

        private void ListBoxRasporedSedenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListViewNerasporedjeniGosti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StopTutorialBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                toggleGrid();
                resetComponents();
                enableComponents();
                TutorialThread.Abort();
                StopTutorialBtn.Visibility = System.Windows.Visibility.Hidden;
                PotvrdiBtn.Background = Brushes.Orange;
            }
            catch (Exception ex) { }
        }

        private void toggleGridEnable()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    TutorialGrid.Visibility = System.Windows.Visibility.Visible;
                    
                }));
            }
            catch (Exception ex) { }
        }
    }
}
